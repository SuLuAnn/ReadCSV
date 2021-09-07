using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FundNoBusinessClass
{
    public abstract class FundNoBusiness : DataBaseTable
    {
        public const string FUND_NO_BUSINESS_DAY = "https://www.sitca.org.tw/ROC/Industry/IN2107.aspx?pid=IN2213_03";
        public Dictionary<string, string> OriginalWeb { get; set; }
        public XDocument TotalDocument { get; set; }
        public enum Fund : int
        {
            NO_BUSINESS_DAY,
            COMPANY_ID,
            TAX_ID,
            FUND_NAME
        }
        public FundNoBusiness(string dataTableName) : base(dataTableName)
        {
            OriginalWeb = new Dictionary<string, string>();
            TotalDocument = new XDocument(new XElement("Root"));
        }

        public override void GetWebs()
        {
            OriginalWeb.Clear();
            string web = GetWebPage(FUND_NO_BUSINESS_DAY);
            MatchCollection headers = Regex.Matches(web, @"id=""(?<id>__.*?)"" value=""(?<value>.*?)""");
            MatchCollection years = Regex.Matches(web, @"value=""(?<year>\d{4})"">");
            foreach (Match year in years)
            {
                string yearWord = year.Groups["year"].Value.Trim();
                MultipartFormDataContent header = new MultipartFormDataContent();
                foreach (Match keyPair in headers)
                {
                    header.Add(new StringContent(keyPair.Groups["value"].Value.Trim()),keyPair.Groups["id"].Value.Trim());
                }
                header.Add(new StringContent(yearWord), "ctl00$ContentPlaceHolder1$ddlQ_Year");
                header.Add(new StringContent(string.Empty), "ctl00$ContentPlaceHolder1$ddlQ_Comid");
                header.Add(new StringContent(string.Empty), "ctl00$ContentPlaceHolder1$ddlQ_Fund");
                header.Add(new StringContent(string.Empty), "ctl00$ContentPlaceHolder1$ddlQ_PAGESIZE");
                header.Add(new StringContent("查詢"), "ctl00$ContentPlaceHolder1$BtnQuery");
                string data = HtmlPost(FUND_NO_BUSINESS_DAY, header, "UTF-8");
                SaveFile(data, $"{yearWord}.html");
                OriginalWeb.Add(yearWord, data);
            }
        }
    }
}
