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
    /// <summary>
    /// 用來產生日期貨盤後統計表的物件
    /// </summary>
    [ExportMetadata("TableName", "日期貨盤後統計表")]
    [Export(typeof(IDataSheet))]
    public class DayFuturesStatistics : DayFutures
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public DayFuturesStatistics() : base("日期貨盤後統計表_luann")
        {
        }

        /// <summary>
        /// 取得xml中介資料
        /// </summary>
        public override void GetXML()
        {
            //只要盤後的資料
            var datas = OriginalWeb.Trim().Split('\n').Skip(1).Select(data => data.Split(',')).Where(fields => fields[(int)Futures.TRADING_HOURS] == "盤後").Select(fields => new
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
            //創建名稱為今年的資料夾，並將組好的xml放入
            string fileName = Path.Combine(CreatDirectory(DateTime.Now.Year.ToString()), "日期貨盤後統計表.xml");
            SaveXml(TotalDocument, fileName);
        }

        /// <summary>
        /// 將xml更新進資料庫
        /// </summary>
        /// <param name="SQLConnection">資料庫連線物件</param>
        public override void WriteDatabase()
        {
            SqlDataAdapter sql = new SqlDataAdapter("SELECT * FROM 日期貨盤後統計表_luann", SQLConnection);
            //放資料庫目前的資料
            DataTable table = new DataTable();
            //放xml做好的最新資料
            DataSet dataSet = new DataSet();
            //讀取xml
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
            //每次更新MTIME都要一起變
            sql.UpdateCommand.Parameters.Add("@MTIME", SqlDbType.BigInt).Value = DateTimeOffset.Now.ToUnixTimeSeconds();
            //做批次處理
            sql.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
            sql.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
            sql.UpdateBatchSize = 0;
            SQLConnection.Open();
            //將資料庫資料填入table
            sql.Fill(table);
            //dataSet.Tables[0].Rows[0]["交易日期"] = "000001010";//測試用
            //dataSet.Tables[0].Rows[1]["收盤價"] = 0;//測試用
            //設定主鍵
            table.PrimaryKey = new DataColumn[] { table.Columns["交易日期"], table.Columns["契約"]};
            dataSet.Tables[0].PrimaryKey = new DataColumn[] { dataSet.Tables[0].Columns["交易日期"], dataSet.Tables[0].Columns["契約"] };
            //將兩張表合併，false意思是當組件相同時已dataSet.Tables[0]為主，Ignoreg是因為兩邊資料型態不同
            table.Merge(dataSet.Tables[0], false, MissingSchemaAction.Ignore);
            //對變動的行做新增和更新
            sql.Update(table);
            SQLConnection.Close();
        }
    }
}
