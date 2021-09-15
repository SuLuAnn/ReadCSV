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
    public class FundNoBusinessStatistics : IDataSheet
    {
        /// <summary>
        /// 存物件所對應的資料表名稱
        /// </summary>
        public string DataTableName { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        public FundNoBusinessStatistics()
        {
            DataTableName = "基金非營業日統計_luann";
        }

        /// <summary>
        /// 取得xml中介資料
        /// </summary>
        public void GetXML()
        {
            string pattern = @"<tr class=""r_blue"">[\s]*?<td.*?>(?<date>[\d]{8})</td><td.*?>(?<company>[\w]{5})</td><td.*?>(?<taxID>[\w]*?)</td><td.*?>(?<name>[\S]*?)</td>.*?</tr>";
            string web = GlobalFunction.GetWebPage(GlobalConst.FUND_NO_BUSINESS_DAY);
            MatchCollection years = Regex.Matches(web, @"value=""(?<year>\d{4})"">");
            foreach (Match year in years)
            {
                string yearWord = year.Groups[GlobalConst.YEAR].Value.Trim();
                string data = GlobalFunction.ReadFile($"{yearWord}.html");
                //存一年的資料之後要弄成xml
                MatchCollection results = Regex.Matches(data, pattern, RegexOptions.Singleline);
                //取得同非營業日同公司代號基金總數
                IEnumerable<XElement> fundXml = results.OfType<Match>()
                                                                                                  .Select(result => new 
                                                                                                                                                           {
                                                                                                                                                               非營業日 = result.Groups[GlobalConst.DATE].Value,
                                                                                                                                                               公司代號 = result.Groups[GlobalConst.COMPANY].Value,
                                                                                                                                                               基金統編 = result.Groups[GlobalConst.TAX_ID].Value,
                                                                                                                                                           })
                                                                                                  .GroupBy(fund => new 
                                                                                                                                             {
                                                                                                                                                 fund.非營業日,
                                                                                                                                                 fund.公司代號
                                                                                                                                             },
                                                                                                                    (key, value) => new 
                                                                                                                    {
                                                                                                                        公司代號 = key.公司代號,
                                                                                                                        非營業日 = key.非營業日,
                                                                                                                        基金總數 = (byte)value.Count()
                                                                                                                    })
                                                                                                 .Select(fund => new XElement(GlobalConst.XML_NODE_NAME, 
                                                                                                                               new XElement(GlobalConst.NO_BUSINESS, fund.非營業日),
                                                                                                                               new XElement(GlobalConst.COMPANY_CODE, fund.公司代號),
                                                                                                                               new XElement(GlobalConst.FUND_TOTAL, fund.基金總數)
                                                                                                                               ));
                XDocument document = new XDocument(new XElement(GlobalConst.XML_ROOT, fundXml));
                string fileName = Path.Combine(GlobalFunction.CreatDirectory(yearWord), GlobalConst.FUND_NO_BUSINESS_STATISTICS);
                GlobalFunction.SaveXml(document, fileName);
                //所有xml存成一個，之後要匯入資料庫
            }
        }

        /// <summary>
        /// 用xml中介資料更新資料庫
        /// </summary>
        public void WriteDatabase()
        {
            XDocument TotalDocument = FundNoBusiness.GetTotalXml("基金非營業日統計");
            DataSet dataSet = new DataSet();
            //讀取xml
            dataSet.ReadXml(TotalDocument.CreateReader());
            //dataSet.Tables[0].Rows[1]["基金總數"] = 0;//測試用
            string sqlCommand = @"MERGE [dbo].[基金非營業日統計_luann] AS A USING @sourceTable AS B
                                                           ON A.[非營業日] = B.[非營業日] 
                                                           AND A.[公司代號] = B.[公司代號]
                                                           WHEN MATCHED AND A.[基金總數] <> B.基金總數
                                                           THEN UPDATE SET [基金總數] = B.基金總數,
                                                                                               [MTIME] = (datediff(second, '1970-01-01', getutcdate()))
                                                           WHEN NOT MATCHED BY TARGET THEN INSERT([非營業日],[公司代號],[基金總數])
                                                                                                                                        VALUES(B.非營業日,B.公司代號,B.基金總數)
                                                           WHEN NOT MATCHED BY SOURCE THEN DELETE; ";
            using (SqlConnection SQLConnection = new SqlConnection(@"Data Source=192.168.10.180;Initial Catalog=StockDB;User ID=test;Password=test; Connection Timeout=180"))
            {
                SqlCommand command = new SqlCommand(sqlCommand, SQLConnection);
                SqlParameter tableParameter = command.Parameters.AddWithValue("@sourceTable", dataSet.Tables[0]);
                tableParameter.SqlDbType = SqlDbType.Structured;
                tableParameter.TypeName = "基金非營業日統計TableType";
                SQLConnection.Open();
                command.ExecuteNonQuery();
                SQLConnection.Close();
            }
        }

        /// <summary>
        /// 取得資料表名稱，因介面沒有屬性，所以要給方法來讓主程式取得物件的資料表名稱
        /// </summary>
        /// <returns>資料表名稱</returns>
        public string GetDataTableName()
        {
            return DataTableName;
        }

        /// <summary>
        /// 取得網站原始資料
        /// </summary>
        public void GetWebs()
        {
            FundNoBusiness.GetWebs();
        }
    }
}
