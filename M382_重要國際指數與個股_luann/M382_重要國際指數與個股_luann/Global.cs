using DataProc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M382_重要國際指數與個股_luann
{
    /// <summary>
    /// 公用類
    /// </summary>
    public class Global
    {
        /// <summary>
        /// 要做dacimal轉換的欄位名稱
        /// </summary>
        public static readonly List<string> COLUMN_NAMES = new List<string>
        {
            "開盤價", "最高價", "最低價", "收盤價"
        };

        /// <summary>
        /// [重要國際指數與個股_luann]的欄位名稱
        /// </summary>
        public const string CTIME = "CTIME";

        /// <summary>
        /// [重要國際指數與個股_luann]的欄位名稱
        /// </summary>
        public const string MTIME = "MTIME";

        /// <summary>
        /// [重要國際指數與個股_luann]的欄位名稱
        /// </summary>
        public const string SOURCE_ID = "SourceID";

        /// <summary>
        /// [重要國際指數與個股_luann]的欄位名稱
        /// </summary>
        public const string DATE = "日期";

        /// <summary>
        /// [重要國際指數與個股_luann]的欄位名稱
        /// </summary>
        public const string STOCK_ID = "代號";

        /// <summary>
        /// [重要國際指數與個股_luann]的欄位名稱
        /// </summary>
        public const string UPPER_PROGRAM = "上端程式";

        /// <summary>
        /// [重要國際指數與個股_luann]的欄位名稱
        /// </summary>
        public const string CONNECT_STRING = "連線字串";

        /// <summary>
        /// 寄信訊息等級
        /// </summary>
        public enum MessagePart : int
        {
            /// <summary>
            /// 紀錄程式執行過程，會保留每個訊息
            /// </summary>
            PROCESS = 2,

            /// <summary>
            /// 程式判斷可能有問題時的警告訊息
            /// </summary>
            WARN = 3,

            /// <summary>
            /// 程式出錯
            /// </summary>
            ERROR = 4,

            /// <summary>
            /// 通知信
            /// </summary>
            NOTICE = 5,

            /// <summary>
            /// 只保留最後一筆訊息
            /// </summary>
            LAST_MESSAGE = 6
        }
    }
}
