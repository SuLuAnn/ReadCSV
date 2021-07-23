using Readcsv2020LuAnn.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Readcsv2020LuAnn
{
    public partial class ReadCSV : Form
    {
        /// <summary>
        /// 存放讀取的表單資料
        /// </summary>
        private Dictionary<string, List<Stock>> Stocks;

        private static Stopwatch Stopwatch;
        /// <summary>
        /// 除了All以外，其他的股票list的第一個就是他的股票資料
        /// </summary>
        public const string ALL = "All";
        public ReadCSV()
        {
            InitializeComponent();
            Stocks = new Dictionary<string, List<Stock>>();
            Stopwatch = new Stopwatch();
        }
        /// <summary>
        /// 點擊讀取檔案讓使用者選擇檔案並觸發讀檔的動作
        /// </summary>
        /// <param name="sender">觸發的物件</param>
        /// <param name="eventArgs">觸發的事件</param>
        private void ClickReadButton(object sender, EventArgs eventArgs)
        {
            Stocks.Clear();
            using (OpenFileDialog openFile = new OpenFileDialog())
            {
                openFile.Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*";

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    Prompt.Text = "讀檔中";
                    string fileName = openFile.FileName;
                    InputBox.Text = fileName;
                }
            }
            if (!ReadData.IsBusy)
            {
                ReadData.RunWorkerAsync();
            }

        }
        /// <summary>
        /// 點擊讀取檔案時觸發新的執行緒已匯入資料
        /// </summary>
        /// <param name="sender">觸發的物件</param>
        /// <param name="doWork">觸發的事件</param>
        private void ReadCSVData(object sender, DoWorkEventArgs doWork)
        {
            List<Stock> stocks = new List<Stock>();
            using (StreamReader streamReader = new StreamReader(File.OpenRead(InputBox.Text), System.Text.Encoding.GetEncoding("Big5")))//打開data.csv檔
            {
                Stopwatch.Restart();
                streamReader.ReadLine();//讀檔
                while (!streamReader.EndOfStream)//假如未讀取完所有資料
                {
                    string[] datas = streamReader.ReadLine().Split(',');//將資料以'，'做切割
                    stocks.Add(new Stock(datas));
                }
            }
            Stocks.Add(ALL, new List<Stock>());
            var list = stocks.GroupBy(stock => stock.StockID).ToDictionary(stock => stock.Key, stock => stock.ToList());
            foreach (var stock in list)
            {
                Stocks[ALL].AddRange(stock.Value);
                Stocks.Add(stock.Key,stock.Value);
            }

        }
        /// <summary>
        /// 當讀完檔案輸出讀取時間及告知使用者讀檔完畢，並出現所有股票的下拉式選單
        /// </summary>
        /// <param name="sender">觸發的物件</param>
        /// <param name="runWorker">觸發的事件</param>
        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorker)
        {
            
            WriteBasicData(Stocks.Values.First());
            TextDisplay.Text = $"讀取時間:{EndStopwatch()}";
            Prompt.Text = "讀取完畢";
            SetDropMenu();
        }

        /// <summary>
        /// 設定下拉式選單的內容
        /// </summary>
        public void SetDropMenu()
        {
            DropMenu.BeginUpdate();
            Stopwatch.Restart();
            var menu = Stocks.Skip(1).Select(stocks => $"{stocks.Key} - {stocks.Value.First().StockName}").ToArray();
            DropMenu.Items.Add(ALL);
            DropMenu.Items.AddRange(menu);
            DropMenu.Text = ALL;
            DropMenu.EndUpdate();
            TextDisplay.Text += $"{Environment.NewLine}ComboBox時間:{EndStopwatch()}";

        }

        /// <summary>
        /// 點擊股票查詢，查詢股票的BuyTotal、SellTotal、AvgPrice、BuySellOver、SecBroker數量
        /// </summary>
        /// <param name="sender">觸發的物件</param>
        /// <param name="eventArgs">觸發的事件</param>
        private void ClickCheckStock(object sender, EventArgs eventArgs)
        {
            Stopwatch.Restart();
            List<Stock> stocks = ReadDropMenu();
            if (!TotalData.IsBusy)
            {
                TotalData.RunWorkerAsync(stocks);
            }
            WriteBasicData(stocks);
        }
        public void DisplayTime(object sender, RunWorkerCompletedEventArgs runWorker)
        {
            TotalList.DataSource = new BindingList<TotalStockDto>((IList<TotalStockDto>)runWorker.Result);
            TextDisplay.Text += $"{Environment.NewLine}查詢時間:{EndStopwatch()}";
        }

        /// <summary>
        /// 讀取下拉式選單中使用者所選的股票
        /// </summary>
        /// <returns>股票集合</returns>
        private List<Stock> ReadDropMenu()
        {
            string option = DropMenu.Text;
            string[] datas;
            if (option.Contains('-'))
            {
                datas = new string[] { option.Split('-').First().Trim()};
            }
            else 
            {
                datas = option.Split(',');
            }
            List<Stock> stocks = new List<Stock>();
            foreach (var data in datas)
            {
                string mark = data;
                if (Stocks.TryGetValue(mark, out List<Stock> stock) && !stocks.Contains(stock.First()) && !stock.Contains(stocks.FirstOrDefault()))
                {
                    stocks.AddRange(stock);
                }
                else
                {
                    if (!string.IsNullOrEmpty(mark))
                    {
                        MessageBox.Show($"搜尋條件錯誤{mark}為無效或重複搜尋");
                    }
                }
            }
            return stocks;
        }
        /// <summary>
        /// 將所選股票的基本資料顯示於畫面
        /// </summary>
        /// <param name="stocks">所選股票</param>
        public void WriteBasicData(List<Stock> stocks)
        {
            StockList.DataSource = new BindingList<Stock>(stocks);
        }
        /// <summary>
        /// 將所選股票的BuyTotal、SellTotal、AvgPrice、BuySellOver、SecBroker顯示於畫面上
        /// </summary>
        /// <param name="stocks">所選股票</param>
        public void WriteTotalData(object sender, DoWorkEventArgs e)
        {
            List<Stock> stocks = (List<Stock>)e.Argument;
            Dictionary<string, TotalStockDto> stockList = new Dictionary<string, TotalStockDto>();
            foreach (var stock in stocks)
            {
                if (stockList.TryGetValue(stock.StockID, out TotalStockDto totalStock))
                {
                    totalStock.Add(stock);
                }
                else 
                {
                    stockList.Add(stock.StockID, new TotalStockDto(stock));
                }
            }
            var totalList = stockList.Values.ToList();
            foreach (var total in totalList)
            {
                total.SettleTotal();
            }
            e.Result = totalList;
        }

        /// <summary>
        /// 點擊買賣超top50 ，計算所選股票相同券商id的最大且大於0及最小且小於0的各50筆買賣超
        /// </summary>
        /// <param name="sender">觸發的物件</param>
        /// <param name="eventArgs">觸發的事件</param>
        private void ClickTop50Button(object sender, EventArgs eventArgs)
        {
            Stopwatch.Restart();
            List<Stock> stocks = ReadDropMenu();
            Dictionary<string, Dictionary<string, Top50Dto>> top50 = GetTop50(stocks);
            List<Top50Dto> top50result = SortTop50(ref top50);
            Top50List.DataSource = new BindingList<Top50Dto>(top50result);
            TextDisplay.Text += $"{Environment.NewLine}Top50 產生時間 :{EndStopwatch()}";

        }

        /// <summary>
        /// 取得所選股票的相同券商id最大且大於0及最小且小於0的各50筆買賣超
        /// </summary>
        /// <param name="stocks">所選股票</param>
        /// <returns>top50資料</returns>
        public Dictionary<string, Dictionary<string, Top50Dto>> GetTop50(List<Stock> stocks)
        {
            Dictionary<string, Dictionary<string, Top50Dto>> result = new Dictionary<string, Dictionary<string, Top50Dto>>();
            foreach (var stock in stocks)
            {
                if (result.TryGetValue((stock.StockID), out Dictionary<string, Top50Dto> record))
                {
                    if (record.TryGetValue(stock.SecBrokerID, out Top50Dto top50))
                    {
                        top50.Add(stock.BuyQty - stock.SellQty);
                    }
                    else
                    {
                        record.Add(stock.SecBrokerID, new Top50Dto(stock));
                    }
                }
                else
                {
                    result.Add(stock.StockID, new Dictionary<string, Top50Dto>
                    {
                        {stock.SecBrokerID,new Top50Dto(stock)}
                    });
                }
            }
            return result;
        }
        /// <summary>
        /// 將top50資料做排序取出相同券商id最大且大於0及最小且小於0的各50筆買賣超
        /// </summary>
        /// <param name="stockList">已轉換成top50形式的所選股票資料</param>
        /// <returns>篩選出的相同券商id最大且大於0及最小且小於0的各50筆買賣超</returns>
        public List<Top50Dto> SortTop50(ref Dictionary<string, Dictionary<string, Top50Dto>> stockList)
        {
            int topNum = 50;
            List<Top50Dto> result50 = new List<Top50Dto>();
            foreach (var stock in stockList.Values)
            {
                result50.AddRange(stock.Values.Where(top50 => top50.BuySellOver > 0).OrderByDescending(top50 => top50.BuySellOver).Take(topNum).ToList());
                result50.AddRange(stock.Values.Where(top50 => top50.BuySellOver < 0).OrderBy(top50 => top50.BuySellOver).Take(topNum).ToList());
            }
            return result50;
        }
        /// <summary>
        /// 當文字顯示框出現新文字，讓ScrollBar滾到最下
        /// </summary>
        /// <param name="sender">觸發的物件</param>
        /// <param name="eventArgs">觸發的事件</param>
        private void ChangTextDisplay(object sender, EventArgs eventArgs)
        {
            TextDisplay.SelectionStart = TextDisplay.Text.Length;
            TextDisplay.ScrollToCaret();
        }
        /// <summary>
        /// 時間結束
        /// </summary>
        /// <returns>所費時間字串</returns>
        public string EndStopwatch()
        {
            Stopwatch.Stop();
            return Stopwatch.Elapsed.ToString(@"hh\:mm\:ss\:fff");
        }





        //public List<Top50Dto> GetTop50(List<Stock> stocks)
        //{
        //    var result1 = stocks.GroupBy(stock => stock.StockID, (id, stockList) => new
        //    {
        //        SecBroker = stockList.GroupBy(record => record.SecBrokerID, (secId, records) => new Top50Dto
        //        {
        //            StockName = stockList.First().StockName,
        //            SecBrokerName = records.First().SecBrokerName,
        //            BuySellOver = records.Sum(data => data.BuyQty - data.SellQty)
        //        }).OrderByDescending(stock => stock.BuySellOver).TakeWhile((stock, index) => (stock.BuySellOver > 0 && index < 51) || (stock.BuySellOver < 0 && index > stockList.Count() - 50))
        //    }).SelectMany(stock=>stock.SecBroker).ToList();
        //    return result1;
        //}
    }
}
