using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncAwaitWinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string fooStr = await GetStringAsync();
        }

        public async Task<string> GetStringAsync()
        {
            HttpClient client = new HttpClient();
            return await client.GetStringAsync("http://www.google.com.tw");
            
        }
    }
}
