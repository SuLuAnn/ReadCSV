using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataTraning
{
    public class StockVote
    {
        private StockDBEntities StockDB;
        public StockVote(StockDBEntities stockDB)
        {
            StockDB = stockDB;
        }

        public IEnumerable<int> GetPageNumber()
        {
            string web = Global.GetWebPage(Constants.STOCK_VOTE);
            if (int.TryParse(Regex.Match(web, @"頁次：1/(?<lastPage>\d+?)<td>").Groups["lastPage"].Value, out int page))
            {
                return Enumerable.Range(1, page);
            }
            return null;
        }

        public void AddReviseStockData()
        {
            StockDB.股東會投票資料表_luann.AddRange(SpiltData().SelectMany(data => data));
            StockDB.SaveChanges();
        }
        public void AddReviseStockDetail()
        {
            StockDB.股東會投票日明細_luann.AddRange(SpiltDetail().SelectMany(data => data));
            StockDB.SaveChanges();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StockVotePageDto> GetData()
        {
            string allPattern = @"""Font_001"">(?<data>.*?)</tr>";
            foreach (int page in GetPageNumber())
            {
                string stockVotePage = Global.GetWebPage($"{Constants.STOCK_VOTE_PAGE}{page}");
                string path = Path.Combine(Global.CreatDirectory(DateTime.Today.ToString("yyyyMMdd")), $"{page}.html");
                Global.SaveFile(stockVotePage, path);
                MatchCollection stockVoteDatas = Regex.Matches(stockVotePage, allPattern, RegexOptions.Singleline);
                yield return new StockVotePageDto(page, stockVoteDatas);
            }
        }

        public IEnumerable<List<股東會投票日明細_luann>> SpiltDetail()
        {
            foreach (StockVotePageDto datas in GetData())
            {
                List<股東會投票日明細_luann> voteDay = new List<股東會投票日明細_luann>();
                foreach (Match data in datas.OnePageVoteData)
                {
                    string pattern = $@"left"">(?<id>.*?)(<a.*?""_blank"">[\s]*?(?<nameLink>[^(]*)\(?(?<convenerLink>[\S]*?)\)?[\s]*?</a>|(?<name>[^(\s]*)\(?(?<convener>[\S]*?)\)?)[\s]*?</td>.*?left"">(?<meetingDate>.*?)</td>.*?left"">(?<voteStartDay>.*?)~(?<voteEndDay>.*?)</td>.*?""_blank"">(?<agency>.*?)</a>.*?left"">(?<phone>.*?)</td>";
                    Match detail = Regex.Match(data.Groups["data"].Value, pattern, RegexOptions.Singleline);
                    voteDay.Add(new 股東會投票日明細_luann
                    {
                        證券代號 = detail.Groups["id"].Value.Trim(),
                        證券名稱 = $"{detail.Groups["nameLink"].Value}{detail.Groups["name"].Value}".Trim(),
                        召集人 = $"{detail.Groups["convenerLink"].Value}{detail.Groups["convener"].Value}".Trim(),
                        投票日期 = Global.ChangeYear(detail.Groups["voteStartDay"].Value.Replace(@"/", string.Empty).Trim()),
                        股東會日期 = Global.ChangeYear(detail.Groups["meetingDate"].Value.Replace(@"/", string.Empty).Trim()),
                        發行代理機構 = detail.Groups["agency"].Value.Trim(),
                        聯絡電話 = detail.Groups["phone"].Value.Trim()
                    });
                }
                Global.SaveCsv(voteDay, Path.Combine(DateTime.Today.ToString("yyyyMMdd"), $"{datas.PageNumber}_股東會投票日明細.csv"));
                yield return voteDay;
            }
        }
        public IEnumerable<List<股東會投票資料表_luann>> SpiltData()
        {
            foreach (StockVotePageDto datas in GetData())
            {
                List<股東會投票資料表_luann> voteDay = new List<股東會投票資料表_luann>();
                foreach (Match data in datas.OnePageVoteData)
                {
                    string pattern = $@"left"">(?<id>.*?)(<a.*?""_blank"">[\s]*?(?<nameLink>[^(]*)\(?(?<convenerLink>[\S]*?)\)?[\s]*?</a>|(?<name>[^(\s]*)\(?(?<convener>[\S]*?)\)?)[\s]*?</td>.*?left"">(?<meetingDate>.*?)</td>.*?left"">(?<voteStartDay>.*?)~(?<voteEndDay>.*?)</td>.*?""_blank"">(?<agency>.*?)</a>.*?left"">(?<phone>.*?)</td>";
                    Match detail = Regex.Match(data.Groups["data"].Value, pattern, RegexOptions.Singleline);
                    voteDay.Add(new 股東會投票資料表_luann
                    {
                        證券代號 = detail.Groups["id"].Value.Trim(),
                        證券名稱 = $"{detail.Groups["nameLink"].Value}{detail.Groups["name"].Value}".Trim(),
                        召集人 = $"{detail.Groups["convenerLink"].Value}{detail.Groups["convener"].Value}".Trim(),
                        投票起日 = Global.ChangeYear(detail.Groups["voteStartDay"].Value.Replace(@"/", string.Empty).Trim()),
                        投票迄日 = Global.ChangeYear(detail.Groups["voteEndDay"].Value.Replace(@"/", string.Empty).Trim()),
                        股東會日期 = Global.ChangeYear(detail.Groups["meetingDate"].Value.Replace(@"/", string.Empty).Trim()),
                        發行代理機構 = detail.Groups["agency"].Value.Trim(),
                        聯絡電話 = detail.Groups["phone"].Value.Trim()
                    });
                }
                Global.SaveCsv(voteDay, Path.Combine(DateTime.Today.ToString("yyyyMMdd"), $"{datas.PageNumber}_股東會投票資料表.csv"));
                yield return voteDay;
            }
        }
    }
}
