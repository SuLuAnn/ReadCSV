using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundNoBusinessClass
{
    public class FundDto
    {
        public string 非營業日 { get; set; }
        public string 公司代號 { get; set; }
        public string 基金統編 { get; set; }
        public string 基金名稱 { get; set; }
        public Nullable<byte> 排序 { get; set; }
    }
}
