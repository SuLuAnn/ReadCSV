using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DayFuturesClass
{
    public abstract class DayFutures : DataBaseTable
    {
        public const string DAY_FUTURES = "https://www.taifex.com.tw/cht/3/dlFutDataDown";
        public List<string> OriginalWeb { get; set; }

        public enum Futures : int
        {
            TRANSACTION_DATE = 0,
            CONTRACT = 1,
            EXPIRY_MONTH = 2,
            OPENING_PRICE = 3,
            HIGHEST_PRICE = 4,
            LOWEST_PRICE = 5,
            CLOSING_PRICE = 6,
            TRADING_HOURS = 17
        }
        public DayFutures(string dataTableName) : base(dataTableName)
        {
            OriginalWeb = new List<string>();
        }

        public override void GetWebs()
        {
            MultipartFormDataContent headers = new MultipartFormDataContent();
            headers.Add(new StringContent("1"), "down_type");
            headers.Add(new StringContent("all"), "commodity_id");
            headers.Add(new StringContent(DateTime.Now.ToString("yyyy/MM/dd")), "queryStartDate");
            headers.Add(new StringContent(DateTime.Now.ToString("yyyy/MM/dd")), "queryEndDate");
            string web = HtmlPost(DAY_FUTURES, headers, "BIG5");
            SaveFile(web, $"{DateTime.Now.Year}.csv");
            OriginalWeb.Add(web);
        }
    }
}
