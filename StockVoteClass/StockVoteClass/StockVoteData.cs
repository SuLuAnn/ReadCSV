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
    /// 產出股東會投票資料表的類別
    /// </summary>
    [ExportMetadata("TableName", "股東會投票資料表")]
    [Export(typeof(IDataSheet))]
    public class StockVoteData : StockVote
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public StockVoteData() : base("股東會投票資料表_luann")
        {
        }

        /// <summary>
        /// 取得xml中介資料
        /// </summary>
        public override void GetXML()
        {
            string pattern = $@"""Font_001"">[\s]*?<td.*?left"">(?<id>.*?)(<a.*?""_blank"">[\s]*(?<nameLink>[^(\s]+)\(?(?<convenerLink>[^(]*?)\)?[\s]*?</a>|(?<name>[^(\s]*)\(?(?<convener>[\S]*?)\)?)[\s]*?</td>.*?left"">(?<meetingDate>.*?)</td>.*?left"">(?<voteStartDay>.*?)~(?<voteEndDay>.*?)</td>.*?""_blank"">(?<agency>.*?)</a>.*?left"">(?<phone>.*?)</td></tr>";
            int page = GetPageNumber();
            for (int i = 1; i <= page; i++)
            {
                string path = Path.Combine(DateTime.Today.ToString(GlobalConst.DATE_FORMAT), $"{i}.html");
                string web = GlobalFunction.ReadFile(path);
                //存一頁的資料之後要弄成xml
                MatchCollection stockVoteDatas = Regex.Matches(web, pattern, RegexOptions.Singleline);
                List<XElement> voteDay = new List<XElement>();
                foreach (Match detail in stockVoteDatas)
                {
                    voteDay.Add(new XElement(GlobalConst.XML_NODE_NAME,
                        new XElement(GlobalConst.STOCK_CODE, detail.Groups[GlobalConst.ID].Value.Trim()),
                        GlobalFunction.ChangeNull(GlobalConst.STOCK_NAME, $"{detail.Groups[GlobalConst.NAME_LINK].Value}{detail.Groups[GlobalConst.NAME].Value}"),
                        GlobalFunction.ChangeNull(GlobalConst.CHINESS_CONVENER, $"{detail.Groups[GlobalConst.CONVENER_LINK].Value}{detail.Groups[GlobalConst.CONVENER].Value}"),
                        new XElement(GlobalConst.CHINESS_MEETING_DATE, ChangeYear(detail.Groups[GlobalConst.MEETING_DATE].Value.Replace(GlobalConst.SLASH, string.Empty).Trim())),
                        GlobalFunction.ChangeNull(GlobalConst.CHINESS_VOTE_START_DAY, ChangeYear(detail.Groups[GlobalConst.VOTE_START_DAY].Value.Replace(GlobalConst.SLASH, string.Empty))),
                        GlobalFunction.ChangeNull(GlobalConst.CHINESS_VOTE_END_DAY, ChangeYear(detail.Groups[GlobalConst.VOTE_END_DAY].Value.Replace(GlobalConst.SLASH, string.Empty))),
                        GlobalFunction.ChangeNull(GlobalConst.CHINESS_AGENCY, detail.Groups[GlobalConst.AGENCY].Value),
                        GlobalFunction.ChangeNull(GlobalConst.CHINESS_PHONE, detail.Groups[GlobalConst.PHONE].Value)
                    ));
                }
                XDocument document = new XDocument(new XElement(GlobalConst.XML_ROOT, voteDay));
                string fileName = Path.Combine(GlobalFunction.CreatDirectory(DateTime.Today.ToString(GlobalConst.STOCK_VOTE_DATA)), $"{i}.xml");
                GlobalFunction.SaveXml(document, fileName);
            }
        }

        /// <summary>
        /// 用xml中介資料更新資料庫
        /// </summary>
        public override void WriteDatabase()
        {
            XDocument TotalDocument = GetTotalXml("股東會投票資料表");
            DataSet dataSet = new DataSet();
            //讀取xml
            dataSet.ReadXml(TotalDocument.CreateReader());
            //dataSet.Tables[0].Rows[1]["證券名稱"] = "AAAAAAAAAAAAA";//測試用
            string sqlCommand = @"MERGE [dbo].[股東會投票資料表_luann] AS A USING @sourceTable AS B 
                                                           ON A.[股東會日期] = B.[股東會日期]
                                                           AND A.[證券代號] = B.[證券代號]
                                                           WHEN MATCHED AND (ISNULL(A.[證券名稱],'') <> ISNULL(B.證券名稱,'')
                                                           OR ISNULL(A.[召集人],'') <> ISNULL(B.召集人,'')
                                                           OR ISNULL(A.[投票起日],'') <> ISNULL(B.投票起日,'')
                                                           OR ISNULL(A.[投票迄日],'') <> ISNULL(B.投票迄日,'')
                                                           OR ISNULL(A.[發行代理機構],'') <> ISNULL(B.發行代理機構,'')
                                                           OR ISNULL(A.[聯絡電話],'') <> ISNULL(B.聯絡電話,''))
                                                           THEN UPDATE SET [證券名稱] = B.證券名稱,
                                                                                               [召集人] = B.召集人,
                                                                                               [投票起日] = B.投票起日,
                                                                                               [投票迄日] = B.投票迄日,
                                                                                               [發行代理機構] = B.發行代理機構,
                                                                                               [聯絡電話] = B.聯絡電話,
                                                                                               [MTIME] = (datediff(second, '1970-01-01', getutcdate()))
                                                           WHEN NOT MATCHED BY TARGET THEN INSERT([證券代號],[證券名稱],[召集人],[股東會日期],[投票起日],[投票迄日],[發行代理機構],[聯絡電話])
                                                                                                                                         VALUES(B.證券代號,B.證券名稱,B.召集人,B.股東會日期,B.投票起日,B.投票迄日,B.發行代理機構,B.聯絡電話)
														   WHEN NOT MATCHED BY SOURCE THEN DELETE;";
            using (SqlConnection SQLConnection = new SqlConnection(@"Data Source=192.168.10.180;Initial Catalog=StockDB;User ID=test;Password=test; Connection Timeout=180"))
            {
                SqlCommand command = new SqlCommand(sqlCommand, SQLConnection);
                SqlParameter tableParameter = command.Parameters.AddWithValue("@sourceTable", dataSet.Tables[0]);
                tableParameter.SqlDbType = SqlDbType.Structured;
                tableParameter.TypeName = "股東會投票資料表TableType";
                SQLConnection.Open();
                command.ExecuteNonQuery();
                SQLConnection.Close();
            }
        }
    }
}
