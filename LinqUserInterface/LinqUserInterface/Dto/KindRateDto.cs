using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqUserInterface.Dto
{
    /// <summary>
    /// 類股報酬率物件
    /// </summary>
    public class KindRateDto
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
        /// C200707整併後交易所產業編號
        /// </summary>
        public string C200707整併後交易所產業編號 { get; set; }

        /// <summary>
        /// 上市櫃
        /// </summary>
        public string 上市櫃 { get; set; }
    }
}
