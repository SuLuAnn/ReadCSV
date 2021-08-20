using LinqUserInterface.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using static LinqUserInterface.Global;

namespace LinqUserInterface
{
    /// <summary>
    /// 所有主要程式邏輯
    /// </summary>
    public partial class LINQTraning : Form
    {
        /// <summary>
        /// 資料庫物件
        /// </summary>
        private StockDBEntities StockDB;

        /// <summary>
        /// 計時器
        /// </summary>
        private Stopwatch Stopwatch;

        /// <summary>
        /// 建構子，建立winform UI
        /// </summary>
        public LINQTraning()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 第一題的所有方法
        /// </summary>
        private MarketIndustryQuery MarketIndustryQuery;

        /// <summary>
        /// 第二題的所有方法
        /// </summary>
        private DailyTranspose DailyTranspose;

        /// <summary>
        /// 第三題的所有方法
        /// </summary>
        private PERList PERList;

        /// <summary>
        /// 第四題的所有方法
        /// </summary>
        private PresidentialPerformance PresidentialPerformance;

        /// <summary>
        /// 第一題的下拉式選單
        /// </summary>
        private Dictionary<string, Dictionary<string, List<交易所產業分類代號表>>> IndustryClassification;

        /// <summary>
        /// 第三題下拉式選單
        /// </summary>
        private Dictionary<string, string> PERIndustry;

        /// <summary>
        /// 當創立顯示時間框框時，載入資料庫，並計時
        /// </summary>
        /// <param name="sender">觸發事件的控件</param>
        /// <param name="e">事件引數</param>
        private void DisplayLoad(object sender, EventArgs e)
        {
            Stopwatch = new Stopwatch();
            Stopwatch.Restart();
            StockDB = new StockDBEntities();
            MarketIndustryQuery = new MarketIndustryQuery(StockDB);
            DailyTranspose = new DailyTranspose(StockDB);
            PERList = new PERList(StockDB);
            PresidentialPerformance = new PresidentialPerformance(StockDB);
            DisplayTime.Text = $"啟動時間：{ShowTime()}";
        }

        /// <summary>
        /// 顯示市場別名稱下拉式選單
        /// </summary>
        /// <param name="sender">觸發事件的控件</param>
        /// <param name="e">事件引數</param>
        private void DisplayMarketMenu(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            IndustryClassification = MarketIndustryQuery.GetMarketMenu();
            MarketMenu.DataSource = IndustryClassification.Keys.ToList();
            DisplayIndustryMenu(sender, e);
            DisplayTime.Text = $"Q1-下拉選單建立：{ShowTime()}{Environment.NewLine}{DisplayTime.Text}";
        }

        /// <summary>
        /// 顯示產業名稱下拉式選單
        /// </summary>
        /// <param name="sender">觸發事件的控件</param>
        /// <param name="e">事件引數</param>
        private void DisplayIndustryMenu(object sender, EventArgs e)
        {
            var industry = IndustryClassification[MarketMenu.Text].Keys.ToList();
            IndustryMenu.DataSource = industry;
        }

        /// <summary>
        /// 點擊查詢，選擇市場別以及產業進行分類查詢
        /// </summary>
        /// <param name="sender">觸發事件的控件</param>
        /// <param name="e">事件引數</param>
        private void ClickCheckButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            CommonTable.Columns.Clear();
            CommonTable.Rows.Clear();
            //這邊用BindingList是因為先查水泥產業在查無所屬會報錯
            CommonTable.DataSource = new BindingList<交易所產業分類代號表>(IndustryClassification[MarketMenu.Text][IndustryMenu.Text]);
            ShowDataNum(CommonTable);
            DisplayTime.Text = $"Q1：{ShowTime()}{Environment.NewLine}{DisplayTime.Text}";
        }

        /// <summary>
        /// 點擊日收盤查詢及轉置
        /// </summary>
        /// <param name="sender">觸發事件的控件</param>
        /// <param name="e">事件引數</param>
        private void ClickDayButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            CommonTable.DataSource = null;
            CommonTable.Rows.Clear();
            CommonTable.Columns.Clear();
            CommonTable.ColumnAdded += AddColumn;
            string[] days = CycleDayMenu.Text.Split('-');
            string first = days.First();
            string last = days.Last();
            string[] keyWords = StockIDNameMenu.Text.Split(',');
            IEnumerable<IEnumerable<object>> data = DailyTranspose.GetDayTranspose(keyWords, first, last);
            CommonTable.ColumnCount = data.First().Count();
            data.Select(item => 
                                                { 
                                                    CommonTable.Rows.Add(item.ToArray());
                                                    return item;
                                                })
                    .ToList();
            CommonTable.Visible = true;
            ShowDataNum(CommonTable);
            CommonTable.ColumnAdded -= AddColumn;
            DisplayTime.Text = $"Q2：{ShowTime()}{Environment.NewLine}{DisplayTime.Text}";
        }

        /// <summary>
        /// 當使用者點擊下拉式選單，顯示日收盤本益比優劣榜的下拉式選單
        /// </summary>
        /// <param name="sender">觸發事件的控件</param>
        /// <param name="e">事件引數</param>
        private void DisplayPERIndustryMenu(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            PERIndustry = PERList.GetPERMenu();
            PERIndustryMenu.DataSource = PERIndustry.Keys.ToList();
            DisplayTime.Text = $"Q3-下拉選單建立：{(ShowTime())}{Environment.NewLine}{DisplayTime.Text}";
        }

        /// <summary>
        /// 當點擊日收盤本益比優劣榜的查詢鍵，從『日收盤』中取出指定產業名稱在指定週期區間內
        /// ，『平均本益比』大於『目標本益比』的資料，並將平均本益比最高與最低的20筆顯示於『共用』分頁中，本益比取到小數第6位。
        /// </summary>
        /// <param name="sender">觸發事件的控件</param>
        /// <param name="e">事件引數</param>
        private void ClickCheckPERButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            CommonTable.Columns.Clear();
            CommonTable.Rows.Clear();
            string[] days = CycleMenu.Text.Split('-');
            string first = days.FirstOrDefault();
            string last = days.LastOrDefault();
            string industryCode = PERIndustry[PERIndustryMenu.Text];
            if (first == null || last == null || industryCode == null)
            {
                return;
            }
            List<PERDto> top20 = PERList.GetPERData(industryCode, first, last, TargetPERMenu.Value);
            CommonTable.DataSource = new BindingList<PERDto>(top20);
            ShowDataNum(CommonTable);
            DisplayTime.Text = $"Q3：{ShowTime()}{Environment.NewLine}{DisplayTime.Text}";
        }

        /// <summary>
        /// 按下"上漲所有類股查詢"按鈕後查詢總統大選後第一個交易日以及第30個交易日類股收盤價以及漲跌
        /// </summary>
        /// <param name="sender">觸發事件的控件</param>
        /// <param name="e">事件引數</param>
        private void ClickRisingButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            CommonTable.Columns.Clear();
            CommonTable.Rows.Clear();
            List<RiseStockDto> results = PresidentialPerformance.GetRiseStocks(TRADING_DAY_30TH);
            CommonTable.DataSource = new BindingList<RiseStockDto>(results);
            ShowDataNum(CommonTable);
            DisplayTime.Text = $"Q4a：{ShowTime()}{Environment.NewLine}{DisplayTime.Text}";
        }

        /// <summary>
        /// 按下"排名相同類股"按鈕找出連續兩次總統大選類股報酬率排名皆為前20名且排名都相同的類股
        /// </summary>
        /// <param name="sender">觸發事件的控件</param>
        /// <param name="e">事件引數</param>
        private void ClickSameRankingButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            CommonTable.Columns.Clear();
            CommonTable.Rows.Clear();
            CommonTable.DataSource = new BindingList<RateReturn>(PresidentialPerformance.GetSameStock());
            ShowDataNum(CommonTable);
            DisplayTime.Text = $"Q4b：{ShowTime()}{Environment.NewLine}{DisplayTime.Text}";
        }

        /// <summary>
        /// 按"成交量前五大的個股"，查詢總統大選報酬率排名相同的類股中的成分個股資訊，找出時間區間內，各類股中平均成交量前五大的個股
        /// </summary>
        /// <param name="sender">觸發事件的控件</param>
        /// <param name="e">事件引數</param>
        private void ClickTop5VolumeButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            CommonTable.Columns.Clear();
            CommonTable.Rows.Clear();
            List<Top5VolumeDto> result = PresidentialPerformance.GetTop5Volume(TRADING_DAY_30TH);
            CommonTable.DataSource = new BindingList<Top5VolumeDto>(result);
            ShowDataNum(CommonTable);
            DisplayTime.Text = $"Q4c：{ShowTime()}{Environment.NewLine}{DisplayTime.Text}";
        }

        /// <summary>
        /// 按"漲跌次數 + 漲跌連續天數是否在報酬率前五"按鈕，查總統大選後100個交易日類股報酬率和排名以及個股漲跌和表現分數
        /// </summary>
        /// <param name="sender">觸發事件的控件</param>
        /// <param name="e">事件引數</param>
        private void ClickRiseFallTop5Button(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            CommonTable.Rows.Clear();
            CommonTable.Columns.Clear();
            PresidentialPerformance.GetRiseFallTop5(out List<RiseFallTop5Dto> riseFall, out List<OriginalDataDto> originals, out List<IndividualStockDto> individuals);
            RiseFallTop5Table.DataSource = new BindingList<RiseFallTop5Dto>(riseFall);
            OriginalDataTable.DataSource = new BindingList<OriginalDataDto>(originals);
            CommonTable.DataSource = new BindingList<IndividualStockDto>(individuals);
            DataNum.Text = $"{CommonTable.RowCount},{RiseFallTop5Table.RowCount},{OriginalDataTable.RowCount}筆";
            DisplayTime.Text = $"Q4d：{ShowTime()}{Environment.NewLine}{DisplayTime.Text}";
        }

        /// <summary>
        /// 顯示時間
        /// </summary>
        /// <returns></returns>
        private string ShowTime()
        {
            Stopwatch.Stop();
            return Stopwatch.ElapsedMilliseconds.ToString();
        }

        /// <summary>
        /// 顯示有幾筆資料
        /// </summary>
        /// <param name="table">計算的資料表</param>
        private void ShowDataNum(DataGridView table)
        {
            DataNum.Text = $"{table.RowCount}筆";
        }

        /// <summary>
        /// 顯示第二題轉置後的標頭，當加入Column時，將index給予標頭
        /// </summary>
        /// <param name="sender">觸發物件</param>
        /// <param name="e">觸發事件</param>
        private void AddColumn(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.HeaderText = e.Column.Index.ToString();
        }
    }
}
