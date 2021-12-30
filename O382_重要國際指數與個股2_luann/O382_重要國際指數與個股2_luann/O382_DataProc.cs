using CMoney.Kernel;
using DataProc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static O382_重要國際指數與個股2_luann.Global;

namespace O382_重要國際指數與個股2_luann
{
    /// <summary>
    /// 處理主程式
    /// </summary>
    public class O382_DataProc : IDataProc
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
        /// 實作資料轉換成CMoney表處理。這個需求是新增美股的中文新聞來源，因為新聞資料量龐大，所以不進mongoDB，
        /// 不跑一般的統一架構，而在處理程式才去叫用解析設定表的方法，使用解析擴充解析，解完接處理程序
        /// </summary>
        /// <param name="ps">程式相關設定</param>
        /// <returns>DataProc資料處理結果</returns>
        public DataResult GetProc(ProgramSetting ps)
        {
            string cycle = ps.DateRange.First();
            string connectString = DB.GetSysDataSetting("上端程式", ps.SysDataDB, "連線字串");
            DataTable targetTable = GetTargetTable(connectString, cycle);
            Dictionary<int, int> priorityTable = GetPriorityTable(connectString, ps.CMenuID);
            DateTime ctime = DateTime.Now;
            int mtime = DB.GetMTime();
            string[] columnNames = new string[] {OPEN_PRICE, HIGHEST_PRICE, LOWEST_PRICE, CLOSE_PRICE };
            
            foreach (SourceInfo source in ps.RunSourceInfo)
            {
                List<string> sampleList = GetSamples(ps, cycle, source);

                foreach (string sample in sampleList)
                {
                    ParseResult parseResult = source.GetResult(cycle, sample);
                    DataRow targetRow = targetTable.Rows.Find(new object[] {cycle, sample});
                    if (targetRow == null)
                    {
                        targetRow = targetTable.NewRow();
                        targetRow[DATE] = cycle;
                        targetRow[STOCK_ID] = sample;
                        targetRow[SOURCE_ID] = source.SourceID;
                        targetRow[CTIME] = ctime;
                        targetRow[MTIME] = mtime;
                        AddTargetTable(parseResult, targetRow, columnNames);
                        targetTable.Rows.Add(targetRow);
                    }
                    else
                    {
                        int targetSource = targetRow.Field<int>(SOURCE_ID);
                        //如果進來source的優先權大於原本source或source是原本的source則判斷是否更新
                        if (source.SourceID == targetSource || priorityTable[source.SourceID] > priorityTable[targetSource])
                        {
                            UpdateTargetTable(parseResult, targetRow, columnNames, mtime, source.SourceID);
                        }
                    }
                }
            }

            DataResult result = new DataResult();
            result.Success = true;
            result.dtData = targetTable;

            return result;
        }

        /// <summary>
        /// 取得樣本
        /// </summary>
        /// <param name="ps">程式相關設定</param>
        /// <param name="cycle">週期</param>
        /// <param name="source">來源資料</param>
        /// <returns>這個來源的所有樣本</returns>
        private List<string> GetSamples(ProgramSetting ps, string cycle, SourceInfo source)
        {
            bool isAllSample = (string.IsNullOrEmpty(ps.Samples.FirstOrDefault()) && ps.Samples.Count == 1) || (ps.Samples.Count == 0);
            List<string> sampleList = new List<string>();

            if (isAllSample)
            {
                ParseResult parseResult = source.GetResult(cycle, string.Empty);
                if (parseResult != null)
                {
                    sampleList = parseResult.RowSamples;
                }
            }
            else
            {
                sampleList = ps.Samples;
            }

            return sampleList;
        }

        /// <summary>
        /// 取得要被更新的目標資料表
        /// </summary>
        /// <param name="connectString">連線字串</param>
        /// <param name="cycle">週期</param>
        /// <returns>目標資料表</returns>
        private DataTable GetTargetTable(string connectString, string cycle)
        {
            string sqlCommand = $"SELECT * FROM [重要國際指數與個股2_luann] WHERE [日期] = '{cycle}'";
            return DB.DoQuerySQLWithSchema(sqlCommand, connectString);
        }

        /// <summary>
        /// 取得Source與優先權的對應
        /// </summary>
        /// <param name="connectString">連線字串</param>
        /// <param name="cmenuID">此次的CMenuID</param>
        /// <returns>Source與優先權的對應</returns>
        private Dictionary<int, int> GetPriorityTable(string connectString, string cmenuID)
        {
            string sqlCommand = $"SELECT [SourceID],[Priority] FROM [StockDB].[dbo].[重要國際指數與個股2_優先權_luann] WHERE [CmenuID] = '{cmenuID}'";
            return DB.DoQuerySQLWithSchema(sqlCommand, connectString).AsEnumerable()
                                                                                                                                       .ToDictionary(row => row.Field<int>(SOURCE_ID),
                                                                                                                                                                 row => row.Field<int>("Priority"));
        }

        /// <summary>
        /// 新增指定欄位的資料
        /// </summary>
        /// <param name="parseResult">解析完的原始資料</param>
        /// <param name="row">要被新增資料的列</param>
        /// <param name="columnNames">要新增資料的欄位名稱</param>
        private void AddTargetTable(ParseResult parseResult, DataRow row, string[] columnNames)
        {
            foreach (string column in columnNames)
            {
                if (decimal.TryParse(parseResult.GetValue(column), out decimal price))
                {
                    row[column] = price;
                }
            }
        }

        /// <summary>
        /// 要更新指定欄位資料的列
        /// </summary>
        /// <param name="parseResult">解析完的原始資料</param>
        /// <param name="targetRow">要被更新資料的列</param>
        /// <param name="columnNames">要更新資料的欄位名稱</param>
        /// <param name="mtime">MTIME</param>
        /// <param name="sourceID">SourceID</param>
        private void UpdateTargetTable(ParseResult parseResult, DataRow targetRow, string[] columnNames, int mtime, int sourceID)
        {
            foreach (string column in columnNames)
            {
                if (decimal.TryParse(parseResult.GetValue(column), out decimal price))
                {
                    if (!targetRow[column].Equals(price))
                    {
                        targetRow[column] = price;
                        targetRow[MTIME] = mtime;
                    }
                }
            }

            if (sourceID != targetRow.Field<int>(SOURCE_ID))
            {
                targetRow[SOURCE_ID] = sourceID;
                targetRow[MTIME] = mtime;
            }
        }
    }
}
