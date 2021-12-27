using JParser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace P382_ParseExtension
{
    /// <summary>
    /// 為Source ID 382638寫的擴充，主要功能為將字串最後一個英文轉大寫在加1
    /// </summary>
    public class ParseExtension382638
    {
        /// <summary>
        /// 證券代號
        /// </summary>
        public const string SECURITIES_ID = "代號";

        /// <summary>
        ///  將DataTable 每個Row的證券代號欄位字串最後一個英文轉大寫在加1
        /// </summary>
        /// <param name="parseData">解析的資料</param>
        public static void ChangeCase(ParseData parseData)
        {
            DataTable securitiesTable = parseData.CreateTable;
            foreach (DataRow row in securitiesTable.Rows)
            {
                string securitiesID = row.Field<string>(SECURITIES_ID);
                int last = securitiesID.Length - 1;
                row[SECURITIES_ID] = Regex.Replace(securitiesID, @"[a-z]$", $"{char.ToUpper(securitiesID[last])}1");
            }
        }
    }
}
