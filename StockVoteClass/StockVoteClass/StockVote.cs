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
    public abstract class StockVote : DataBaseTable
    {
        /// <summary>
        /// stockvote電子投票首頁
        /// </summary>
        public const string STOCK_VOTE = "https://www.stockvote.com.tw/evote/login/index/meetingInfoMore.html";

        /// <summary>
        /// stockvote電子投票取得每頁內容的網址
        /// </summary>
        public const string STOCK_VOTE_PAGE = "https://www.stockvote.com.tw/evote/login/index/meetingInfoMore.html?stockName=&orderType=0&stockId=&searchType=0&meetingDate=&meetinfo=";

        /// <summary>
        /// 所有html組成的xml檔
        /// </summary>
        public XDocument TotalDocument { get; set; }

        /// <summary>
        /// key為頁數，value為html原始資料
        /// </summary>
        public Dictionary<int, string> OriginalWeb { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="dataTableName">該類別要產出的資料表名</param>
        public StockVote(string dataTableName) : base(dataTableName)
        {
            OriginalWeb = new Dictionary<int, string>();
            TotalDocument = new XDocument(new XElement("Root"));
        }

        /// <summary>
        /// 取得網站原始資料
        /// </summary>
        public override void GetWebs()
        {
            OriginalWeb.Clear();
            //取得總共有幾頁
            int page = GetPageNumber();
            for (int i = 1; i <= page; i++)
            {
                //取每頁html內容
                string stockVotePage = GetWebPage($"{STOCK_VOTE_PAGE}{i}");
                string path = Path.Combine(CreatDirectory(DateTime.Today.ToString("yyyyMMdd")), $"{i}.html");
                SaveFile(stockVotePage, path);
                OriginalWeb.Add(i, stockVotePage);
            }
        }

        /// <summary>
        /// 取得總共有幾頁
        /// </summary>
        /// <returns>頁數</returns>
        public int GetPageNumber()
        {
            string web = GetWebPage(STOCK_VOTE);
            int.TryParse(Regex.Match(web, @"頁次：1/(?<lastPage>\d+?)<td>").Groups["lastPage"].Value, out int page);
            return page;
        }

        /// <summary>
        /// 將民國改西元
        /// </summary>
        /// <param name="year">民國</param>
        /// <returns>西元</returns>
        public string ChangeYear(string year)
        {
            if (int.TryParse(year, out int result))
            {
                year = (result + 19110000).ToString();
            }
            return year;
        }
    }
}
