using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqUserInterface
{
    /// <summary>
    /// 呈現Q3"日收盤本益比優劣榜"的物件
    /// </summary>
    public class PERDto
    {
        /// <summary>
        /// 股票代號
        /// </summary>
        public string 股票代號 { get; set; }

        /// <summary>
        /// 股票名稱
        /// </summary>
        public string 股票名稱 { get; set; }

        /// <summary>
        /// 平均本益比
        /// </summary>
        public string 平均本益比 { get; set; }
    }
}
