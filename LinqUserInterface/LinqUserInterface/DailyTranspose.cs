using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinqUserInterface
{
    /// <summary>
    /// 第二題日收盤查詢及轉置的所有方法
    /// </summary>
    public class DailyTranspose
    {
        /// <summary>
        /// 資料庫物件
        /// </summary>
        private StockDBEntities StockDB;

        /// <summary>
        /// 建構子，傳入資料庫物件
        /// </summary>
        /// <param name="stockDB">資料庫物件</param>
        public DailyTranspose(StockDBEntities stockDB)
        {
            StockDB = stockDB;
        }

        /// <summary>
        /// 依使用者所給條件查詢日收盤，並用交易所產業分類代號表找出對應的市場別名稱與產業名稱，並做轉置(行列對調)
        /// </summary>
        /// <param name="keyWords">使用者想查的股票代號與股票名稱</param>
        /// <param name="firstDay">使用者想查得開始週期</param>
        /// <param name="lastDay">使用者想查得結束週期</param>
        /// <returns>符合條件資料轉置後結果</returns>
        public IEnumerable<IEnumerable<object>> GetDayTranspose(string[] keyWords, string firstDay, string lastDay)
        {
            return StockDB.日收盤.Where(data => (keyWords.Contains(data.股票代號) || keyWords.Contains(data.股票名稱))
                                                                                    && string.Compare(data.日期, firstDay) >= 0 && string.Compare(data.日期, lastDay) <= 0)
                                                    .Select(stock => new
                                                    {
                                                        stock.日期,
                                                        stock.股票代號,
                                                        stock.股票名稱,
                                                        stock.參考價,
                                                        stock.開盤價,
                                                        stock.最高價,
                                                        stock.最低價,
                                                        stock.收盤價,
                                                        stock.漲跌,
                                                        市場別名稱 = StockDB.交易所產業分類代號表.FirstOrDefault(data => SqlFunctions.StringConvert((decimal)data.市場別).Trim() == stock.上市櫃).市場別名稱, //找到交易所產業分類代號表對應的市場別名稱
                                                        產業名稱 = stock.產業代號 == null ? "無產業名稱" : StockDB.交易所產業分類代號表.FirstOrDefault(data => data.代號 == stock.產業代號).名稱,
                                                        //找到交易所產業分類代號表對應的產業名稱，無則填入無產業名稱
                                                        stock.漲停價,
                                                        stock.跌停價,
                                                        stock.漲跌狀況,
                                                        stock.最後委買價,
                                                        stock.最後委賣價,
                                                        漲幅_比率 = stock.漲幅___,
                                                        振幅_比率 = stock.振幅___,
                                                        成交量_股 = stock.成交量_股_,
                                                        stock.成交筆數,
                                                        成交值比重_比率 = stock.成交值比重___,
                                                        成交量變動_比率 = stock.成交量變動___,
                                                        總市值_億 = stock.總市值_億_,
                                                        stock.本益比,
                                                        stock.委買張數,
                                                        stock.委賣張數,
                                                        stock.成交金額_元,
                                                        stock.均價,
                                                        stock.週轉率
                                                    })
                                                    .AsEnumerable()
                                                    .OrderByDescending(stock => stock.日期)
                                                    .ThenBy(stock => stock.股票代號)//排序
                                                    .SelectMany(stock => stock.GetType().GetProperties(), //將資料中所有屬性質抽出
                                                                                            (stock, property) => new
                                                                                            {
                                                                                                data = property.GetValue(stock), name = property.Name
                                                                                            }
                                                                            )
                                                     .GroupBy(data => data.name, data => data.data)//依屬性名分組
                                                     .ToList().Select(data => new string[] { data.Key }.Concat(data)); //分組結果要再加上屬性名
         }
    }
}
