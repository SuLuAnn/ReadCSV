using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace M157_DownloadExtent_TEST
{
    /// <summary>
    /// 計算新聞利空利多分數
    /// </summary>
    public abstract class NewsPaserBase
    {
        /// <summary>
        /// 取得新聞標題 及 內容
        /// </summary>
        /// <param name="HtmlContents">新聞網頁來源內容整個網頁</param>
        /// <returns></returns>
        protected abstract List<string> GetArticle(string HtmlContents);

        /// <summary>
        /// 判斷是否拿到利空利多的分數
        /// </summary>
        /// <param name="WebContentStr">新聞網頁來源內容(整個網頁)</param>
        /// <param name="BearScore">利空分數</param>
        /// <param name="BullScore">利多分數</param>
        /// <param name="keywordTable">[新聞利多利空關鍵字]</param>
        /// <returns></returns>
        public bool TryGetScore(string WebContentStr, out int BearScore, out int BullScore, DataTable keywordTable)
        {
            List<string> article;
            string content = WebContentStr;
            bool is_ok = false;
            BearScore = 0;
            BullScore = 0;

            if (content != null)
            {
                // 取得新聞標題 & 內容
                article = GetArticle(content);

                // 計算分數
                if (article.Count > 0)
                {
                    GetScore(article, out BearScore, out BullScore, keywordTable);
                    is_ok = true;
                }
            }

            return is_ok;
        }

        /// <summary>
        /// 計算新聞利空利多分數
        /// </summary>
        /// <param name="article">新聞標題 及 內容</param>
        /// <param name="BearScore">利空分數</param>
        /// <param name="BullScore">利多分數</param>
        /// <param name="keywordTable">新聞利多利空關鍵字</param>
        private void GetScore(List<string> article, out int BearScore, out int BullScore, DataTable keywordTable)
        {
            // 利多分數
            BullScore = 0;

            // 利空分數
            BearScore = 0;

            foreach (DataRow row in keywordTable.Rows)
            {
                string key_word = row["關鍵字"].ToString();
                int scor = Convert.ToInt32(row["強度"]);
                int kind = Convert.ToInt32(row["利多利空"].ToString());

                foreach (string str in article)
                {
                    MatchCollection matchs = Regex.Matches(str, key_word);
                    if (matchs.Count > 0)
                    {
                        //資料表[新聞利多利空關鍵字]的利多利空欄位以 1 表示利多 / 0 表示利空
                        if (kind == 1)
                        {
                            BullScore += scor * matchs.Count;
                        }
                        else
                        {
                            BearScore += scor * matchs.Count;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 解析網頁內容取得新聞標題 及 內容
    /// </summary>
    public class SourceID_181001_Paser : NewsPaserBase
    {
        /// <summary>
        /// 解析網頁內容取得新聞標題及內容
        /// </summary>
        /// <param name="HtmlContents">新聞網頁來源內容整個網頁</param>
        /// <returns>新聞標題 及 內容</returns>
        protected override List<string> GetArticle(string HtmlContents)
        {
            List<string> article = new List<string>();

            //取得title
            string title = Regex.Match(HtmlContents, @"<h1.*?>(?<title>.*?)</h1>", RegexOptions.IgnoreCase).Groups["title"].Value;
            article.Add(title);

            //取得內文
            string detail = string.Empty;
            Match articleMatch = Regex.Match(HtmlContents, @"<article.*?>.*?</article>", RegexOptions.IgnoreCase);
            MatchCollection paragraph = Regex.Matches(articleMatch.Value, @"<p.*?>(?<paragraph>.*?)</p>", RegexOptions.IgnoreCase);

            //LINQ取新聞實際的內容
            var clsList = from Match m in paragraph
                          select new
                          {
                              m.Groups["paragraph"].Value
                          };

            article.Add(clsList.Aggregate(string.Empty, (c, n) => c + n));

            return article;
        }
    }
}
