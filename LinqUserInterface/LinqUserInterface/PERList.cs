using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqUserInterface.Dto;
using static LinqUserInterface.Global;

namespace LinqUserInterface
{
    public class PERList
    {
        /// <summary>
        /// 資料庫物件
        /// </summary>
        private StockDBEntities StockDB;
        public PERList(StockDBEntities stockDB)
        {
            StockDB = stockDB;
        }

        public Dictionary<string, string> GetPERMenu()
        {
            int[] marketNum = new int[]
            {
                1, 2
            };
            return StockDB.交易所產業分類代號表
                .Where(industry => marketNum.Contains(industry.市場別))
                .GroupBy(industry => industry.代號)
                .OrderBy(industry => industry.Key)
                .ToDictionary(industry => industry.Select(stock=>stock.名稱).Aggregate((first,second)=>first.Length<second.Length ? first : second)
                                        , industry => industry.Key);
        }

        public List<PERDto> GetPERData(string industryCode,string firstDay, string lastDay,decimal targetERP)
        {
            int decimalPlaces = 6;
            List<PERDto> records = StockDB.日收盤.Where(data => data.產業代號 == industryCode && string.Compare(data.日期, firstDay) >= 0
      && string.Compare(data.日期, lastDay) <= 0).GroupBy(data => data.股票代號, data => data, (stockID, stocks) => new 
      {
          股票代號 = stockID,
          股票名稱 = stocks.Select(stock=>stock.股票名稱),
          平均本益比 = decimal.Round((decimal)(stocks.Average(erp => erp.本益比)), decimalPlaces)
      }).Where(data => data.平均本益比 > targetERP).OrderByDescending(data => data.平均本益比)
      .ToList()
      .Select(stock=>new PERDto {
          股票代號=stock.股票代號,
          股票名稱=stock.股票名稱.Aggregate((first, second) => first.Length < second.Length ? first : second),
          平均本益比 =stock.平均本益比
      }).ToList();
            List<PERDto> top20 = records.Take(TOP_20).Union(records.OrderBy(data => data.平均本益比).Take(TOP_20)).ToList();
            return top20;
        }
    }
}
