using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readcsv2020LuAnn.Dto
{
    public class Top50Dto
    {
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
        public int BuyCellOver { get; set; }
    }
}
