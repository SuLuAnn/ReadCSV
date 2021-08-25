using System;
using System.Collections.Generic;
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

        public void AddReviseStockDetail()
        {
            int.TryParse(Regex.Match(Global.GetWebPage(Global.STOCK_VOTE), @"頁次：1/(?<lastPage>\d+)<td>").Groups["lastPage"].Value, out int page);
            string allPattern = @"""Font_001"">(?<data>.*?)</tr>";
            string namePattern;
            for (int i = 1; i <= page; i++)
            {
                string stockVotePage = Global.GetWebPage($"{Global.STOCK_VOTE_PAGE}{i}");
                MatchCollection stockVoteDatas = Regex.Matches(stockVotePage, allPattern, RegexOptions.Singleline);
                foreach (Match data in stockVoteDatas)
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
                    string id = detail.Groups["id"].Value.Trim();
                    string name = names.First().Trim();
                    string convener = names.Length > 1 ? names[1].Replace(@")", string.Empty).Trim() : null;
                    string voteDate = Global.ChangeYear(detail.Groups["voteStartDay"].Value.Replace(@"/", string.Empty).Trim());
                    string meetingDate = Global.ChangeYear(detail.Groups["meetingDate"].Value.Replace(@"/", string.Empty).Trim());
                    string agency = detail.Groups["agency"].Value.Trim();
                    string phone = detail.Groups["phone"].Value.Trim();
                    股東會投票日明細_luann stockDataDetail =StockDB.股東會投票日明細_luann.SingleOrDefault(date => date.投票日期 == voteDate && date.證券代號 == id);
                    if (stockDataDetail == null)
                    {
                        StockDB.股東會投票日明細_luann.Add(new 股東會投票日明細_luann
                        {
                            證券代號 = id,
                            證券名稱 = name,
                            召集人 = convener,
                            投票日期 = voteDate,
                            股東會日期 = meetingDate,
                            發行代理機構 = agency,
                            聯絡電話 = phone
                        });
                    }
                    else
                    {
                        if (stockDataDetail.證券名稱 != name || stockDataDetail.召集人 != convener || stockDataDetail.股東會日期 != meetingDate || stockDataDetail.發行代理機構 != agency || stockDataDetail.聯絡電話 != phone)
                        {
                            stockDataDetail.MTIME = DateTimeOffset.Now.ToUnixTimeSeconds();
                            stockDataDetail.證券名稱 = name;
                            stockDataDetail.召集人 = convener;
                            stockDataDetail.股東會日期 = meetingDate;
                            stockDataDetail.發行代理機構 = agency;
                            stockDataDetail.聯絡電話 = phone;
                        }
                    }
                }
            }
            StockDB.SaveChanges();
        }
    }
}
