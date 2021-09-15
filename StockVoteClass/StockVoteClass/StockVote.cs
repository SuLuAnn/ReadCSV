using Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StockVoteClass
{
    /// <summary>
    /// 來源為stockvote電子投票的抽象類別
    /// </summary>
    public abstract class StockVote
    {
        /// <summary>
        /// 取得網站原始資料
        /// </summary>
        public static void GetWebs()
        {
            //取得總共有幾頁
            int page = GetPageNumber();
            for (int i = 1; i <= page; i++)
            {
                //取每頁html內容
                string stockVotePage = GlobalFunction.GetWebPage($"{GlobalConst.STOCK_VOTE_PAGE}{i}");
                string path = Path.Combine(GlobalFunction.CreatDirectory(DateTime.Today.ToString(GlobalConst.DATE_FORMAT)), $"{i}.html");
                GlobalFunction.SaveFile(stockVotePage, path);
            }
        }

        /// <summary>
        /// 取得總共有幾頁
        /// </summary>
        /// <returns>頁數</returns>
        public static int GetPageNumber()
        {
            string web = GlobalFunction.GetWebPage(GlobalConst.STOCK_VOTE);
            int.TryParse(Regex.Match(web, @"頁次：\d*?/(?<lastPage>\d+?)<").Groups["lastPage"].Value, out int page);
            return page;
        }

        /// <summary>
        /// 將民國改西元
        /// </summary>
        /// <param name="year">民國</param>
        /// <returns>西元</returns>
        public static string ChangeYear(string year)
        {
            if (int.TryParse(year, out int result))
            {
                year = (result + 19110000).ToString();
            }
            return year;
        }

        /// <summary>
        /// 取得這張表所有xml內容
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>存好的所有xml內容</returns>
        public static XDocument GetTotalXml(string tableName)
        {
            XDocument TotalDocument = new XDocument(new XElement(GlobalConst.XML_ROOT));
            int page = GetPageNumber();
            for (int i = 1; i <= page; i++)
            {
                string path = Path.Combine(Environment.CurrentDirectory, GlobalConst.FOLDER_NAME, DateTime.Today.ToString($"yyyyMMdd/{tableName}"), $"{i}.xml");
                TotalDocument.Root.Add(XElement.Load(path).Elements(GlobalConst.XML_NODE_NAME));
            }
            return TotalDocument;
        }
    }
}
