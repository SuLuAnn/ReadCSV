using LinqUserInterface.Dto;
using System;
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

namespace LinqUserInterface
{
    /// <summary>
    /// 所有主要程式邏輯
    /// </summary>
    public partial class LINQTraning : Form
    {
        /// <summary>
        /// 交易所產業分類下拉選單要加"無分類"這選項
        /// </summary>
        private const string NO_CLASSIFICATION = "無分類";

        /// <summary>
        /// 資料庫物件
        /// </summary>
        private StockDBEntities StockDB;

        /// <summary>
        /// 計時器
        /// </summary>
        private static Stopwatch Stopwatch;

        /// <summary>
        /// 建構子，建立winform UI
        /// </summary>
        public LINQTraning()
        {
            InitializeComponent();
        }

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
            DisplayTime.Text = $"啟動時間：{ShowTime(Stopwatch)}";
        }

        /// <summary>
        /// 顯示市場別名稱下拉式選單
        /// </summary>
        /// <param name="sender">觸發事件的控件</param>
        /// <param name="e">事件引數</param>
        private void DisplayMarketMenu(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            Dictionary<string, Dictionary<string, List<交易所產業分類代號表>>> IndustryClassification = StockDB.交易所產業分類代號表
                .GroupBy(industry => industry.市場別).ToDictionary(industries => industries.First().市場別名稱, markets => markets.GroupBy(industry => industry.名稱).ToDictionary(item => item.Key, item => item.ToList()));
            var noClassification = IndustryClassification.SelectMany(Industry => Industry.Value.Values.SelectMany(item => item)).ToList();
            IndustryClassification.Add(NO_CLASSIFICATION, new Dictionary<string, List<交易所產業分類代號表>> { { NO_CLASSIFICATION, noClassification } });
            MarketMenu.DisplayMember = "Key";
            MarketMenu.ValueMember = "Value";
            MarketMenu.DataSource = new BindingSource(IndustryClassification, null);
            DisplayIndustryMenu(sender, e);
            DisplayTime.Text = $"Q1-下拉選單建立：{ShowTime(Stopwatch)}{Environment.NewLine}{DisplayTime.Text}";
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
            var industry = (Dictionary<string, List<交易所產業分類代號表>>)MarketMenu.SelectedValue;
            if (!industry.ContainsKey(NO_CLASSIFICATION))
            {
                industry.Add(NO_CLASSIFICATION, industry.Values.SelectMany(item => item).ToList());
            }
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
            CommonTable.DataSource = new BindingList<交易所產業分類代號表>((List<交易所產業分類代號表>)IndustryMenu.SelectedValue);
            ShowDataNum(CommonTable);
            DisplayTime.Text = $"Q1：{ShowTime(Stopwatch)}{Environment.NewLine}{DisplayTime.Text}";
        }

        /// <summary>
        /// 顯示時間
        /// </summary>
        /// <param name="stopwatch">計時器</param>
        /// <returns></returns>
        private static string ShowTime(Stopwatch stopwatch)
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
        /// 當點擊日收盤本益比優劣榜的查詢鍵，從『日收盤』中取出指定產業名稱在指定週期區間內
        /// ，『平均本益比』大於『目標本益比』的資料，並將平均本益比最高與最低的20筆顯示於『共用』分頁中，本益比取到小數第6位。
        /// </summary>
        /// <param name="sender">觸發事件的控件</param>
        /// <param name="e">事件引數</param>
        private void ClickCheckPERButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            int needDataNum = 20;
            string[] days = CycleMenu.Text.Split('-');
            string first = days.FirstOrDefault();
            string last = days.LastOrDefault();
            decimal targetERP = TargetPERMenu.Value;
            string industryCode = (string)ERPIndustryMenu.SelectedValue;
            if (first == last || industryCode == null)
            {
                return;
            }
            var records = StockDB.日收盤.Where(data => data.產業代號 == industryCode && string.Compare(data.日期, first) >= 0
                 && string.Compare(data.日期, last) <= 0).GroupBy(data => data.股票代號, data => data, (stockID, stocks) => new ERPDto
                 {
                     股票代號 = stockID,
                     股票名稱 = stocks.Where(stock => stock.股票名稱.Length == stocks.Min(item => item.股票名稱.Length)).FirstOrDefault().股票名稱,
                     平均本益比 = decimal.Round((decimal)(stocks.Average(erp => erp.本益比)), 6)
                 }).ToList().Where(data => data.平均本益比 > targetERP).OrderByDescending(data => data.平均本益比);
            var top20 = records.Take(needDataNum).ToList();
            top20.AddRange(records.Skip(needDataNum).OrderBy(data => data.平均本益比).Take(needDataNum));
            CommonTable.DataSource = new BindingList<ERPDto>(top20);
            ShowDataNum(CommonTable);
            DisplayTime.Text = $"Q3：{ShowTime(Stopwatch)}{Environment.NewLine}{DisplayTime.Text}";
        }

        /// <summary>
        /// 當使用者點擊下拉式選單，顯示日收盤本益比優劣榜的下拉式選單
        /// </summary>
        /// <param name="sender">觸發事件的控件</param>
        /// <param name="e">事件引數</param>
        private void DisplayERPIndustryMenu(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            ERPIndustryMenu.DisplayMember = "Key";
            ERPIndustryMenu.ValueMember = "Value";
            var list = StockDB.交易所產業分類代號表.Where(industry => industry.市場別 == 1 || industry.市場別 == 2).GroupBy(industry => industry.代號)
                .OrderBy(industry => industry.Key).ToDictionary(industry => industry.FirstOrDefault(item => item.名稱.Length == industry.Min(record => record.名稱.Length)).名稱, industry => industry.Key);
            ERPIndustryMenu.DataSource = new BindingSource(list, null);
            DisplayTime.Text = $"Q3-下拉選單建立：{ShowTime(Stopwatch)}{Environment.NewLine}{DisplayTime.Text}";
        }

        /// <summary>
        /// 點擊日收盤查詢及轉置
        /// </summary>
        /// <param name="sender">觸發事件的控件</param>
        /// <param name="e">事件引數</param>
        private void ClickDayButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            string[] days = CycleDayMenu.Text.Split('-');
            string first = days.FirstOrDefault();
            string last = days.LastOrDefault();
            string[] keyWords = StockIDNameMenu.Text.Split(',');
            var dayStock = StockDB.日收盤.Where(data => (keyWords.Contains(data.股票代號) || keyWords.Contains(data.股票名稱)) && string.Compare(data.日期, first) >= 0
                  && string.Compare(data.日期, last) <= 0).OrderByDescending(stock => stock.日期).ThenBy(stock => stock.股票代號).Select(dayStocks => new DayStockChange
                  {
                      參考價 = dayStocks.參考價,
                      均價 = dayStocks.均價,
                      委買張數 = dayStocks.委買張數,
                      委賣張數 = dayStocks.委賣張數,
                      市場別名稱 = StockDB.交易所產業分類代號表.FirstOrDefault(data => SqlFunctions.StringConvert((decimal)data.市場別).Trim() == dayStocks.上市櫃).市場別名稱,
                      產業名稱 = dayStocks.產業代號 == null ? "無產業名稱" : StockDB.交易所產業分類代號表.FirstOrDefault(data => data.代號 == dayStocks.產業代號).名稱,
                      成交值比重_比率 = dayStocks.成交值比重___,
                      成交筆數 = dayStocks.成交筆數,
                      成交量_股 = dayStocks.成交量_股_,
                      成交量變動_比率 = dayStocks.成交量變動___,
                      成交金額_元 = dayStocks.成交金額_元,
                      振幅_比率 = dayStocks.振幅___,
                      收盤價 = dayStocks.收盤價,
                      日期 = dayStocks.日期,
                      最低價 = dayStocks.最低價,
                      最後委買價 = dayStocks.最後委買價,
                      最後委賣價 = dayStocks.最後委賣價,
                      最高價 = dayStocks.最高價,
                      本益比 = dayStocks.本益比,
                      漲停價 = dayStocks.漲停價,
                      漲幅_比率 = dayStocks.漲幅___,
                      漲跌 = dayStocks.漲跌,
                      漲跌狀況 = dayStocks.漲跌狀況,
                      總市值_億 = dayStocks.總市值_億_,
                      股票代號 = dayStocks.股票代號,
                      股票名稱 = dayStocks.股票名稱,
                      跌停價 = dayStocks.跌停價,
                      週轉率 = dayStocks.週轉率,
                      開盤價 = dayStocks.開盤價,
                  }).ToList();
            CommonTable.DataSource = dayStock;
            ShowDataNum(CommonTable);
            DisplayTime.Text = $"Q2：{ShowTime(Stopwatch)}{Environment.NewLine}{DisplayTime.Text}";
        }

        private void ClickRisingButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            var results = GetRiseStocks("20080322");
            results.AddRange(GetRiseStocks("20120114"));
            CommonTable.DataSource =new BindingList<RiseStockDto>( results);
            DisplayTime.Text = $"Q4a：{ShowTime(Stopwatch)}{Environment.NewLine}{DisplayTime.Text}";
        }

        private List<RiseStockDto> GetRiseStocks(string targetDay)
        {
            var dayInterval = GetPresidentialDay(targetDay);
            var kindStocks = GetKindStocks();
            return StockDB.日收盤.Join(kindStocks, day => day.股票代號, code => code.代號, (day, code) => new
            {
                day.日期,
                code.代號,
                code.名稱,
                day.收盤價
            }).GroupBy(day => day.代號, (id, days) => new RiseStockDto
            {
                年度 = days.FirstOrDefault(day => day.日期 == dayInterval.LastDay).日期.Substring(0, 4),
                類股股票代號 = days.FirstOrDefault().代號,
                類股股票名稱 = days.FirstOrDefault().名稱,
                第30個交易日日期 = days.FirstOrDefault(day => day.日期 == dayInterval.LastDay).日期,
                第30個交易日收盤價 = days.FirstOrDefault(day => day.日期 == dayInterval.LastDay).收盤價,
                第1個交易日日期 = days.FirstOrDefault(day => day.日期 == dayInterval.FirstDay).日期,
                第1個交易日收盤價 = days.FirstOrDefault(day => day.日期 == dayInterval.FirstDay).收盤價,
                股價漲跌 = days.FirstOrDefault(day => day.日期 == dayInterval.LastDay).收盤價 - days.FirstOrDefault(day => day.日期 == dayInterval.FirstDay).收盤價
            }).Where(day => day.股價漲跌 > 0).OrderBy(day => day.年度).ThenBy(day => day.類股股票代號).ToList();
        }

        private IQueryable<KindStock> GetKindStocks()
        {
            return StockDB.代號表指數.Where(code => (code.代號.StartsWith("TWB") || code.代號.StartsWith("TWC")) && code.代號 != "TWC00" &&
             code.C200707整併後交易所產業編號 != null).Select(code => new KindStock
             {
                 代號 = code.代號,
                 名稱 = code.名稱,
                 C200707整併後交易所產業編號 = code.C200707整併後交易所產業編號
             });
        }

        private DayDto GetPresidentialDay(string targetDay)
        {
            int lastDayPosition = 29;
            var days = StockDB.日收盤.Where(day => day.股票代號 == "TWC00" && string.Compare(day.日期, targetDay) > 0)
                .OrderBy(day => day.日期).Select(day => day.日期).ToList();
            return new DayDto
            {
                FirstDay = days.First(),
                LastDay = days.ElementAt(lastDayPosition)
            };
        }

        private void ClickSameRankingButton(object sender, EventArgs e)
        {
            int firstRank = 1;
            Stopwatch.Restart();
            var result2008 = GetRateReturns("20080322");
            var result2012 = GetRateReturns("20120114");
            var results = result2008.Zip(result2012, (early, late) => (early, late))
                .Select((day, index) => new List<RateReturn>
                {
                    day.early.SetRank(index+firstRank),day.late.SetRank(index+firstRank)
                })
                .Where(day => day.First().類股股票代號 == day.Last().類股股票代號)
                .SelectMany(day => day).OrderBy(day => day.年度).ThenBy(day => day.報酬率).ToList();
            CommonTable.DataSource = new BindingList<RateReturn>(results);
            DisplayTime.Text = $"Q4b：{ShowTime(Stopwatch)}{Environment.NewLine}{DisplayTime.Text}";
        }

        private List<RateReturn> GetRateReturns(string targetDay)
        {
            var kindStocks = GetKindStocks();
            var dayInterval = GetPresidentialDay(targetDay);
            return StockDB.日收盤.Join(kindStocks, day => day.股票代號, code => code.代號, (day, code) => new
            {
                day.日期,
                code.代號,
                code.名稱,
                day.收盤價
            }).GroupBy(day => day.代號, (id, days) => new
            {
                年度 = days.FirstOrDefault(day => day.日期 == dayInterval.LastDay).日期.Substring(0, 4),
                類股股票代號 = days.FirstOrDefault().代號,
                類股股票名稱 = days.FirstOrDefault().名稱,
                第30個交易日收盤價 = days.FirstOrDefault(day => day.日期 == dayInterval.LastDay).收盤價,
                第1個交易日收盤價 = days.FirstOrDefault(day => day.日期 == dayInterval.FirstDay).收盤價
            }).Select(day => new RateReturn
            {
                年度 = day.年度,
                類股股票代號 = day.類股股票代號,
                類股股票名稱 = day.類股股票名稱,
                報酬率 = (day.第30個交易日收盤價 - day.第1個交易日收盤價) / day.第1個交易日收盤價 * 100
            }).OrderByDescending(day => day.報酬率).Take(20).ToList();
        }
    }
}
