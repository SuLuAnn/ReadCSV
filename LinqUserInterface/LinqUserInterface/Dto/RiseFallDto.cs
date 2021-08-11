using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqUserInterface.Dto
{
    /// <summary>
    /// 承接個股漲跌的計算結果
    /// </summary>
    public class RiseFallDto
    {
        /// <summary>
        /// 年度
        /// </summary>
        public string 年度 { get; set; }

        /// <summary>
        /// 股票代號
        /// </summary>
        public string 股票代號 { get; set; }

        /// <summary>
        /// 股票名稱
        /// </summary>
        public string 股票名稱 { get; set; }

        /// <summary>
        /// 上漲表現
        /// </summary>
        public int 上漲表現 { get; set; }

        /// <summary>
        /// 下跌表現
        /// </summary>
        public int 下跌表現 { get; set; }

        /// <summary>
        /// 上市櫃
        /// </summary>
        public string 上市櫃 { get; set; }

        /// <summary>
        /// 產業代號
        /// </summary>
        public string 產業代號 { get; set; }
    }
}
