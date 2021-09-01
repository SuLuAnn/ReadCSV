using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper
{
    public class Stock
    {
        public string 投票日期 { get; set; }
        public string 證券代號 { get; set; }
        public string 證券名稱 { get; set; }
        public string 召集人 { get; set; }
        public string 股東會日期 { get; set; }
        public string 發行代理機構 { get; set; }
        public string 聯絡電話 { get; set; }
        public Nullable<System.DateTime> CTIME { get; set; }
        public Nullable<long> MTIME { get; set; }
    }
}
