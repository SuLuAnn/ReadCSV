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
        
        public DayFuturesPrice() : base("日期貨盤後行情表_luann")
        {
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
            dataSet.Tables[0].Columns["到期月份_週別"].ColumnName = "到期月份(週別)";
            sql.InsertCommand = new SqlCommand("INSERT INTO [dbo].[日期貨盤後行情表_luann]([交易日期],[契約],[到期月份(週別)],[開盤價],[最高價],[最低價],[收盤價])VALUES(@交易日期, @契約, @到期月份,@開盤價,@最高價,@最低價,@收盤價)", SQLConnection);
            sql.InsertCommand.Parameters.Add("@交易日期", SqlDbType.Char, 8, "交易日期");
            sql.InsertCommand.Parameters.Add("@契約", SqlDbType.VarChar, 3, "契約");
            sql.InsertCommand.Parameters.Add("@到期月份", SqlDbType.VarChar, 17, "到期月份(週別)");
            sql.InsertCommand.Parameters.Add("@開盤價", SqlDbType.Decimal, 9, "開盤價");
            sql.InsertCommand.Parameters.Add("@最高價", SqlDbType.Decimal, 9, "最高價");
            sql.InsertCommand.Parameters.Add("@最低價", SqlDbType.Decimal, 9, "最低價");
            sql.InsertCommand.Parameters.Add("@收盤價", SqlDbType.Decimal, 9, "收盤價");
            sql.UpdateCommand = new SqlCommand("UPDATE [dbo].[日期貨盤後行情表_luann] SET  [開盤價] = @開盤價,[最高價] = @最高價,[最低價] = @最低價,[收盤價] = @收盤價,[MTIME] = @MTIME WHERE [交易日期] = @交易日期 AND [契約] = @契約 AND [到期月份(週別)] = @到期月份", SQLConnection);
            sql.UpdateCommand.Parameters.Add("@交易日期", SqlDbType.Char, 8, "交易日期");
            sql.UpdateCommand.Parameters.Add("@契約", SqlDbType.VarChar, 3, "契約");
            sql.UpdateCommand.Parameters.Add("@到期月份", SqlDbType.VarChar, 17, "到期月份(週別)");
            sql.UpdateCommand.Parameters.Add("@開盤價", SqlDbType.Decimal, 9, "開盤價");
            sql.UpdateCommand.Parameters.Add("@最高價", SqlDbType.Decimal, 9, "最高價");
            sql.UpdateCommand.Parameters.Add("@最低價", SqlDbType.Decimal, 9, "最低價");
            sql.UpdateCommand.Parameters.Add("@收盤價", SqlDbType.Decimal, 9, "收盤價");
            sql.UpdateCommand.Parameters.Add("@MTIME", SqlDbType.BigInt).Value =DateTimeOffset.Now.ToUnixTimeSeconds();
            sql.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
            sql.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
            sql.UpdateBatchSize = 0;
            SQLConnection.Open();
            sql.Fill(table);
            //dataSet.Tables[0].Rows[0]["交易日期"] = "000001010";
            //dataSet.Tables[0].Rows[1]["收盤價"] = 0;
            table.PrimaryKey = new DataColumn[] { table.Columns["交易日期"], table.Columns["契約"], table.Columns["到期月份(週別)"] };
            dataSet.Tables[0].PrimaryKey = new DataColumn[] { dataSet.Tables[0].Columns["交易日期"], dataSet.Tables[0].Columns["契約"], dataSet.Tables[0].Columns["到期月份(週別)"] };
            table.Merge(dataSet.Tables[0],false,MissingSchemaAction.Ignore);
            sql.Update(table);
            SQLConnection.Close();
        }
    }
}
