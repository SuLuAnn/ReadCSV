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
    /// 產出基金非營業日明細的類別
    /// </summary>
    [ExportMetadata("TableName", "基金非營業日明細")]
    [Export(typeof(IDataSheet))]
    public class FundNoBusinessDetail : FundNoBusiness
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public FundNoBusinessDetail() : base("基金非營業日明細_luann")
        {
        }

        /// <summary>
        /// 取得xml中介資料
        /// </summary>
        public override void GetXML()
        {
            string pattern = @"<tr class=""r_blue"">[\s]*?<td.*?>(?<date>[\d]{8})</td><td.*?>(?<company>[\w]{5})</td><td.*?>(?<taxID>[\w]*?)</td><td.*?>(?<name>.*?)</td>.*?</tr>";
            string web = GetWebPage(FUND_NO_BUSINESS_DAY);
            MatchCollection years = Regex.Matches(web, @"value=""(?<year>\d{4})"">");
            foreach (Match year in years)
            {
                string yearWord = year.Groups["year"].Value.Trim();
                string data = ReadFile($"{yearWord}.html");
                //存一年的資料之後要弄成xml
                List<FundDto> fundDetail = new List<FundDto>();
                MatchCollection results = Regex.Matches(data, pattern, RegexOptions.Singleline);

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
                //依基金名稱遞減排名，相同字數要同名次
                List<FundDto> fundResult = fundDetail.GroupBy(fund => fund.非營業日)
                                                .SelectMany(funds => 
                                                {
                                                    int maxLength = 0; //目前基金名稱字數
                                                    byte rank = 0; //目前累積排名
                                                    funds.OrderByDescending(fund => fund.基金名稱.Length).ToList().ForEach(fund => 
                                                    {
                                                        if (fund.基金名稱.Length != maxLength)
                                                        {
                                                            maxLength = fund.基金名稱.Length;
                                                            rank++;
                                                        }
                                                        fund.排序 = rank;
                                                    });
                                                    return funds;
                                                }).ToList();
                IEnumerable<XElement> fundXml = fundResult.Select(fund => new XElement("Data", //做成xml檔
                    new XElement("非營業日", fund.非營業日),
                    new XElement("公司代號", fund.公司代號),
                    new XElement("基金統編", fund.基金統編),
                    new XElement("基金名稱", fund.基金名稱),
                    new XElement("排序", fund.排序)
                    ));
                XDocument document = new XDocument(new XElement("Root", fundXml));
                string fileName = Path.Combine(CreatDirectory(yearWord), "基金非營業日明細.xml");
                SaveXml(document, fileName);
            }
        }

        /// <summary>
        /// 用xml中介資料更新資料庫
        /// </summary>
        public override void WriteDatabase()
        {
            XDocument TotalDocument = GetTotalXml("基金非營業日明細");
            DataSet dataSet = new DataSet();
            //讀取xml
            dataSet.ReadXml(TotalDocument.CreateReader());
            //dataSet.Tables[0].Rows[1]["基金名稱"] = "AAAAAAA";//測試用
            string sqlCommand = @"MERGE [dbo].[基金非營業日明細_luann] AS A USING @sourceTable AS B ON A.[非營業日] = B.[非營業日] 
                                                           AND A.[基金統編] = B.[基金統編] WHEN MATCHED AND (A.[公司代號] <> B.公司代號 OR A.[基金名稱] <> 
                                                           B.基金名稱 OR A.[排序] <> B.排序) THEN UPDATE SET [公司代號] = B.公司代號,[基金名稱] = B.基金名稱,[排序] 
                                                           = B.排序,[MTIME] = (datediff(second, '1970-01-01', getutcdate())) WHEN NOT MATCHED BY TARGET 
                                                           THEN INSERT([非營業日],[公司代號],[基金統編],[基金名稱],[排序]) VALUES(B.非營業日,B.公司代號, B.基金統編,
                                                           B.基金名稱,B.排序) WHEN NOT MATCHED BY SOURCE THEN DELETE;";
            SqlCommand command = new SqlCommand(sqlCommand, SQLConnection);
            SqlParameter tableParameter = command.Parameters.AddWithValue("@sourceTable", dataSet.Tables[0]);
            tableParameter.SqlDbType = SqlDbType.Structured;
            tableParameter.TypeName = "基金非營業日明細TableType";
            SQLConnection.Open();
            command.ExecuteNonQuery();
            SQLConnection.Close();
        }
    }
}
