using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqUserInterface.Dto;
using static LinqUserInterface.Global;

namespace LinqUserInterface
{
    /// <summary>
    /// 第三題查詢日收盤於特定週期區間內的股票資訊
    /// </summary>
    public class PERList
    {
        /// <summary>
        /// 資料庫物件
        /// </summary>
        private StockDBEntities StockDB;

        /// <summary>
        /// 建構子，傳入資料庫物件
        /// </summary>
        /// <param name="stockDB">資料庫物件</param>
        public PERList(StockDBEntities stockDB)
        {
            StockDB = stockDB;
        }

        /// <summary>
        /// 產生產業名稱下拉選單
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetPERMenu()
        {
            int[] marketNum = new int[] //市場別須為上市上櫃
            {
                1, //上市
                2//上櫃
            };
            return StockDB.交易所產業分類代號表.AsEnumerable()
                                                                                 .Where(industry => marketNum.Contains(industry.市場別))//找出指定市場別
                                                                                 .OrderBy(industry => industry.名稱.Length)
                                                                                 .GroupBy(industry => industry.代號)//依代號分類
                                                                                 .OrderBy(industry => industry.Key)//排序
                                                                                 .ToDictionary(industry => industry.Select(stock => stock.名稱)
                                                                                                                                                  .First(), //找到該代號名稱最短的
                                                                                                           industry => industry.Key); //這個<名稱,代號>的Dictionary，當使用者選好名稱，即用這個代號查
        }

        /// <summary>
        /// 從『日收盤』中取出指定產業在指定週期區間內，『平均本益比』大於『目標本益比』的資料，並將平均本益比最高與最低的20筆
        /// </summary>
        /// <param name="industryCode">指定產業代號</param>
        /// <param name="firstDay">起始日期</param>
        /// <param name="lastDay">結束日期</param>
        /// <param name="targetERP">目標本益比</param>
        /// <returns>查詢結果</returns>
        public List<PERDto> GetPERData(string industryCode, string firstDay, string lastDay, decimal targetERP)
        {
            decimal decimalPlaces = 1000000m; //處理本益比小數位數用
            var records = StockDB.日收盤.Where(data => data.產業代號 == industryCode
                                                                                                                      && string.Compare(data.日期, firstDay) >= 0
                                                                                                                      && string.Compare(data.日期, lastDay) <= 0)
                                                                                                                      .Select(stock => new
                                                                                                                      {
                                                                                                                          stock.股票代號,
                                                                                                                          stock.股票名稱,
                                                                                                                          stock.本益比,
                                                                                                                      })
                                                                                                                      .AsEnumerable()
                                                                                        .OrderBy(data => data.股票名稱.Length)
                                                                                        .GroupBy(data => data.股票代號, (stockID, stocks) => new
                                                                                        {
                                                                                            股票代號 = stockID,
                                                                                            平均本益比 = stocks.Average(erp => erp.本益比),
                                                                                            股票名稱 = stocks.FirstOrDefault().股票名稱//在這裡取first會"等候操作已逾時"
                                                                                            //取指定小數位數的平均本益比
                                                                                        })
                                                                                        .Where(data => data.平均本益比 != null && data.平均本益比 > targetERP)
                                                                                        .OrderByDescending(data => data.平均本益比).ToList();
            List<PERDto> top20 = records.Take(TOP_20).Union(records.AsEnumerable().Reverse()
                                                                                                                            .Take(TOP_20))
                                                                     .Select(stock => new PERDto
                                                                     {
                                                                         股票代號 = stock.股票代號,
                                                                         股票名稱 = stock.股票名稱,
                                                                         平均本益比 = (Math.Floor((decimal)stock.平均本益比 * decimalPlaces) / decimalPlaces).ToString("0.000000")
                                                                     })
                                                                     .ToList(); //取本益比前後20名
            return top20;
         }
    }
}
