using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqUserInterface.Dto
{
    /// <summary>
    /// 呈現Q4-d計算後原始資料分頁的物件
    /// </summary>
    public class OriginalDataDto
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
        /// 上漲表現
        /// </summary>
        public int 上漲表現 { get; set; }

        /// <summary>
        /// 下跌表現
        /// </summary>
        public int 下跌表現 { get; set; }
    }
}
