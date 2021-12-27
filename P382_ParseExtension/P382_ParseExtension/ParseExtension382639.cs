using JParser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P382_ParseExtension
{
    /// <summary>
    /// 為Source ID 382639寫的擴充，主要功能為將相同股票代號的資料作合併
    /// </summary>
    public class ParseExtension382639
    {
        /// <summary>
        /// 來源網站欄位名稱 恢復交易日期
        /// </summary>
        public const string RECOVER_DATE = "恢復交易日期";

        /// <summary>
        /// 來源網站欄位名稱 恢復交易時間
        /// </summary>
        public const string RECOVER_TIME = "恢復交易時間";

        /// <summary>
        /// 來源網站欄位名稱 暫停交易日期
        /// </summary>
        public const string STOP_DATE = "暫停交易日期";

        /// <summary>
        /// 來源網站沒資料時顯示的符號
        /// </summary>
        public const string EMPTY_SYMBLE = "-";

        /// <summary>
        /// 將相同股票代號的資料作合併
        /// </summary>
        /// <param name="parseData">解析資料</param>
        public static void MergeSameStock(ParseData parseData)
        {
            DataTable securitiesTable = parseData.CreateTable;
            IEnumerable<IGrouping<object, DataRow>> stocks = securitiesTable.AsEnumerable().GroupBy(row => row["證券代號"]);

            foreach (IGrouping<object, DataRow> stock in stocks)
            {
                DataRow earlyStock = stock.SingleOrDefault(data => data.Field<string>(RECOVER_DATE) == EMPTY_SYMBLE);
                DataRow latelyStock = stock.SingleOrDefault(data => data.Field<string>(STOP_DATE) == EMPTY_SYMBLE);
                earlyStock[RECOVER_DATE] = latelyStock[RECOVER_DATE];
                earlyStock[RECOVER_TIME] = latelyStock[RECOVER_TIME];
                securitiesTable.Rows.Remove(latelyStock);
            }
        }
    }
}
