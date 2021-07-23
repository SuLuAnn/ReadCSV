using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readcsv2020LuAnn.Dto
{
    public class StockDto
    {

        /// <summary>
        /// 交易日期
        /// </summary>
        public string DealDate { get; set; }
        /// <summary>
        /// 股票代號
        /// </summary>
        public string StockID { get; set; }
        /// <summary>
        /// 股票名稱
        /// </summary>
        public string StockName { get; set; }
        /// <summary>
        /// 券商代號
        /// </summary>
        public string SecBrokerID { get; set; }
        /// <summary>
        /// 券商名稱
        /// </summary>
        public string SecBrokerName { get; set; }
        /// <summary>
        /// 股價
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 買進數量
        /// </summary>
        public int BuyQty { get; set; }
        /// <summary>
        /// 賣出數量
        /// </summary>
        public int SellQty { get; set; }
    }
}
