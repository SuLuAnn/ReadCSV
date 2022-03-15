using CMoney.Kernel;
using DownloadSystem.DataLibs.Interface;
using DownloadSystem.DataLibs.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace M157_DownloadExtent_TEST
{
    [Export(typeof(DownloadSystem.DataLibs.Interface.ITaskExtension))]
    [ExportMetadata("Name", "M157_DownloadExtent_TEST.181001002_TEST")]
    [ExportMetadata("ContainsDatabaseOperation", true)]
    [ExportMetadata("ContainsInternetOperation", false)]
    public class DownloadExtension_181001003 : ITaskExtension
    {
        /// <summary>
        /// 股票集合(ID+Name)
        /// </summary>
        private Dictionary<string, string> Stocks;

        /// <summary>
        /// 新聞利多利空關鍵字表
        /// </summary>
        private DataTable KeywordTable;

        /// <summary>
        /// 建立股票清單(ID+Name)
        /// </summary>
        private void GetStock()
        {
            DataTable table = new DataTable();
            string year = DateTime.Now.ToString(Constant.YEAR);

            table = DB.DoQuerySQL($"SELECT [股票代號],[股票名稱] FROM [StockDb].[dbo].[上市櫃基本資料表] WHERE [年度]='{year}'", KernelConst.DEF_DBCONNECTION_SRV1);

            Stocks = table.AsEnumerable().ToDictionary(row => row["股票代號"].ToString(), row => row["股票名稱"].ToString());
        }

        /// <summary>
        /// 確認下載內容
        /// </summary>
        /// <param name="taskInfo">任務</param>
        /// <param name="downloadedWebSourceDataList">下載清單</param>
        /// <param name="failedList">下載失敗清單</param>
        /// <returns></returns>
        public bool CheckContent(ref TaskInfo taskInfo, ref List<WebSourceData> downloadedWebSourceDataList, out List<WebSourceData> failedList)
        {
            failedList = new List<WebSourceData>();

            return failedList.Count == 0;
        }

        /// <summary>
        /// 將同股票代號之新聞合併成一htm取得所需欄位，並建新孫任務清單
        /// </summary>
        /// <param name="webSourceDataPrototypeList">原任務清單</param>
        /// <param name="cycleRange">週期</param>
        /// <param name="samples">樣本</param>
        /// <param name="extraParameter">外部參數</param>
        /// <param name="parentList">母任務清單</param>
        /// <returns>新孫任務清單</returns>
        public List<WebSourceData> GetList(List<WebSourceData> webSourceDataPrototypeList, string cycleRange, string samples, string extraParameter, List<WebSourceData> parentList)
        {
            //建立股票代號與名稱的字典
            GetStock();

            //WebSourceData 以股票代號為 Group
            var webSourceGroup = parentList.GroupBy(x =>new { x.Sample, x.Cycle }).Select(x => x).ToList();
            List<WebSourceData> resultWebSourceData = new List<WebSourceData>();
            WebSourceData newwebData;
            WebSourceData webSource = webSourceDataPrototypeList.First();

            //建新聞利多利空關鍵字表
            KeywordTable = DB.DoQuerySQL("SELECT [關鍵字],[利多利空],[強度] FROM [StockDB].[dbo].[新聞利多利空關鍵字]", KernelConst.DEF_DBCONNECTION_SRV1);

            //新增一帶有需求指定欄位之table
            DataTable table;
            string sample = string.Empty;
            string time = DateTime.Now.ToString(Constant.TIME);

            //尋覽所有股票代號Group
            foreach (var webDataGroup in webSourceGroup)
            {
                //因股票代號不同，重建一table
                table = NewADataTable();
                newwebData = new WebSourceData(webSource);

                //尋覽各股票代號Group之WebSourceData
                foreach (WebSourceData webDataItem in webDataGroup)
                {
                    sample = webDataItem.Sample;

                    string webData = Encoding.GetEncoding(webDataItem.EncodingName).GetString(webDataItem.WebContent);

                    //datatable新增一row，取得需求指定欄位資料(股票代號相同，所以建再同一個datatable)
                    ReplaceWebData(webData, webDataItem.Cycle, sample, webDataItem.URI, table);
                }

                //將datatable轉成html格式
                string result = DataTableToHtml(table);
                newwebData.WebContent = Encoding.GetEncoding(newwebData.EncodingName).GetBytes(result);

                //設定Sample(股票代號_時間) => 以免一天載三次檔案被覆蓋
                newwebData.Sample = $"{sample}_{time}";
                newwebData.Cycle = webDataGroup.Key.Cycle;
                resultWebSourceData.Add(newwebData);
            }
            return resultWebSourceData;
        }

        /// <summary>
        /// 新增一帶有需求指定欄位之table
        /// </summary>
        /// <returns>有需求指定欄位之table</returns>
        private DataTable NewADataTable()
        {
            DataTable table = new DataTable();

            //樣本欄位名稱
            List<string> ColumnName = new List<string> { "CM樣本", "CM週期", "股票名稱", "新聞標題", "資料來源", "網址", "利多分數", "利空分數" };

            foreach (var item in ColumnName)
            {
                table.Columns.Add(item);
            }

            return table;
        }

        /// <summary>
        /// 取得需求欄位之資料
        /// </summary>
        /// <param name="content">新聞網頁內容</param>
        /// <param name="cycle">新聞日期</param>
        /// <param name="sample">股票代號</param>
        /// <param name="uri">新聞網址</param>
        /// <param name="table">[新聞利多利空關鍵字]</param>
        private void ReplaceWebData(string content, string cycle, string sample, string uri, DataTable table)
        {
            string result = string.Empty;

            NewsPaserBase newsPaser = new SourceID_181001_Paser();

            DataRow row;
            row = table.NewRow();
            row["CM樣本"] = sample;
            row["CM週期"] = cycle;

            //查詢對應之股票代號之股票名稱
            string stockName = Stocks[sample];
            row["股票名稱"] = stockName;

            string title = Regex.Match(content, @"<h1.*?>(?<title>.*?)</h1>", RegexOptions.IgnoreCase).Groups["title"].Value;
            row["新聞標題"] = title;
            string source = Regex.Match(content, @"""author"".*?""name"":""(?<source>.*?)""", RegexOptions.IgnoreCase).Groups["source"].Value;
            row["資料來源"] = source;
            row["網址"] = uri;

            //計算利空利多分數
            if (newsPaser.TryGetScore(content, out int bear_score, out int bull_score, KeywordTable))
            {
                row["利多分數"] = bull_score;
                row["利空分數"] = bear_score;
            }
            table.Rows.Add(row);
        }

        /// <summary>
        /// DataTable轉成Html
        /// </summary>
        /// <param name="datatable">一股票代號帶有所有欄位資料的table</param>
        /// <returns>Html格式字串</returns>
        private string DataTableToHtml(DataTable datatable)
        {
            StringBuilder strHTMLBuilder = new StringBuilder();
            strHTMLBuilder.Append("<html><head></head><body>");
            strHTMLBuilder.Append("<table BORDER='1' STYLE='width: 450px;border-collapse:collapse;table-layout:fixed; word-wrap:break-word;' BORDERCOLOR='BLACK'>");

            strHTMLBuilder.Append("<tr>");
            foreach (DataColumn myColumn in datatable.Columns)
            {
                strHTMLBuilder.Append("<td bgColor='#104E8B' width='60'><div align='center'><font color='FFFFFF'>");
                strHTMLBuilder.Append(myColumn.ColumnName);
                strHTMLBuilder.Append("</font></div></td>");
            }
            strHTMLBuilder.Append("</tr>");

            foreach (DataRow myRow in datatable.Rows)
            {
                strHTMLBuilder.Append("<tr >");
                foreach (DataColumn myColumn in datatable.Columns)
                {
                    strHTMLBuilder.Append("<td><div align='center'>");
                    strHTMLBuilder.Append(myRow[myColumn.ColumnName].ToString());
                    strHTMLBuilder.Append("</div></td>");
                }
                strHTMLBuilder.Append("</tr>");
            }

            //Close tags.  
            strHTMLBuilder.Append("</table></body></html>");

            string Htmltext = strHTMLBuilder.ToString();

            return Htmltext;
        }
    }
}
