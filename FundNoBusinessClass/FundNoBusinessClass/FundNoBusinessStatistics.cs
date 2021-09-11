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
            TotalDocument.Root.RemoveNodes();
            string pattern = @"<tr class=""r_blue"">[\s]*?<td.*?>(?<date>[\d]{8})</td><td.*?>(?<company>[\w]{5})</td><td.*?>(?<taxID>[\w]*?)</td><td.*?>(?<name>[\S]*?)</td>.*?</tr>";
            foreach (var data in OriginalWeb)
            {
                //存一年的資料之後要弄成xml
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
                string fileName = Path.Combine(CreatDirectory(DateTime.Now.Year.ToString()), "基金非營業日統計.xml");
                SaveXml(document, fileName);
                //所有xml存成一個，之後要匯入資料庫
                TotalDocument.Root.Add(fundXml);
            }
        }

        /// <summary>
        /// 用xml中介資料更新資料庫
        /// </summary>
        /// <param name="SQLConnection">資料庫連線字串</param>
        public override void WriteDatabase()
        {
            SqlDataAdapter sql = new SqlDataAdapter("SELECT * FROM 基金非營業日統計_luann", SQLConnection);
            //放資料庫目前的資料
            DataTable table = new DataTable();
            //放xml做好的最新資料
            DataSet dataSet = new DataSet();
            //讀取xml
            dataSet.ReadXml(TotalDocument.CreateReader());
            sql.InsertCommand = new SqlCommand("INSERT INTO [dbo].[基金非營業日統計_luann] ([非營業日],[公司代號],[基金總數]) VALUES(@非營業日,@公司代號,@基金總數)", SQLConnection);
            sql.InsertCommand.Parameters.Add("@非營業日", SqlDbType.Char, 8, "非營業日");
            sql.InsertCommand.Parameters.Add("@公司代號", SqlDbType.Char, 5, "公司代號");
            sql.InsertCommand.Parameters.Add("@基金總數", SqlDbType.TinyInt, 8, "基金總數");
            sql.UpdateCommand = new SqlCommand("UPDATE [dbo].[基金非營業日統計_luann] SET [基金總數] = @基金總數,[MTIME] = @MTIME WHERE [非營業日] = @非營業日 AND [公司代號] = @公司代號", SQLConnection);
            sql.UpdateCommand.Parameters.Add("@非營業日", SqlDbType.Char, 8, "非營業日");
            sql.UpdateCommand.Parameters.Add("@公司代號", SqlDbType.Char, 5, "公司代號");
            sql.UpdateCommand.Parameters.Add("@基金總數", SqlDbType.TinyInt, 8, "基金總數");
            //每次更新MTIME都要一起變
            sql.UpdateCommand.Parameters.Add("@MTIME", SqlDbType.BigInt).Value = DateTimeOffset.Now.ToUnixTimeSeconds();
            //做批次處理
            sql.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
            sql.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
            sql.UpdateBatchSize = 0;
            //dataSet.Tables[0].Rows[0]["基金總數"] = 0;//測試用
            SQLConnection.Open();
            //將資料庫資料填入table
            sql.Fill(table);
            table.PrimaryKey = new DataColumn[] { table.Columns["非營業日"], table.Columns["公司代號"] };
            dataSet.Tables[0].PrimaryKey = new DataColumn[] { dataSet.Tables[0].Columns["非營業日"], dataSet.Tables[0].Columns["公司代號"] };
            //將兩張表合併，false意思是當組件相同時已dataSet.Tables[0]為主，Ignoreg是因為兩邊資料型態不同
            table.Merge(dataSet.Tables[0], false, MissingSchemaAction.Ignore);
            //對變動的行做新增和更新
            sql.Update(table);
            SQLConnection.Close();
        }
    }
}
