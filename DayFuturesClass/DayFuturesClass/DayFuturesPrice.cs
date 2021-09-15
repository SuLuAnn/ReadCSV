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
            string OriginalWeb = GlobalFunction.ReadFile($"{DateTime.Now.Year}.csv");
            //只要盤後的資料
            IEnumerable<string[]> datas = OriginalWeb.Trim().Split('\n').Skip(1).Select(data => data.Split(GlobalConst.COMMA)).Where(fields => fields[GlobalConst.TRADING_HOURS] == GlobalConst.CHINESS_TRADING_HOURS);
            XDocument TotalDocument = new XDocument(new XElement(GlobalConst.XML_ROOT,
            datas.Select(data =>
            new XElement(GlobalConst.XML_NODE_NAME,
                new XElement(GlobalConst.CHINESS_TRANSACTION_DATE, data[GlobalConst.TRANSACTION_DATE].Replace(GlobalConst.SLASH, string.Empty)),
                new XElement(GlobalConst.CHINESS_CONTRACT, data[GlobalConst.CONTRACT]),
                new XElement(GlobalConst.CHINESS_EXPIRY_MONTH, data[GlobalConst.EXPIRY_MONTH]),
                decimal.TryParse(data[GlobalConst.OPENING_PRICE], out decimal openPrice) ? new XElement(GlobalConst.CHINESS_OPENING_PRICE, (decimal?)openPrice) : null,
                decimal.TryParse(data[GlobalConst.HIGHEST_PRICE], out decimal highPrice) ? new XElement(GlobalConst.CHINESS_HIGHEST_PRICE,  (decimal?)highPrice) : null,
                decimal.TryParse(data[GlobalConst.LOWEST_PRICE], out decimal lowPrice) ? new XElement(GlobalConst.CHINESS_LOWEST_PRICE,  (decimal?)lowPrice) : null,
                decimal.TryParse(data[GlobalConst.CLOSING_PRICE], out decimal closePrice) ? new XElement(GlobalConst.CHINESS_CLOSING_PRICE, (decimal?)closePrice) : null
                )
            )));
            //創建名稱為今年的資料夾，並將組好的xml放入
            string fileName = Path.Combine(GlobalFunction.CreatDirectory(DateTime.Now.Year.ToString()), "日期貨盤後行情表.xml");
            GlobalFunction.SaveXml(TotalDocument, fileName);
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
            //dataSet.Tables[0].Rows[2]["開盤價"] = 100;
            dataSet.Tables[0].Columns["到期月份_週別"].ColumnName = "到期月份(週別)";
            //dataSet.Tables[0].Rows[1]["收盤價"] = 0;//測試用
            string sqlCommand = @"MERGE [dbo].[日期貨盤後行情表_luann] AS A USING @sourceTable AS B 
                                                           ON A.[交易日期] = B.[交易日期] 
                                                           AND A.[契約] = B.[契約] 
                                                           AND A.[到期月份(週別)] = B.[到期月份(週別)] 
                                                           WHEN MATCHED AND (ISNULL(A.[開盤價],-1000000) <> ISNULL(B.開盤價,-1000000)
														                                     OR ISNULL(A.[最高價],-1000000) <> ISNULL(B.最高價,-1000000)
                                                                                             OR ISNULL(A.[最低價],-1000000) <> ISNULL(B.最低價,-1000000)
                                                                                             OR ISNULL(A.[收盤價],-1000000) <> ISNULL(B.收盤價,-1000000)) 
                                                           THEN UPDATE SET [開盤價] = B.開盤價,
                                                                                               [最高價] = B.最高價,
                                                                                               [最低價] = B.最低價,
                                                                                               [收盤價] = B.收盤價,
                                                                                               [MTIME] = (datediff(second, '1970-01-01', getutcdate()))
                                                           WHEN NOT MATCHED BY TARGET THEN INSERT([交易日期],[契約],[到期月份(週別)],[開盤價],[最高價],[最低價],[收盤價]) 
                                                                                                                                         VALUES(B.交易日期,B.契約, B.[到期月份(週別)], B.開盤價, B.最高價, B.最低價, B.收盤價)
                                                           WHEN NOT MATCHED BY SOURCE THEN DELETE;";
            using (SqlConnection SQLConnection = new SqlConnection(@"Data Source=192.168.10.180;Initial Catalog=StockDB;User ID=test;Password=test; Connection Timeout=180")) 
            {
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
}
