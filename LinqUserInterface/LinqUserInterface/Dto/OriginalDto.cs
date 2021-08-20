using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqUserInterface.Dto
{
    /// <summary>
    /// 儲存原始資料與類股資訊
    /// </summary>
    public class OriginalDto
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
        /// 裝存在這支股票產業代號的類股們
        /// </summary>
        public IOrderedEnumerable<KindRateDto> Kind { get; set; }
    }
}
