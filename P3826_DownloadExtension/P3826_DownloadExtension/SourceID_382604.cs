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
    /// 為Sourceid382604做的擴充
    /// </summary>
    [Export(typeof(ITaskExtension))]
    [ExportMetadata("Name", "P3826_DownloadExtension.SourceID_382604")] //TaskInfo.ExensionName
    [ExportMetadata("ContainsDatabaseOperation", false)]//註明是否包含讀取資料庫內容的操作
    [ExportMetadata("ContainsInternetOperation", true)]//註明是否有額外存取網路資源的操作
    public class SourceID_382604 : ITaskExtension
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
            //如果不是BIG5而是UTF-8的話系統維護中會是亂碼，但如果是BIG5正式資料會是亂碼，所以這邊才把確認是否出錯的編碼寫死
            List<WebSourceData> faildDatas = downloadedWebSourceDataList.Where(webSource => webSource.WebContent == null
                                                                                                                                                                                     || webSource.WebContent.Length == 0
                                                                                                                                                                                     || Encoding.GetEncoding("BIG5").GetString(webSource.WebContent).Contains("系統維護中"))
                                                                                                                                            .ToList();
            downloadedWebSourceDataList.RemoveAll(webSource => faildDatas.Select(faildWebSource => faildWebSource.Cycle).Contains(webSource.Cycle));
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
            List<WebSourceData> webSourceDatas = new List<WebSourceData>();
            //預設為今年
            List<int> cycleList = new List<int> { DateTime.Now.Year };
            //如果有傳入週期，則將傳入的週期取出
            if (!string.IsNullOrEmpty(cycleRange))
            {
                cycleList = GetCycles(cycleRange);
            }
            //這個下載任務WebSourceData設定，設定只有一個
            WebSourceData originalWebSource = webSourceDataPrototypeList.FirstOrDefault();
            //取得母任務結果
            string parentWebContent = Encoding.GetEncoding(originalWebSource.EncodingName).GetString(parentList.FirstOrDefault().WebContent);
            //從母任務取得post所需的data
            string pattern = @"VIEWSTATE"" value=""(?<viewState>.*?)"".*?VIEWSTATEGENERATOR"" value=""(?<viewStateGenerator>.*?)"".*?EVENTVALIDATION"" value=""(?<eventValidation>.*?)""";
            Match postData = Regex.Match(parentWebContent, pattern, RegexOptions.Singleline);
            //取得的post data要轉成url字串編碼
            string viewState = HttpUtility.UrlEncode(postData.Groups["viewState"].Value.Trim());
            string viewStateGenerator = postData.Groups["viewStateGenerator"].Value.Trim();
            string eventValidation = HttpUtility.UrlEncode(postData.Groups["eventValidation"].Value.Trim());
            foreach (int cycle in cycleList)
            {
                //用原始的WebSourceData藉由拼接post data及年度來取得所有子任務的WebSourceData
                WebSourceData newWebSource = new WebSourceData(originalWebSource);
                newWebSource.PostData = string.Format(newWebSource.PostData, viewState, viewStateGenerator, eventValidation, cycle);
                newWebSource.Cycle = cycle.ToString();
                webSourceDatas.Add(newWebSource);
            }
            MailInfo mail = new MailInfo("P382");
            mail.sendMail($"{originalWebSource.ID}蘇柔安測試寄信用", $"下載完成", "下載關鍵字錯誤");
            return webSourceDatas;
        }

        /// <summary>
        /// 取的週期
        /// </summary>
        /// <param name="cycleRange">週期期間的字串</param>
        /// <returns>所有週期結果</returns>
        public List<int> GetCycles(string cycleRange)
        {
            //起始日在第0個
            int startDay = 0;
            //結束日如果存在在第1個
            int endDay = 1;
            List<int> cycleList = new List<int>();
            string[] cycleDays = cycleRange.Split('-');
            if (int.TryParse(cycleDays[startDay], out int startCycleDay))
            {
                //大於1代表輸入週期是一個區間，而非只有一個
                if (cycleDays.Count() > 1 && int.TryParse(cycleDays[endDay], out int endCycleDay))
                {
                    for (int i = startCycleDay; i <= endCycleDay; i++)
                    {
                        cycleList.Add(i);
                    }
                }
                else
                {
                    cycleList.Add(startCycleDay);
                }
            }
            return cycleList;
        }
    }
}
