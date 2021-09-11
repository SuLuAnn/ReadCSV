using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DapperTest
{
    public partial class Form1 : Form
    {
        private Stopwatch StopWatch;
        private XDocument TotalDocument;
        public Form1()
        {
            InitializeComponent();
            StopWatch = new Stopwatch();
            TotalDocument = new XDocument(new XElement("Root"));
            for (int i = 2005; i <= DateTime.Now.Year; i++)
            {
                string path = Path.Combine(Environment.CurrentDirectory, "BackupFile", i.ToString(), "基金非營業日明細.xml");
                TotalDocument.Root.Add(XElement.Load(path).Elements("Data"));
            }
        }

        private void ClickAdoNet(object sender, EventArgs e)
        {
            StopWatch.Restart();
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = "Data Source=192.168.10.180;Initial Catalog=StockDB;User ID=test;Password=test";
            sqlConnection.FireInfoMessageEventOnUserErrors = false;
            DataSet dataSet = new DataSet();
            //讀取xml
            dataSet.ReadXml(TotalDocument.CreateReader());
            string sqlCommand = @"MERGE [dbo].[基金非營業日明細_luann] AS A USING @sourceTable AS B ON A.[非營業日] = B.[非營業日] 
                                                           AND A.[基金統編] = B.[基金統編] WHEN MATCHED AND (A.[公司代號] <> B.公司代號 OR A.[基金名稱] <> 
                                                           B.基金名稱 OR A.[排序] <> B.排序) THEN UPDATE SET [公司代號] = B.公司代號,[基金名稱] = B.基金名稱,[排序] 
                                                           = B.排序,[MTIME] = (datediff(second, '1970-01-01', getutcdate())) WHEN NOT MATCHED BY TARGET 
                                                           THEN INSERT([非營業日],[公司代號],[基金統編],[基金名稱],[排序]) VALUES(B.非營業日,B.公司代號, B.基金統編,
                                                           B.基金名稱,B.排序) WHEN NOT MATCHED BY SOURCE THEN DELETE;";
            SqlCommand command = new SqlCommand(sqlCommand, sqlConnection);
            SqlParameter tableParameter = command.Parameters.AddWithValue("@sourceTable", dataSet.Tables[0]);
            tableParameter.SqlDbType = SqlDbType.Structured;
            tableParameter.TypeName = "基金非營業日明細TableType";
            sqlConnection.Open();
            command.ExecuteNonQuery();
            sqlConnection.Close();
            StopWatch.Stop();
            textBox1.Text += $"{StopWatch.ElapsedMilliseconds}{Environment.NewLine}";
        }

        private void ClickDapper(object sender, EventArgs e)
        {
            StopWatch.Restart();
            DataSet dataSet = new DataSet();
            //讀取xml
            dataSet.ReadXml(TotalDocument.CreateReader());
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = "Data Source=192.168.10.180;Initial Catalog=StockDB;User ID=test;Password=test";
            sqlConnection.FireInfoMessageEventOnUserErrors = false;
            string sqlCommand = @"MERGE [dbo].[基金非營業日明細_luann] AS A USING @sourceTable AS B ON A.[非營業日] = B.[非營業日] 
                                                           AND A.[基金統編] = B.[基金統編] WHEN MATCHED AND (A.[公司代號] <> B.公司代號 OR A.[基金名稱] <> 
                                                           B.基金名稱 OR A.[排序] <> B.排序) THEN UPDATE SET [公司代號] = B.公司代號,[基金名稱] = B.基金名稱,[排序] 
                                                           = B.排序,[MTIME] = (datediff(second, '1970-01-01', getutcdate())) WHEN NOT MATCHED BY TARGET 
                                                           THEN INSERT([非營業日],[公司代號],[基金統編],[基金名稱],[排序]) VALUES(B.非營業日,B.公司代號, B.基金統編,
                                                           B.基金名稱,B.排序) WHEN NOT MATCHED BY SOURCE THEN DELETE;";
            SqlCommand command = new SqlCommand(sqlCommand, sqlConnection);
            SqlParameter tableParameter = command.Parameters.AddWithValue("@sourceTable", dataSet.Tables[0]);
            tableParameter.SqlDbType = SqlDbType.Structured;
            tableParameter.TypeName = "基金非營業日明細TableType";
            sqlConnection.Execute(sqlCommand);
            //var funds = sqlConnection.Query<FundDto>(sqlCommand).ToList();
            StopWatch.Stop();
            textBox1.Text += $"{StopWatch.ElapsedMilliseconds}{Environment.NewLine}";
        }

        private void ClickEF(object sender, EventArgs e)
        {

        }
    }
}
