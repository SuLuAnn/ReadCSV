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
    public abstract class DayFutures
    {
        /// <summary>
        /// 取得網站原始資料，並存成csv檔
        /// </summary>
        public static void GetWebs()
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
        public static XDocument GetTotalXml(string tableName)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "BackupFile", DateTime.Now.Year.ToString(), $"{tableName}.xml");
            XDocument TotalDocument = XDocument.Load(path);
            return TotalDocument;
        }
    }
}
