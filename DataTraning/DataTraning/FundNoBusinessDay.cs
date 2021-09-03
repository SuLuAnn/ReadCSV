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
        private StockDBEntities StockDB;
        public FundNoBusinessDay(StockDBEntities stockDB)
        {
            StockDB = stockDB;
        }

        public void AddReviseFundDetail()
        {
            List<基金非營業日明細_luann> funds = SpiltDetailData();
            funds.GroupBy(fund => fund.非營業日.Substring(0, 4)).ToList().ForEach(fund => Global.SaveCsv(fund.ToList(), $"{fund.Key}_基金非營業日明細.csv"));
            StockDB.基金非營業日明細_luann.AddRange(funds);
            //foreach (var fund in funds)
            //{
            //    基金非營業日明細_luann data = StockDB.基金非營業日明細_luann.SingleOrDefault(dataFund => dataFund.非營業日 == fund.非營業日 && dataFund.基金統編 == fund.基金統編);
            //    if (data == null)
            //    {
            //        StockDB.基金非營業日明細_luann.Add(fund);
            //    }
            //    else
            //    {
            //        if (data.公司代號 != fund.公司代號 || data.基金名稱 != fund.基金名稱 || data.排序 != fund.排序)
            //        {
            //            data.公司代號 = fund.公司代號;
            //            data.基金名稱 = fund.基金名稱;
            //            data.排序 = fund.排序;
            //            data.MTIME = DateTimeOffset.Now.ToUnixTimeSeconds();
            //        }
            //    }
        //}
            StockDB.SaveChanges();
        }

        public IEnumerable<Dictionary<string, string>> GetHeader()
        {
            string web = Global.GetWebPage(Constants.FUND_NO_BUSINESS_DAY);
            if (web == null)
            {
                yield return null;
            }
            MatchCollection headers = Regex.Matches(web, @"id=""(?<id>__.*?)"" value=""(?<value>.*?)""");
            MatchCollection years = Regex.Matches(web, @"value=""(?<year>\d{4})"">");
            foreach (Match year in years)
            {
                Dictionary<string, string> header = new Dictionary<string, string>();
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
            IEnumerable<Dictionary<string, string>> headers = GetHeader();
            if (headers == null)
            {
                yield return null;
            }
            foreach (Dictionary<string, string> header in headers)
            {
                string data = Global.HtmlPost(Constants.FUND_NO_BUSINESS_DAY, header, "UTF-8");
                Global.SaveFile(data, $"{header["ctl00$ContentPlaceHolder1$ddlQ_Year"]}.html");
                yield return data;
            }
        }

        public List<基金非營業日明細_luann> SpiltDetailData()
        {
            List<基金非營業日明細_luann> fundDetail = new List<基金非營業日明細_luann>();
            foreach (string data in GetYearData())
            {
                MatchCollection results = Regex.Matches(data, @"<tr class=""r_blue"">[\s]*?<td.*?>(?<date>[\d]{8})</td><td.*?>(?<company>[\w]{5})</td><td.*?>(?<taxID>[\w]*?)</td><td.*?>(?<name>[\S]*?)</td>.*?</tr>", RegexOptions.Singleline);
                foreach (Match result in results)
                {
                    fundDetail.Add( new 基金非營業日明細_luann
                    {
                        非營業日 = result.Groups["date"].Value,
                        公司代號 = result.Groups["company"].Value,
                        基金統編 = result.Groups["taxID"].Value,
                        基金名稱 = result.Groups["name"].Value
                    });
                }
            }
            return fundDetail.GroupBy(fund => fund.非營業日)
                                                .SelectMany(funds =>{
                                                    int maxLength = 0;
                                                    byte rank = 0;
                                                    funds.OrderByDescending(fund => fund.基金名稱.Length).ToList().ForEach(fund => {
                                                        if (fund.基金名稱.Length != maxLength)
                                                        {
                                                            maxLength = fund.基金名稱.Length;
                                                            rank++;
                                                        }
                                                        fund.排序 = rank;
                                                    });
                                                    return funds;
                                                }).ToList();
        }

        public List<FundStatisticDto> SpiltStatisticData()
        {
            List<FundStatisticDto> fund = new List<FundStatisticDto>();
            foreach (string data in GetYearData())
            {
                MatchCollection results = Regex.Matches(data, @"<tr class=""r_blue"">(?<result>.*?)</tr>", RegexOptions.Singleline);
                foreach (Match result in results)
                {
                    MatchCollection fundData = Regex.Matches(result.Groups["result"].Value, @">(?<detail>.*?)</td>");
                    fund.Add( new FundStatisticDto
                    {
                        非營業日 = fundData[(int)Enums.Fund.NO_BUSINESS_DAY].Groups["detail"].Value,
                        公司代號 = fundData[(int)Enums.Fund.COMPANY_ID].Groups["detail"].Value,
                        基金統編 = fundData[(int)Enums.Fund.TAX_ID].Groups["detail"].Value,
                    });
                }
            }
            return fund;
        }
        public IEnumerable<基金非營業日統計_luann> GetStatistic() 
        {
            return SpiltStatisticData().GroupBy(data => new  {
                                                                                                            data.非營業日,
                                                                                                            data.公司代號
                                                                                                        },
                                                                                                        (key, value) => new 基金非營業日統計_luann
                                                                                                        {
                                                                                                            公司代號 = key.公司代號,
                                                                                                            非營業日 = key.非營業日,
                                                                                                            基金總數 = (byte)value.Count()
                                                                                                        });
        }
        public void AddReviseFundStatistic()
        {
            List<基金非營業日統計_luann> funds = GetStatistic().ToList();
            funds.GroupBy(fund => fund.非營業日.Substring(0, 4)).ToList().ForEach(fund => Global.SaveCsv(fund.ToList(), $"{fund.Key}_基金非營業日統計.csv"));
            StockDB.基金非營業日統計_luann.AddRange(funds);
            //foreach (var fund in GetStatistic())
            //{
            //StockDB.基金非營業日統計_luann.Add(fund);
            //基金非營業日統計_luann data = StockDB.基金非營業日統計_luann.AsEnumerable().SingleOrDefault(dataFund => dataFund.非營業日 == fund.非營業日 && dataFund.公司代號 == fund.公司代號);
            //if (data == null)
            //{

            //}
            //else
            //{
            //    if (data.基金總數 != fund.基金總數)
            //    {
            //        data.基金總數 = fund.基金總數;
            //        data.MTIME = DateTimeOffset.Now.ToUnixTimeSeconds();
            //    }
            //}
            //}
            StockDB.SaveChanges();
        }
    }
}
