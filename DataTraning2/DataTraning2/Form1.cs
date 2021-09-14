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
            if (this.InvokeRequired)
            {
                //這段在避免跨執行緒錯誤
                Action<string, string, string> action = new Action<string, string, string>(Log);
                this.Invoke(action, step, result, time);
            }
            else 
            {
                string log = $"Log時間：{DateTime.Now}, 步驟：{step}, 執行時間：{time} 毫秒, 結果：{result}{Environment.NewLine}";
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
            RepeatExecution($"清空{(string)e.Argument}的資料", Delete);
        }

        /// <summary>
        /// 新增修改指定資料表
        /// </summary>
        /// <param name="sender">多執行緒物件</param>
        /// <param name="e">觸發事件</param>
        private void WorkAddRevise(object sender, DoWorkEventArgs e)
        {
            //取得網站原始資料
            RepeatExecution($"{(string)e.Argument}的原始資料已取得", DataSheet.GetWebs);
            //將原始資料轉為中介資料XML檔
            RepeatExecution($"{(string)e.Argument}的Xml已取得", DataSheet.GetXML);
            //將中介資料轉入資料庫做新增修改
            RepeatExecution($"{(string)e.Argument}已更新並寫入資料庫", DataSheet.WriteDatabase);
        }

        /// <summary>
        /// 清空資料表的方法
        /// </summary>
        private void Delete() 
        {
            SqlCommand query = new SqlCommand($"TRUNCATE TABLE {DataSheet.GetDataTableName()}", SQLConnection);
            SQLConnection.Open();
            query.ExecuteNonQuery();
            SQLConnection.Close();
        }

        /// <summary>
        /// 做當報錯時方法會重複執行這件事
        /// </summary>
        /// <param name="step">步驟名稱</param>
        /// <param name="action">要執行的方法</param>
        private void RepeatExecution(string step, Action action)
        {
            string result = string.Empty;
            int frequency = 3;//最多執行次數
            while (frequency > 0)
            {
                Stopwatch.Restart();
                try
                {
                    action();
                    result = "成功";
                    frequency = 0;
                }
                catch (Exception ex)
                {
                    result = $"失敗，因為{ex.Message}";
                    frequency --;
                }
                Stopwatch.Stop();
                Log(step, result, Stopwatch.ElapsedMilliseconds.ToString());
            }
        }
    }
}
