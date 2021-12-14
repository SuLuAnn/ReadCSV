using CMoney.Kernel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L382_一目均衡表_luann
{
    /// <summary>
    /// 公共類，放常數及靜態方法
    /// </summary>
    public class Global
    {
        /// <summary>
        /// [一目均衡表_luann]的欄位名稱
        /// </summary>
        public const string CTIME = "CTIME";

        /// <summary>
        /// [一目均衡表_luann]的欄位名稱
        /// </summary>
        public const string MTIME = "MTIME";

        /// <summary>
        /// [一目均衡表_luann]的欄位名稱
        /// </summary>
        public const string SOURCE_ID = "SourceID";

        /// <summary>
        /// [一目均衡表_luann]的欄位名稱
        /// </summary>
        public const string STOCK_NAME = "股票名稱";

        /// <summary>
        /// [一目均衡表_luann]的欄位名稱
        /// </summary>
        public const string STOCK_ID = "股票代號";

        /// <summary>
        /// [一目均衡表_luann]的欄位名稱
        /// </summary>
        public const string DATE = "日期";

        /// <summary>
        /// [一目均衡表_luann]的欄位名稱
        /// </summary>
        public const string CLOSE_PRICE = "日收盤還原表收盤價";

        /// <summary>
        /// [一目均衡表_luann]的欄位名稱
        /// </summary>
        public const string CHANGE_LINE = "轉換線";

        /// <summary>
        /// [一目均衡表_luann]的欄位名稱
        /// </summary>
        public const string HUB_LINE = "樞紐線";

        /// <summary>
        /// [重要國際指數與個股_luann]的欄位名稱
        /// </summary>
        public const string UPPER_PROGRAM = "上端程式";

        /// <summary>
        /// [重要國際指數與個股_luann]的欄位名稱
        /// </summary>
        public const string CONNECT_STRING = "連線字串";

        /// <summary>
        /// 股票名稱對應表
        /// </summary>
        private static Dictionary<string, string> NameMapping;

        /// <summary>
        /// 股票名稱對應表的當前年份
        /// </summary>
        private static string MappingYear;

        /// <summary>
        /// 所有日期資料
        /// </summary>
        private static List<int> AllDate;

        /// <summary>
        /// 轉換線 = (近9日[最高價]+近9日[最低價])/2
        /// </summary>
        public const int DAYS_9 = 9;

        /// <summary>
        /// 樞紐線 = (近26日[最高價]+近26日最低價)/2
        /// </summary>
        public const int DAYS_26 = 26;

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

        /// <summary>
        /// 取得股票名稱對應表
        /// </summary>
        /// <param name="year">要取得的年份</param>
        /// <param name="connectString">連線字串</param>
        /// <returns>股票名稱對應表</returns>
        public static Dictionary<string, string> GetNameMapping(string year, string connectString)
        {
            if (!year.Equals(MappingYear))
            {
                string sqlCommand = $"SELECT [股票代號],[股票名稱] FROM [StockDb].[dbo].[上市櫃基本資料表] WHERE [年度] = '{year}' UNION (SELECT [代號],[名稱] FROM [StockDb].[dbo].[ETF基本資料表] WHERE [年度] = '{year}')";
                NameMapping = DB.DoQuerySQLWithSchema(sqlCommand, connectString).AsEnumerable().ToDictionary(row => row.Field<string>(STOCK_ID), row => row.Field<string>(STOCK_NAME));
                MappingYear = year;
            }

            return NameMapping;
        }

        /// <summary>
        /// 取得[日收盤還原表]的所有日期
        /// </summary>
        /// <param name="connectString">連線字串</param>
        private static void GetAllDate(string connectString)
        {
            string sqlCommand = $"SELECT DISTINCT([日期]) FROM [StockDb].[dbo].[日收盤還原表] ORDER BY [日期] DESC";
            AllDate = DB.DoQuerySQLWithSchema(sqlCommand, connectString).AsEnumerable().Select(row => int.Parse(row.Field<string>(DATE))).ToList();
        }

        /// <summary>
        /// 取得目標週期的近幾天的範圍
        /// </summary>
        /// <param name="connectString">連線字串</param>
        /// <param name="day">日期</param>
        /// <param name="dayNumber">要近多少天</param>
        /// <returns>串起來的日期字串</returns>
        public static string GetNearlyDate(string connectString, int day, int dayNumber)
        {
            string result = null;
            if (AllDate == null)
            {
                GetAllDate(connectString);
            }
            string[] days = AllDate.Where(number => number <= day).Take(dayNumber).Select(number => $"'{number}'").ToArray();
            if (days.Length == dayNumber)
            {
                result = string.Join(",", days);
            }
            return result;
        }
    }
}
