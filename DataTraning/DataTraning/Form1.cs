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

        private HttpClient HttpGetter;
        public Form1()
        {
            InitializeComponent();
            StockDB = new StockDBEntities();
            HttpGetter = new HttpClient();
        }

        private void ClickVoteDayAddButton(object sender, EventArgs e)
        {
            int.TryParse(Regex.Match(GetWebPage(Global.STOCK_VOTE), @"頁次：1/(?<lastPage>\d+)<td>").Groups["lastPage"].Value, out int page);
            for (int i = 1; i < page; i++)
            {
                string stockVotePage = GetWebPage($"{Global.STOCK_VOTE_PAGE}{i}");
                MatchCollection stockVoteDatas = Regex.Matches(stockVotePage, @"<tr class=""Font_001"">(.*?)</tr>", RegexOptions.Singleline);
                foreach (Match data in stockVoteDatas)
                {
                    
                }
                TimeText.Text = stockVotePage;
            }
        }

        public string GetWebPage(string url)
        {
            var responseMessage = HttpGetter.GetAsync(url).Result;
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string responseResult = responseMessage.Content.ReadAsStringAsync().Result;
                return responseResult;
            }
            return null;
        }
    }
}
