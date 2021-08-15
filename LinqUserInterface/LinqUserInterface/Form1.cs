using LinqUserInterface.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private MarketIndustryQuery MarketIndustryQuery;

        private DailyTranspose DailyTranspose;

        private PERList PERList;

        private PresidentialElectionPerformance PresidentialElectionPerformance;

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
            PresidentialElectionPerformance = new PresidentialElectionPerformance(StockDB);
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
            Dictionary<string, Dictionary<string, List<交易所產業分類代號表>>> IndustryClassification = MarketIndustryQuery.GetMarketMenu();
            MarketMenu.DisplayMember = "Key";
            MarketMenu.ValueMember = "Value";
            MarketMenu.DataSource = new BindingSource(IndustryClassification, null);
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
            IndustryMenu.DisplayMember = "Key";
            IndustryMenu.ValueMember = "Value";
            Dictionary<string, List<交易所產業分類代號表>> industry = (Dictionary<string, List<交易所產業分類代號表>>)MarketMenu.SelectedValue;
            IndustryMenu.DataSource = new BindingSource(industry, null);
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
            CommonTable.DataSource = new BindingList<交易所產業分類代號表>( (List<交易所產業分類代號表>)IndustryMenu.SelectedValue);
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
            string[] days = CycleDayMenu.Text.Split('-');
            string first = days.FirstOrDefault();
            string last = days.LastOrDefault();
            string[] keyWords = StockIDNameMenu.Text.Split(',');
            //CommonTable.ColumnCount = 512;
            var data = DailyTranspose.GetDayTranspose(keyWords,first,last, CommonTable);
            CommonTable.Columns.Add((data.First().Value.Count()-1).ToString(), (data.First().Value.Count()-1).ToString());
            CommonTable.Rows.Add(data["日期"]);
            CommonTable.Rows.Add(data["股票代號"]);
            CommonTable.Rows.Add(data["股票名稱"]);
            CommonTable.Rows.Add(data["參考價"]);
            CommonTable.Rows.Add(data["開盤價"]);
            CommonTable.Rows.Add(data["最高價"]);
            CommonTable.Rows.Add(data["最低價"]);
            CommonTable.Rows.Add(data["收盤價"]);
            CommonTable.Rows.Add(data["漲跌"]);
            CommonTable.Rows.Add(data["市場別名稱"]);
            CommonTable.Rows.Add(data["產業名稱"]);
            CommonTable.Rows.Add(data["漲停價"]);
            CommonTable.Rows.Add(data["跌停價"]);
            CommonTable.Rows.Add(data["漲跌狀況"]);
            CommonTable.Rows.Add(data["最後委買價"]);
            CommonTable.Rows.Add(data["最後委賣價"]);
            CommonTable.Rows.Add(data["漲幅_比率"]);
            CommonTable.Rows.Add(data["振幅_比率"]);
            CommonTable.Rows.Add(data["成交量_股"]);
            CommonTable.Rows.Add(data["成交筆數"]);
            CommonTable.Rows.Add(data["成交值比重_比率"]);
            CommonTable.Rows.Add(data["成交量變動_比率"]);
            CommonTable.Rows.Add(data["總市值_億"]);
            CommonTable.Rows.Add(data["本益比"]);
            CommonTable.Rows.Add(data["委買張數"]);
            CommonTable.Rows.Add(data["委賣張數"]);
            CommonTable.Rows.Add(data["成交金額_元"]);
            CommonTable.Rows.Add(data["均價"]);
            CommonTable.Rows.Add(data["週轉率"]);
            ShowDataNum(CommonTable);
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
            PERIndustryMenu.DisplayMember = "Key";
            PERIndustryMenu.ValueMember = "Value";
            PERIndustryMenu.DataSource = new BindingSource(PERList.GetPERMenu(), null);
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
            string industryCode = (string)PERIndustryMenu.SelectedValue;
            if (first == last || industryCode == null)
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
            List<RiseStockDto> results = PresidentialElectionPerformance.GetRiseAllStocks();
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
            CommonTable.DataSource = new BindingList<RateReturn>(PresidentialElectionPerformance.GetSameRanking());
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
            List<Top5VolumeDto> result = PresidentialElectionPerformance.GetTop5AllVolume();
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
            PresidentialElectionPerformance.GetRiseFallTop5(out List<RiseFallTop5Dto> riseFall, out List<OriginalDataDto> originals, out List<IndividualStockDto> individuals);
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
    }
}
