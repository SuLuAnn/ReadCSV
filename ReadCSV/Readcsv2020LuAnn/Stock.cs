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
        /// 交易日在第零個
        /// </summary>
        private const int DEAL_DATE = 0;

        /// <summary>
        /// 股票代號在第一個
        /// </summary>
        private const int STOCK_ID = 1;

        /// <summary>
        /// 股票名在第二個
        /// </summary>
        private const int STOCK_NAME = 2;

        /// <summary>
        /// 券商代號在第三個
        /// </summary>
        private const int SEC_BROKER_ID = 3;

        /// <summary>
        /// 券商名稱在第四個
        /// </summary>
        private const int SEC_BROKER_NAME = 4;

        /// <summary>
        /// 股價在第五個
        /// </summary>
        private const int PRICE = 5;

        /// <summary>
        /// 買進量在第六個
        /// </summary>
        private const int BUY_QTY = 6;

        /// <summary>
        /// 賣出量在第七個
        /// </summary>
        private const int SELL_QTY = 7;

        /// <summary>
        /// 股票的建構子
        /// </summary>
        /// <param name="datas">傳入一個字串陣列放入從csv讀取到的一檔股票交易紀錄的資料</param>
        public Stock(string[] datas)
        {
            StockID = datas[STOCK_ID];
            StockName = datas[STOCK_NAME];
            DealDate = datas[DEAL_DATE];
            SecBrokerID = datas[SEC_BROKER_ID];
            SecBrokerName = datas[SEC_BROKER_NAME];
            Price = decimal.Parse(datas[PRICE]);
            BuyQty = int.Parse(datas[BUY_QTY]);
            SellQty = int.Parse(datas[SELL_QTY]);
        }
    }
}
