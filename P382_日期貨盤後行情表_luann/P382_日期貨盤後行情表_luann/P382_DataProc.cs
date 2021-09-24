using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMoney.Kernel;
using DataProc;

namespace P382_日期貨盤後行情表_luann
{
    /// <summary>
    /// 主要dll類
    /// </summary>
    public class P382_DataProc : IDataProc
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
            ProcessChangeEventArgs msg = new ProcessChangeEventArgs();
            string cycle = ps.DateRange.FirstOrDefault(); //取傳入週期
            string connStr = DB.GetSysDataSetting("上端程式", ps.SysDataDB, "連線字串");
            string sqlCmd = $"SELECT * FROM {ps.ProgramName} WHERE 日期='{cycle}'";
            SendMessage(msg, ps, 2, "取得原始資料表內容");
            DataTable dtable = DB.DoQuerySQLWithSchema(sqlCmd, connStr); //回傳的Table包含資料表Schema
            string futuresSqlCmd = $"SELECT [年度],[代號],[名稱] FROM 期貨契約基本資料表 WHERE 年度='{cycle.Substring(0, 4)}'";
            DataTable futuresDt = DB.DoQuerySQLWithSchema(futuresSqlCmd, connStr); //回傳的Table包含資料表Schema
            SendMessage(msg, ps, 2, "取得期貨契約基本資料表內容");
            DataTable dt = dtable.Clone();
            DataResult result = new DataResult(); //宣告DataResult物件
            if (!DB.IsHoliday(cycle))
            {
                bool isALLSample = (string.IsNullOrEmpty(ps.Samples.FirstOrDefault()) && ps.Samples.Count() == 1) || (ps.Samples.Count == 0); //這也是判斷是否有傳入樣本，用於自動模式時，來源沒傳入樣本時的情形
                List<string> sampleList = new List<string>(); //準備執行的樣本
                //決定要執行的樣本
                if (isALLSample)
                {
                    ParseResult sourceData = ps.RunSourceInfo.FirstOrDefault().GetResult(cycle, string.Empty);
                    sampleList = sourceData.RowSamples;
                }
                else
                {
                    sampleList = ps.Samples;
                }
                Dictionary<string, string> futureNameList = futuresDt.Rows.Cast<DataRow>().ToDictionary(row => row.Field<string>(GlobalConst.CODENAME), row => row.Field<string>(GlobalConst.NAME));
                DateTime addTime = DateTime.Now;
                int changeTime = DB.GetMTime();
                foreach (SourceInfo src in ps.RunSourceInfo)
                {
                    foreach (string sample in sampleList)
                    {
                        ParseResult firstSample = src.GetResult(cycle, sample); //先用第一樣本取出資料
                        foreach (string secondSample in firstSample.RowSamples)
                        {
                            DataRow row = dtable.NewRow();
                            ParseResult srcData = firstSample.GetSingleResult(secondSample); //使用GetSingleResult()取出資料
                            ProcessData(row, srcData, futureNameList, addTime, changeTime);
                            UpdateData(dtable, row);
                        }
                    }
                }
                SendMessage(msg, ps, 2, "解析資料處理完成");
                result.dtData = dtable;
                result.Success = true;
                SendMessage(msg, ps, 2, "成功更新資料庫");
            }
            return result;
        }

        /// <summary>
        /// 將解析後的資料處理成藥放入資料庫的樣子
        /// </summary>
        /// <param name="row">要放處理好資料的row</param>
        /// <param name="srcData">要處理的資料</param>
        /// <param name="future">契約與名字對應的Dictionary</param>
        /// <param name="addTime">CTIME</param>
        /// <param name="changeTime">MTIME</param>
        public void ProcessData(DataRow row, ParseResult srcData, Dictionary<string, string> future, DateTime addTime, int changeTime)
        {
            row[GlobalConst.CTIME] = addTime;
            row[GlobalConst.MTIME] = changeTime;
            row[GlobalConst.DATE] = srcData.GetValue(GlobalConst.TRANSACTION_DATE); //使用GetValue()取出指定欄位值
            string contract = srcData.GetValue(GlobalConst.CONTRACT);
            row[GlobalConst.CODENAME] = contract;
            if (future.TryGetValue(contract, out string name))
            {
                row[GlobalConst.NAME] = name;
            }
            row[GlobalConst.OUTPUT_CODE] = $"{srcData.GetValue(GlobalConst.CONTRACT)}{srcData.GetValue(GlobalConst.EXPIRY_MONTH)}{GlobalConst.PM}";
            row[GlobalConst.DELIVERY_MONTH] = srcData.GetValue(GlobalConst.EXPIRY_MONTH);
            PutDecimalRow(row, GlobalConst.OPENING_PRICE, srcData.GetValue(GlobalConst.OPENING_PRICE));
            PutDecimalRow(row, GlobalConst.HIGHEST_PRICE, srcData.GetValue(GlobalConst.HIGHEST_PRICE));
            PutDecimalRow(row, GlobalConst.LOWEST_PRICE, srcData.GetValue(GlobalConst.LOWEST_PRICE));
            PutDecimalRow(row, GlobalConst.CLOSING_PRICE, srcData.GetValue(GlobalConst.CLOSING_PRICE));
            PutDecimalRow(row, GlobalConst.UP_DOWN, srcData.GetValue(GlobalConst.UP_DOWN_PRICE));
            row[GlobalConst.VOLUME] = srcData.GetValue(GlobalConst.VOLUME);
            if (int.TryParse(srcData.GetValue(GlobalConst.CONTRACT_NUMBER), out int contractNum))
            {
                row[GlobalConst.CONTRACT_NUMBER] = contractNum;
            }
        }

        /// <summary>
        /// 寄送訊息
        /// </summary>
        /// <param name="msg">存放訊息物件</param>
        /// <param name="ps">用於寄送的物件</param>
        /// <param name="part">信的等級</param>
        /// <param name="processMessage">信的內容</param>
        public void SendMessage(ProcessChangeEventArgs msg, ProgramSetting ps, int part, string processMessage)
        {
            msg.Part = part;
            msg.ProcessMessage = processMessage;
            ps.SendMessage(msg);
        }

        /// <summary>
        /// 將decimal的欄位由字串轉decimal，並取到小數點後兩位，然後放入列裡
        /// </summary>
        /// <param name="row">要放入值的列</param>
        /// <param name="rowName">該值的欄位名</param>
        /// <param name="dataValue">要轉換的字串</param>
        public void PutDecimalRow(DataRow row, string rowName, string dataValue)
        {
            if (decimal.TryParse(dataValue, out decimal value))
            {
                value = decimal.Truncate((decimal)value * 100) / 100;
                row[rowName] = value;
            }
        }

        /// <summary>
        /// 判斷要不要更新並更新資料表的方法
        /// </summary>
        /// <param name="dtable">被更新的原資料表</param>
        /// <param name="row">要判斷的資料內容</param>
        public void UpdateData(DataTable dtable, DataRow row)
        {
            DataRow commonRow = dtable.Rows.Find(new object[] { row.Field<string>(GlobalConst.DATE), row.Field<string>(GlobalConst.CODENAME), row.Field<string>(GlobalConst.DELIVERY_MONTH) });
            if (commonRow != null)
            {
                if (!(row.Field<string>(GlobalConst.NAME) == commonRow.Field<string>(GlobalConst.NAME) &&
                       row.Field<string>(GlobalConst.OUTPUT_CODE) == commonRow.Field<string>(GlobalConst.OUTPUT_CODE) &&
                       row.Field<decimal?>(GlobalConst.OPENING_PRICE) == commonRow.Field<decimal?>(GlobalConst.OPENING_PRICE) &&
                       row.Field<decimal?>(GlobalConst.HIGHEST_PRICE) == commonRow.Field<decimal?>(GlobalConst.HIGHEST_PRICE) &&
                       row.Field<decimal?>(GlobalConst.LOWEST_PRICE) == commonRow.Field<decimal?>(GlobalConst.LOWEST_PRICE) &&
                       row.Field<decimal?>(GlobalConst.CLOSING_PRICE) == commonRow.Field<decimal?>(GlobalConst.CLOSING_PRICE) &&
                       row.Field<decimal?>(GlobalConst.UP_DOWN) == commonRow.Field<decimal?>(GlobalConst.UP_DOWN) &&
                       row.Field<int?>(GlobalConst.VOLUME) == commonRow.Field<int?>(GlobalConst.VOLUME) &&
                       row.Field<int?>(GlobalConst.CONTRACT_NUMBER) == commonRow.Field<int?>(GlobalConst.CONTRACT_NUMBER)))
                {
                    commonRow.ItemArray = row.ItemArray;
                }
            }
            else
            {
                dtable.Rows.Add(row);
            }
        }
    }
}
