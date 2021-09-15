using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DayFuturesClass
{
    /// <summary>
    /// 資料來源為日期貨盤後交易行情的抽象類
    /// </summary>
    public abstract class DayFutures : IDataSheet
    {
        /// <summary>
        /// 存物件所對應的資料表名稱
        /// </summary>
        public string DataTableName { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="dataTableName">物件所產生的資料表名</param>
        public DayFutures(string dataTableName)
        {
            DataTableName = dataTableName;
        }

        /// <summary>
        /// 取得資料表名稱，因介面沒有屬性，所以要給方法來讓主程式取得物件的資料表名稱
        /// </summary>
        /// <returns>資料表名稱</returns>
        public string GetDataTableName()
        {
            return DataTableName;
        }

        /// <summary>
        /// 取得網站原始資料，並存成csv檔
        /// </summary>
        public void GetWebs()
        {
            MultipartFormDataContent formDatas = new MultipartFormDataContent();
            formDatas.Add(new StringContent("1"), "down_type");
            formDatas.Add(new StringContent("all"), "commodity_id");
            formDatas.Add(new StringContent(DateTime.Now.ToString("yyyy/MM/dd")), "queryStartDate");
            formDatas.Add(new StringContent(DateTime.Now.ToString("yyyy/MM/dd")), "queryEndDate");
            string web = GlobalFunction.HtmlPost(GlobalConst.DAY_FUTURES, formDatas, "BIG5");
            GlobalFunction.SaveFile(web, $"{DateTime.Now.Year}.csv");
        }

        /// <summary>
        /// 取得這張表所有xml內容
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>存好的所有xml內容</returns>
        public XDocument GetTotalXml(string tableName)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "BackupFile", DateTime.Now.Year.ToString(), $"{tableName}.xml");
            XDocument TotalDocument = XDocument.Load(path);
            return TotalDocument;
        }

        /// <summary>
        /// 取得xml中介資料
        /// </summary>
        public abstract void GetXML();

        /// <summary>
        /// 將資料寫入資料庫
        /// </summary>
        public abstract void WriteDatabase();
    }
}
