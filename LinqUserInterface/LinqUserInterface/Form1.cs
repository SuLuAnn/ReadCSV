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
        /// 2008年總統大選日期
        /// </summary>
        private const string PRESIDENTIAL_DATE_2008 = "20080322";

        /// <summary>
        /// 2012年總統大選日期
        /// </summary>
        private const string PRESIDENTIAL_DATE_2012 = "20120114";

        /// <summary>
        /// 總統大選後30天
        /// </summary>
        private const int TRADING_DAY_30TH = 30;

        /// <summary>
        /// 總統大選後100天
        /// </summary>
        private const int TRADING_DAY_100TH = 100;

        /// <summary>
        /// 年有4個字
        /// </summary>
        private const int YEAR_WORDS = 4;

        /// <summary>
        /// 前5筆
        /// </summary>
        private const int TOP_5 = 5;

        /// <summary>
        /// 前20筆
        /// </summary>
        private const int TOP_20 = 20;

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
        /// 當創立顯示時間框框時，載入資料庫，並計時
        /// </summary>
        /// <param name="sender">觸發事件的控件</param>
        /// <param name="e">事件引數</param>
        private void DisplayLoad(object sender, EventArgs e)
        {
            Stopwatch = new Stopwatch();
            Stopwatch.Restart();
            StockDB = new StockDBEntities();
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
            Dictionary<string, Dictionary<string, List<交易所產業分類代號表>>> IndustryClassification =Enumerable.Repeat(StockDB.交易所產業分類代號表
                .GroupBy(industry => industry.市場別)
                .ToDictionary(industries => industries.First().市場別名稱, markets => markets
                    .GroupBy(industry => industry.名稱).ToDictionary(item => item.Key, item => item.ToList())), 2).Aggregate((first, second) => {
                        first.Add(NO_CLASSIFICATION, new Dictionary<string, List<交易所產業分類代號表>> { { NO_CLASSIFICATION, second.SelectMany(Industry => Industry.Value.Values.SelectMany(item => item)).ToList() } });
                        return first; });
            
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
            string[] days = CycleDayMenu.Text.Split('-');
            string first = days.FirstOrDefault();
            string last = days.LastOrDefault();
            string[] keyWords = StockIDNameMenu.Text.Split(',');
            List<日收盤> dayStock = StockDB.日收盤
                .Where(data => (keyWords.Contains(data.股票代號) || keyWords.Contains(data.股票名稱)) 
                                                && string.Compare(data.日期, first) >= 0  
                                                && string.Compare(data.日期, last) <= 0)
                .OrderByDescending(stock => stock.日期)
                .ThenBy(stock => stock.股票代號).ToList()
                .Select((dayStocks, index) =>
                    {
                          CommonTable.Columns.Add(index.ToString(), index.ToString());
                          return dayStocks;
                      })
                .ToList();
            CommonTable.Columns.Add(dayStock.Count().ToString(), dayStock.Count().ToString());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.日期).Prepend("日期").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.股票代號).Prepend("股票代號").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.股票名稱).Prepend("股票名稱").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.參考價.ToString()).Prepend("參考價").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.開盤價.ToString()).Prepend("開盤價").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.最高價.ToString()).Prepend("最高價").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.最低價.ToString()).Prepend("最低價").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.收盤價.ToString()).Prepend("收盤價").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.漲跌.ToString()).Prepend("漲跌").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => StockDB.交易所產業分類代號表
            .FirstOrDefault(data => 
                SqlFunctions.StringConvert((decimal)data.市場別).Trim() == stock.上市櫃).市場別名稱).Prepend("市場別名稱").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.產業代號 == null ? "無產業名稱" : StockDB.交易所產業分類代號表
                .FirstOrDefault(data =>
                data.代號 == stock.產業代號).名稱).Prepend("產業名稱").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.漲停價.ToString()).Prepend("漲停價").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.跌停價.ToString()).Prepend("跌停價").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.漲跌狀況).Prepend("漲跌狀況").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.最後委買價.ToString()).Prepend("最後委買價").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.最後委賣價.ToString()).Prepend("最後委賣價").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.漲幅___.ToString()).Prepend("漲幅_比率").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.振幅___.ToString()).Prepend("振幅_比率").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.成交量_股_.ToString()).Prepend("成交量_股").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.成交筆數.ToString()).Prepend("成交筆數").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.成交值比重___.ToString()).Prepend("成交值比重_比率").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.成交量變動___.ToString()).Prepend("成交量變動_比率").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.總市值_億_.ToString()).Prepend("總市值_億").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.本益比.ToString()).Prepend("本益比").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.委買張數.ToString()).Prepend("委買張數").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.委賣張數.ToString()).Prepend("委賣張數").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.成交金額_元.ToString()).Prepend("成交金額_元").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.均價.ToString()).Prepend("均價").ToArray());
            CommonTable.Rows.Add(dayStock.Select(stock => stock.週轉率.ToString()).Prepend("週轉率").ToArray());
            ShowDataNum(CommonTable);
            DisplayTime.Text = $"Q2：{ShowTime()}{Environment.NewLine}{DisplayTime.Text}";
        }

        /// <summary>
        /// 當使用者點擊下拉式選單，顯示日收盤本益比優劣榜的下拉式選單
        /// </summary>
        /// <param name="sender">觸發事件的控件</param>
        /// <param name="e">事件引數</param>
        private void DisplayERPIndustryMenu(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            int[] marketNum = new int[]
            {
                1, 2
            };
            ERPIndustryMenu.DisplayMember = "Key";
            ERPIndustryMenu.ValueMember = "Value";
            Dictionary<string, string> list = StockDB.交易所產業分類代號表
                .Where(industry => marketNum.Contains(industry.市場別))
                .GroupBy(industry => industry.代號)
                .OrderBy(industry => industry.Key)
                .ToDictionary(industry => industry.FirstOrDefault(item => item.名稱.Length == industry.Min(record => record.名稱.Length)).名稱
                                        , industry => industry.Key);
            ERPIndustryMenu.DataSource = new BindingSource(list, null);
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
            int decimalPlaces = 6;
            string[] days = CycleMenu.Text.Split('-');
            string first = days.FirstOrDefault();
            string last = days.LastOrDefault();
            decimal targetERP = TargetPERMenu.Value;
            string industryCode = (string)ERPIndustryMenu.SelectedValue;
            if (first == last || industryCode == null)
            {
                return;
            }
           IOrderedEnumerable<ERPDto> records = StockDB.日收盤.Where(data => data.產業代號 == industryCode && string.Compare(data.日期, first) >= 0
                 && string.Compare(data.日期, last) <= 0).GroupBy(data => data.股票代號, data => data, (stockID, stocks) => new ERPDto
                 {
                     股票代號 = stockID,
                     股票名稱 = stocks.Where(stock => stock.股票名稱.Length == stocks.Min(item => item.股票名稱.Length)).FirstOrDefault().股票名稱,
                     平均本益比 = decimal.Round((decimal)(stocks.Average(erp => erp.本益比)), decimalPlaces)
                 }).ToList().Where(data => data.平均本益比 > targetERP).OrderByDescending(data => data.平均本益比);
            //ToList()放這原因:https://www.notion.so/Q3_ClickCheckPERButton-03114dacc81f498da2bdf369c9996823
            List<ERPDto> top20 = records.Take(TOP_20).ToList();
            top20.AddRange(records.Skip(TOP_20).OrderBy(data => data.平均本益比).Take(TOP_20));
            CommonTable.DataSource = new BindingList<ERPDto>(top20);
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
            List<RiseStockDto> results = GetRiseStocks(PRESIDENTIAL_DATE_2008, TRADING_DAY_30TH);
            results.AddRange(GetRiseStocks(PRESIDENTIAL_DATE_2012, TRADING_DAY_30TH));
            CommonTable.Columns.Clear();
            CommonTable.Rows.Clear();
            CommonTable.DataSource = new BindingList<RiseStockDto>(results);
            ShowDataNum(CommonTable);
            DisplayTime.Text = $"Q4a：{ShowTime()}{Environment.NewLine}{DisplayTime.Text}";
        }

        /// <summary>
        /// 查詢總統大選後第一個交易日以及第30個交易日類股收盤價以及漲跌
        /// </summary>
        /// <param name="targetDay">總統大選日期</param>
        /// <param name="lastDayPosition">選後幾個交易日</param>
        /// <returns>類股收盤價以及漲跌</returns>
        private List<RiseStockDto> GetRiseStocks(string targetDay, int lastDayPosition)
        {
            List<string> dayInterval = GetPresidentialDay(targetDay, lastDayPosition);
            string LastDay = dayInterval.Last();
            string FirstDay = dayInterval.First();
            IQueryable<KindStock> kindStocks = GetKindStocks();
            return StockDB.日收盤.Join(kindStocks, day => day.股票代號, code => code.代號, (day, code) => new
                    {
                        day.日期,
                        code.代號,
                        code.名稱,
                        day.收盤價
                    })
                .GroupBy(day => day.代號, (id, days) => new RiseStockDto
                    {
                        年度 = days.FirstOrDefault(day => day.日期 == LastDay).日期.Substring(0, YEAR_WORDS),
                        類股股票代號 = days.FirstOrDefault().代號,
                        類股股票名稱 = days.FirstOrDefault().名稱,
                        第30個交易日日期 = days.FirstOrDefault(day => day.日期 == LastDay).日期,
                        第30個交易日收盤價 = days.FirstOrDefault(day => day.日期 == LastDay).收盤價,
                        第1個交易日日期 = days.FirstOrDefault(day => day.日期 == FirstDay).日期,
                        第1個交易日收盤價 = days.FirstOrDefault(day => day.日期 == FirstDay).收盤價,
                        股價漲跌 = days.FirstOrDefault(day => day.日期 == LastDay).收盤價 - days.FirstOrDefault(day => day.日期 == FirstDay).收盤價
                    })
                .Where(day => day.股價漲跌 > 0).OrderBy(day => day.年度).ThenBy(day => day.類股股票代號).ToList();
        }

        /// <summary>
        /// 查所有類股的資料
        /// </summary>
        /// <returns>類股的資料</returns>
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

        /// <summary>
        /// 查總統大選後到指定數量交易日的範圍
        /// </summary>
        /// <param name="targetDay">總統大選日</param>
        /// <param name="lastDayPosition">指定數量</param>
        /// <returns>交易日的範圍</returns>
        private List<string> GetPresidentialDay(string targetDay, int lastDayPosition)
        {
            List<string> days = StockDB.日收盤.Where(day => day.股票代號 == "TWA00" && string.Compare(day.日期, targetDay) > 0)
                .OrderBy(day => day.日期).Select(day => day.日期).Take(lastDayPosition).ToList();
            return days;
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
            CommonTable.DataSource = new BindingList<RateReturn>(GetSameRanking());
            ShowDataNum(CommonTable);
            DisplayTime.Text = $"Q4b：{ShowTime()}{Environment.NewLine}{DisplayTime.Text}";
        }

        /// <summary>
        /// 找出前20名且排名相同的類股
        /// </summary>
        /// <returns>排名相同的類股</returns>
        private List<RateReturn> GetSameRanking()
        {
            IEnumerable<RateReturn> result2008 = GetRateReturns(GetRate(PRESIDENTIAL_DATE_2008, TRADING_DAY_30TH)).Take(TOP_20);
            IEnumerable<RateReturn> result2012 = GetRateReturns(GetRate(PRESIDENTIAL_DATE_2012, TRADING_DAY_30TH)).Take(TOP_20);
            return result2008.Zip(result2012, (early, late) => (early, late))
                .Select(day => new List<RateReturn>
                    {
                        day.early, day.late
                    })
                .Where(day => day.First().類股股票代號 == day.Last().類股股票代號)
                .SelectMany(day => day).OrderBy(day => day.年度).ThenBy(day => day.報酬率).ToList();
        }

        /// <summary>
        /// 找出類股排名
        /// </summary>
        /// <param name="kindStocks">要排名的類股</param>
        /// <returns>排好名的類股</returns>
        private IEnumerable<RateReturn> GetRateReturns(IEnumerable<KindRateDto> kindStocks)
        {
            return kindStocks.Select((stock, index) => new RateReturn
                {
                    年度 = stock.年度,
                    類股股票代號 = stock.類股股票代號,
                    類股股票名稱 = stock.類股股票名稱,
                    報酬率 = stock.報酬率,
                    報酬率排名 = index + 1
                });
        }

        /// <summary>
        /// 取得指定期間的類股報酬率
        /// </summary>
        /// <param name="targetDay">起始日</param>
        /// <param name="statisticsDay">起始日後幾個交易日</param>
        /// <returns>類股報酬率資訊</returns>
        private IEnumerable<KindRateDto> GetRate(string targetDay, int statisticsDay)
        {
            IQueryable<KindStock> kindStocks = GetKindStocks();
            List<string> dayInterval = GetPresidentialDay(targetDay, statisticsDay);
            string LastDay = dayInterval.Last();
            string FirstDay = dayInterval.First();
            int rateOfReturn = 100;
            return StockDB.日收盤
                .Join(kindStocks, day => day.股票代號, code => code.代號, (day, code) => new
                    {
                        day.日期,
                        code.代號,
                        code.名稱,
                        code.C200707整併後交易所產業編號,
                        day.收盤價,
                        day.上市櫃,
                    })
                .GroupBy(day => day.代號, (id, days) => new
                    {
                        年度 = days.FirstOrDefault(day => day.日期 == LastDay).日期.Substring(0, YEAR_WORDS),
                        類股股票代號 = days.FirstOrDefault().代號,
                        類股股票名稱 = days.FirstOrDefault().名稱,
                        days.FirstOrDefault().C200707整併後交易所產業編號,
                        days.FirstOrDefault().上市櫃,
                        第30個交易日收盤價 = days.FirstOrDefault(day => day.日期 == LastDay).收盤價,
                        第1個交易日收盤價 = days.FirstOrDefault(day => day.日期 == FirstDay).收盤價
                    })
                .Select(day => new KindRateDto
                    {
                        年度 = day.年度,
                        類股股票代號 = day.類股股票代號,
                        類股股票名稱 = day.類股股票名稱,
                        報酬率 = (day.第30個交易日收盤價 - day.第1個交易日收盤價) / day.第1個交易日收盤價 * rateOfReturn,
                        上市櫃 = day.上市櫃,
                        C200707整併後交易所產業編號 = day.C200707整併後交易所產業編號
                    })
                .OrderByDescending(day => day.報酬率);
        }

        /// <summary>
        /// 查詢總統大選報酬率排名相同的類股中的成分個股資訊，找出時間區間內，各類股中平均成交量前五大的個股
        /// </summary>
        /// <param name="targetDay">總統大選日期</param>
        /// <param name="sameStock">報酬率排名相同的類股</param>
        /// <returns>類股中平均成交量前五大的個股</returns>
        private List<Top5VolumeDto> GetTop5Volume(string targetDay, IEnumerable<Top5VolumeDto> sameStock)
        {
            List<string> dayInterval = GetPresidentialDay(targetDay, TRADING_DAY_30TH);
            string targetYear = targetDay.Substring(0, YEAR_WORDS);
            var sameData = sameStock.Where(same => same.年度 == targetYear)
                .Select(same => new
                    {
                        same.年度,
                        same.類股股票代號,
                        same.類股股票名稱,
                        same.產業代號,
                       上市櫃 = StockDB.日收盤.FirstOrDefault(stock => stock.股票代號 == same.類股股票代號).上市櫃
                    });
            return StockDB.日收盤.Where(stock => dayInterval.Contains(stock.日期) && stock.產業代號 != null)
                            .GroupBy(stock => new
                                {
                                    stock.股票代號,
                                    stock.產業代號,
                                    stock.上市櫃
                                },
                                (key, data) => new
                                {
                                    key.上市櫃,
                                    key.股票代號,
                                    data.FirstOrDefault().股票名稱,
                                    key.產業代號,
                                    平均成交量 = data.Average(item => (decimal)item.成交量)
                                }).AsEnumerable()
                                .Join(sameData,
                                                stock => new { stock.產業代號, stock.上市櫃 },
                                                same => new { same.產業代號, same.上市櫃 },
                                                (stock, same) => new Top5VolumeDto
                                                    {
                                                        年度 = same.年度,
                                                        類股股票代號 = same.類股股票代號,
                                                        類股股票名稱 = same.類股股票名稱,
                                                        股票代號 = stock.股票代號,
                                                        股票名稱 = stock.股票名稱,
                                                        產業代號 = stock.產業代號,
                                                        平均成交量 = stock.平均成交量
                                                    })
                                .OrderByDescending(stock => stock.平均成交量).Take(TOP_5).ToList();
        }

        /// <summary>
        /// 將類股依C200707整併後交易所產業編號，分成不同筆資料，以利後續join
        /// </summary>
        /// <returns>分割完的類股資料</returns>
        private IEnumerable<Top5VolumeDto> GetKindCodes()
        {
            IQueryable<KindStock> kindStocks = GetKindStocks();
            return GetSameRanking().Join(kindStocks, same => same.類股股票代號, kind => kind.代號, (same, kind) => new
            {
                same.年度,
                same.類股股票代號,
                same.類股股票名稱,
                C200707整併後交易所產業編號 = kind.C200707整併後交易所產業編號.Split(',')
            })
                .SelectMany(same => same.C200707整併後交易所產業編號, (same, code) => new Top5VolumeDto
                    {
                        年度 = same.年度,
                        類股股票代號 = same.類股股票代號,
                        類股股票名稱 = same.類股股票名稱,
                        產業代號 = code.Replace("\'", string.Empty)
                    });
        }

        /// <summary>
        /// 按"成交量前五大的個股"，查詢總統大選報酬率排名相同的類股中的成分個股資訊，找出時間區間內，各類股中平均成交量前五大的個股
        /// </summary>
        /// <param name="sender">觸發事件的控件</param>
        /// <param name="e">事件引數</param>
        private void ClickTop5VolumeButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            IEnumerable<Top5VolumeDto> sameStock = GetKindCodes();
            List<Top5VolumeDto> result = GetTop5Volume(PRESIDENTIAL_DATE_2008, sameStock);
            result.AddRange(GetTop5Volume(PRESIDENTIAL_DATE_2012, sameStock));
            CommonTable.Columns.Clear();
            CommonTable.Rows.Clear();
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
            List<RiseFallDto> riseFall2008 = RiseFall(PRESIDENTIAL_DATE_2008).ToList();
            List<KindRateDto> kinds2008 = GetRate(PRESIDENTIAL_DATE_2008, TRADING_DAY_100TH).ToList();
            List<RiseFallDto> riseFall2012 = RiseFall(PRESIDENTIAL_DATE_2012).ToList();
            List<KindRateDto> kinds2012 = GetRate(PRESIDENTIAL_DATE_2012, TRADING_DAY_100TH).ToList();
            IEnumerable<RiseFallTop5Dto> riseTop2008 = RiseFallTop5(kinds2008);
            IEnumerable<RiseFallTop5Dto> riseTop2012 = RiseFallTop5(kinds2012);
            IEnumerable<RiseFallTop5Dto> riseAfter2008 = RiseFallAfter5(kinds2008);
            IEnumerable<RiseFallTop5Dto> riseAfter2012 = RiseFallAfter5(kinds2012);
            IEnumerable<OriginalDataDto> originalTop2008 = GetRiseOriginalData(riseFall2008, kinds2008);
            IEnumerable<OriginalDataDto> originalTop2012 = GetRiseOriginalData(riseFall2012, kinds2012);
            IEnumerable<OriginalDataDto> originalAfter2008 = GetFallOriginalData(riseFall2008, kinds2008);
            IEnumerable<OriginalDataDto> originalAfter2012 = GetFallOriginalData(riseFall2012, kinds2012);
            RiseFallTop5Table.DataSource = new BindingList<RiseFallTop5Dto>(riseTop2008.Union(riseAfter2008).Union(riseTop2012).Union(riseAfter2012).ToList());
            OriginalDataTable.DataSource = new BindingList<OriginalDataDto>(originalTop2008.Union(originalAfter2008).Union(originalTop2012).Union(originalAfter2012).ToList());
            CommonTable.DataSource = new BindingList<IndividualStockDto>(
                GetIndividualStocks(riseTop2008, originalTop2008)
                .Union(GetIndividualStocks(riseAfter2008, originalAfter2008))
                .Union(GetIndividualStocks(riseTop2012, originalTop2012))
                .Union(GetIndividualStocks(riseAfter2012, originalAfter2012))
                .ToList());
            DataNum.Text = $"{CommonTable.RowCount},{RiseFallTop5Table.RowCount},{OriginalDataTable.RowCount}筆";
            DisplayTime.Text = $"Q4d：{ShowTime()}{Environment.NewLine}{DisplayTime.Text}";
        }

        /// <summary>
        /// 取得報酬率大於0前5名類股
        /// </summary>
        /// <param name="kinds">所有類股資料</param>
        /// <returns>報酬率大於0前5名類股</returns>
        private IEnumerable<RiseFallTop5Dto> RiseFallTop5( List<KindRateDto> kinds)
        {
            IEnumerable<RiseFallTop5Dto> top5 = kinds.Where(kind => kind.報酬率 > 0).Take(TOP_5)
                .Select((top, index) => new RiseFallTop5Dto
                    {
                    年度 = top.年度,
                    類股股票代號 = top.類股股票代號,
                    類股股票名稱 = top.類股股票名稱,
                    報酬率 = top.報酬率,
                    項目 = "最高",
                    排名 = index + 1
                });
            return top5;
        }

        /// <summary>
        /// 取得報酬率小於0後5名類股
        /// </summary>
        /// <param name="kinds">所有類股資料</param>
        /// <returns>酬率小於0後5名類股</returns>
        private IEnumerable<RiseFallTop5Dto> RiseFallAfter5( List<KindRateDto> kinds)
        {
            IEnumerable<RiseFallTop5Dto> after5 = kinds.Where(kind => kind.報酬率 < 0)
                .OrderBy(kind => kind.報酬率).Take(5)
                .Select((after, index) => new RiseFallTop5Dto
                {
                    年度 = after.年度,
                    類股股票代號 = after.類股股票代號,
                    類股股票名稱 = after.類股股票名稱,
                    報酬率 = after.報酬率,
                    項目 = "最低",
                    排名 = index + 1
                });
            return after5;
        }

        /// <summary>
        /// 取得上漲表現遞減，股票代號遞增，取得10檔個股，以及包含這些個股的類股
        /// </summary>
        /// <param name="riseFall">個股資訊</param>
        /// <param name="kinds">類股資訊</param>
        /// <returns>計算完上漲的10檔個股資訊</returns>
        private IEnumerable<OriginalDataDto> GetRiseOriginalData(List<RiseFallDto> riseFall, List<KindRateDto> kinds)
        {
            IOrderedEnumerable<OriginalDataDto> riseResult = riseFall
                .OrderByDescending(rise => rise.上漲表現)
                .ThenBy(rise => rise.股票代號).Take(10)
                .Select(rise => new
                    {
                        rise.年度,
                        rise.股票代號,
                        rise.股票名稱,
                        rise.上漲表現,
                        rise.下跌表現,
                        kind = kinds.Where(kind => kind.C200707整併後交易所產業編號.Contains(rise.產業代號) && kind.上市櫃 == rise.上市櫃)
                    })
                .SelectMany(rise => rise.kind, (rise, kind) => new OriginalDataDto
                    {
                        年度 = rise.年度,
                        類股股票代號 = kind.類股股票代號,
                        類股股票名稱 = kind.類股股票名稱,
                        股票代號 = rise.股票代號,
                        股票名稱 = rise.股票名稱,
                        上漲表現 = rise.上漲表現,
                        下跌表現 = rise.下跌表現,
                    })
                .OrderByDescending(rise => rise.上漲表現).ThenBy(rise => rise.股票代號).ThenBy(rise => rise.類股股票代號);
            return riseResult;
        }

        /// <summary>
        /// 取得下跌表現遞減，股票代號遞增，取得10檔個股，以及包含這些個股的類股
        /// </summary>
        /// <param name="riseFall">個股資訊</param>
        /// <param name="kinds">類股資訊</param>
        /// <returns>計算完下跌的10檔個股資訊</returns>
        private IEnumerable<OriginalDataDto> GetFallOriginalData(List<RiseFallDto> riseFall, List<KindRateDto> kinds)
        {
            IOrderedEnumerable<OriginalDataDto> fallResult = riseFall.OrderByDescending(rise => rise.下跌表現).ThenBy(rise => rise.股票代號).Take(10)
                .Select(rise => new
                    {
                        rise.年度,
                        rise.股票代號,
                        rise.股票名稱,
                        rise.上漲表現,
                        rise.下跌表現,
                        kind = kinds.Where(kind => kind.C200707整併後交易所產業編號.Contains(rise.產業代號) && kind.上市櫃 == rise.上市櫃)
                    })
                .SelectMany(rise => rise.kind, (rise, kind) => new OriginalDataDto
                    {
                        年度 = rise.年度,
                        類股股票代號 = kind.類股股票代號,
                        類股股票名稱 = kind.類股股票名稱,
                        股票代號 = rise.股票代號,
                        股票名稱 = rise.股票名稱,
                        上漲表現 = rise.上漲表現,
                        下跌表現 = rise.下跌表現,
                    })
                .OrderByDescending(rise => rise.下跌表現).ThenBy(rise => rise.股票代號).ThenBy(rise => rise.類股股票代號);
            return fallResult;
        }

        /// <summary>
        /// 查詢於總統大選100天後，股票的上漲表現與下跌表現
        /// </summary>
        /// <param name="targetDay">總統大選日期</param>
        /// <returns>計算完上漲表現與下跌表現的股票</returns>
        private IEnumerable<RiseFallDto> RiseFall(string targetDay)
        {
            List<string> day = GetPresidentialDay(targetDay, TRADING_DAY_100TH);
            var FirstDay = day.First();
            var LastDay = day.Last();
            int count;
            int riseDay;
            int riseAllDay;
            int fallCount;
            int fallDay;
            int fallAllDay;
            IQueryable<日收盤> records = StockDB.日收盤.Where(stock => day.Contains(stock.日期) && stock.產業代號 != null);
            return records.ToList().GroupBy(stock => new { stock.股票代號, stock.上市櫃, stock.產業代號 }, (id, stocks) =>
              {
                  count = 0;
                  riseDay = 0;
                  fallCount = 0;
                  fallDay = 0;
                  riseAllDay = 0;
                  fallAllDay = 0;
                  var record = day.GroupJoin(stocks, days => days, stock => stock.日期, (days, stock) => new
                      {
                          days,
                          stock
                      })
                  .SelectMany(days => days.stock.DefaultIfEmpty(), (days, stock) => stock);
                  record.Select(riseStock =>
                  {
                      if (riseStock == null)
                      {
                          if (count < riseDay)
                          {
                              count = riseDay;
                          }
                          riseDay = 0;
                          if (fallCount < fallDay)
                          {
                              fallCount = fallDay;
                          }
                          fallDay = 0;
                      }
                      else
                      {
                          if (riseStock.漲跌 > 0)
                          {
                              riseAllDay++;
                              riseDay++;
                          }
                          else
                          {
                              if (count < riseDay)
                              {
                                  count = riseDay;
                              }
                              riseDay = 0;
                          }
                          if (riseStock.漲跌 < 0)
                          {
                              fallAllDay++;
                              fallDay++;
                          }
                          else
                          {
                              if (fallCount < fallDay)
                              {
                                  fallCount = fallDay;
                              }
                              fallDay = 0;
                          }
                      }
                      return count;
                  }).ToList();
                  if (count < riseDay)
                  {
                      count = riseDay;
                  }
                  riseDay = 0;
                  if (fallCount < fallDay)
                  {
                      fallCount = fallDay;
                  }
                  fallDay = 0;
                  return new RiseFallDto
                  {
                      年度 = stocks.Select(data => data.日期).FirstOrDefault().Substring(0, YEAR_WORDS),
                      股票代號 = id.股票代號,
                      股票名稱 = stocks.Select(data => data.股票名稱).FirstOrDefault(),
                      上漲表現 = riseAllDay + count,
                      下跌表現 = fallAllDay + fallCount,
                      產業代號 = id.產業代號,
                      上市櫃 = id.上市櫃
                  };
              });
        }

        /// <summary>
        /// 查詢上漲下跌表現前10位的個股與類股前5位的對應關係
        /// </summary>
        /// <param name="riseTop">類股前5</param>
        /// <param name="originalTop">上漲下跌表現前10位</param>
        /// <returns>計算後的資訊</returns>
        private IEnumerable<IndividualStockDto> GetIndividualStocks(IEnumerable<RiseFallTop5Dto> riseTop, IEnumerable<OriginalDataDto> originalTop)
        {
            return originalTop.Select(stock => new IndividualStockDto
                    {
                        年度 = stock.年度,
                        類股股票代號 = stock.類股股票代號,
                        類股股票名稱 = stock.類股股票名稱,
                        項目 = stock.上漲表現 > stock.下跌表現 ? "最高" : "最低",
                        是否存在 = riseTop.Any(rise => rise.類股股票代號 == stock.類股股票代號) ? "是" : "否"
                    })
                .GroupBy(stock => stock.類股股票代號).Select(stocks => stocks.FirstOrDefault())
                .OrderByDescending(stock => stock.是否存在)
                .ThenByDescending(stock => stock.項目)
                .ThenBy(stock => stock.類股股票代號);
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
