﻿using Common;
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
            string allPattern = @"""Font_001"">(?<data>.*?)</tr>";
            int page = GetPageNumber();
            for (int i = 1; i <= page; i++)
            {
                string path = Path.Combine(DateTime.Today.ToString("yyyyMMdd"), $"{i}.html");
                string web = ReadFile(path);
                //存一頁的資料之後要弄成xml
                MatchCollection stockVoteDatas = Regex.Matches(web, allPattern, RegexOptions.Singleline);
                List<XElement> voteDay = new List<XElement>();
                foreach (Match data in stockVoteDatas)
                {
                    string pattern = $@"left"">(?<id>.*?)(<a.*?""_blank"">[\s]*?(?<nameLink>[^(]*)\(?(?<convenerLink>[\S]*?)\)?[\s]*?</a>|(?<name>[^(\s]*)\(?(?<convener>[\S]*?)\)?)[\s]*?</td>.*?left"">(?<meetingDate>.*?)</td>.*?left"">(?<voteStartDay>.*?)~(?<voteEndDay>.*?)</td>.*?""_blank"">(?<agency>.*?)</a>.*?left"">(?<phone>.*?)</td>";
                    Match detail = Regex.Match(data.Groups["data"].Value, pattern, RegexOptions.Singleline);
                    voteDay.Add(new XElement("Data",
                        new XElement("證券代號", detail.Groups["id"].Value.Trim()),
                        new XElement("證券名稱", $"{detail.Groups["nameLink"].Value}{detail.Groups["name"].Value}".Trim()),
                        new XElement("召集人", $"{detail.Groups["convenerLink"].Value}{detail.Groups["convener"].Value}".Trim()),
                        new XElement("股東會日期", ChangeYear(detail.Groups["meetingDate"].Value.Replace(@"/", string.Empty).Trim())),
                         new XElement("投票起日", ChangeYear(detail.Groups["voteStartDay"].Value.Replace(@"/", string.Empty).Trim())),
                         new XElement("投票迄日", ChangeYear(detail.Groups["voteEndDay"].Value.Replace(@"/", string.Empty).Trim())),
                        new XElement("發行代理機構", detail.Groups["agency"].Value.Trim()),
                        new XElement("聯絡電話", detail.Groups["phone"].Value.Trim())
                    ));
                }
                XDocument document = new XDocument(new XElement("Root", voteDay));
                string fileName = Path.Combine(CreatDirectory(DateTime.Today.ToString("yyyyMMdd/股東會投票資料表")), $"{i}.xml");
                SaveXml(document, fileName);
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
            string sqlCommand = @"MERGE [dbo].[股東會投票資料表_luann] AS A USING @sourceTable AS B ON A.[股東會日期] = B.[股東會日期]
                                                           AND A.[證券代號] = B.[證券代號] WHEN MATCHED AND (A.[證券名稱] <> B.證券名稱 OR A.[召集人] <> 
                                                           B.召集人 OR A.[投票起日] <> B.投票起日 OR A.[投票迄日] <> B.投票迄日 OR A.[發行代理機構] <> 
                                                           B.發行代理機構 OR A.[聯絡電話] <> B.聯絡電話) THEN UPDATE SET [證券名稱] = B.證券名稱,[召集人] = 
                                                           B.召集人,[投票起日] = B.投票起日,[投票迄日] = B.投票迄日,[發行代理機構] = B.發行代理機構,[聯絡電話] = 
                                                           B.聯絡電話 ,[MTIME] = (datediff(second, '1970-01-01', getutcdate())) WHEN NOT MATCHED BY TARGET 
                                                           THEN INSERT([證券代號],[證券名稱],[召集人],[股東會日期],[投票起日],[投票迄日],[發行代理機構],[聯絡電話])
                                                           VALUES(B.證券代號,B.證券名稱,B.召集人,B.股東會日期,B.投票起日,B.投票迄日,B.發行代理機構,B.聯絡電話)
														   WHEN NOT MATCHED BY SOURCE THEN DELETE;";
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
