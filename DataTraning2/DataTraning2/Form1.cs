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
    /// <summary>
    /// 主要程式邏輯
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// 乘載使用者下拉選單所選的物件
        /// </summary>
        public IDataSheet DataSheet { get; set; }

        /// <summary>
        /// 計時器
        /// </summary>
        private Stopwatch Stopwatch;

        /// <summary>
        /// 用於傳送LOG字串陣列時每個字串對應的內容
        /// </summary>
        private enum LogMessage : int
        {
            /// <summary>
            /// 步驟
            /// </summary>
            STEP,

            /// <summary>
            /// 成功與否
            /// </summary>
            RESULT,

            /// <summary>
            /// 執行時間
            /// </summary>
            TIME
        }

        /// <summary>
        /// 匯入所有有實作IDataSheet的物件， IDataSheetNews為物件所對應的資料表名
        /// </summary>
        [ImportMany(typeof(IDataSheet))]
        public IEnumerable<Lazy<IDataSheet, IDataSheetNews>> DataSheets { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            //dll物件的初始化
            Compose();
            Stopwatch = new Stopwatch();
            //取得下拉選單的內容，所有IDataSheetNews的資料表名稱資訊
            DropDownMenu.DataSource = DataSheets.Select(sheet => sheet.Metadata.TableName).ToList();
        }

        /// <summary>
        /// dll物件的初始化，載入dll
        /// </summary>
        private void Compose()
        {
            //從文件夾中尋找可用於導入的部件，讀取執行檔位置下的dll
            var catalog = new DirectoryCatalog(".");
            //管理組合部件
            CompositionContainer container = new CompositionContainer(catalog);
            //創建可組合部件
            container.ComposeParts(this);
        }

        /// <summary>
        /// 使用者按下新增修改按鈕
        /// </summary>
        /// <param name="sender">按下的物件</param>
        /// <param name="e">觸發事件</param>
        private void ClickAddReviseButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            //以多執行緒開始執行新增修改方法，並傳入使用者所選表名
            AddReviseWorker.RunWorkerAsync(DropDownMenu.Text);
        }

        /// <summary>
        /// 使用者按下刪除按鈕
        /// </summary>
        /// <param name="sender">按下的物件</param>
        /// <param name="e">觸發事件</param>
        private void ClickDeleteButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            //以多執行緒開始執行刪除方法，並傳入使用者所選表名
            DeleteWorker.RunWorkerAsync(DropDownMenu.Text);
        }

        /// <summary>
        /// 寫入Log時間、程式執行時間、步驟、成功與否等資訊，並將log存在桌面的log.txt
        /// </summary>
        /// <param name="step">步驟</param>
        /// <param name="result">成功與否</param>
        /// <param name="time">執行時間</param>
        public void Log(string step, string result, string time)
        {
            string log = $"Log時間：{DateTime.Now.ToString()}, 步驟：{step}, 執行時間：{time}, 結果：{result}{Environment.NewLine}";
            TimeText.Text += log;
            //讓scroll跟著新訊息捲動
            TimeText.SelectionStart = TimeText.Text.Length;
            TimeText.ScrollToCaret();
            //寫入log
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "log.txt");
            using (StreamWriter writer = new StreamWriter(path, true, Encoding.UTF8))
            {
                writer.WriteLine(log);
            }
        }

        /// <summary>
        /// 當下拉選單所選被改變，讓所選內容被放入DataSheet
        /// </summary>
        /// <param name="sender">下拉選單物件</param>
        /// <param name="e">觸發事件</param>
        private void ChangedDropDownMenu(object sender, EventArgs e)
        {
            DataSheet = DataSheets.SingleOrDefault(sheet => sheet.Metadata.TableName == DropDownMenu.Text).Value;
        }

        /// <summary>
        /// 刪除所選資料表的所有內容
        /// </summary>
        /// <param name="sender">下拉選單物件</param>
        /// <param name="e">觸發事件</param>
        private void WorkDelete(object sender, DoWorkEventArgs e)
        {
            SqlCommand query = new SqlCommand($"TRUNCATE TABLE {DataSheet.GetDataTableName()}", SQLConnection);
            SQLConnection.Open();
            query.ExecuteNonQuery();
            SQLConnection.Close();
            Stopwatch.Stop();
            e.Result = new string[] { $"清空{(string)e.Argument}的資料", "成功", Stopwatch.ElapsedMilliseconds.ToString() };
        }

        /// <summary>
        /// 刪除物件後做log
        /// </summary>
        /// <param name="sender">多執行緒物件</param>
        /// <param name="runWorker">事件</param>
        private void Completed(object sender, RunWorkerCompletedEventArgs runWorker)
        {
            string[] logWords = (string[])runWorker.Result;
            Log(logWords[(int)LogMessage.STEP], logWords[(int)LogMessage.RESULT], logWords[(int)LogMessage.TIME]);
        }

        /// <summary>
        /// 新增修改指定資料表
        /// </summary>
        /// <param name="sender">多執行緒物件</param>
        /// <param name="e">觸發事件</param>
        private void WorkAddRevise(object sender, DoWorkEventArgs e)
        {
            Stopwatch.Restart();
            //取得網站原始資料
            DataSheet.GetWebs();
            Stopwatch.Stop();
            string[] report = new string[] { $"{(string)e.Argument}的原始資料已取得", "成功", Stopwatch.ElapsedMilliseconds.ToString() };
            AddReviseWorker.ReportProgress(0, report);
            Stopwatch.Restart();
            //將原始資料轉為中介資料XML檔
            DataSheet.GetXML();
            Stopwatch.Stop();
            report = new string[] { $"{(string)e.Argument}的Xml已取得", "成功", Stopwatch.ElapsedMilliseconds.ToString() };
            AddReviseWorker.ReportProgress(0, report);
            Stopwatch.Restart();
            //將中介資料轉入資料庫做新增修改
            DataSheet.WriteDatabase(SQLConnection);
            Stopwatch.Stop();
            report = new string[] { $"{(string)e.Argument}已更新並寫入資料庫", "成功", Stopwatch.ElapsedMilliseconds.ToString() };
            AddReviseWorker.ReportProgress(0, report);
        }

        /// <summary>
        /// 再新增修改期間每執行一步做一次log紀錄
        /// </summary>
        /// <param name="sender">多執行緒物件</param>
        /// <param name="e">觸發事件</param>
        private void Report(object sender, ProgressChangedEventArgs e)
        {
            //傳入步驟，執行時間，成功與否字串陣列
            string[] logWords = (string[])e.UserState;
            Log(logWords[(int)LogMessage.STEP], logWords[(int)LogMessage.RESULT], logWords[(int)LogMessage.TIME]);
        }
    }
}
