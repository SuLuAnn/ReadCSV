using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readcsv2020LuAnn.Dto
{
    /// <summary>
    /// 第二個框框​計算股票總額的物件
    /// </summary>
    public class TotalStockDto
    {
        /// <summary>
        /// TotalStockDto建構子，用一筆股票資料建立
        /// </summary>
        /// <param name="stock">一筆股票資料</param>
        public TotalStockDto(Stock stock)
        {
            StockID = stock.StockID;
            StockName = stock.StockName;
            BuyTotal = stock.BuyQty;
            SellTotal = stock.SellQty;
            TotalPrice = stock.Price * (stock.SellQty + stock.BuyQty);
            SecBrokerID = new HashSet<string> 
            {
                stock.SecBrokerID
            };
        }

        /// <summary>
        /// 股票代號
        /// </summary>
        public string StockID { get; set; }

        /// <summary>
        /// 股票名稱
        /// </summary>
        public string StockName { get; set; }

        /// <summary>
        /// 買進總量
        /// </summary>
        public int BuyTotal { get; set; }

        /// <summary>
        /// 賣出總量
        /// </summary>
        public int SellTotal { get; set; }

        /// <summary>
        /// 加權平均
        /// </summary>
        public decimal AvgPrice { get; set; }

        /// <summary>
        /// 買賣超
        /// </summary>
        public int BuySellOver { get; set; }

        /// <summary>
        /// 券商總數
        /// </summary>
        public int SecBrokerCnt { get; set; }

        /// <summary>
        /// 所有交易數量*價格的總額
        /// </summary>
        private decimal TotalPrice;

        /// <summary>
        /// 所有券商id
        /// </summary>
        private HashSet<string> SecBrokerID;

        /// <summary>
        /// 將這筆股票資料存進來
        /// </summary>
        /// <param name="stock">股票資料</param>
        public void Add(Stock stock)
        {
            BuyTotal += stock.BuyQty;
            SellTotal += stock.SellQty;
            TotalPrice += stock.Price * (stock.SellQty + stock.BuyQty);
            SecBrokerID.Add(stock.SecBrokerID);
        }

        /// <summary>
        /// 所有資料存完後，要計算AvgPrice和券商數量
        /// </summary>
        public void SettleTotal()
        {
            if (TotalPrice == 0)
            {
                AvgPrice = 0;
            }
            else 
            {
                AvgPrice = TotalPrice / (BuyTotal + SellTotal);
            }
            SecBrokerCnt = SecBrokerID.Count();
            BuySellOver = BuyTotal - SellTotal;
        }

    }

}
