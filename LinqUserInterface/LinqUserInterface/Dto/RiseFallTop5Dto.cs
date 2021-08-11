using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqUserInterface.Dto
{
    /// <summary>
    /// 呈現Q4-d類股報酬率最高、最低前五名分頁資訊
    /// </summary>
    public class RiseFallTop5Dto
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
        /// 項目
        /// </summary>
        public string 項目 { get; set; }

        /// <summary>
        /// 排名
        /// </summary>
        public int 排名 { get; set; }
    }
}
