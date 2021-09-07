using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 所有產生表的物件共通繼承的介面
    /// </summary>
    public interface IDataSheet
    {
        /// <summary>
        /// 取得要產的資料表的名稱
        /// </summary>
        /// <returns>資料表名稱</returns>
        string GetDataTableName();

        /// <summary>
        /// 從網站取得原始資料內容
        /// </summary>
        void GetWebs();

        /// <summary>
        /// 解析原始資料取得xml檔的中介資料
        /// </summary>
        void GetXML();

        /// <summary>
        /// 將中介資料寫入資料庫
        /// </summary>
        /// <param name="SQLConnection">資料庫連線物件</param>
        void WriteDatabase(SqlConnection SQLConnection);
    }
}
