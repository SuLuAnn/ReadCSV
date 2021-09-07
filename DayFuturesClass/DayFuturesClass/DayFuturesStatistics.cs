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
using System.Xml;
using System.Xml.Linq;

namespace DayFuturesClass
{
    [ExportMetadata("TableName", "日期貨盤後統計表")]
    [Export(typeof(IDataSheet))]
    public class DayFuturesStatistics : DayFutures
    {
        private XDocument TotalDocument;
        public DayFuturesStatistics() : base("日期貨盤後統計表_luann")
        {
            TotalDocument = new XDocument();
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
            TotalDocument = new XDocument(new XElement("Root",
            datas.Select(data =>
            new XElement("Data",
                new XElement("交易日期", data.交易日期),
                new XElement("契約", data.契約),
                data.開盤價 != null ? new XElement("開盤價", data.開盤價) : null,
                data.最高價 != null ? new XElement("最高價", data.最高價) : null,
                data.最低價 != null ? new XElement("最低價", data.最低價) : null,
                data.收盤價 != null ? new XElement("收盤價", data.收盤價) : null
                )
            )));
            string fileName = Path.Combine(CreatDirectory(DateTime.Now.Year.ToString()), "日期貨盤後統計表.xml");
            SaveXml(TotalDocument, fileName);
        }

        public override void WriteDatabase(SqlConnection SQLConnection)
        {
            SqlDataAdapter sql = new SqlDataAdapter("SELECT * FROM 日期貨盤後統計表_luann", SQLConnection);
            DataTable table = new DataTable();
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(TotalDocument.CreateReader());
            sql.InsertCommand = new SqlCommand("INSERT INTO [dbo].[日期貨盤後統計表_luann]([交易日期],[契約],[開盤價],[最高價],[最低價],[收盤價])VALUES(@交易日期, @契約,@開盤價,@最高價,@最低價,@收盤價)", SQLConnection);
            sql.InsertCommand.Parameters.Add("@交易日期", SqlDbType.Char, 8, "交易日期");
            sql.InsertCommand.Parameters.Add("@契約", SqlDbType.VarChar, 3, "契約");
            sql.InsertCommand.Parameters.Add("@開盤價", SqlDbType.Decimal, 9, "開盤價");
            sql.InsertCommand.Parameters.Add("@最高價", SqlDbType.Decimal, 9, "最高價");
            sql.InsertCommand.Parameters.Add("@最低價", SqlDbType.Decimal, 9, "最低價");
            sql.InsertCommand.Parameters.Add("@收盤價", SqlDbType.Decimal, 9, "收盤價");
            sql.UpdateCommand = new SqlCommand("UPDATE [dbo].[日期貨盤後統計表_luann] SET  [開盤價] = @開盤價,[最高價] = @最高價,[最低價] = @最低價,[收盤價] = @收盤價,[MTIME] = @MTIME WHERE [交易日期] = @交易日期 AND [契約] = @契約", SQLConnection);
            sql.UpdateCommand.Parameters.Add("@交易日期", SqlDbType.Char, 8, "交易日期");
            sql.UpdateCommand.Parameters.Add("@契約", SqlDbType.VarChar, 3, "契約");
            sql.UpdateCommand.Parameters.Add("@開盤價", SqlDbType.Decimal, 9, "開盤價");
            sql.UpdateCommand.Parameters.Add("@最高價", SqlDbType.Decimal, 9, "最高價");
            sql.UpdateCommand.Parameters.Add("@最低價", SqlDbType.Decimal, 9, "最低價");
            sql.UpdateCommand.Parameters.Add("@收盤價", SqlDbType.Decimal, 9, "收盤價");
            sql.UpdateCommand.Parameters.Add("@MTIME", SqlDbType.BigInt).Value = DateTimeOffset.Now.ToUnixTimeSeconds();
            SQLConnection.Open();
            sql.Fill(table);
            //dataSet.Tables[0].Rows[0]["交易日期"] = "000001010";
            //dataSet.Tables[0].Rows[1]["收盤價"] = 0;
            table.PrimaryKey = new DataColumn[] { table.Columns["交易日期"], table.Columns["契約"]};
            dataSet.Tables[0].PrimaryKey = new DataColumn[] { dataSet.Tables[0].Columns["交易日期"], dataSet.Tables[0].Columns["契約"] };
            table.Merge(dataSet.Tables[0], false, MissingSchemaAction.Ignore);
            sql.Update(table);
            SQLConnection.Close();
        }
    }
}
