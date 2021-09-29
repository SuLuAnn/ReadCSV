using DownloadSystem.DataLibs.Interface;
using DownloadSystem.DataLibs.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using DownloadSystem.DataLibs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using MailForHandler;

namespace P3826_DownloadExtension
{
    /// <summary>
    /// 為Sourceid382605做的擴充
    /// </summary>
    [Export(typeof(ITaskExtension))]
    [ExportMetadata("Name", "P3826_DownloadExtension.SourceID_382606")] //TaskInfo.ExensionName
    [ExportMetadata("ContainsDatabaseOperation", false)]//註明是否包含讀取資料庫內容的操作
    [ExportMetadata("ContainsInternetOperation", true)]//註明是否有額外存取網路資源的操作
    public class SourceID_382606 : ITaskExtension
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
                                                                                                                                                                                     || webSource.WebContent.Length == 0
                                                                                                                                                                                     || Encoding.GetEncoding(webSource.EncodingName).GetString(webSource.WebContent).Contains("網頁載入時發生錯誤"))
                                                                                                                                            .ToList();
            downloadedWebSourceDataList.RemoveAll(webSource => faildDatas.Select(faildWebSource => faildWebSource.URL).Contains(webSource.URL));
            failedList = faildDatas;
            return failedList.Count() == 0;
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
            //這個下載任務WebSourceData設定，設定只有一個
            WebSourceData originalWebSource = webSourceDataPrototypeList.FirstOrDefault();
            originalWebSource.Cycle = DateTime.Today.ToString("yyyyMMdd");
            //取得母任務結果
            string parentWebContent = Encoding.GetEncoding(originalWebSource.EncodingName).GetString(parentList.FirstOrDefault().WebContent);
            string pattern = @"ptoken=(?<ptoken>.*?)"".*uniqueToken"" value=""(?<uniqueToken>.*?)"".*rendertime_"" value=""(?<rendertime>.*?)"".*(?<id>j_id.*?)""";
            Match postDatas = Regex.Match(parentWebContent, pattern, RegexOptions.Singleline);
            originalWebSource.URL = string.Format(originalWebSource.URL, postDatas.Groups["ptoken"].Value);
            originalWebSource.PostData = string.Format(originalWebSource.PostData, postDatas.Groups["uniqueToken"].Value, postDatas.Groups["rendertime"].Value, postDatas.Groups["id"].Value);
            MailInfo mail = new MailInfo("P382");
            mail.sendMail($"[測試]{originalWebSource.ID}蘇柔安測試寄信用", $"下載完成", "下載關鍵字錯誤");
            return webSourceDataPrototypeList;
        }
    }
}
