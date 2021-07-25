using Readcsv2020LuAnn.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Readcsv2020LuAnn
{
    /// <summary>
    /// 存放一筆csv資料
    /// </summary>
    public class Stock
    {
        /// <summary>
        /// 交易日期
        /// </summary>
        public string DealDate { get; set; }

        /// <summary>
        /// 股票代號
        /// </summary>
        public string StockID { get; set; }

        /// <summary>
        /// 股票名稱
        /// </summary>
        public string StockName { get; set; }

        /// <summary>
        /// 券商代號
        /// </summary>
        public string SecBrokerID { get; set; }

        /// <summary>
        /// 券商名稱
        /// </summary>
        public string SecBrokerName { get; set; }

        /// <summary>
        /// 股價
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 買進數量
        /// </summary>
        public int BuyQty { get; set; }

        /// <summary>
        /// 賣出數量
        /// </summary>
        public int SellQty { get; set; }

        /// <summary>
        ///股票的建構子
        /// </summary>
        /// <param name="datas">傳入一個字串陣列放入從csv讀取到的一檔股票交易紀錄的資料</param>
        public Stock(string[] datas)
        {
            StockID = datas[(int)Column.STOCK_ID];
            StockName = datas[(int)Column.STOCK_NAME];
            DealDate = datas[(int)Stock.Column.DEAL_DATE];
            SecBrokerID = datas[(int)Stock.Column.SEC_BROKER_ID];
            SecBrokerName = datas[(int)Stock.Column.SEC_BROKER_NAME];
            Price = decimal.Parse(datas[(int)Stock.Column.PRICE]);
            BuyQty = int.Parse(datas[(int)Stock.Column.BUY_QTY]);
            SellQty = int.Parse(datas[(int)Stock.Column.SELL_QTY]);
        }

        /// <summary>
        /// 將csv轉檔時欄位對應位置的enum
        /// </summary>
        public enum Column 
        {
            /// <summary>
            /// 交易日在第零個
            /// </summary>
            DEAL_DATE,

            /// <summary>
            /// 股票代號在第一個
            /// </summary>
            STOCK_ID,

            /// <summary>
            /// 股票名在第二個
            /// </summary>
            STOCK_NAME,

            /// <summary>
            /// 券商代號在第三個
            /// </summary>
            SEC_BROKER_ID,

            /// <summary>
            /// 券商名稱在第四個
            /// </summary>
            SEC_BROKER_NAME,

            /// <summary>
            /// 股價在第五個
            /// </summary>
            PRICE,

            /// <summary>
            /// 買進量在第六個
            /// </summary>
            BUY_QTY,

            /// <summary>
            /// 賣出量在第七個
            /// </summary>
            SELL_QTY
        }

    }

}
