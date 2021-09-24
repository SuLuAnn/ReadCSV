using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P382_日期貨盤後行情表_luann
{
    /// <summary>
    /// 放所有共用常數的類
    /// </summary>
    public class GlobalConst
    {
        /// <summary>
        /// 日期貨盤後行情表資料表欄位名稱CTIME
        /// </summary>
        public const string CTIME = "CTIME";

        /// <summary>
        /// 日期貨盤後行情表資料表欄位名稱MTIME
        /// </summary>
        public const string MTIME = "MTIME";

        /// <summary>
        /// 日期貨盤後行情表資料表欄位名稱日期
        /// </summary>
        public const string DATE = "日期";

        /// <summary>
        /// 日期貨盤後行情表資料表欄位名稱代號
        /// </summary>
        public const string CODENAME = "代號";

        /// <summary>
        /// 日期貨盤後行情表資料表欄位名稱名稱
        /// </summary>
        public const string NAME = "名稱";

        /// <summary>
        /// 日期貨盤後行情表資料表欄位名稱輸出代號
        /// </summary>
        public const string OUTPUT_CODE = "輸出代號";

        /// <summary>
        /// 日期貨盤後行情表資料表欄位名稱交割月份
        /// </summary>
        public const string DELIVERY_MONTH = "交割月份";

        /// <summary>
        /// 日期貨盤後行情表資料表欄位名稱開盤價
        /// </summary>
        public const string OPENING_PRICE = "開盤價";

        /// <summary>
        /// 日期貨盤後行情表資料表欄位名稱最高價
        /// </summary>
        public const string HIGHEST_PRICE = "最高價";

        /// <summary>
        /// 日期貨盤後行情表資料表欄位名稱最低價
        /// </summary>
        public const string LOWEST_PRICE = "最低價";

        /// <summary>
        /// 日期貨盤後行情表資料表欄位名稱收盤價
        /// </summary>
        public const string CLOSING_PRICE = "收盤價";

        /// <summary>
        /// 日期貨盤後行情表資料表欄位名稱漲跌
        /// </summary>
        public const string UP_DOWN = "漲跌";

        /// <summary>
        /// 日期貨盤後行情表資料表欄位名稱成交量
        /// </summary>
        public const string VOLUME = "成交量";

        /// <summary>
        /// 日期貨盤後行情表資料表欄位名稱未沖銷契約數
        /// </summary>
        public const string CONTRACT_NUMBER = "未沖銷契約數";

        /// <summary>
        /// 中介資料的欄位名稱交易日期
        /// </summary>
        public const string TRANSACTION_DATE = "交易日期";

        /// <summary>
        /// 中介資料的欄位名稱契約
        /// </summary>
        public const string CONTRACT = "契約";

        /// <summary>
        /// 中介資料的欄位名稱到期月份(週別)
        /// </summary>
        public const string EXPIRY_MONTH = "到期月份(週別)";

        /// <summary>
        /// 中介資料的欄位名稱漲跌價
        /// </summary>
        public const string UP_DOWN_PRICE = "漲跌價";

        /// <summary>
        /// 日期貨盤後行情表資料表欄位名稱輸出代號最後都要加PM
        /// </summary>
        public const string PM = "PM";
    }
}
