using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqUserInterface.Dto
{
    /// <summary>
    /// 呈現Q4-c成交量前五大的個股分頁資訊
    /// </summary>
    public class Top5VolumeDto
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
        /// 股票代號
        /// </summary>
        public string 股票代號 { get; set; }

        /// <summary>
        /// 股票名稱
        /// </summary>
        public string 股票名稱 { get; set; }

        /// <summary>
        /// 產業代號
        /// </summary>
        public string 產業代號 { get; set; }

        /// <summary>
        /// 平均成交量
        /// </summary>
        public decimal 平均成交量 { get; set; }
    }
}
