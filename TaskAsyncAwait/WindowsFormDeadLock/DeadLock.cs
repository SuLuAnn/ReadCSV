using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormDeadLock
{
    public partial class DeadLock : Form
    {
        private CancellationTokenSource Cancel = new CancellationTokenSource();
        public DeadLock()
        {
            InitializeComponent();
        }

        // My "library" method.
        private async Task<string> GetJsonAsync(string uri)
        {
            HttpClient client = new HttpClient();
            string jsonString = string.Empty;

            jsonString = await client.GetStringAsync(uri);


            return jsonString;
        }

        private async Task<string> GetJsonAsync1(string uri)
        {
            HttpClient client = new HttpClient();
            string jsonString = string.Empty;

            jsonString =await client.GetStringAsync(uri).ConfigureAwait(false);


            return jsonString;
        }

        private  void Dead1_Click(object sender, EventArgs e)
        {
            textBox1.Text = GetStringAsync().Result;
        }

        private async Task<string> GetStringAsync()
        {
            return await _httpClient.GetStringAsync("https://www.google.com").ConfigureAwait(false);
        }
        static HttpClient _httpClient = new HttpClient();
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = GetStringAsync().Result;
        }

        private async void Dead2_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = await GetJsonAsync("https://www.google.com.tw/");
        }

        private void Dead3_Click(object sender, EventArgs e)
        {
            //從當前的同步環境取得工作排程器
            var synchronizationContext = TaskScheduler.FromCurrentSynchronizationContext();
            var result = GetStringAsync();
           result.ContinueWith(task => textBox1.Text =  task.Result, synchronizationContext);
        }

        private void Dead4_Click(object sender, EventArgs e)
        {
            textBox1.Text = GetJsonAsync1("https://www.google.com.tw/").Result;
        }

        private void Dead5_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Console.WriteLine("Long operation on another thread");
                Dead5.Invoke(new Action(() =>
                   // This will occur on the UI Thread
                   textBox1.Text = "operation finished"
                    ));
                //刪掉Wait就不會死鎖
                }).Wait();
            Console.WriteLine("Do more stuff after the long operation is finished");
        }
    }
}
