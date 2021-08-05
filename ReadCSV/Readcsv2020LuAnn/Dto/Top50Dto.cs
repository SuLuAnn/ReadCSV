using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readcsv2020LuAnn.Dto
{
    /// <summary>
    /// 買賣超前50存放物件
    /// </summary>
    public class Top50Dto
    {
        /// <summary>
        /// 建構子，用一筆stock資料創出物件
        /// </summary>
        /// <param name="stock">一筆stock資料</param>
        public Top50Dto(Stock stock)
        {
            StockName = stock.StockName;
            SecBrokerName = stock.SecBrokerName;
            BuySellOver = stock.BuyQty - stock.SellQty;
        }

        /// <summary>
        /// 股票名稱
        /// </summary>
        public string StockName { get; set; }

        /// <summary>
        /// 券商名稱
        /// </summary>
        public string SecBrokerName { get; set; }

        /// <summary>
        /// 該券商買賣超
        /// </summary>
        public int BuySellOver { get; set; }
    }
}
