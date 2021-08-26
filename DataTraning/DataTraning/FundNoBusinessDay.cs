using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataTraning
{
    public class FundNoBusinessDay
    {
        private TextBox text;
        private StockDBEntities StockDB;
        public FundNoBusinessDay(StockDBEntities stockDB,TextBox text)
        {
            StockDB = stockDB;
            this.text = text;
        }

        public void AddReviseFundDetail()
        {
            SpiltData();
        }

        public IEnumerable<Dictionary<string, string>> GetHeader()
        {
            string web = Global.GetWebPage(Global.FUND_NO_BUSINESS_DAY);
            MatchCollection headers = Regex.Matches(web, @"id=""(?<id>__.*?)"" value=""(?<value>.*?)""");
            MatchCollection years = Regex.Matches(web, @"value=""(?<year>\d{4})"">");
            
            foreach (Match year in years)
            {
                var header = new Dictionary<string, string>();
                header.Add("__EVENTTARGET", "");
                header.Add("__EVENTARGUMENT", "");
                header.Add("__LASTFOCUS", "");
                foreach (Match keyPair in headers)
                {
                    header.Add(keyPair.Groups["id"].Value, keyPair.Groups["value"].Value);
                }
                header.Add("ctl00$ContentPlaceHolder1$ddlQ_Year", year.Groups["year"].Value);
                header.Add("ctl00$ContentPlaceHolder1$ddlQ_Comid", "");
                header.Add("ctl00$ContentPlaceHolder1$ddlQ_Fund", "");
                header.Add("ctl00$ContentPlaceHolder1$ddlQ_PAGESIZE", "5");
                header.Add("ctl00$ContentPlaceHolder1$BtnQuery", "查詢");
                yield return header;
            }
        }

        public IEnumerable<string> GetYearData()
        {
            foreach (Dictionary<string, string> header in GetHeader())
            {
                yield return Global.HtmlPost(Global.FUND_NO_BUSINESS_DAY, header).Result;
            }
        }

        public void SpiltData()
        {
            text.Text = GetYearData().ToList().First();//Global.ToJson(GetHeader().First());
            //foreach (string data in GetYearData())
            //{

            //}
        }
    }
}
