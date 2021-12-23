using CMoney.Kernel;
using DownloadSystem.DataLibs.ExtensionLibs;
using DownloadSystem.DataLibs.Interface;
using DownloadSystem.DataLibs.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P382_DownloadExtension
{
    /// <summary>
    /// SourceID 382609的下載擴充，取[StockDB_US].[dbo].[代號總表]的股票代號當樣本
    /// </summary>
    [Export(typeof(ITaskExtension))]
    [ExportMetadata("Name", "P382_DownloadExtension.SourceID_382609")] //TaskInfo.ExensionName
    [ExportMetadata("ContainsDatabaseOperation", true)]//註明是否包含讀取資料庫內容的操作
    [ExportMetadata("ContainsInternetOperation", false)]//註明是否有額外存取網路資源的操作
    public class SourceID_382609 : ITaskExtension
    {
        /// <summary>
        /// 排除錯誤下載結果、提供下載器需要處理的下載資訊
        /// </summary>
        /// <param name="taskInfo">TaskInfo資料表的內容</param>
        /// <param name="downloadedWebSourceDataList">下載結果</param>
        /// <param name="failedList">錯誤下載結果</param>
        /// <returns>是否下載結果都正常</returns>
        public bool CheckContent(ref TaskInfo taskInfo, ref List<WebSourceData> downloadedWebSourceDataList, out List<WebSourceData> failedList)
        {
            failedList = new List<WebSourceData>();
            string worstWord = "Symbol not exists";
            List<WebSourceData> notExistIds = downloadedWebSourceDataList.Where(webSource => webSource.GetWebContentString().Contains(worstWord))
                                                                                                                                            .ToList();
            List<WebSourceData> faildDatas = downloadedWebSourceDataList.Where(webSource => webSource.WebContent == null
                                                                                                                                                                                     || webSource.WebContent.Count() == 0)
                                                                                                                                            .ToList();
            notExistIds.AddRange(faildDatas);
            List<string> failUrls = notExistIds.Select(faildWebSource => faildWebSource.Sample).ToList();
            downloadedWebSourceDataList.RemoveAll(webSource => failUrls.Contains(webSource.Sample));
            failedList = faildDatas;
            return !failedList.Any();
        }

        /// <summary>
        /// 解析母任務結果、建立新的下載資料
        /// </summary>
        /// <param name="webSourceDataPrototypeList">該TaskID using的所有WebSourceData</param>
        /// <param name="cycleRange">傳入下載週期</param>
        /// <param name="samples">傳入下載樣本</param>
        /// <param name="extraParameter">額外參數</param>
        /// <param name="parentList">帶入母子任務的母任務下載結果</param>
        /// <returns>子任務的WebSourceData</returns>
        public List<WebSourceData> GetList(List<WebSourceData> webSourceDataPrototypeList, string cycleRange, string samples, string extraParameter, List<WebSourceData> parentList = null)
        {
            List<WebSourceData> webSourceDatas = new List<WebSourceData>();
            WebSourceData originalWebSource = webSourceDataPrototypeList.First();
            originalWebSource.Cycle = DateTime.Today.ToString("yyyyMM");
            Dictionary<string, string> stockIds = GetStockIDs();

            foreach (KeyValuePair<string, string> stockId in stockIds)
            {
                WebSourceData newWebSource = new WebSourceData(originalWebSource);
                newWebSource.URI = string.Format(newWebSource.URI, stockId.Value);
                newWebSource.Sample = stockId.Key;
                webSourceDatas.Add(newWebSource);
            }

            List<WebSourceData> webSourceDatas2 = webSourceDatas.Take(1).ToList();
            return webSourceDatas2;
        }

        /// <summary>
        /// 取得A開頭有^符號的所有股票代號對應表
        /// </summary>
        /// <returns>股票代號對應表</returns>
        private Dictionary<string, string> GetStockIDs()
        {
            string id = "代號";
            string exchangeId = "交易所樣本代號";
            string command = $@"SELECT [{id}],[{exchangeId}] FROM [StockDB_US].[dbo].[代號總表] WHERE [{exchangeId}] LIKE 'A%^%'";
            return DB.DoQuerySQL(command, StocksUS.Util.CONN_STR_STOCKSDB_US)
                                                                                       .AsEnumerable()
                                                                                       .ToDictionary(row => row.Field<string>(id), 
                                                                                                                 row => row.Field<string>(exchangeId));
        }
    }
}
