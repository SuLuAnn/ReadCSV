using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LinqUserInterface.Global;

namespace LinqUserInterface
{
    /// <summary>
    /// 第一題查詢交易所產業分類代號表，要能依市場別以及產業別查詢
    /// </summary>
    public class MarketIndustryQuery
    {
        /// <summary>
        /// 資料庫物件
        /// </summary>
        private StockDBEntities StockDB;

        /// <summary>
        /// 建構子，傳入資料庫物件
        /// </summary>
        /// <param name="stockDB">資料庫物件</param>
        public MarketIndustryQuery(StockDBEntities stockDB)
        {
            StockDB = stockDB;
        }

        /// <summary>
        /// 要產生市場別名稱與產業名稱的巢狀Dictionary，每個Dictionary裡都要有一個無分類是包含這類的全部
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, List<交易所產業分類代號表>>> GetMarketMenu()
        {
            return StockDB.交易所產業分類代號表
                         .AsEnumerable()
                         .GroupBy(industry => industry.市場別名稱)//依市場別分類
                         .ToDictionary(industries => industries.Key,
                                                   industries => industries.GroupBy(market => market.名稱)
                                                                                                .ToDictionary(item => item.Key,
                                                                                                                          item => item.ToList())
                                                                                                .Union(new Dictionary<string, List<交易所產業分類代號表>>()
                                                                                                                                                                                                                    {
                                                                                                                                                                                                                      { "無分類", industries.ToList()}
                                                                                                                                                                                                                                                                            })
                                                                                                .ToDictionary(group => group.Key, group => group.Value))
                        .Union(new Dictionary<string, Dictionary<string, List<交易所產業分類代號表>>>()
                                                                                                                                                                                 {
                                                                                                                                                                                    { NO_CLASSIFICATION,
                                                                                                                                                                                    new Dictionary<string, List<交易所產業分類代號表>>()
                                                                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                                                            { NO_CLASSIFICATION,
                                                                                                                                                                                                                                                                                                StockDB.交易所產業分類代號表.ToList()}
                                                                                                                                                                                                                                                                                                                                                                         }
                                                                                                                                                                                                                                                                                                                                                                           }
                                                                                                                                                                                                                                                                                                                                                                           })
                        .ToDictionary(item => item.Key,
                                                  item => item.Value);
        }
    }
}
