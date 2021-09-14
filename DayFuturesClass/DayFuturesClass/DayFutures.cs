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
    public abstract class DayFutures : DataBaseTable
    {

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="dataTableName">物件所產生的資料表名</param>
        public DayFutures(string dataTableName) : base(dataTableName)
        {
        }

        /// <summary>
        /// 取得網站原始資料，並存成csv檔
        /// </summary>
        public override void GetWebs()
        {
            MultipartFormDataContent formDatas = new MultipartFormDataContent();
            formDatas.Add(new StringContent("1"), "down_type");
            formDatas.Add(new StringContent("all"), "commodity_id");
            formDatas.Add(new StringContent(DateTime.Now.ToString("yyyy/MM/dd")), "queryStartDate");
            formDatas.Add(new StringContent(DateTime.Now.ToString("yyyy/MM/dd")), "queryEndDate");
            string web = HtmlPost(GlobalConst.DAY_FUTURES, formDatas, "BIG5");
            SaveFile(web, $"{DateTime.Now.Year}.csv");
        }

        public override XDocument GetTotalXml(string tableName)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "BackupFile", DateTime.Now.Year.ToString(), $"{tableName}.xml");
            XDocument TotalDocument = XDocument.Load(path);
            return TotalDocument;
        }
    }
}
