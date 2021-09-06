using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FundNoBusinessClass
{
    [ExportMetadata("TableName", "基金非營業日明細")]
    [Export(typeof(IDataSheet))]
    public class FundNoBusinessDetail : FundNoBusiness
    {
        public FundNoBusinessDetail() : base("基金非營業日明細_luann")
        {
        }

        public override void GetXML()
        {
            string pattern = @"<tr class=""r_blue"">[\s]*?<td.*?>(?<date>[\d]{8})</td><td.*?>(?<company>[\w]{5})</td><td.*?>(?<taxID>[\w]*?)</td><td.*?>(?<name>[\S]*?)</td>.*?</tr>";
            foreach (var data in OriginalWeb)
            {
                List<FundDto> fundDetail = new List<FundDto>();
                MatchCollection results = Regex.Matches(data.Value, pattern, RegexOptions.Singleline);
                foreach (Match result in results)
                {
                    fundDetail.Add(new FundDto
                    {
                        非營業日 = result.Groups["date"].Value,
                        公司代號 = result.Groups["company"].Value,
                        基金統編 = result.Groups["taxID"].Value,
                        基金名稱 = result.Groups["name"].Value
                    });
                }
                List<FundDto> fundResult = fundDetail.GroupBy(fund => fund.非營業日)
                                                .SelectMany(funds => {
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
                IEnumerable<XElement> fundXml = fundResult.Select(fund => new XElement("Data",
                    new XElement("非營業日", fund.非營業日),
                    new XElement("公司代號", fund.公司代號),
                    new XElement("基金統編", fund.基金統編),
                    new XElement("基金名稱", fund.基金名稱),
                    new XElement("排序", fund.排序)
                    ));
                XDocument document = new XDocument(new XElement("Root", fundXml));
                string fileName = Path.Combine(CreatDirectory(DateTime.Now.Year.ToString()), "基金非營業日明細.xml");
                SaveXml(document, fileName);
            }
        }

        public override void WriteDatabase(SqlConnection SQLConnection)
        {
            throw new NotImplementedException();
        }
    }
}
