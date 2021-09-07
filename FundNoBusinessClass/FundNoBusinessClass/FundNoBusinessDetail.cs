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
            TotalDocument.Root.RemoveNodes();
            string pattern = @"<tr class=""r_blue"">[\s]*?<td.*?>(?<date>[\d]{8})</td><td.*?>(?<company>[\w]{5})</td><td.*?>(?<taxID>[\w]*?)</td><td.*?>(?<name>.*?)</td>.*?</tr>";
            foreach (var data in OriginalWeb)
            {
                //存一年的資料之後要弄成xml
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
                string fileName = Path.Combine(CreatDirectory(DateTime.Now.Year.ToString()), "基金非營業日明細.xml");
                SaveXml(document, fileName);
                TotalDocument.Root.Add(fundXml); //所有xml存成一個，之後要匯入資料庫
            }
        }

        /// <summary>
        /// 用xml中介資料更新資料庫
        /// </summary>
        /// <param name="SQLConnection">資料庫連線字串</param>
        public override void WriteDatabase(SqlConnection SQLConnection)
        {
            SqlDataAdapter sql = new SqlDataAdapter("SELECT * FROM 基金非營業日明細_luann", SQLConnection);
            //放資料庫目前的資料
            DataTable table = new DataTable();
            //放xml做好的最新資料
            DataSet dataSet = new DataSet();
            //讀取xml
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
            //每次更新MTIME都要一起變
            sql.UpdateCommand.Parameters.Add("@MTIME", SqlDbType.BigInt).Value = DateTimeOffset.Now.ToUnixTimeSeconds();
            //做批次處理
            sql.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
            sql.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
            sql.UpdateBatchSize = 0;
            SQLConnection.Open();
            //將資料庫資料填入table
            sql.Fill(table);
            //設定主鍵
            table.PrimaryKey = new DataColumn[] { table.Columns["非營業日"], table.Columns["基金統編"] };
            dataSet.Tables[0].PrimaryKey = new DataColumn[] { dataSet.Tables[0].Columns["非營業日"], dataSet.Tables[0].Columns["基金統編"] };
            //將兩張表合併，false意思是當組件相同時已dataSet.Tables[0]為主，Ignoreg是因為兩邊資料型態不同
            table.Merge(dataSet.Tables[0], false, MissingSchemaAction.Ignore);
            //對變動的行做新增和更新
            sql.Update(table);
            SQLConnection.Close();
        }
    }
}
