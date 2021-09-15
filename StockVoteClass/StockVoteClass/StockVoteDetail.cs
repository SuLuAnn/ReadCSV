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
    public class StockVoteDetail : IDataSheet
    {
        /// <summary>
        /// 存物件所對應的資料表名稱
        /// </summary>
        public string DataTableName { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        public StockVoteDetail()
        {
            DataTableName = "股東會投票日明細_luann";
        }

        /// <summary>
        /// 取得xml中介資料
        /// </summary>
        public void GetXML()
        {
            string pattern = $@"""Font_001"">[\s]*?<td.*?left"">(?<id>.*?)(<a.*?""_blank"">[\s]*(?<nameLink>[^(\s]+)\(?(?<convenerLink>[^(]*?)\)?[\s]*?</a>|(?<name>[^(\s]*)\(?(?<convener>[\S]*?)\)?)[\s]*?</td>.*?left"">(?<meetingDate>.*?)</td>.*?left"">(?<voteStartDay>.*?)~(?<voteEndDay>.*?)</td>.*?""_blank"">(?<agency>.*?)</a>.*?left"">(?<phone>.*?)</td></tr>";
            int page = StockVote.GetPageNumber();
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
                        new XElement(GlobalConst.VOTE_DATE, StockVote.ChangeYear(detail.Groups[GlobalConst.VOTE_START_DAY].Value.Replace(GlobalConst.SLASH, string.Empty).Trim())),
                        new XElement(GlobalConst.STOCK_CODE, detail.Groups[GlobalConst.ID].Value.Trim()),
                        GlobalFunction.ChangeNull(GlobalConst.STOCK_NAME, $"{detail.Groups[GlobalConst.NAME_LINK].Value}{detail.Groups[GlobalConst.NAME].Value}"),
                        GlobalFunction.ChangeNull(GlobalConst.CHINESS_CONVENER, $"{detail.Groups[GlobalConst.CONVENER_LINK].Value}{detail.Groups[GlobalConst.CONVENER].Value}"),
                        GlobalFunction.ChangeNull(GlobalConst.CHINESS_MEETING_DATE, StockVote.ChangeYear(detail.Groups[GlobalConst.MEETING_DATE].Value.Replace(GlobalConst.SLASH, string.Empty))),
                        GlobalFunction.ChangeNull(GlobalConst.CHINESS_AGENCY, detail.Groups[GlobalConst.AGENCY].Value),
                        GlobalFunction.ChangeNull(GlobalConst.CHINESS_PHONE, detail.Groups[GlobalConst.PHONE].Value)
                    ));
                }
                XDocument document = new XDocument(new XElement(GlobalConst.XML_ROOT, voteDay));
                string fileName = Path.Combine(GlobalFunction.CreatDirectory(DateTime.Today.ToString(GlobalConst.STOCK_VOTE_DETAIL)), $"{i}.xml");
                GlobalFunction.SaveXml(document, fileName);
            }
        }

        /// <summary>
        /// 用xml中介資料更新資料庫
        /// </summary>
        public void WriteDatabase()
        {
            XDocument TotalDocument = StockVote.GetTotalXml("股東會投票日明細");
            DataSet dataSet = new DataSet();
            //讀取xml
            dataSet.ReadXml(TotalDocument.CreateReader());
            //dataSet.Tables[0].Rows[1]["證券名稱"] = "AAAAAAAAAAAAA";//測試用
            string sqlCommand = @"MERGE [dbo].[股東會投票日明細_luann] AS A USING @sourceTable AS B
                                                            ON A.[投票日期] = B.[投票日期] 
                                                           AND A.[證券代號] = B.[證券代號] 
                                                           WHEN MATCHED AND (ISNULL(A.[證券名稱],'') <> ISNULL(B.證券名稱,'')
                                                                                             OR ISNULL(A.[召集人],'') <> ISNULL(B.召集人,'')
                                                                                             OR ISNULL(A.[股東會日期],'') <> ISNULL(B.股東會日期,'')
                                                                                             OR ISNULL(A.[發行代理機構],'') <> ISNULL(B.發行代理機構,'')
                                                                                             OR ISNULL(A.[聯絡電話],'') <> ISNULL(B.聯絡電話,''))
                                                           THEN UPDATE SET [證券名稱] = B.證券名稱,
                                                                                               [召集人] = B.召集人,
                                                                                               [股東會日期] = B.股東會日期,
                                                                                               [發行代理機構] = B.發行代理機構,
                                                                                               [聯絡電話] = B.聯絡電話,
                                                                                               [MTIME] = (datediff(second, '1970-01-01',getutcdate()))
                                                           WHEN NOT MATCHED BY TARGET THEN INSERT([投票日期],[證券代號],[證券名稱],[召集人],[股東會日期],[發行代理機構],[聯絡電話])
                                                                                                                                        VALUES(B.投票日期,B.證券代號,B.證券名稱,B.召集人,B.股東會日期,B.發行代理機構,B.聯絡電話)
                                                           WHEN NOT MATCHED BY SOURCE THEN DELETE;";
            using (SqlConnection SQLConnection = new SqlConnection(@"Data Source=192.168.10.180;Initial Catalog=StockDB;User ID=test;Password=test; Connection Timeout=180"))
            {
                SqlCommand command = new SqlCommand(sqlCommand, SQLConnection);
                SqlParameter tableParameter = command.Parameters.AddWithValue("@sourceTable", dataSet.Tables[0]);
                tableParameter.SqlDbType = SqlDbType.Structured;
                tableParameter.TypeName = "股東會投票日明細TableType";
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
            StockVote.GetWebs();
        }
    }
}
