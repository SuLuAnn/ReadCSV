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
            string OriginalWeb = ReadFile($"{DateTime.Now.Year}.csv");
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
            XDocument TotalDocument = new XDocument(new XElement("Root",
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
        /// 將資料寫入資料庫
        /// </summary>
        public override void WriteDatabase()
        {
            XDocument TotalDocument = GetTotalXml("日期貨盤後統計表");
            DataSet dataSet = new DataSet();
            //讀取xml
            dataSet.ReadXml(TotalDocument.CreateReader());
            //dataSet.Tables[0].Rows[1]["收盤價"] = 0;//測試用
            string sqlCommand = @"MERGE [dbo].[日期貨盤後統計表_luann] AS A USING @sourceTable AS B ON A.[交易日期] = B.[交易日期] 
                                                           AND A.[契約] = B.[契約] WHEN MATCHED AND 
                                                           (A.[開盤價] <> B.開盤價 OR (A.[開盤價] IS NULL AND B.開盤價 IS NOT NULL) OR (A.[開盤價] IS NOT NULL AND
                                                           B.開盤價 IS NULL) OR A.[最高價] <> B.最高價 OR (A.[最高價] IS NULL AND B.最高價 IS NOT NULL) OR (A.[最高價] IS NOT NULL AND
                                                           B.最高價 IS NULL) OR A.[最低價] <> B.最低價 OR (A.[最低價] IS NULL AND B.最低價 IS NOT NULL) OR (A.[最低價] IS NOT NULL AND
                                                           B.最低價 IS NULL) OR A.[收盤價] <> B.收盤價 OR (A.[收盤價] IS NULL AND B.收盤價 IS NOT NULL) OR (A.[收盤價] IS NOT NULL AND
                                                           B.收盤價 IS NULL)) THEN UPDATE SET[開盤價] = B.開盤價,[最高價] = B.最高價,[最低價] = B.最低價,[收盤價] 
                                                           = B.收盤價,[MTIME] = (datediff(second, '1970-01-01', getutcdate())) WHEN NOT MATCHED BY TARGET 
                                                           THEN INSERT([交易日期],[契約],[開盤價],[最高價],[最低價],[收盤價]) VALUES(B.交易日期, 
                                                            B.契約,B.開盤價, B.最高價, B.最低價, B.收盤價) WHEN NOT MATCHED BY SOURCE 
                                                            THEN DELETE;";
            SqlCommand command = new SqlCommand(sqlCommand, SQLConnection);
            SqlParameter tableParameter = command.Parameters.AddWithValue("@sourceTable", dataSet.Tables[0]);
            tableParameter.SqlDbType = SqlDbType.Structured;
            tableParameter.TypeName = "日期貨盤後統計表TableType";
            SQLConnection.Open();
            command.ExecuteNonQuery();
            SQLConnection.Close();
        }
    }
}
