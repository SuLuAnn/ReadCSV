using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
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
        /// 日期貨盤後交易行情的網址
        /// </summary>
        public const string DAY_FUTURES = "https://www.taifex.com.tw/cht/3/dlFutDataDown"; 
        /// <summary>
        /// 取回的html原始資料
        /// </summary>
        public string OriginalWeb { get; set; }

        /// <summary>
        /// 原始資料產生的xml中介資料
        /// </summary>
        public XDocument TotalDocument { get; set; }

        /// <summary>
        /// 原始資料切割後的陣列與陣列內容的對應
        /// </summary>
        public enum Futures : int
        {
            TRANSACTION_DATE = 0,
            CONTRACT = 1,
            EXPIRY_MONTH = 2,
            OPENING_PRICE = 3,
            HIGHEST_PRICE = 4,
            LOWEST_PRICE = 5,
            CLOSING_PRICE = 6,
            TRADING_HOURS = 17
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="dataTableName">物件所產生的資料表名</param>
        public DayFutures(string dataTableName) : base(dataTableName)
        {
            TotalDocument = new XDocument();
        }

        /// <summary>
        /// 取得網站原始資料，並存成csv檔
        /// </summary>
        public override void GetWebs()
        {
            MultipartFormDataContent headers = new MultipartFormDataContent();
            headers.Add(new StringContent("1"), "down_type");
            headers.Add(new StringContent("all"), "commodity_id");
            headers.Add(new StringContent(DateTime.Now.ToString("yyyy/MM/dd")), "queryStartDate");
            headers.Add(new StringContent(DateTime.Now.ToString("yyyy/MM/dd")), "queryEndDate");
            string web = HtmlPost(DAY_FUTURES, headers, "BIG5");
            SaveFile(web, $"{DateTime.Now.Year}.csv");
            OriginalWeb = web;
        }
    }
}
