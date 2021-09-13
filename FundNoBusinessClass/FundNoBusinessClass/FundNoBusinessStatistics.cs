using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FundNoBusinessClass
{
    /// <summary>
    /// 產出基金非營業日統計的類別
    /// </summary>
    [ExportMetadata("TableName", "基金非營業日統計")]
    [Export(typeof(IDataSheet))]
    public class FundNoBusinessStatistics : FundNoBusiness
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public FundNoBusinessStatistics() : base("基金非營業日統計_luann")
        {
        }

        /// <summary>
        /// 取得xml中介資料
        /// </summary>
        public override void GetXML()
        {
            string pattern = @"<tr class=""r_blue"">[\s]*?<td.*?>(?<date>[\d]{8})</td><td.*?>(?<company>[\w]{5})</td><td.*?>(?<taxID>[\w]*?)</td><td.*?>(?<name>[\S]*?)</td>.*?</tr>";
            string web = GetWebPage(FUND_NO_BUSINESS_DAY);
            MatchCollection years = Regex.Matches(web, @"value=""(?<year>\d{4})"">");
            foreach (Match year in years)
            {
                string yearWord = year.Groups["year"].Value.Trim();
                string data = ReadFile($"{yearWord}.html");
                //存一年的資料之後要弄成xml
                List<FundDto> fundStatistics = new List<FundDto>();
                MatchCollection results = Regex.Matches(data, pattern, RegexOptions.Singleline);
                foreach (Match result in results)
                {
                    fundStatistics.Add(new FundDto
                    {
                        非營業日 = result.Groups["date"].Value,
                        公司代號 = result.Groups["company"].Value,
                        基金統編 = result.Groups["taxID"].Value,
                    });
                }
                //取得同非營業日同公司代號基金總數
                var fundResult = fundStatistics.GroupBy(fund => new 
                                                                                                                      {
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
                string fileName = Path.Combine(CreatDirectory(yearWord), "基金非營業日統計.xml");
                SaveXml(document, fileName);
                //所有xml存成一個，之後要匯入資料庫
            }
        }

        /// <summary>
        /// 用xml中介資料更新資料庫
        /// </summary>
        public override void WriteDatabase()
        {
            XDocument TotalDocument = GetTotalXml("基金非營業日統計");
            DataSet dataSet = new DataSet();
            //讀取xml
            dataSet.ReadXml(TotalDocument.CreateReader());
            //dataSet.Tables[0].Rows[1]["基金總數"] = 0;//測試用
            string sqlCommand = @"MERGE [dbo].[基金非營業日統計_luann] AS A USING @sourceTable AS B ON A.[非營業日] = B.[非營業日] 
                                                           AND A.[公司代號] = B.[公司代號] WHEN MATCHED AND A.[基金總數] <> B.基金總數 THEN 
                                                           UPDATE SET[基金總數] = B.基金總數,[MTIME] = (datediff(second, '1970-01-01', getutcdate())) WHEN NOT
                                                           MATCHED BY TARGET THEN INSERT([非營業日],[公司代號],[基金總數]) VALUES(B.非營業日,B.公司代號, 
                                                           B.基金總數) WHEN NOT MATCHED BY SOURCE THEN DELETE; ";
            SqlCommand command = new SqlCommand(sqlCommand, SQLConnection);
            SqlParameter tableParameter = command.Parameters.AddWithValue("@sourceTable", dataSet.Tables[0]);
            tableParameter.SqlDbType = SqlDbType.Structured;
            tableParameter.TypeName = "基金非營業日統計TableType";
            SQLConnection.Open();
            command.ExecuteNonQuery();
            SQLConnection.Close();
        }
    }
}
