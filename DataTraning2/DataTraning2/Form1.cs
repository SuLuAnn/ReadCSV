using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.Composition;
using Common;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using StockVoteClass;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Threading;
using System.IO;

namespace DataTraning2
{
    public partial class Form1 : Form
    {
        public IDataSheet DataSheet { get; set; }

        private Stopwatch Stopwatch;

        [ImportMany(typeof(IDataSheet))]
        public IEnumerable<Lazy<IDataSheet, IDataSheetNews>> DataSheets { get; set; }
        public Form1()
        {
            InitializeComponent();
            Compose();
            Stopwatch = new Stopwatch();
            DropDownMenu.DataSource = DataSheets.Select(sheet => sheet.Metadata.TableName).ToList();
            Text = string.Empty;
        }
        private void Compose()
        {
            var catalog = new DirectoryCatalog(".");
            CompositionContainer container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        private void ClickAddReviseButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            AddReviseWorker.RunWorkerAsync(DropDownMenu.Text);
        }

        private void ClickDeleteButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            DeleteWorker.RunWorkerAsync(DropDownMenu.Text);
        }

        public void Log(string step, string result)
        {
            Stopwatch.Stop();
            string log = $"Log時間：{DateTime.Now.ToString()}, 步驟：{step}, 執行時間：{Stopwatch.ElapsedMilliseconds}, 結果：{result}{Environment.NewLine}";
            TimeText.Text += log;
            TimeText.SelectionStart = TimeText.Text.Length;
            TimeText.ScrollToCaret();
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "log.txt");
            using (StreamWriter writer = new StreamWriter(path, true, Encoding.UTF8))
            {
                writer.WriteLine(log);
            }
        }

        private void ChangedDropDownMenu(object sender, EventArgs e)
        {
            DataSheet = DataSheets.SingleOrDefault(sheet => sheet.Metadata.TableName == DropDownMenu.Text).Value;
        }

        private void WorkDelete(object sender, DoWorkEventArgs e)
        {
            SqlCommand query = new SqlCommand($"TRUNCATE TABLE {DataSheet.GetDataTableName()}", SQLConnection);
            SQLConnection.Open();
            query.ExecuteNonQuery();
            SQLConnection.Close();
            e.Result = new string[] { $"清空{(string)e.Argument}的資料", "成功" };
        }
        private void Completed(object sender, RunWorkerCompletedEventArgs runWorker)
        {
            int step = 0;
            int result = 1;
            string[] logWords =(string[])runWorker.Result;
            Log(logWords[step], logWords[result]);
        }

        private void WorkAddRevise(object sender, DoWorkEventArgs e)
        {
            Stopwatch.Restart();
            DataSheet.GetWebs();
            string[] report = new string[] { $"{(string)e.Argument}的原始資料已取得", "成功" };
            AddReviseWorker.ReportProgress(0, report);
            Stopwatch.Restart();
            DataSheet.GetXML();
            report[0] =  $"{(string)e.Argument}的Xml已取得";
            AddReviseWorker.ReportProgress(0, report);
            DataSheet.WriteDatabase(SQLConnection);
            report[0] = $"{(string)e.Argument}已更新並寫入資料庫";
            AddReviseWorker.ReportProgress(0, report);
        }

        private void Report(object sender, ProgressChangedEventArgs e)
        {
            int step = 0;
            int result = 1;
            string[] logWords = (string[])e.UserState;
            Log(logWords[step], logWords[result]);
        }
    }
}
