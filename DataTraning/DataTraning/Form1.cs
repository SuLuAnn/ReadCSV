using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataTraning
{
    public partial class Form1 : Form
    {
        private StockDBEntities StockDB;

        private StockVote StockVote;

        private FundNoBusinessDay FundNoBusinessDay;

        private Futures Futures;

        /// <summary>
        /// 計時器
        /// </summary>
        private Stopwatch Stopwatch;


        public Form1()
        {
            Stopwatch = new Stopwatch();
            InitializeComponent();
            StockDB = new StockDBEntities();
            StockVote = new StockVote(StockDB);
            FundNoBusinessDay = new FundNoBusinessDay(StockDB);
            Futures = new Futures(StockDB);
        }

        private void ClickVoteDayAddButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            StockVote.AddReviseStockDetail();
            TimeText.Text = $"時間：{ShowTime()}";
        }

        private void ClickVoteDayDeleteButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            StockDB.股東會投票日明細_luann.RemoveRange(StockDB.股東會投票日明細_luann);
            StockDB.SaveChanges();
            TimeText.Text = $"時間：{ShowTime()}";
        }

        private void ClickVoteDataAddButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            StockVote.AddReviseStockData();
            TimeText.Text = $"時間：{ShowTime()}";
        }

        private void ClickVoteDataDeleteButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            StockDB.股東會投票資料表_luann.RemoveRange(StockDB.股東會投票資料表_luann);
            StockDB.SaveChanges();
            TimeText.Text = $"時間：{ShowTime()}";
        }

        private void ClickFundDetailAddButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            FundNoBusinessDay.AddReviseFundDetail();
            TimeText.Text = $"時間：{ShowTime()}";
        }

        private void ClickFundDetailDeleteButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            StockDB.基金非營業日明細_luann.RemoveRange(StockDB.基金非營業日明細_luann);
            StockDB.SaveChanges();
            TimeText.Text = $"時間：{ShowTime()}";
        }

        private void ClickFundStatisticAddButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            FundNoBusinessDay.AddReviseFundStatistic();
            TimeText.Text = $"時間：{ShowTime()}";
        }

        private void ClickFundStatisticDeleteButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            StockDB.基金非營業日統計_luann.RemoveRange(StockDB.基金非營業日統計_luann);
            StockDB.SaveChanges();
            TimeText.Text = $"時間：{ShowTime()}";
        }
        private void ClickFuturesPriceAddButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            Futures.AddReviseFutursDetail();
            TimeText.Text = $"時間：{ShowTime()}";
        }

        private void ClickFuturesPriceDeleteButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            StockDB.日期貨盤後行情表_luann.RemoveRange(StockDB.日期貨盤後行情表_luann);
            StockDB.SaveChanges();
            TimeText.Text = $"時間：{ShowTime()}";
        }

        private void ClickFuturesStatisticAddButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            Futures.AddReviseFutursStatistic();
            TimeText.Text = $"時間：{ShowTime()}";
        }

        private void ClickFuturesStatisticDeleteButton(object sender, EventArgs e)
        {
            Stopwatch.Restart();
            StockDB.日期貨盤後統計表_luann.RemoveRange(StockDB.日期貨盤後統計表_luann);
            StockDB.SaveChanges();
            TimeText.Text = $"時間：{ShowTime()}";
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
    }
}
