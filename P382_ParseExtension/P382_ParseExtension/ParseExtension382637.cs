using JParser;
using System;
using System.Data;
using System.Globalization;

namespace P382_ParseExtension
{
    /// <summary>
    /// 新增一筆資料，淨值日期為資料中最大的淨值日期的下一天，淨值為資料中最大的淨值日期的淨值
    /// </summary>
    public class ParseExtension382637
    {
        /// <summary>
        /// 淨值日期的常數
        /// </summary>
        public const string NET_WORTH_DAY = "淨值日期";

        /// <summary>
        /// 淨值的常數
        /// </summary>
        public const string NET_WORTH = "淨值";

        /// <summary>
        /// 日期格式
        /// </summary>
        public const string DAY_FORMAT = "yyyyMMdd";

        /// <summary>
        /// datatable第一行的位置
        /// </summary>
        public const int FIRST_POSITION = 0;

        /// <summary>
        /// 新增一筆資料，淨值日期為資料中最大的淨值日期的下一天，淨值為資料中最大的淨值日期的淨值的擴充方法
        /// </summary>
        /// <param name="parseData">解析的資料</param>
        public static void CopyMaxDay(ParseData parseData)
        {
            DataTable netWorthTable = parseData.CreateTable;
            //取出所有資料中淨值日期最大的那行
            DataRow maxDayRow = netWorthTable.Select(string.Empty, "淨值日期 DESC")[FIRST_POSITION];
            DataRow newRow = netWorthTable.NewRow();
            //將最大的淨值日期轉成DateTime
            DateTime.TryParseExact(maxDayRow.Field<string>(NET_WORTH_DAY), DAY_FORMAT, null, DateTimeStyles.None, out DateTime date);
            //將DateTime加一天再轉字串賦值給新的一行
            newRow.SetField<string>(NET_WORTH_DAY, date.AddDays(1).ToString(DAY_FORMAT));
            //將淨值日期最大的那行的淨值賦值給新的一行
            newRow.SetField<string>(NET_WORTH, maxDayRow.Field<string>(NET_WORTH));
            //將新行插入開頭，在淨值日期最大的那行的前面
            netWorthTable.Rows.InsertAt(newRow, FIRST_POSITION);
        }
    }
}
