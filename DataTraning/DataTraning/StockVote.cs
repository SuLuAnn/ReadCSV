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
            string web = Global.GetWebPage(Global.STOCK_VOTE);
            if (int.TryParse(Regex.Match(web, @"頁次：1/(?<lastPage>\d+)<td>").Groups["lastPage"].Value, out int page))
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
        public IEnumerable<Tuple<int, MatchCollection>> GetData()
        {
            foreach (var page in GetPageNumber())
            {
                string stockVotePage = Global.GetWebPage($"{Global.STOCK_VOTE_PAGE}{page}");
                string path = Path.Combine(Global.CreatDirectory(DateTime.Today.ToString("yyyyMMdd")), $"{page}.html");
                Global.SaveFile(stockVotePage, path);
                string allPattern = @"""Font_001"">(?<data>.*?)</tr>";
                MatchCollection stockVoteDatas = Regex.Matches(stockVotePage, allPattern, RegexOptions.Singleline);
                yield return new Tuple<int, MatchCollection>(page, stockVoteDatas);
            }
        }

        public IEnumerable<List<股東會投票日明細_luann>> SpiltDetail()
        {
            string namePattern;
            foreach (Tuple<int, MatchCollection> datas in GetData())
            {
                List<股東會投票日明細_luann> voteDay = new List<股東會投票日明細_luann>();
                foreach (Match data in datas.Item2)
                {
                    if (Regex.IsMatch(data.Groups["data"].Value, @"td-link"))
                    {
                        namePattern = @"<a.*?""_blank"">(?<name>.*?)</a>";
                    }
                    else
                    {
                        namePattern = @"(?<name>\S*?)";
                    }
                    string pattern = $@"left"">(?<id>.*?){namePattern}[\s]*?</td>.*?left"">(?<meetingDate>.*?)</td>.*?left"">(?<voteStartDay>.*?)~(?<voteEndDay>.*?)</td>.*?""_blank"">(?<agency>.*?)</a>.*?left"">(?<phone>.*?)</td>";
                    Match detail = Regex.Match(data.Groups["data"].Value, pattern, RegexOptions.Singleline);
                    string[] names = Regex.Split(detail.Groups["name"].Value, @"\(");
                    voteDay.Add(new 股東會投票日明細_luann
                    {
                        證券代號 = detail.Groups["id"].Value.Trim(),
                        證券名稱 = names.First().Trim(),
                        召集人 = names.Length > 1 ? names[1].Replace(@")", string.Empty).Trim() : null,
                        投票日期 = Global.ChangeYear(detail.Groups["voteStartDay"].Value.Replace(@"/", string.Empty).Trim()),
                        股東會日期 = Global.ChangeYear(detail.Groups["meetingDate"].Value.Replace(@"/", string.Empty).Trim()),
                        發行代理機構 = detail.Groups["agency"].Value.Trim(),
                        聯絡電話 = detail.Groups["phone"].Value.Trim()
                    });
                }
                Global.SaveCsv<股東會投票日明細_luann>(voteDay, Path.Combine(DateTime.Today.ToString("yyyyMMdd"),$"{datas.Item1}_股東會投票日明細.csv"));
                yield return voteDay;
            }
        }
        public IEnumerable<List<股東會投票資料表_luann>> SpiltData()
        {
            string namePattern;
            foreach (Tuple<int, MatchCollection> datas in GetData())
            {
                List<股東會投票資料表_luann> voteDay = new List<股東會投票資料表_luann>();
                foreach (Match data in datas.Item2)
                {
                    if (Regex.IsMatch(data.Groups["data"].Value, @"td-link"))
                    {
                        namePattern = @"<a.*?""_blank"">(?<name>.*?)</a>";
                    }
                    else
                    {
                        namePattern = @"(?<name>\S*?)";
                    }
                    string pattern = $@"left"">(?<id>.*?){namePattern}[\s]*?</td>.*?left"">(?<meetingDate>.*?)</td>.*?left"">(?<voteStartDay>.*?)~(?<voteEndDay>.*?)</td>.*?""_blank"">(?<agency>.*?)</a>.*?left"">(?<phone>.*?)</td>";
                    Match detail = Regex.Match(data.Groups["data"].Value, pattern, RegexOptions.Singleline);
                    string[] names = Regex.Split(detail.Groups["name"].Value, @"\(");
                    voteDay.Add(new 股東會投票資料表_luann
                    {
                        證券代號 = detail.Groups["id"].Value.Trim(),
                        證券名稱 = names.First().Trim(),
                        召集人 = names.Length > 1 ? names[1].Replace(@")", string.Empty).Trim() : null,
                        投票起日 = Global.ChangeYear(detail.Groups["voteStartDay"].Value.Replace(@"/", string.Empty).Trim()),
                        投票迄日 = Global.ChangeYear(detail.Groups["voteEndDay"].Value.Replace(@"/", string.Empty).Trim()),
                        股東會日期 = Global.ChangeYear(detail.Groups["meetingDate"].Value.Replace(@"/", string.Empty).Trim()),
                        發行代理機構 = detail.Groups["agency"].Value.Trim(),
                        聯絡電話 = detail.Groups["phone"].Value.Trim()
                    });
                }
                Global.SaveCsv<股東會投票資料表_luann>(voteDay, Path.Combine(DateTime.Today.ToString("yyyyMMdd"), $"{datas.Item1}_股東會投票資料表.csv"));
                yield return voteDay;
            }
        }
    }
}
