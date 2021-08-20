using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LinqUserInterface.Global;

namespace LinqUserInterface
{
    /// <summary>
    /// 存所有靜態屬性
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// 交易所產業分類下拉選單要加"無分類"這選項
        /// </summary>
        public const string NO_CLASSIFICATION = "無分類";

        /// <summary>
        /// 2008年總統大選日期
        /// </summary>
        public const string PRESIDENTIAL_DATE_2008 = "20080322";

        /// <summary>
        /// 2012年總統大選日期
        /// </summary>
        public const string PRESIDENTIAL_DATE_2012 = "20120114";

        /// <summary>
        /// 總統大選日
        /// </summary>
        public static readonly string[] PRESIDENTIAL_DATE = { "20080322", "20120114" }; 

        /// <summary>
        /// 總統大選後30天
        /// </summary>
        public const int TRADING_DAY_30TH = 30;

        /// <summary>
        /// 總統大選後100天
        /// </summary>
        public const int TRADING_DAY_100TH = 100;

        /// <summary>
        /// 年有4個字
        /// </summary>
        public const int YEAR_WORDS = 4;

        /// <summary>
        /// 前5筆
        /// </summary>
        public const int TOP_5 = 5;

        /// <summary>
        /// 前20筆
        /// </summary>
        public const int TOP_20 = 20;
    }
}
