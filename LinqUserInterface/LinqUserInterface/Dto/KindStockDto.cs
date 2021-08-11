using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqUserInterface
{
    /// <summary>
    /// 接從資料過來的類股資訊
    /// </summary>
    public class KindStock
    {
        /// <summary>
        /// 代號
        /// </summary>
        public string 代號 { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string 名稱 { get; set; }

        /// <summary>
        /// C200707整併後交易所產業編號
        /// </summary>
        public string C200707整併後交易所產業編號 { get; set; }
    }
}
