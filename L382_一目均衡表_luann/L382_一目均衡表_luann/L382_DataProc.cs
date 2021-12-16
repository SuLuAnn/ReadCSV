using CMoney.Kernel;
using DataProc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static L382_一目均衡表_luann.Global;

namespace L382_一目均衡表_luann
{
    /// <summary>
    /// 主要dll類
    /// </summary>
    public class L382_DataProc : IDataProc
    {
        /// <summary>
        /// 實作檢查機制
        /// </summary>
        /// <param name="CheckList">檢查項目</param>
        /// <param name="Cycle">週期</param>
        /// <param name="Sample">樣本</param>
        /// <returns>檢查結果</returns>
        public List<CheckResult> DataCheck(List<int> CheckList, string Cycle, string Sample)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 實作檢查機制
        /// </summary>
        /// <param name="CheckList">檢查項目</param>
        /// <param name="CheckData">檢查資料表</param>
        /// <returns>檢查結果</returns>
        public List<CheckResult> DataCheck(List<int> CheckList, DataTable CheckData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 實作資料轉換成CMoney表處理。
        /// </summary>
        /// <param name="ps">程式相關設定</param>
        /// <returns>DataProc資料處理結果</returns>
        public DataResult GetProc(ProgramSetting ps)
        {
            string cycle = ps.DateRange.FirstOrDefault();
            DataResult result = new DataResult();
            string connectString = DB.GetSysDataSetting(UPPER_PROGRAM, ps.SysDataDB, CONNECT_STRING);
            string message = string.Empty;
            //取開頭四字為年
            int fourWord = 4;
            if (!DB.IsHoliday(cycle))
            {
                DataTable targetTable = GetTargetTable(connectString, cycle);
                foreach (SourceInfo sourceInfo in ps.RunSourceInfo)
                {
                    int sourceId = sourceInfo.SourceID;
                    DataTable sourceTable = GetDayCloseReduction(connectString, cycle);
                    if (sourceTable.Rows.Count == 0)
                    {
                        message = $"{message}{Environment.NewLine}週期:{cycle} 沒有近9天或近26天";
                        continue;
                    }
                    StoreData(targetTable, sourceTable, sourceId, GetNameMapping(cycle.Substring(0, fourWord), connectString), cycle);
                }
                if (message.Length != 0)
                {
                    SendMessage(ps, MessagePart.WARN, message);
                }
                result.Success = true;
                result.dtData = targetTable.GetChanges();
            }

            return result;
        }

        /// <summary>
        /// 取得特定週期日收盤還原表資料
        /// </summary>
        /// <param name="connectString">連線字串</param>
        /// <param name="cycle">日期</param>
        /// <returns>日收盤還原表資料</returns>
        private DataTable GetDayCloseReduction(string connectString, string cycle)
        {
            int day = int.Parse(cycle);
            int oneYear = 10000;
            string sqlCommand = 
            $@";WITH CTE AS (SELECT [股票代號],[最高價],[最低價],ROW_NUMBER() OVER (PARTITION BY [股票代號] ORDER BY [日期] DESC) AS [序號] 
            FROM [dbo].[日收盤還原表] WHERE [日期] BETWEEN N'{day - oneYear}' AND N'{day}'  AND [最高價] IS NOT NULL AND [最低價] IS NOT NULL),
            CTE2 AS (SELECT [股票代號],MAX([最高價]) AS [近9日最高價], MIN([最低價]) AS [近9日最低價] FROM CTE WHERE [序號] <= 9 GROUP BY [股票代號] 
            HAVING COUNT(*) = 9),
            CTE3 AS (SELECT [股票代號],MAX([最高價]) AS [近26日最高價], MIN([最低價]) AS [近26日最低價] FROM CTE WHERE [序號] <= 26 GROUP BY [股票代號] 
            HAVING COUNT(*) = 26),
            CTE4 AS(SELECT A.[股票代號],[近9日最高價],[近9日最低價],[近26日最高價],[近26日最低價] FROM CTE2 AS A LEFT JOIN CTE3 AS B 
            ON A.[股票代號] = B.[股票代號])
            SELECT [日期],A.[股票代號],[股票名稱],[近9日最高價],[近9日最低價],[近26日最高價],[近26日最低價],[收盤價] AS [日收盤還原表收盤價] 
            FROM[StockDb].[dbo].[日收盤還原表] 
            AS A LEFT JOIN CTE4 AS B ON A.股票代號 = B.股票代號 WHERE[日期] = N'{day}'";

            return DB.DoQuerySQLWithSchema(sqlCommand, connectString);
        }

        /// <summary>
        /// 取得要更新的目標資料表
        /// </summary>
        /// <param name="connectString">連線字串</param>
        /// <param name="cycle">日期</param>
        /// <returns>一目均衡表_luann的資料</returns>
        private DataTable GetTargetTable(string connectString, string cycle)
        {
            string sqlCommand = $"SELECT * FROM [StockDB].[dbo].[一目均衡表_luann] WHERE [日期] = N'{cycle}'";
            return DB.DoQuerySQLWithSchema(sqlCommand, connectString);
        }

        /// <summary>
        /// 對資料做新增或更新
        /// </summary>
        /// <param name="targetTable">目標資料表</param>
        /// <param name="sourceTable">來源資料表</param>
        /// <param name="sourceId">sourceId</param>
        /// <param name="nameMapping">股票名稱對應表</param>
        /// <param name="cycle">日期</param>
        private void StoreData(DataTable targetTable, DataTable sourceTable, int sourceId, Dictionary<string, string> nameMapping, string cycle)
        {
            DateTime ctime = DateTime.Now;
            int mtime = DB.GetMTime();
            string[] columnNames = new string[] { DATE, STOCK_ID, CLOSE_PRICE};
            foreach (DataRow sourceRow in sourceTable.Rows)
            {
                string stockId = sourceRow.Field<string>(STOCK_ID);
                DataRow targetRow = targetTable.Rows.Find(new object[] { cycle, stockId });
                if (targetRow == null)
                {
                    targetRow = targetTable.NewRow();
                    targetRow[CTIME] = ctime;
                    targetRow[MTIME] = mtime; 
                    targetRow[SOURCE_ID] = sourceId;
                    targetRow[CHANGE_LINE] = GetAverage(sourceRow, NEARLY_9_MAX_PRICE, NEARLY_9_MIN_PRICE);
                    targetRow[HUB_LINE] = GetAverage(sourceRow, NEARLY_26_MAX_PRICE, NEARLY_26_MIN_PRICE);
                    foreach (string columnName in columnNames)
                    {
                        targetRow[columnName] = sourceRow[columnName];
                    }

                    targetRow[STOCK_NAME] = GetStockName(nameMapping, sourceRow, stockId);
                    targetTable.Rows.Add(targetRow);
                }
                else
                {
                    if (UpdateData(sourceRow, targetRow, nameMapping, stockId))
                    {
                        targetRow[MTIME] = mtime;
                    }
                }
            }
        }

        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="sourceRow">來源資料列</param>
        /// <param name="targetRow">目標資料列</param>
        /// <param name="nameMapping">股票名稱對應表</param>
        /// <param name="stockId">股票代號</param>
        /// <returns>是否有資料被更新</returns>
        private bool UpdateData(DataRow sourceRow, DataRow targetRow, Dictionary<string, string> nameMapping, string stockId)
        {
            bool isUpdated = false;
            string[] columnNames = new string[] { CLOSE_PRICE, CHANGE_LINE, HUB_LINE };
            foreach (string columnName in columnNames)
            {
                object target = targetRow[columnName];
                object source;
                switch (columnName)
                {
                    case STOCK_NAME:
                        source = GetStockName(nameMapping, sourceRow, stockId);
                        break;
                    case CHANGE_LINE:
                        source = GetAverage(sourceRow, NEARLY_9_MAX_PRICE, NEARLY_9_MIN_PRICE);
                        break;
                    case HUB_LINE:
                        source = GetAverage(sourceRow, NEARLY_26_MAX_PRICE, NEARLY_26_MIN_PRICE);
                        break;
                    default:
                        source = sourceRow[columnName];
                        break;
                }
                if (source != DBNull.Value && !target.Equals(source))
                {
                    targetRow[columnName] = source;
                    isUpdated = true;
                }
            }

            return isUpdated;
        }

        /// <summary>
        /// 取得股票名稱
        /// </summary>
        /// <param name="nameMapping">股票名稱對應表</param>
        /// <param name="sourceRow">來源資料列</param>
        /// <param name="stockId">股票代號</param>
        /// <returns>股票名稱</returns>
        private string GetStockName(Dictionary<string, string> nameMapping, DataRow sourceRow, string stockId)
        {
            string result;
            if (nameMapping.TryGetValue(stockId, out string stockName))
            {
                result = stockName;
            }
            else
            {
                result = sourceRow.Field<string>(STOCK_NAME);
            }
            return result;
        }

        /// <summary>
        /// 記送訊息
        /// </summary>
        /// <param name="programSetting">程式相關設定</param>
        /// <param name="part">信件等級</param>
        /// <param name="message">信件內容</param>
        private void SendMessage(ProgramSetting programSetting, MessagePart part, string message)
        {
            ProcessChangeEventArgs eventArgs = new ProcessChangeEventArgs();
            eventArgs.Part = (int)part;
            eventArgs.ProcessMessage = message;
            programSetting.SendMessage(eventArgs);
        }

        /// <summary>
        /// 兩值除2取平均數
        /// </summary>
        /// <param name="row">要取值的資料列</param>
        /// <param name="columnOne">被取值第一個欄位名稱</param>
        /// <param name="columnTwo">被取值第二個欄位名稱</param>
        /// <returns>兩欄位值的平均</returns>
        private object GetAverage(DataRow row, string columnOne, string columnTwo)
        {
            int count = 2;
            object result = (row.Field<decimal?>(columnOne) + row.Field<decimal?>(columnTwo)) / count;
            if (result == null)
            {
                result = DBNull.Value;
            }
            return result;
        }
    }
}
