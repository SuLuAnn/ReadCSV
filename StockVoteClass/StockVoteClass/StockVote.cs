using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StockVoteClass
{
    public abstract class StockVote : DataBaseTable
    {
        public const string STOCK_VOTE = "https://www.stockvote.com.tw/evote/login/index/meetingInfoMore.html";
        public const string STOCK_VOTE_PAGE = "https://www.stockvote.com.tw/evote/login/index/meetingInfoMore.html?stockName=&orderType=0&stockId=&searchType=0&meetingDate=&meetinfo=";
        public Dictionary<int, string> OriginalWeb { get; set; }
        public StockVote(string dataTableName) : base(dataTableName)
        {
            OriginalWeb = new Dictionary<int, string>();
        }

        public override void GetWebs()
        {
            int page = GetPageNumber();
            for (int i = 1; i <= page; i++)
            {
                string stockVotePage = GetWebPage($"{STOCK_VOTE_PAGE}{i}");
                string path = Path.Combine(CreatDirectory(DateTime.Today.ToString("yyyyMMdd")), $"{i}.html");
                SaveFile(stockVotePage, path);
                OriginalWeb.Add(i, stockVotePage);
            }
        }

        public int GetPageNumber()
        {
            string web = GetWebPage(STOCK_VOTE);
            int.TryParse(Regex.Match(web, @"頁次：1/(?<lastPage>\d+?)<td>").Groups["lastPage"].Value, out int page);
            return page;
        }

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
