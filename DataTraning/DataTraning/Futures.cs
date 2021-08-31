using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataTraning
{
    public class Futures
    {
        private StockDBEntities StockDB;
        public Futures(StockDBEntities stockDB)
        {
            StockDB = stockDB;
        }

        public string GetTodayData()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("down_type", "1");
            headers.Add("commodity_id", "all");
            headers.Add("queryStartDate", DateTime.Now.ToString("yyyy/MM/dd"));
            headers.Add("queryEndDate", DateTime.Now.ToString("yyyy/MM/dd"));
            return Global.HtmlPost(Constants.DAY_FUTURES, headers, "BIG5");
        }

        public List<日期貨盤後行情表_luann> GetFutursDetail()
        {
            string web = GetTodayData();
            Global.SaveFile(web, $"{DateTime.Now.Year}.csv");
            string[] stringDatas = web.Trim().Split('\n');
            IEnumerable<string> datas = stringDatas.Skip(1);
            List<日期貨盤後行情表_luann> futures = new List<日期貨盤後行情表_luann>();
            foreach (string data in datas)
            {
                string[] fields = data.Split(',');
                if (fields[(int)Enums.Futures.TRADING_HOURS] == "盤後")
                {
                    futures.Add(new 日期貨盤後行情表_luann
                    {
                        交易日期 = fields[(int)Enums.Futures.TRANSACTION_DATE].Replace("/", string.Empty),
                        契約 = fields[(int)Enums.Futures.CONTRACT],
                        到期月份_週別_ = fields[(int)Enums.Futures.EXPIRY_MONTH],
                        開盤價 = decimal.TryParse(fields[(int)Enums.Futures.OPENING_PRICE], out decimal openPrice) ? (decimal?)openPrice : null,
                        最高價 = decimal.TryParse(fields[(int)Enums.Futures.HIGHEST_PRICE], out decimal highPrice) ? (decimal?)highPrice : null,
                        最低價 = decimal.TryParse(fields[(int)Enums.Futures.LOWEST_PRICE], out decimal lowPrice) ? (decimal?)lowPrice : null,
                        收盤價 = decimal.TryParse(fields[(int)Enums.Futures.CLOSING_PRICE], out decimal closePrice) ? (decimal?)closePrice : null
                    });
                }
            }
            return futures;
        }
        public void AddReviseFutursDetail()
        {
            List<日期貨盤後行情表_luann> futures = GetFutursDetail();
            Global.SaveCsv(futures, $"{DateTime.Now.Year}_日期貨盤後行情表.csv");
            StockDB.日期貨盤後行情表_luann.AddRange(futures);
            StockDB.SaveChanges();
        }

        public IEnumerable<日期貨盤後統計表_luann> GetFutursStatistic()
        {
            return GetFutursDetail().GroupBy(data => new
            {
                data.交易日期,
                data.契約
            }, (key, value) => new 日期貨盤後統計表_luann
            {
                交易日期 = key.交易日期,
                契約 = key.契約,
                開盤價 = value.Max(data => data.開盤價),
                最高價 = value.Max(data => data.最高價),
                最低價 = value.Min(data => data.最低價),
                收盤價 = value.Max(data => data.收盤價)
            });
        }

        public void AddReviseFutursStatistic()
        {
            List<日期貨盤後統計表_luann> futures = GetFutursStatistic().ToList();
            Global.SaveCsv(futures, $"{DateTime.Now.Year}_日期貨盤後統計表.csv");
            StockDB.日期貨盤後統計表_luann.AddRange(futures);
            StockDB.SaveChanges();
        }
    }
}
