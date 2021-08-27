using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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


        public Form1()
        {
            InitializeComponent();
            StockDB = new StockDBEntities();
            StockVote = new StockVote(StockDB);
            FundNoBusinessDay = new FundNoBusinessDay(StockDB, TimeText);

        }

        private void ClickVoteDayAddButton(object sender, EventArgs e)
        {
            StockVote.AddReviseStockDetail();
        }

        private void ClickVoteDayDeleteButton(object sender, EventArgs e)
        {
            StockDB.股東會投票日明細_luann.RemoveRange(StockDB.股東會投票日明細_luann);
            StockDB.SaveChanges();
        }

        private void ClickVoteDataAddButton(object sender, EventArgs e)
        {
            StockVote.AddReviseStockData();
        }

        private void ClickVoteDataDeleteButton(object sender, EventArgs e)
        {
            StockDB.股東會投票資料表_luann.RemoveRange(StockDB.股東會投票資料表_luann);
            StockDB.SaveChanges();
        }

        private void ClickFundDetailAddButton(object sender, EventArgs e)
        {
           FundNoBusinessDay.AddReviseFundDetail();
        }

        private void ClickFundDetailDeleteButton(object sender, EventArgs e)
        {
            StockDB.基金非營業日明細_luann.RemoveRange(StockDB.基金非營業日明細_luann);
            StockDB.SaveChanges();
        }

        private void ClickFuturesPriceAddButton(object sender, EventArgs e)
        {

        }

        private void ClickFuturesPriceDeleteButton(object sender, EventArgs e)
        {
            StockDB.日期貨盤後行情表_luann.RemoveRange(StockDB.日期貨盤後行情表_luann);
            StockDB.SaveChanges();
        }

        private void ClickFundStatisticAddButton(object sender, EventArgs e)
        {

        }

        private void ClickFundStatisticDeleteButton(object sender, EventArgs e)
        {
            StockDB.基金非營業日統計_luann.RemoveRange(StockDB.基金非營業日統計_luann);
            StockDB.SaveChanges();
        }

        private void ClickFuturesStatisticAddButton(object sender, EventArgs e)
        {

        }

        private void ClickFuturesStatisticDeleteButton(object sender, EventArgs e)
        {
            StockDB.日期貨盤後統計表_luann.RemoveRange(StockDB.日期貨盤後統計表_luann);
            StockDB.SaveChanges();
        }
    }
}
