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
    [ExportMetadata("TableName", "基金非營業日明細")]
    [Export(typeof(IDataSheet))]
    public class FundNoBusinessDetail : FundNoBusiness
    {
        public FundNoBusinessDetail() : base("基金非營業日明細_luann")
        {
        }

        public override void GetXML()
        {
            TotalDocument.Root.RemoveNodes();
            string pattern = @"<tr class=""r_blue"">[\s]*?<td.*?>(?<date>[\d]{8})</td><td.*?>(?<company>[\w]{5})</td><td.*?>(?<taxID>[\w]*?)</td><td.*?>(?<name>.*?)</td>.*?</tr>";
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
                TotalDocument.Root.Add(fundXml);
            }
        }

        public override void WriteDatabase(SqlConnection SQLConnection)
        {
            SqlDataAdapter sql = new SqlDataAdapter("SELECT * FROM 基金非營業日明細_luann", SQLConnection);
            DataTable table = new DataTable();
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(TotalDocument.CreateReader());
            sql.InsertCommand = new SqlCommand("INSERT INTO [dbo].[基金非營業日明細_luann] ([非營業日],[公司代號],[基金統編],[基金名稱],[排序]) VALUES(@非營業日,@公司代號,@基金統編,@基金名稱,@排序)", SQLConnection);
            sql.InsertCommand.Parameters.Add("@非營業日", SqlDbType.Char, 8, "非營業日");
            sql.InsertCommand.Parameters.Add("@公司代號", SqlDbType.Char, 5, "公司代號");
            sql.InsertCommand.Parameters.Add("@基金統編", SqlDbType.VarChar, 9, "基金統編");
            sql.InsertCommand.Parameters.Add("@基金名稱", SqlDbType.NVarChar, 80, "基金名稱");
            sql.InsertCommand.Parameters.Add("@排序", SqlDbType.TinyInt, 8, "排序");
            sql.UpdateCommand = new SqlCommand("UPDATE [dbo].[基金非營業日明細_luann] SET [公司代號] = @公司代號,[基金名稱] = @基金名稱,[排序] = @排序,[MTIME] = @MTIME WHERE [非營業日] = @非營業日 AND [基金統編] = @基金統編", SQLConnection);
            sql.UpdateCommand.Parameters.Add("@非營業日", SqlDbType.Char, 8, "非營業日");
            sql.UpdateCommand.Parameters.Add("@公司代號", SqlDbType.Char, 5, "公司代號");
            sql.UpdateCommand.Parameters.Add("@基金統編", SqlDbType.VarChar, 9, "基金統編");
            sql.UpdateCommand.Parameters.Add("@基金名稱", SqlDbType.NVarChar, 80, "基金名稱");
            sql.UpdateCommand.Parameters.Add("@排序", SqlDbType.TinyInt, 8, "排序");
            sql.UpdateCommand.Parameters.Add("@MTIME", SqlDbType.BigInt).Value = DateTimeOffset.Now.ToUnixTimeSeconds();
            sql.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
            sql.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
            sql.UpdateBatchSize = 0;
            SQLConnection.Open();
            sql.Fill(table);
            table.PrimaryKey = new DataColumn[] { table.Columns["非營業日"], table.Columns["基金統編"] };
            dataSet.Tables[0].PrimaryKey = new DataColumn[] { dataSet.Tables[0].Columns["非營業日"], dataSet.Tables[0].Columns["基金統編"] };
            table.Merge(dataSet.Tables[0], false, MissingSchemaAction.Ignore);
            sql.Update(table);
            SQLConnection.Close();
        }
    }
}
