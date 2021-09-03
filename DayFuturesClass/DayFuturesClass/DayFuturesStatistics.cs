using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DayFuturesClass
{
    [ExportMetadata("TableName", "日期貨盤後統計表")]
    [Export(typeof(IDataSheet))]
    public class DayFuturesStatistics : DayFutures
    {
        public DayFuturesStatistics() : base("日期貨盤後統計表_luann")
        {
        }

        public override void GetXML()
        {
            var datas = OriginalWeb.First().Trim().Split('\n').Skip(1).Select(data => data.Split(',')).Where(fields => fields[(int)Futures.TRADING_HOURS] == "盤後").Select(fields => new
            {
                交易日期 = fields[(int)Futures.TRANSACTION_DATE].Replace("/", string.Empty),
                契約 = fields[(int)Futures.CONTRACT],
                開盤價 = decimal.TryParse(fields[(int)Futures.OPENING_PRICE], out decimal openPrice) ? (decimal?)openPrice : null,
                最高價 = decimal.TryParse(fields[(int)Futures.HIGHEST_PRICE], out decimal highPrice) ? (decimal?)highPrice : null,
                最低價 = decimal.TryParse(fields[(int)Futures.LOWEST_PRICE], out decimal lowPrice) ? (decimal?)lowPrice : null,
                收盤價 = decimal.TryParse(fields[(int)Futures.CLOSING_PRICE], out decimal closePrice) ? (decimal?)closePrice : null
            }).GroupBy(data => new
            {
                data.交易日期,
                data.契約
            }, (key, value) => new 
            {
                交易日期 = key.交易日期,
                契約 = key.契約,
                開盤價 = value.Max(data => data.開盤價),
                最高價 = value.Max(data => data.最高價),
                最低價 = value.Min(data => data.最低價),
                收盤價 = value.Max(data => data.收盤價)
            });
            XDocument document = new XDocument(new XElement("Root",
            datas.Select(data =>
            new XElement("Data",
                new XElement("交易日期", data.交易日期),
                new XElement("契約", data.契約),
                new XElement("開盤價", data.開盤價),
                new XElement("最高價", data.最高價),
                new XElement("最低價", data.最低價),
                new XElement("收盤價", data.收盤價)
                )
            )));
            string fileName = Path.Combine(CreatDirectory(DateTime.Now.Year.ToString()), "日期貨盤後統計表.xml");
            SaveXml(document, fileName);
        }
    }
}
