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
    /// <summary>
    /// 用來產生日期貨盤後行情表的物件
    /// </summary>
    [ExportMetadata("TableName", "日期貨盤後行情表")]
    [Export(typeof(IDataSheet))]
    public class DayFuturesPrice : DayFutures
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public DayFuturesPrice() : base("日期貨盤後行情表_luann")
        {
        }

        /// <summary>
        /// 取得xml中介資料
        /// </summary>
        public override void GetXML()
        {
            string OriginalWeb = ReadFile($"{DateTime.Now.Year}.csv");
            //只要盤後的資料
            IEnumerable<string[]> datas = OriginalWeb.Trim().Split('\n').Skip(1).Select(data => data.Split(',')).Where(fields => fields[(int)Futures.TRADING_HOURS] == "盤後");
            XDocument TotalDocument = new XDocument(new XElement("Root",
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
            //創建名稱為今年的資料夾，並將組好的xml放入
            string fileName = Path.Combine(CreatDirectory(DateTime.Now.Year.ToString()), "日期貨盤後行情表.xml");
            SaveXml(TotalDocument, fileName);
        }

        /// <summary>
        /// 將xml更新進資料庫
        /// </summary>
        public override void WriteDatabase()
        {
            XDocument TotalDocument = GetTotalXml("日期貨盤後行情表");
            DataSet dataSet = new DataSet();
            //讀取xml
            dataSet.ReadXml(TotalDocument.CreateReader());
            dataSet.Tables[0].Columns["到期月份_週別"].ColumnName = "到期月份(週別)";
            //dataSet.Tables[0].Rows[1]["收盤價"] = 0;//測試用
            string sqlCommand = @"MERGE [dbo].[日期貨盤後行情表_luann] AS A USING @sourceTable AS B ON A.[交易日期] = B.[交易日期] 
                                                           AND A.[契約] = B.[契約] AND A.[到期月份(週別)] = B.[到期月份(週別)] WHEN MATCHED AND 
                                                           (A.[開盤價] <> B.開盤價 OR (A.[開盤價] IS NULL AND B.開盤價 IS NOT NULL) OR (A.[開盤價] IS NOT NULL AND
                                                           B.開盤價 IS NULL) OR A.[最高價] <> B.最高價 OR (A.[最高價] IS NULL AND B.最高價 IS NOT NULL) OR (A.[最高價] IS NOT NULL AND
                                                           B.最高價 IS NULL) OR A.[最低價] <> B.最低價 OR (A.[最低價] IS NULL AND B.最低價 IS NOT NULL) OR (A.[最低價] IS NOT NULL AND
                                                           B.最低價 IS NULL) OR A.[收盤價] <> B.收盤價 OR (A.[收盤價] IS NULL AND B.收盤價 IS NOT NULL) OR (A.[收盤價] IS NOT NULL AND
                                                           B.收盤價 IS NULL)) THEN UPDATE SET[開盤價] = B.開盤價,[最高價] = B.最高價,[最低價] = B.最低價,[收盤價] 
                                                           = B.收盤價,[MTIME] = (datediff(second, '1970-01-01', getutcdate())) WHEN NOT MATCHED BY TARGET 
                                                           THEN INSERT([交易日期],[契約],[到期月份(週別)],[開盤價],[最高價],[最低價],[收盤價]) VALUES(B.交易日期, 
                                                            B.契約, B.[到期月份(週別)], B.開盤價, B.最高價, B.最低價, B.收盤價) WHEN NOT MATCHED BY SOURCE 
                                                            THEN DELETE; ";
            SqlCommand command = new SqlCommand(sqlCommand, SQLConnection);
            SqlParameter tableParameter = command.Parameters.AddWithValue("@sourceTable", dataSet.Tables[0]);
            tableParameter.SqlDbType = SqlDbType.Structured;
            tableParameter.TypeName = "日期貨盤後行情表TableType";
            SQLConnection.Open();
            command.ExecuteNonQuery();
            SQLConnection.Close();
        }
    }
}
