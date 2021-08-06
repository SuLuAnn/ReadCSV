using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqUserInterface
{
    public class DayStockChange
    {
        public string 日期 { get; set; }
        public string 股票代號 { get; set; }
        public string 股票名稱 { get; set; }
        public Nullable<decimal> 參考價 { get; set; }
        public Nullable<decimal> 開盤價 { get; set; }
        public Nullable<decimal> 最高價 { get; set; }
        public Nullable<decimal> 最低價 { get; set; }
        public Nullable<decimal> 收盤價 { get; set; }
        public Nullable<decimal> 漲跌 { get; set; }
        public string 市場別名稱 { get; set; }
        public string 產業名稱 { get; set; }
        public Nullable<decimal> 漲停價 { get; set; }
        public Nullable<decimal> 跌停價 { get; set; }
        public string 漲跌狀況 { get; set; }
        public Nullable<decimal> 最後委買價 { get; set; }
        public Nullable<decimal> 最後委賣價 { get; set; }
        public Nullable<decimal> 漲幅_比率{ get; set; }
        public Nullable<decimal> 振幅_比率 { get; set; }
        public Nullable<long> 成交量_股 { get; set; }
        public Nullable<long> 成交筆數 { get; set; }
        public Nullable<decimal> 成交值比重_比率 { get; set; }
        public Nullable<decimal> 成交量變動_比率 { get; set; }
        public Nullable<decimal> 總市值_億 { get; set; }
        public Nullable<decimal> 本益比 { get; set; }
        public Nullable<int> 委買張數 { get; set; }
        public Nullable<int> 委賣張數 { get; set; }
        public Nullable<long> 成交金額_元 { get; set; }
        public Nullable<decimal> 均價 { get; set; }
        public Nullable<decimal> 週轉率 { get; set; }
    }
}
