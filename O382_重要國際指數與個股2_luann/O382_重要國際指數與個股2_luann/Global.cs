using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O382_重要國際指數與個股2_luann
{
    /// <summary>
    /// 公用類，放常數或共用方法等
    /// </summary>
    public class Global
    {
        /// <summary>
        /// [StockDB].[dbo].[重要國際指數與個股2_luann] 的欄位名稱
        /// </summary>
        public const string DATE = "日期";

        /// <summary>
        /// [StockDB].[dbo].[重要國際指數與個股2_luann] 的欄位名稱 代號
        /// </summary>
        public const string STOCK_ID = "代號";

        /// <summary>
        /// [StockDB].[dbo].[重要國際指數與個股2_優先權_luann] 的欄位名稱 SourceID
        /// </summary>
        public const string SOURCE_ID = "SourceID";

        /// <summary>
        ///  [StockDB].[dbo].[重要國際指數與個股2_luann] 的欄位名稱 ColumnSourceID
        /// </summary>
        public const string COLUMN_SOURCE_ID = "ColumnSourceID";

        /// <summary>
        /// [StockDB].[dbo].[重要國際指數與個股2_luann] 的欄位名稱 開盤價
        /// </summary>
        public const string OPEN_PRICE = "開盤價";

        /// <summary>
        /// [StockDB].[dbo].[重要國際指數與個股2_luann] 的欄位名稱 最高價
        /// </summary>
        public const string HIGHEST_PRICE = "最高價";

        /// <summary>
        /// [StockDB].[dbo].[重要國際指數與個股2_luann] 的欄位名稱 最低價
        /// </summary>
        public const string LOWEST_PRICE = "最低價";

        /// <summary>
        /// [StockDB].[dbo].[重要國際指數與個股2_luann] 的欄位名稱 "收盤價
        /// </summary>
        public const string CLOSE_PRICE = "收盤價";

        /// <summary>
        /// [StockDB].[dbo].[重要國際指數與個股2_luann] 的欄位名稱 CTIME
        /// </summary>
        public const string CTIME = "CTIME";

        /// <summary>
        /// [StockDB].[dbo].[重要國際指數與個股2_luann] 的欄位名稱 MTIME
        /// </summary>
        public const string MTIME = "MTIME";

        /// <summary>
        /// [StockDB].[dbo].[重要國際指數與個股2_luann] 的欄位名稱 ColumnName
        /// </summary>
        public const string COLUMN_NAME = "ColumnName";

        /// <summary>
        ///  [StockDB].[dbo].[重要國際指數與個股2_luann] 的欄位名稱 Priority
        /// </summary>
        public const string PRIORITY = "Priority";

        /// <summary>
        /// 同為decimal的欄位，在新增及更新時會做相同的事
        /// </summary>
        public static readonly string[] PRICE_COLUMN_NAMES = new string[] { OPEN_PRICE, HIGHEST_PRICE, LOWEST_PRICE, CLOSE_PRICE };
    }
}
