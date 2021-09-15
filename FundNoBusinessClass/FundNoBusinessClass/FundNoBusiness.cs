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

namespace FundNoBusinessClass
{
    /// <summary>
    /// 來源為中華民國證券投資信託暨顧問商業同業公會-基金非營業日的抽象類別
    /// </summary>
    public abstract class FundNoBusiness
    {
        /// <summary>
        /// 取得網站原始資料
        /// </summary>
        public static void GetWebs()
        {
            string web = GlobalFunction.GetWebPage(GlobalConst.FUND_NO_BUSINESS_DAY);
            //取得post要得form data值
            MatchCollection postDatas = Regex.Matches(web, @"id=""(?<id>__.*?)"" value=""(?<value>.*?)""");
            MatchCollection years = Regex.Matches(web, @"value=""(?<year>\d{4})"">");
            foreach (Match year in years)
            {
                string yearWord = year.Groups[GlobalConst.YEAR].Value.Trim();
                MultipartFormDataContent formData = new MultipartFormDataContent();
                foreach (Match postData in postDatas)
                {
                    formData.Add(new StringContent(postData.Groups[GlobalConst.VALUE].Value.Trim()), postData.Groups[GlobalConst.ID].Value.Trim());
                }
                formData.Add(new StringContent(yearWord), GlobalConst.POST_DATA_YEAR);
                formData.Add(new StringContent(string.Empty), GlobalConst.POST_DATA_COMID);
                formData.Add(new StringContent(string.Empty), GlobalConst.POST_DATA_FUND);
                formData.Add(new StringContent(string.Empty), GlobalConst.POST_DATA_PAGESIZE);
                formData.Add(new StringContent(GlobalConst.QUERY), GlobalConst.POST_DATA_QUERY);
                string data = GlobalFunction.HtmlPost(GlobalConst.FUND_NO_BUSINESS_DAY, formData, GlobalConst.ENCODED);
                GlobalFunction.SaveFile(data, $"{yearWord}.html");
            }
        }

        /// <summary>
        /// 取得這張表所有xml內容
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>存好的所有xml內容</returns>
        public static XDocument GetTotalXml(string tableName)
        {
            XDocument TotalDocument = new XDocument(new XElement(GlobalConst.XML_ROOT));
            string web = GlobalFunction.GetWebPage(GlobalConst.FUND_NO_BUSINESS_DAY);
            MatchCollection years = Regex.Matches(web, @"value=""(?<year>\d{4})"">");
            foreach (Match year in years)
            {
                string yearWord = year.Groups[GlobalConst.YEAR].Value.Trim();
                string path = Path.Combine(Environment.CurrentDirectory, GlobalConst.FOLDER_NAME, yearWord, $"{tableName}.xml");
                TotalDocument.Root.Add(XElement.Load(path).Elements(GlobalConst.XML_NODE_NAME));
            }
            return TotalDocument;
        }
    }
}
