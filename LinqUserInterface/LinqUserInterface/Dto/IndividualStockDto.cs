using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqUserInterface.Dto
{
    /// <summary>
    /// 呈現Q.4-d共用頁的物件
    /// </summary>
    public class IndividualStockDto
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
        ///  項目
        /// </summary>
        public string 項目 { get; set; }

        /// <summary>
        /// 是否存在
        /// </summary>
        public string 是否存在 { get; set; }
    }
}
