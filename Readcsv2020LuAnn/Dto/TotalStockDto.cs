using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readcsv2020LuAnn.Dto
{
    public class TotalStockDto
    {
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
        public double AvgPrice { get; set; }
        /// <summary>
        /// 買賣超
        /// </summary>
        public int BuyCellOver { get; set; }
        /// <summary>
        /// 券商總數
        /// </summary>
        public int SecBrokerCnt { get; set; }
    }
}
