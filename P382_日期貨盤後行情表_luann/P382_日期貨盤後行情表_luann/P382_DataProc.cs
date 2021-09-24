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
            string futuresSqlCmd = $"SELECT * FROM 期貨契約基本資料表 WHERE 年度='{cycle.Substring(0, 4)}'";
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
                foreach (SourceInfo src in ps.RunSourceInfo)
                {
                    foreach (string sample in sampleList)
                    {
                        ParseResult firstSample = src.GetResult(cycle, sample); //先用第一樣本取出資料
                        foreach (string secondSample in firstSample.RowSamples)
                        {
                            DataRow row = dt.NewRow();
                            ParseResult srcData = firstSample.GetSingleResult(secondSample); //使用GetSingleResult()取出資料
                            DataRow futureRow = futuresDt.AsEnumerable().SingleOrDefault(future => future.Field<string>("年度") == cycle.Substring(0, 4) && future.Field<string>("代號") == srcData.GetValue("契約"));
                            ProcessData(row, srcData, futureRow);
                            dt.Rows.Add(row);
                        }
                    }
                }
                SendMessage(msg, ps, 2, "解析資料處理完成");
                UpdateData(dtable, dt);
                result.dtData = dtable;
                result.Success = true;
                SendMessage(msg, ps, 2, "成功更新資料庫");
            }
            return result;
        }

        /// <summary>
        /// 用新的資料對原始資料作新增和更新
        /// </summary>
        /// <param name="dtable">原始資料表</param>
        /// <param name="dt">新處理好的資料表</param>
        public void UpdateData(DataTable dtable, DataTable dt)
        {
            IEnumerable<DataRow> AddRow = dt.AsEnumerable().Except(dtable.AsEnumerable(), new RowKeyComparer());
            IEnumerable<DataRow> differentRow = dt.AsEnumerable().Except(dtable.AsEnumerable(), new RowComparer()).Except(AddRow, new RowKeyComparer());
            foreach (DataRow i in AddRow)
            {
                dtable.Rows.Add(i.ItemArray);
            }
            foreach (DataRow i in differentRow)
            {
                DataRow target = dtable.AsEnumerable().SingleOrDefault(row => row.Field<string>("日期") == i.Field<string>("日期") &&
                                                                                      row.Field<string>("代號") == i.Field<string>("代號") &&
                                                                                      row.Field<string>("交割月份") == i.Field<string>("交割月份"));
                target["名稱"] = i.Field<string>("名稱");
                target["輸出代號"] = i.Field<string>("輸出代號");
                target["開盤價"] = i.Field<decimal?>("開盤價");
                target["最高價"] = i.Field<decimal?>("最高價");
                target["最低價"] = i.Field<decimal?>("最低價");
                target["收盤價"] = i.Field<decimal?>("收盤價");
                target["漲跌"] = i.Field<decimal?>("漲跌");
                target["成交量"] = i.Field<int?>("成交量");
                target["未沖銷契約數"] = i.Field<int?>("未沖銷契約數");
            }
        }

        /// <summary>
        /// 將解析後的資料處理成藥放入資料庫的樣子
        /// </summary>
        /// <param name="row">要放處理好資料的row</param>
        /// <param name="srcData">要處理的資料</param>
        /// <param name="name">從期貨契約基本資料表取得的名稱</param>
        public void ProcessData(DataRow row, ParseResult srcData, DataRow futureRow)
        {
            row["CTIME"] = srcData.CTime;
            row["MTIME"] = DB.GetMTime();
            row["日期"] = srcData.GetValue("交易日期"); //使用GetValue()取出指定欄位值
            row["代號"] = srcData.GetValue("契約");
            if (futureRow != null)
            {
                row["名稱"] = futureRow.Field<string>("名稱");
            }
            row["輸出代號"] = $"{srcData.GetValue("契約")}{srcData.GetValue("到期月份(週別)")}PM";
            row["交割月份"] = srcData.GetValue("到期月份(週別)");
            if (decimal.TryParse(srcData.GetValue("開盤價"), out decimal openPrice))
            {
                openPrice = decimal.Truncate((decimal)openPrice * 100) / 100;
                row["開盤價"] = openPrice;
            }
            if (decimal.TryParse(srcData.GetValue("最高價"), out decimal highPrice))
            {
                highPrice = decimal.Truncate((decimal)highPrice * 100) / 100;
                row["最高價"] = highPrice;
            }
            if (decimal.TryParse(srcData.GetValue("最低價"), out decimal lowPrice))
            {
                lowPrice = decimal.Truncate((decimal)lowPrice * 100) / 100;
                row["最低價"] = lowPrice;
            }
            if (decimal.TryParse(srcData.GetValue("收盤價"), out decimal closePrice))
            {
                closePrice = decimal.Truncate((decimal)closePrice * 100) / 100;
                row["收盤價"] = closePrice;
            }
            if (decimal.TryParse(srcData.GetValue("漲跌價"), out decimal upDown))
            {
                upDown = decimal.Truncate((decimal)upDown * 100) / 100;
                row["漲跌"] = upDown;
            }
            row["成交量"] = srcData.GetValue("成交量");
            if (decimal.TryParse(srcData.GetValue("未沖銷契約數"), out decimal contractNumber))
            {
                row["未沖銷契約數"] = contractNumber;
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
    }
}
