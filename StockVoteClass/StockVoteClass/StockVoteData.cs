using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StockVoteClass
{
    [ExportMetadata("TableName", "股東會投票資料表")]
    [Export(typeof(IDataSheet))]
    public class StockVoteData : StockVote
    {
        public StockVoteData() : base("股東會投票資料表_luann")
        {
        }

        public override void GetXML()
        {
            string allPattern = @"""Font_001"">(?<data>.*?)</tr>";
            foreach (var web in OriginalWeb)
            {
                MatchCollection stockVoteDatas = Regex.Matches(web.Value, allPattern, RegexOptions.Singleline);
                List<XElement> voteDay = new List<XElement>();
                foreach (Match data in stockVoteDatas)
                {
                    string pattern = $@"left"">(?<id>.*?)(<a.*?""_blank"">[\s]*?(?<nameLink>[^(]*)\(?(?<convenerLink>[\S]*?)\)?[\s]*?</a>|(?<name>[^(\s]*)\(?(?<convener>[\S]*?)\)?)[\s]*?</td>.*?left"">(?<meetingDate>.*?)</td>.*?left"">(?<voteStartDay>.*?)~(?<voteEndDay>.*?)</td>.*?""_blank"">(?<agency>.*?)</a>.*?left"">(?<phone>.*?)</td>";
                    Match detail = Regex.Match(data.Groups["data"].Value, pattern, RegexOptions.Singleline);
                    voteDay.Add(new XElement("Data",
                        new XElement("證券代號",detail.Groups["id"].Value.Trim()),
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
                string fileName = Path.Combine(CreatDirectory(DateTime.Today.ToString("yyyyMMdd/股東會投票資料表")), $"{web.Key}.xml");
                SaveXml(document, fileName);
            }
        }
    }
}
