using DownloadSystem.DataLibs.ExtensionLibs;
using DownloadSystem.DataLibs.Interface;
using DownloadSystem.DataLibs.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace P382_DownloadExtension
{
    /// <summary>
    /// SourceID 382608的子任務，取得股票代號及期別的頁面
    /// </summary>
    [Export(typeof(ITaskExtension))]
    [ExportMetadata("Name", "P382_DownloadExtension.SourceID_382608_股票代號與期別")] //TaskInfo.ExensionName
    [ExportMetadata("ContainsDatabaseOperation", false)]//註明是否包含讀取資料庫內容的操作
    [ExportMetadata("ContainsInternetOperation", false)]//註明是否有額外存取網路資源的操作
    public class SourceID_382608_股票代號與期別 : ITaskExtension
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
            List<WebSourceData> faildDatas = downloadedWebSourceDataList.Where(webSource => webSource.WebContent == null
                                                                                                                                                                                     || webSource.WebContent.Count() == 0)
                                                                                                                                            .ToList();
            List<string> failUrls = faildDatas.Select(faildWebSource => faildWebSource.URI).ToList();
            downloadedWebSourceDataList.RemoveAll(webSource => failUrls.Contains(webSource.URI));
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
            List<string> securitiesCodes = GetSecuritiesCode(parentList.First().GetWebContentString());

            foreach (string code in securitiesCodes)
            {
                WebSourceData newWebSource = new WebSourceData(originalWebSource);
                newWebSource.URI = string.Format(newWebSource.URI, code);
                webSourceDatas.Add(newWebSource);
            }

            List<WebSourceData> webSourceDatas2 = webSourceDatas.Take(1).ToList();

            return webSourceDatas2;
        }

        /// <summary>
        /// 從網頁內文取出所有有價證券代號不含最後的英文字母且不重複
        /// </summary>
        /// <param name="webContent">網頁內文</param>
        /// <returns>不重複的有價證券代號</returns>
        public List<string> GetSecuritiesCode(string webContent)
        {
            string pattern = $@"bgcolor=#FAFAD2>(?<ID>\d+)[A-Z]*?</td>";
            return Regex.Matches(webContent, pattern).Cast<Match>().Select(match => match.Groups["ID"].Value).Distinct().ToList();
        }
    }
}
