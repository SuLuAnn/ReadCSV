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

namespace StockVoteClass
{
    /// <summary>
    /// 產出股東會投票日明細的類別
    /// </summary>
    [ExportMetadata("TableName", "股東會投票日明細")]
    [Export(typeof(IDataSheet))]
    public class StockVoteDetail : StockVote
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public StockVoteDetail() : base("股東會投票日明細_luann")
        {
        }

        /// <summary>
        /// 取得xml中介資料
        /// </summary>
        public override void GetXML()
        {
            string allPattern = @"""Font_001"">(?<data>.*?)</tr>";
            TotalDocument.Root.RemoveNodes();
            foreach (var web in OriginalWeb)
            {
                //存一頁的資料之後要弄成xml
                MatchCollection stockVoteDatas = Regex.Matches(web.Value, allPattern, RegexOptions.Singleline);
                List<XElement> voteDay = new List<XElement>();
                foreach (Match data in stockVoteDatas)
                {
                    string pattern = $@"left"">(?<id>.*?)(<a.*?""_blank"">[\s]*?(?<nameLink>[^(]*)\(?(?<convenerLink>[\S]*?)\)?[\s]*?</a>|(?<name>[^(\s]*)\(?(?<convener>[\S]*?)\)?)[\s]*?</td>.*?left"">(?<meetingDate>.*?)</td>.*?left"">(?<voteStartDay>.*?)~(?<voteEndDay>.*?)</td>.*?""_blank"">(?<agency>.*?)</a>.*?left"">(?<phone>.*?)</td>";
                    Match detail = Regex.Match(data.Groups["data"].Value, pattern, RegexOptions.Singleline);
                    voteDay.Add(new XElement("Data",
                        new XElement("投票日期", ChangeYear(detail.Groups["voteStartDay"].Value.Replace(@"/", string.Empty).Trim())),
                        new XElement("證券代號", detail.Groups["id"].Value.Trim()),
                        new XElement("證券名稱", $"{detail.Groups["nameLink"].Value}{detail.Groups["name"].Value}".Trim()),
                        new XElement("召集人", $"{detail.Groups["convenerLink"].Value}{detail.Groups["convener"].Value}".Trim()),
                        new XElement("股東會日期", ChangeYear(detail.Groups["meetingDate"].Value.Replace(@"/", string.Empty).Trim())),
                        new XElement("發行代理機構", detail.Groups["agency"].Value.Trim()),
                        new XElement("聯絡電話", detail.Groups["phone"].Value.Trim())
                    ));
                }
                XDocument document = new XDocument(new XElement("Root", voteDay));
                string fileName = Path.Combine(CreatDirectory(DateTime.Today.ToString("yyyyMMdd/股東會投票日明細")), $"{web.Key}.xml");
                SaveXml(document, fileName);
                TotalDocument.Root.Add(voteDay);
            }
        }

        /// <summary>
        /// 用xml中介資料更新資料庫
        /// </summary>
        /// <param name="SQLConnection">資料庫連線字串</param>
        public override void WriteDatabase()
        {
            //放資料庫目前的資料
            SqlDataAdapter sql = new SqlDataAdapter("SELECT * FROM 股東會投票日明細_luann", SQLConnection);
            //放xml做好的最新資料
            DataTable table = new DataTable();
            //讀取xml
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(TotalDocument.CreateReader());
            sql.InsertCommand = new SqlCommand("INSERT INTO[dbo].[股東會投票日明細_luann] ([證券代號],[證券名稱],[召集人],[股東會日期],[投票日期],[發行代理機構],[聯絡電話])VALUES(@證券代號, @證券名稱, @召集人, @股東會日期, @投票日期, @發行代理機構, @聯絡電話)", SQLConnection);
            sql.InsertCommand.Parameters.Add("@證券代號", SqlDbType.VarChar, 6, "證券代號");
            sql.InsertCommand.Parameters.Add("@證券名稱", SqlDbType.NVarChar, 16, "證券名稱");
            sql.InsertCommand.Parameters.Add("@召集人", SqlDbType.NVarChar, 20, "召集人");
            sql.InsertCommand.Parameters.Add("@股東會日期", SqlDbType.Char, 8, "股東會日期");
            sql.InsertCommand.Parameters.Add("@投票日期", SqlDbType.Char, 8, "投票日期");
            sql.InsertCommand.Parameters.Add("@發行代理機構", SqlDbType.NVarChar, 11, "發行代理機構");
            sql.InsertCommand.Parameters.Add("@聯絡電話", SqlDbType.Char, 12, "聯絡電話");
            sql.UpdateCommand = new SqlCommand("UPDATE [dbo].[股東會投票日明細_luann] SET [證券名稱] = @證券名稱,[召集人] = @召集人,[股東會日期] = @股東會日期,[發行代理機構] = @發行代理機構,[聯絡電話] = @聯絡電話 WHERE [證券代號] = @證券代號 AND [投票日期] = @投票日期", SQLConnection);
            sql.UpdateCommand.Parameters.Add("@證券代號", SqlDbType.VarChar, 6, "證券代號");
            sql.UpdateCommand.Parameters.Add("@證券名稱", SqlDbType.NVarChar, 16, "證券名稱");
            sql.UpdateCommand.Parameters.Add("@召集人", SqlDbType.NVarChar, 20, "召集人");
            sql.UpdateCommand.Parameters.Add("@股東會日期", SqlDbType.Char, 8, "股東會日期");
            sql.UpdateCommand.Parameters.Add("@投票日期", SqlDbType.Char, 8, "投票日期");
            sql.UpdateCommand.Parameters.Add("@發行代理機構", SqlDbType.NVarChar, 11, "發行代理機構");
            sql.UpdateCommand.Parameters.Add("@聯絡電話", SqlDbType.Char, 12, "聯絡電話");
            //每次更新MTIME都要一起變
            sql.UpdateCommand.Parameters.Add("@MTIME", SqlDbType.BigInt).Value = DateTimeOffset.Now.ToUnixTimeSeconds();
            //做批次處理
            sql.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
            sql.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
            sql.UpdateBatchSize = 0;
            SQLConnection.Open();
            //將資料庫資料填入table
            sql.Fill(table);
            //dataSet.Tables[0].Rows[0]["證券代號"] = "Annie";//測試用
            //dataSet.Tables[0].Rows[1]["召集人"] = "Annie";//測試用
            //設定主鍵
            table.PrimaryKey = new DataColumn[] { table.Columns["證券代號"], table.Columns["投票日期"] };
            dataSet.Tables[0].PrimaryKey = new DataColumn[] { dataSet.Tables[0].Columns["證券代號"], dataSet.Tables[0].Columns["投票日期"] };
            //將兩張表合併，false意思是當組件相同時已dataSet.Tables[0]為主，Ignoreg是因為兩邊資料型態不同
            table.Merge(dataSet.Tables[0], false, MissingSchemaAction.Ignore);
            //對變動的行做新增和更新
            sql.Update(table);
            SQLConnection.Close();
        }
    }
}
