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
            foreach (var fund in GetRank())
            {
                基金非營業日明細_luann data = StockDB.基金非營業日明細_luann.SingleOrDefault(dataFund => dataFund.非營業日 == fund.非營業日 && dataFund.基金統編 == fund.基金統編);
                if (data == null)
                {
                    StockDB.基金非營業日明細_luann.Add(data);
                }
                else
                {
                    if (data.公司代號 != fund.公司代號 || data.基金名稱 != fund.基金名稱 || data.排序 != fund.排序)
                    {
                        data.公司代號 = fund.公司代號;
                        data.基金名稱 = fund.基金名稱;
                        data.排序 = fund.排序;
                        data.MTIME = DateTimeOffset.Now.ToUnixTimeSeconds();
                    }
                }
            }
            StockDB.SaveChanges();
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
                    header.Add(keyPair.Groups["id"].Value.Trim(), keyPair.Groups["value"].Value.Trim());
                }
                header.Add("ctl00$ContentPlaceHolder1$ddlQ_Year", year.Groups["year"].Value.Trim());
                header.Add("ctl00$ContentPlaceHolder1$ddlQ_Comid", "");
                header.Add("ctl00$ContentPlaceHolder1$ddlQ_Fund", "");
                header.Add("ctl00$ContentPlaceHolder1$ddlQ_PAGESIZE", "");
                header.Add("ctl00$ContentPlaceHolder1$BtnQuery", "查詢");
                yield return header;
            }
        }

        public IEnumerable<string> GetYearData()
        {
            foreach (Dictionary<string, string> header in GetHeader())
            {
                yield return Global.HtmlPost(Global.FUND_NO_BUSINESS_DAY, header);
            }
        }

        public IEnumerable<基金非營業日明細_luann> SpiltData()
        {
            foreach (string data in GetYearData())
            {
                MatchCollection results = Regex.Matches(data, @"<tr class=""r_blue"">(?<result>.*?)</tr>",RegexOptions.Singleline);
                foreach (Match result in results)
                {
                    MatchCollection items = Regex.Matches(result.Groups["result"].Value, @">(.*?)</td>");
                    yield return new 基金非營業日明細_luann
                    {
                        非營業日 = items[(int)Global.Fund.NO_BUSINESS_DAY].Groups[1].Value,
                        公司代號 = items[(int)Global.Fund.COMPANY_ID].Groups[1].Value,
                        基金統編 = items[(int)Global.Fund.TAX_ID].Groups[1].Value,
                        基金名稱 = items[(int)Global.Fund.FUND_NAME].Groups[1].Value
                    };
                }
            }
        }
        public IEnumerable<基金非營業日明細_luann> GetRank()
        {
            return SpiltData().GroupBy(fund => fund.非營業日)
                               .SelectMany(funds => funds.GroupBy(fund => fund.基金名稱.Length)
                                                                                    .OrderByDescending(fund => fund.Key)
                                                                                    .SelectMany((fund, index) =>
                                                                                                                                        {
                                                                                                                                            fund.ToList().ForEach(item => item.排序 = (byte)(index + 1));
                                                                                                                                            return funds;
                                                                                                                                            }));
        }
    }
}
