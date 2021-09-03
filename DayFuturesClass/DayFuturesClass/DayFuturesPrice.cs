using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DayFuturesClass
{
    [ExportMetadata("TableName", "日期貨盤後行情表")]
    [Export(typeof(IDataSheet))]
    public class DayFuturesPrice : DayFutures
    {
        public DayFuturesPrice() : base("日期貨盤後行情表_luann")
        {
        }

        public override void GetXML()
        {
            IEnumerable<string[]> datas = OriginalWeb.First().Trim().Split('\n').Skip(1).Select(data => data.Split(',')).Where(fields => fields[(int)Futures.TRADING_HOURS] == "盤後");
            XDocument document = new XDocument(new XElement("Root",
            datas.Select(data =>
            new XElement("Data",
                new XElement("交易日期", data[(int)Futures.TRANSACTION_DATE].Replace("/", string.Empty)),
                new XElement("契約", data[(int)Futures.CONTRACT]),
                new XElement("到期月份_週別", data[(int)Futures.EXPIRY_MONTH]),
                new XElement("開盤價", decimal.TryParse(data[(int)Futures.OPENING_PRICE], out decimal openPrice) ? (decimal?)openPrice : null),
                new XElement("最高價", decimal.TryParse(data[(int)Futures.HIGHEST_PRICE], out decimal highPrice) ? (decimal?)highPrice : null),
                new XElement("最低價", decimal.TryParse(data[(int)Futures.LOWEST_PRICE], out decimal lowPrice) ? (decimal?)lowPrice : null),
                new XElement("收盤價", decimal.TryParse(data[(int)Futures.CLOSING_PRICE], out decimal closePrice) ? (decimal?)closePrice : null)
                )
            )));
            string fileName = Path.Combine(CreatDirectory(DateTime.Now.Year.ToString()), "日期貨盤後行情表.xml");
            SaveXml(document, fileName);
        }
    }
}
