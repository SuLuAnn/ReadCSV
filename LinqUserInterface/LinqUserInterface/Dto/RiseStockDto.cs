using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqUserInterface
{
    public class RiseStockDto
    {
        public string 年度 { get; set; }
        public string 類股股票代號 { get; set; }
        public string 類股股票名稱 { get; set; }
        public string 第30個交易日日期 { get; set; }
        public decimal? 第30個交易日收盤價 { get; set; }
        public string 第1個交易日日期 { get; set; }
        public decimal? 第1個交易日收盤價 { get; set; }
        public decimal? 股價漲跌 { get; set; }
    }
}
