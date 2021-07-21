using Readcsv2020LuAnn.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        /// <summary>
        /// 除了All以外，其他的股票list的第一個就是他的股票資料
        /// </summary>
        public const int STOCK_POSITION = 0;
        public ReadCSV()
        {
            InitializeComponent();
            Stocks = new Dictionary<string, List<Stock>>();
        }
        /// <summary>
        /// 點擊讀取檔案讓使用者選擇檔案並觸發讀檔的動作
        /// </summary>
        /// <param name="sender">觸發的物件</param>
        /// <param name="eventArgs">觸發的事件</param>
        private void ClickReadButton(object sender, EventArgs eventArgs)
        {
            Stocks.Clear();
            Stocks.Add("All", new List<Stock>());
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
            if (ReadData.IsBusy != true)
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
            using (StreamReader streamReader = new StreamReader(File.OpenRead(InputBox.Text), System.Text.Encoding.GetEncoding("Big5")))//打開data.csv檔
            {
                StopWatch.GetStopWatch().Start();
                streamReader.ReadLine();//讀檔
                while (!streamReader.EndOfStream)//假如未讀取完所有資料
                {
                    string[] datas = streamReader.ReadLine().Split(',');//將資料以'，'做切割
                    if (Stocks.TryGetValue(datas[(int)Stock.Column.STOCK_ID], out List<Stock> stock))
                    {
                        stock.First().AddRecord(datas);
                    }
                    else
                    {
                        List<Stock> list = new List<Stock>();
                        list.Add(new Stock(datas));
                        Stocks.Add(datas[(int)Stock.Column.STOCK_ID], list);//將切割結果作為參數建立股票紀錄物件並放入股票紀錄物件集合中
                        Stocks["All"].Add(list.First());
                    }
                }
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
            TextDisplay.Text = $"讀取時間:{StopWatch.GetStopWatch().End()}";
            Prompt.Text = "讀取完畢";
            SetDropMenu();
        }

        /// <summary>
        /// 設定下拉式選單的內容
        /// </summary>
        public void SetDropMenu()
        {
            DropMenu.BeginUpdate();
            StopWatch.GetStopWatch().Start();
            var menu = Stocks.Skip(1).Select(stocks => $"{stocks.Key} - {stocks.Value.First().StockName}").ToArray();
            DropMenu.Items.Add("All");
            DropMenu.Items.AddRange(menu);
            DropMenu.Text = "All";
            DropMenu.EndUpdate();
            TextDisplay.Text += $"{Environment.NewLine}ComboBox時間:{StopWatch.GetStopWatch().End()}";

        }

        /// <summary>
        /// 點擊股票查詢，查詢股票的BuyTotal、SellTotal、AvgPrice、BuySellOver、SecBroker數量
        /// </summary>
        /// <param name="sender">觸發的物件</param>
        /// <param name="eventArgs">觸發的事件</param>
        private void ClickCheckStock(object sender, EventArgs eventArgs)
        {
            StopWatch.GetStopWatch().Start();
            List<Stock> stocks = ReadDropMenu();
            WriteBasicData(stocks);
            WriteTotalData(stocks);
            TextDisplay.Text += $"{Environment.NewLine}查詢時間:{StopWatch.GetStopWatch().End()}";
        }


        /// <summary>
        /// 讀取下拉式選單中使用者所選的股票
        /// </summary>
        /// <returns>股票集合</returns>
        private List<Stock> ReadDropMenu()
        {
            string option = DropMenu.Text;
            option = option.Split('-').First();
            var datas = option.Split(',');
            List<Stock> stocks = new List<Stock>();
            foreach (var data in datas)
            {
                string mark = data.Trim();
                if (Stocks.TryGetValue(mark, out List<Stock> stock) && !stocks.Contains(stock.First()))
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
            var stockList = stocks.SelectMany(stock => stock.StockRecord, (stock, record) => new StockDto
            {
                DealDate = record.DealDate,
                StockID = stock.StockID,
                StockName = stock.StockName,
                SecBrokerID = record.SecBrokerID,
                SecBrokerName = record.SecBrokerName,
                BuyQty = record.BuyQty,
                SellQty = record.SellQty,
                Price = record.Price
            }).ToList();
            StockList.DataSource = new BindingList<StockDto>(stockList);
        }
        /// <summary>
        /// 將所選股票的BuyTotal、SellTotal、AvgPrice、BuySellOver、SecBroker顯示於畫面上
        /// </summary>
        /// <param name="stocks">所選股票</param>
        public void WriteTotalData(List<Stock> stocks)
        {
            List<TotalStockDto> stockList = new List<TotalStockDto>();
            int totalBuy;
            int totalSell;
            double totalPrice;
            HashSet<string> secBrokerID = new HashSet<string>();
            List<Record> stockRecord;
            foreach (var stock in stocks)
            {
                totalBuy = 0;
                totalSell = 0;
                totalPrice = 0;
                stockRecord = stock.StockRecord;
                foreach (var record in stockRecord)
                {
                    totalBuy += record.BuyQty;
                    totalSell += record.SellQty;
                    totalPrice += record.Price * (record.SellQty + record.BuyQty);
                    secBrokerID.Add(record.SecBrokerID);
                }
                stockList.Add(new TotalStockDto
                {
                    StockID = stock.StockID,
                    StockName = stock.StockName,
                    BuyTotal = totalBuy,
                    SellTotal = totalSell,
                    AvgPrice = totalPrice / (totalBuy + totalSell),
                    SecBrokerCnt = secBrokerID.Count(),
                    BuyCellOver = totalBuy - totalSell
                });
                secBrokerID.Clear();
            }
            TotalList.DataSource = new BindingList<TotalStockDto>(stockList);
        }

        /// <summary>
        /// 點擊買賣超top50 ，計算所選股票相同券商id的最大且>0及最小且<0的各50筆買賣超
        /// </summary>
        /// <param name="sender">觸發的物件</param>
        /// <param name="eventArgs">觸發的事件</param>
        private void ClickTop50Button(object sender, EventArgs eventArgs)
        {
            StopWatch.GetStopWatch().Start();
            List<Stock> stocks = ReadDropMenu();
            var top50 = GetTop50(stocks);
            Top50List.DataSource = new BindingList<Top50Dto>(top50);
            TextDisplay.Text += $"{Environment.NewLine}Top50 產生時間 :{StopWatch.GetStopWatch().End()}";
        }

        /// <summary>
        /// 取得所選股票的相同券商id最大且>0及最小且<0的各50筆買賣超
        /// </summary>
        /// <param name="stocks">所選股票</param>
        /// <returns>top50資料</returns>
        public List<Top50Dto> GetTop50(List<Stock> stocks)
        {
            List<Top50Dto> result = new List<Top50Dto>();
            foreach (var stock in stocks)
            {
                var stockList = stock.StockRecord.GroupBy(record => new
                {
                    record.SecBrokerID,
                    record.SecBrokerName
                }).Select(record => new Top50Dto
                {
                    StockName = stock.StockName,
                    SecBrokerName = record.Key.SecBrokerName,
                    BuyCellOver = record.Sum(total => total.BuyQty - total.SellQty)
                }).ToList();
                result.AddRange(SortTop50(stockList));
            }
            return result;

        }
        /// <summary>
        /// 將top50資料做排序取出相同券商id最大且>0及最小且<0的各50筆買賣超
        /// </summary>
        /// <param name="stockList">已轉換成top50形式的所選股票資料</param>
        /// <returns>篩選出的相同券商id最大且>0及最小且<0的各50筆買賣超</returns>
        public List<Top50Dto> SortTop50(List<Top50Dto> stockList)
        {
            int topNum = 50;
            List<Top50Dto> result50 = new List<Top50Dto>();
            List<Top50Dto> resultMin50 = new List<Top50Dto>();
            for (int i = 0; i < topNum; i++)
            {
                var max = stockList.First();
                var min = max;
                foreach (var stock in stockList)
                {
                    var brokerCnt = stock;
                    if (max.BuyCellOver < stock.BuyCellOver)
                    {
                        max = stock;
                    }
                    if (min.BuyCellOver > stock.BuyCellOver)
                    {
                        min = stock;
                    }
                }
                if (max.BuyCellOver > 0)
                {
                    result50.Add(max);
                    stockList.Remove(max);
                }
                if (min.BuyCellOver < 0)
                {
                    resultMin50.Add(min);
                    stockList.Remove(min);
                }
                if (stockList.Count == 0)
                {
                    break;
                }
            }
            result50.AddRange(resultMin50);
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
    }
}
