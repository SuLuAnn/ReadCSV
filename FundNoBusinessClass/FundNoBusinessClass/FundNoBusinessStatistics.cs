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
    [ExportMetadata("TableName", "基金非營業日統計")]
    [Export(typeof(IDataSheet))]
    public class FundNoBusinessStatistics : FundNoBusiness
    {
        public FundNoBusinessStatistics() : base("基金非營業日統計_luann")
        {
        }

        public override void GetXML()
        {
            string pattern = @"<tr class=""r_blue"">[\s]*?<td.*?>(?<date>[\d]{8})</td><td.*?>(?<company>[\w]{5})</td><td.*?>(?<taxID>[\w]*?)</td><td.*?>(?<name>[\S]*?)</td>.*?</tr>";
            foreach (var data in OriginalWeb)
            {
                List<FundDto> fundStatistics = new List<FundDto>();
                MatchCollection results = Regex.Matches(data.Value, pattern, RegexOptions.Singleline);
                foreach (Match result in results)
                {
                    fundStatistics.Add(new FundDto
                    {
                        非營業日 = result.Groups["date"].Value,
                        公司代號 = result.Groups["company"].Value,
                        基金統編 = result.Groups["taxID"].Value,
                    });
                }
                var fundResult = fundStatistics.GroupBy(fund => new {
                                                                                                                          fund.非營業日,
                                                                                                                          fund.公司代號
                                                                                                                      },
                                                                                                        (key, value) => new 
                                                                                                        {
                                                                                                            公司代號 = key.公司代號,
                                                                                                            非營業日 = key.非營業日,
                                                                                                            基金總數 = (byte)value.Count()
                                                                                                        });
                IEnumerable<XElement> fundXml = fundResult.Select(fund => new XElement("Data",
                    new XElement("非營業日", fund.非營業日),
                    new XElement("公司代號", fund.公司代號),
                    new XElement("基金總數", fund.基金總數)
                    ));
                XDocument document = new XDocument(new XElement("Root", fundXml));
                string fileName = Path.Combine(CreatDirectory(DateTime.Now.Year.ToString()), "基金非營業日統計.xml");
                SaveXml(document, fileName);
            }
        }

        public override void WriteDatabase(SqlConnection SQLConnection)
        {
            throw new NotImplementedException();
        }
    }
}
