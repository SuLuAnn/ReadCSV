using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StockVoteClass
{
    public abstract class StockVote : DataBaseTable
    {
        private const string Url = "https://www.stockvote.com.tw/evote/login/index/meetingInfoMore.html";
        public StockVote(string dataTableName) : base(dataTableName)
        {
        }
        public override List<OriginalWeb> GetWebs()
        {
            List<OriginalWeb> originalWebs = new List<OriginalWeb>();
            GetWebPage(Url);
            return originalWebs;
        }

    }
}
