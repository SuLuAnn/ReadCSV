using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqUserInterface
{
    /// <summary>
    /// 呈現4-a上漲所有類股查詢分頁的物件
    /// </summary>
    public class RiseStockDto
    {
        /// <summary>
        /// 年度
        /// </summary>
        public string 年度 { get; set; }

        /// <summary>
        /// 類股股票代號
        /// </summary>
        public string 類股股票代號 { get; set; }

        /// <summary>
        /// 類股股票名稱
        /// </summary>
        public string 類股股票名稱 { get; set; }

        /// <summary>
        /// 第30個交易日日期
        /// </summary>
        public string 第30個交易日日期 { get; set; }

        /// <summary>
        /// 第30個交易日收盤價
        /// </summary>
        public decimal? 第30個交易日收盤價 { get; set; }

        /// <summary>
        /// 第1個交易日日期
        /// </summary>
        public string 第1個交易日日期 { get; set; }

        /// <summary>
        /// 第1個交易日收盤價
        /// </summary>
        public decimal? 第1個交易日收盤價 { get; set; }

        /// <summary>
        /// 股價漲跌
        /// </summary>
        public decimal? 股價漲跌 { get; set; }
    }
}
