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

        public IEnumerable<MultipartFormDataContent> GetHeader()
        {
            string web = Global.GetWebPage(Global.FUND_NO_BUSINESS_DAY);
            MatchCollection headers = Regex.Matches(web, @"id=""(?<id>__.*?)"" value=""(?<value>.*?)""");
            MatchCollection years = Regex.Matches(web, @"value=""(?<year>\d{4})"">");
            
            foreach (Match year in years)
            {
                var header = new MultipartFormDataContent();
                foreach (Match keyPair in headers)
                {
                    header.Add(new StringContent(keyPair.Groups["value"].Value), keyPair.Groups["id"].Value);
                }
                header.Add(new StringContent(year.Groups["year"].Value), "ctl00$ContentPlaceHolder1$ddlQ_Year");
                yield return header;
            }
        }

        public IEnumerable<string> GetYearData()
        {
            foreach (MultipartFormDataContent header in GetHeader())
            {
                yield return Global.HtmlPost(Global.FUND_NO_BUSINESS_DAY, header).Result;
            }
        }

        public void SpiltData()
        {
            text.Text = GetYearData().First();
            //foreach (string data in GetYearData())
            //{

            //}
        }
    }
}
