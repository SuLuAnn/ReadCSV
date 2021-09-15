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
    public class DayFuturesStatistics : IDataSheet
    {
        /// <summary>
        /// 存物件所對應的資料表名稱
        /// </summary>
        public string DataTableName { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        public DayFuturesStatistics()
        {
            DataTableName = "日期貨盤後統計表_luann";
        }

        /// <summary>
        /// 取得xml中介資料
        /// </summary>
        public void GetXML()
        {
            string OriginalWeb = GlobalFunction.ReadFile($"{DateTime.Now.Year}.csv");
            //只要盤後的資料
            var datas = OriginalWeb.Trim().Split('\n').Skip(1).Select(data => data.Split(',')).Where(fields => fields[GlobalConst.TRADING_HOURS] == GlobalConst.CHINESS_TRADING_HOURS).Select(fields => new
            {
                交易日期 = fields[GlobalConst.TRANSACTION_DATE].Replace(GlobalConst.SLASH, string.Empty),
                契約 = fields[GlobalConst.CONTRACT],
                開盤價 = decimal.TryParse(fields[GlobalConst.OPENING_PRICE], out decimal openPrice) ? (decimal?)openPrice : null,
                最高價 = decimal.TryParse(fields[GlobalConst.HIGHEST_PRICE], out decimal highPrice) ? (decimal?)highPrice : null,
                最低價 = decimal.TryParse(fields[GlobalConst.LOWEST_PRICE], out decimal lowPrice) ? (decimal?)lowPrice : null,
                收盤價 = decimal.TryParse(fields[GlobalConst.CLOSING_PRICE], out decimal closePrice) ? (decimal?)closePrice : null
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
            XDocument TotalDocument = new XDocument(new XElement(GlobalConst.XML_ROOT,
            datas.Select(data =>
            new XElement(GlobalConst.XML_NODE_NAME,
                new XElement(GlobalConst.CHINESS_TRANSACTION_DATE, data.交易日期),
                new XElement(GlobalConst.CHINESS_CONTRACT, data.契約),
                data.開盤價 != null ? new XElement(GlobalConst.CHINESS_OPENING_PRICE, data.開盤價) : null,
                data.最高價 != null ? new XElement(GlobalConst.CHINESS_HIGHEST_PRICE, data.最高價) : null,
                data.最低價 != null ? new XElement(GlobalConst.CHINESS_LOWEST_PRICE, data.最低價) : null,
                data.收盤價 != null ? new XElement(GlobalConst.CHINESS_CLOSING_PRICE, data.收盤價) : null
                )
            )));
            //創建名稱為今年的資料夾，並將組好的xml放入
            string fileName = Path.Combine(GlobalFunction.CreatDirectory(DateTime.Now.Year.ToString()), "日期貨盤後統計表.xml");
            GlobalFunction.SaveXml(TotalDocument, fileName);
        }

        /// <summary>
        /// 將資料寫入資料庫
        /// </summary>
        public void WriteDatabase()
        {
            XDocument TotalDocument = DayFutures.GetTotalXml("日期貨盤後統計表");
            DataSet dataSet = new DataSet();
            //讀取xml
            dataSet.ReadXml(TotalDocument.CreateReader());
            //dataSet.Tables[0].Rows[1]["收盤價"] = 0;//測試用
            string sqlCommand = @"MERGE [dbo].[日期貨盤後統計表_luann] AS A USING @sourceTable AS B 
                                                           ON A.[交易日期] = B.[交易日期] 
                                                           AND A.[契約] = B.[契約] 
                                                           WHEN MATCHED AND (ISNULL(A.[開盤價],-1000000) <> ISNULL(B.開盤價,-1000000)
                                                                                             OR ISNULL(A.[最高價],-1000000) <> ISNULL(B.最高價,-1000000)
                                                                                             OR ISNULL(A.[最低價],-1000000) <> ISNULL(B.最低價,-1000000)
                                                                                             OR ISNULL(A.[收盤價],-1000000) <> ISNULL(B.收盤價,-1000000))
														   THEN UPDATE SET [開盤價] = B.開盤價,
                                                                                               [最高價] = B.最高價,
                                                                                               [最低價] = B.最低價,
                                                                                               [收盤價] = B.收盤價,
                                                                                               [MTIME] = (datediff(second, '1970-01-01', getutcdate())) 
                                                           WHEN NOT MATCHED BY TARGET THEN INSERT([交易日期],[契約],[開盤價],[最高價],[最低價],[收盤價])
                                                                                                                                        VALUES(B.交易日期,B.契約,B.開盤價, B.最高價, B.最低價, B.收盤價) 
                                                           WHEN NOT MATCHED BY SOURCE THEN DELETE;";
            using (SqlConnection SQLConnection = new SqlConnection(@"Data Source=192.168.10.180;Initial Catalog=StockDB;User ID=test;Password=test; Connection Timeout=180"))
            {
                SqlCommand command = new SqlCommand(sqlCommand, SQLConnection);
                SqlParameter tableParameter = command.Parameters.AddWithValue("@sourceTable", dataSet.Tables[0]);
                tableParameter.SqlDbType = SqlDbType.Structured;
                tableParameter.TypeName = "日期貨盤後統計表TableType";
                SQLConnection.Open();
                command.ExecuteNonQuery();
                SQLConnection.Close();
            }
        }

        /// <summary>
        /// 取得資料表名稱，因介面沒有屬性，所以要給方法來讓主程式取得物件的資料表名稱
        /// </summary>
        /// <returns>資料表名稱</returns>
        public string GetDataTableName()
        {
            return DataTableName;
        }

        /// <summary>
        /// 取得網站原始資料
        /// </summary>
        public void GetWebs()
        {
            DayFutures.GetWebs();
        }
    }
}
