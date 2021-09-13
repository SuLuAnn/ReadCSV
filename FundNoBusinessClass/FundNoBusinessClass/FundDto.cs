using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundNoBusinessClass
{
    /// <summary>
    /// 基金非營業日明細的物件
    /// </summary>
    public class FundDto
    {
        /// <summary>
        /// 非營業日
        /// </summary>
        public string 非營業日 { get; set; }

        /// <summary>
        /// 公司代號
        /// </summary>
        public string 公司代號 { get; set; }

        /// <summary>
        /// 基金統編
        /// </summary>
        public string 基金統編 { get; set; }

        /// <summary>
        /// 基金名稱
        /// </summary>
        public string 基金名稱 { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public byte 排序 { get; set; }
    }
}
