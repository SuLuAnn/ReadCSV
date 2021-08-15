using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LinqUserInterface.Global;

namespace LinqUserInterface
{
    public class MarketIndustryQuery
    {
        /// <summary>
        /// 資料庫物件
        /// </summary>
        private StockDBEntities StockDB;

        public MarketIndustryQuery(StockDBEntities stockDB)
        {
            StockDB = stockDB;
        }

        public Dictionary<string, Dictionary<string, List<交易所產業分類代號表>>> GetMarketMenu()
        {
            return Enumerable
                .Repeat(
                    StockDB.交易所產業分類代號表
                    .GroupBy(industry => industry.市場別)
                    .ToDictionary(industries => industries.First().市場別名稱,
                                                markets => Enumerable.Repeat(markets.GroupBy(industry => industry.名稱).ToDictionary(item => item.Key, item => item.ToList()), 2).Aggregate((first, second) => { first.Add(NO_CLASSIFICATION, second.Values.SelectMany(item => item).ToList());
                                                    return first;
                                                }))
                    , 2)
                    .Aggregate((first, second) => 
                            {
                                    first.Add(NO_CLASSIFICATION,
                                                    new Dictionary<string, List<交易所產業分類代號表>> {{
                                                            NO_CLASSIFICATION,
                                                            second.SelectMany(Industry => Industry.Value[NO_CLASSIFICATION]).ToList()
                                                     }}
                                                    );
                        return first;
                              }
                    );
        }
    }
}
