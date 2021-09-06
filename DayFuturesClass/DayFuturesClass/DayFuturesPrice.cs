using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
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
        private XDocument TotalDocument;
        public DayFuturesPrice() : base("日期貨盤後行情表_luann")
        {
            TotalDocument = new XDocument();
        }

        public override void GetXML()
        {
            IEnumerable<string[]> datas = OriginalWeb.First().Trim().Split('\n').Skip(1).Select(data => data.Split(',')).Where(fields => fields[(int)Futures.TRADING_HOURS] == "盤後");
            TotalDocument = new XDocument(new XElement("Root",
            datas.Select(data =>
            new XElement("Data",
                new XElement("交易日期", data[(int)Futures.TRANSACTION_DATE].Replace("/", string.Empty)),
                new XElement("契約", data[(int)Futures.CONTRACT]),
                new XElement("到期月份_週別", data[(int)Futures.EXPIRY_MONTH].Trim()),
                decimal.TryParse(data[(int)Futures.OPENING_PRICE], out decimal openPrice) ? new XElement("開盤價", (decimal?)openPrice) : null,
                decimal.TryParse(data[(int)Futures.HIGHEST_PRICE], out decimal highPrice) ? new XElement("最高價",  (decimal?)highPrice) : null,
                decimal.TryParse(data[(int)Futures.LOWEST_PRICE], out decimal lowPrice) ? new XElement("最低價",  (decimal?)lowPrice) : null,
                decimal.TryParse(data[(int)Futures.CLOSING_PRICE], out decimal closePrice) ? new XElement("收盤價", (decimal?)closePrice) : null
                )
            )));
            string fileName = Path.Combine(CreatDirectory(DateTime.Now.Year.ToString()), "日期貨盤後行情表.xml");
            SaveXml(TotalDocument, fileName);
        }

        public override void WriteDatabase(SqlConnection SQLConnection)
        {
            SqlDataAdapter sql = new SqlDataAdapter("SELECT * FROM 日期貨盤後行情表_luann", SQLConnection);
            DataTable table = new DataTable();
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(TotalDocument.CreateReader());
            dataSet.AcceptChanges();
            //SqlBulkCopy bulkCopy = new SqlBulkCopy(SQLConnection);
            //bulkCopy.DestinationTableName = "日期貨盤後行情表_luann";
            //bulkCopy.ColumnMappings.Add("交易日期", "交易日期");
            //bulkCopy.ColumnMappings.Add("契約", "契約");
            //bulkCopy.ColumnMappings.Add("到期月份_週別", "[到期月份(週別)]");
            //bulkCopy.ColumnMappings.Add("開盤價", "開盤價");
            //bulkCopy.ColumnMappings.Add("最高價", "最高價");
            //bulkCopy.ColumnMappings.Add("最低價", "最低價");
            //bulkCopy.ColumnMappings.Add("收盤價", "收盤價");
            dataSet.Tables[0].Columns["到期月份_週別"].ColumnName = "到期月份(週別)";
            SQLConnection.Open();
            //bulkCopy.WriteToServer(dataSet.Tables[0]);
            sql.Fill(table);
            table.AcceptChanges();
            table.PrimaryKey = new DataColumn[] { table.Columns["交易日期"], table.Columns["契約"], table.Columns["到期月份(週別)"] };
            dataSet.Tables[0].PrimaryKey = new DataColumn[] { dataSet.Tables[0].Columns["交易日期"], dataSet.Tables[0].Columns["契約"], dataSet.Tables[0].Columns["到期月份(週別)"] };
            table.Merge(dataSet.Tables[0]);
            SqlCommandBuilder builder = new SqlCommandBuilder(sql);
            sql.Update(table);
            SQLConnection.Close();
        }
    }
}
