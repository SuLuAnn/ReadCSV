using Readcsv2020LuAnn.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Readcsv2020LuAnn
{
    public class Stock
    {

        /// <summary>
        /// 股票代號
        /// </summary>
        public string StockID { get; set; }
        /// <summary>
        /// 股票名稱
        /// </summary>
        public string StockName { get; set; }
        /// <summary>
        /// 存該隻股票的所有交易紀錄
        /// </summary>
        public List<Record> StockRecord { get; set; }
        /// <summary>
        ///股票的建構子
        /// </summary>
        /// <param name="datas">傳入一個字串陣列放入從csv讀取到的一檔股票交易紀錄的資料</param>
        public Stock(string[] datas)
        {
            StockID = datas[(int)Column.STOCK_ID];
            StockName = datas[(int)Column.STOCK_NAME];
            StockRecord = new List<Record>();
            AddRecord(datas);
        }
        /// <summary>
        /// 將csv轉檔時欄位對應位置的enum
        /// </summary>
        public enum Column 
        {
            DEAL_DATE,
            STOCK_ID,
            STOCK_NAME,
            SEC_BROKER_ID,
            SEC_BROKER_NAME,
            PRICE,
            BUY_QTY,
            SELL_QTY
        }
        /// <summary>
        /// 新增一筆交易紀錄
        /// </summary>
        /// <param name="datas">交易紀錄資料</param>
        public void AddRecord(string[] datas) 
        {
            StockRecord.Add(new Record(datas));
        }

    }
}
