using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqUserInterface.Dto
{
    /// <summary>
    /// 呈現Q4-b排名相同類股分頁的資訊
    /// </summary>
    public class RateReturn
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
        /// 報酬率
        /// </summary>
        public decimal? 報酬率 { get; set; }

        /// <summary>
        /// 報酬率排名
        /// </summary>
        public int 報酬率排名 { get; set; }
    }
}
