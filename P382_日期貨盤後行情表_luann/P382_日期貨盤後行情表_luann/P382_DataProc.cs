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
            string futuresSqlCmd = $"SELECT [代號],[名稱] FROM 期貨契約基本資料表 WHERE 年度='{cycle.Substring(0, 4)}'";
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
                            ParseResult srcData = firstSample.GetSingleResult(secondSample); //使用GetSingleResult()取出資料
                            ProcessData(dtable, srcData, futureNameList, addTime, changeTime);
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
        /// 將解析完的資料處理成進資料庫的樣子，和資料庫比對，不同則覆蓋，不存在則新增
        /// </summary>
        /// <param name="dtable">資料庫的資料</param>
        /// <param name="srcData">解析好的資料</param>
        /// <param name="future">期貨契約基本資料表經過處理的代號和名稱對應</param>
        /// <param name="addTime">CTIME</param>
        /// <param name="changeTime">MTIME</param>
        public void ProcessData(DataTable dtable, ParseResult srcData, Dictionary<string, string> future, DateTime addTime, int changeTime)
        {
            string date = srcData.GetValue(GlobalConst.TRANSACTION_DATE); //使用GetValue()取出指定欄位值
            string contract = srcData.GetValue(GlobalConst.CONTRACT);
            string deliveryMonth = srcData.GetValue(GlobalConst.EXPIRY_MONTH);
            future.TryGetValue(contract, out string name);
            string outputCode = $"{srcData.GetValue(GlobalConst.CONTRACT)}{srcData.GetValue(GlobalConst.EXPIRY_MONTH)}{GlobalConst.PM}";
            decimal? openingPrice = PutDecimalRow(srcData.GetValue(GlobalConst.OPENING_PRICE));
            decimal? highestPrice = PutDecimalRow(srcData.GetValue(GlobalConst.HIGHEST_PRICE));
            decimal? lowestPrice = PutDecimalRow(srcData.GetValue(GlobalConst.LOWEST_PRICE));
            decimal? closingPrice = PutDecimalRow(srcData.GetValue(GlobalConst.CLOSING_PRICE));
            decimal? onDownPrice = PutDecimalRow(srcData.GetValue(GlobalConst.UP_DOWN_PRICE));
            int.TryParse(srcData.GetValue(GlobalConst.VOLUME), out int volume);
            int.TryParse(srcData.GetValue(GlobalConst.CONTRACT_NUMBER), out int contractNum);
            DataRow commonRow = dtable.Rows.Find(new object[] { date, contract, deliveryMonth });
            bool isChange = false;
            if (commonRow != null)
            {
                UpdateData<string>(name, commonRow, GlobalConst.NAME, ref isChange);
                UpdateData<string>(outputCode, commonRow, GlobalConst.OUTPUT_CODE, ref isChange);
                UpdateData<decimal?>(openingPrice, commonRow, GlobalConst.OPENING_PRICE, ref isChange);
                UpdateData<decimal?>(highestPrice, commonRow, GlobalConst.HIGHEST_PRICE, ref isChange);
                UpdateData<decimal?>(lowestPrice, commonRow, GlobalConst.LOWEST_PRICE, ref isChange);
                UpdateData<decimal?>(closingPrice, commonRow, GlobalConst.CLOSING_PRICE, ref isChange);
                UpdateData<decimal?>(onDownPrice, commonRow, GlobalConst.UP_DOWN, ref isChange);
                UpdateData<int?>(volume, commonRow, GlobalConst.VOLUME, ref isChange);
                UpdateData<int?>(contractNum, commonRow, GlobalConst.CONTRACT_NUMBER, ref isChange);
                if (isChange)
                {
                    commonRow.SetField<int>(GlobalConst.MTIME, changeTime);
                }  
            }
            else
            {
                DataRow row = dtable.NewRow();
                row.ItemArray = new object[] {addTime, changeTime, date, contract, name, outputCode, deliveryMonth, openingPrice, highestPrice, lowestPrice, closingPrice, onDownPrice, volume, contractNum};
                dtable.Rows.Add(row);
            }
        }

        /// <summary>
        /// 確認進來的質是否和原本資料庫的值相同，不相同則覆蓋
        /// </summary>
        /// <typeparam name="T">要比對的資料型別</typeparam>
        /// <param name="dataValue">要比對的資料</param>
        /// <param name="row">原始資料</param>
        /// <param name="rowName">資料欄位名稱</param>
        /// <param name="isChange">是否有更改</param>
        public void UpdateData<T>(T dataValue, DataRow row, string rowName, ref bool isChange)
        {

            T rowValue = row.Field<T>(rowName);
            if ((dataValue == null && rowValue == null) || dataValue.Equals(rowValue))
            {
                return;
            }
            isChange = true;
            row.SetField<T>(rowName, dataValue);
        }

        /// <summary>
        /// 將進來的字串轉成decimal
        /// </summary>
        /// <param name="dataValue">要轉的字串</param>
        /// <returns></returns>
        public decimal? PutDecimalRow(string dataValue)
        {
            //為了取小數點後兩位要乘十在除十
            int decimalPoint = 2;
            if (decimal.TryParse(dataValue, out decimal value))
            {
                return decimal.Round(value, decimalPoint);
            }
            return null;
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

        public void ProcessData1(DataTable dtable, ParseResult srcData, Dictionary<string, string> future, DateTime addTime, int changeTime)
        {
            Dictionary<string, string> keyList = new Dictionary<string, string>();
            string[] keyNames =new string[]{ GlobalConst.TRANSACTION_DATE, GlobalConst.CONTRACT,GlobalConst.EXPIRY_MONTH};
            string[] keys = keyList.Values.ToArray();
            foreach (string keyName in keyNames)
            {
                keyList.Add(keyName, srcData.GetValue(keyName));
            }
            DataRow commonRow = dtable.Rows.Find(keys);
            if (commonRow == null)
            {
                commonRow = dtable.NewRow();
                for (int i = 0; i < keyList.Count(); i++)
                {
                    commonRow[dtable.PrimaryKey[i].ColumnName] = keys[i];
                }
                commonRow[GlobalConst.CTIME] = addTime;
                dtable.Rows.Add(commonRow);
            }
            DataColumn[] copyColumns = new DataColumn[dtable.Columns.Count];
            dtable.Columns.CopyTo(copyColumns, 0);
            List<DataColumn> noKeyColumns = copyColumns.ToList<DataColumn>();
            foreach (DataColumn column in dtable.PrimaryKey)
            {
                noKeyColumns.Remove(column);
            }
            foreach (DataColumn column in noKeyColumns)
            {
                switch (column.ColumnName)
                {
                    case GlobalConst.NAME:
                        future.TryGetValue(keyList[GlobalConst.CONTRACT], out string name);
                        break;
                }
                UpdateData<string>(name, commonRow, GlobalConst.OUTPUT_CODE, ref isChange);
            }
            
            string outputCode = $"{srcData.GetValue(GlobalConst.CONTRACT)}{srcData.GetValue(GlobalConst.EXPIRY_MONTH)}{GlobalConst.PM}";
            decimal? openingPrice = PutDecimalRow(srcData.GetValue(GlobalConst.OPENING_PRICE));
            decimal? highestPrice = PutDecimalRow(srcData.GetValue(GlobalConst.HIGHEST_PRICE));
            decimal? lowestPrice = PutDecimalRow(srcData.GetValue(GlobalConst.LOWEST_PRICE));
            decimal? closingPrice = PutDecimalRow(srcData.GetValue(GlobalConst.CLOSING_PRICE));
            decimal? onDownPrice = PutDecimalRow(srcData.GetValue(GlobalConst.UP_DOWN_PRICE));
            int.TryParse(srcData.GetValue(GlobalConst.VOLUME), out int volume);
            int.TryParse(srcData.GetValue(GlobalConst.CONTRACT_NUMBER), out int contractNum);
            
            bool isChange = false;
            if (commonRow != null)
            {
                //UpdateData<string>(name, commonRow, GlobalConst.NAME, ref isChange);
                UpdateData<string>(outputCode, commonRow, GlobalConst.OUTPUT_CODE, ref isChange);
                UpdateData<decimal?>(openingPrice, commonRow, GlobalConst.OPENING_PRICE, ref isChange);
                UpdateData<decimal?>(highestPrice, commonRow, GlobalConst.HIGHEST_PRICE, ref isChange);
                UpdateData<decimal?>(lowestPrice, commonRow, GlobalConst.LOWEST_PRICE, ref isChange);
                UpdateData<decimal?>(closingPrice, commonRow, GlobalConst.CLOSING_PRICE, ref isChange);
                UpdateData<decimal?>(onDownPrice, commonRow, GlobalConst.UP_DOWN, ref isChange);
                UpdateData<int?>(volume, commonRow, GlobalConst.VOLUME, ref isChange);
                UpdateData<int?>(contractNum, commonRow, GlobalConst.CONTRACT_NUMBER, ref isChange);
                if (isChange)
                {
                    commonRow.SetField<int>(GlobalConst.MTIME, changeTime);
                }
            }
            else
            {
                DataRow row = dtable.NewRow();
                //row.ItemArray = new object[] { addTime, changeTime, date, contract, name, outputCode, deliveryMonth, openingPrice, highestPrice, lowestPrice, closingPrice, onDownPrice, volume, contractNum };
                dtable.Rows.Add(row);
            }
        }
    }
}
