using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readcsv2020LuAnn
{
    public class Record
    {
        /// <summary>
        /// 交易日期
        /// </summary>
        public string DealDate { get; set; }
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
        public double Price { get; set; }
        /// <summary>
        /// 買進數量
        /// </summary>
        public int BuyQty { get; set; }
        /// <summary>
        /// 賣出數量
        /// </summary>
        public int SellQty { get; set; }
        public Record(string[] datas) 
        {
            DealDate = datas[(int)Stock.Column.DEAL_DATE];
            SecBrokerID = datas[(int)Stock.Column.SEC_BROKER_ID];
            SecBrokerName = datas[(int)Stock.Column.SEC_BROKER_NAME];
            Price = double.Parse(datas[(int)Stock.Column.PRICE]);
            BuyQty = int.Parse(datas[(int)Stock.Column.BUY_QTY]);
            SellQty = int.Parse(datas[(int)Stock.Column.SELL_QTY]);
        }
    }
}
