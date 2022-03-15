using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmericaStockTest
{
    public partial class Form1 : Form
    {
        private readonly static HttpClient HttpGetter = new HttpClient();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = "https://www.nasdaq.com/";
            var result = HttpGetter.GetAsync(url).Result;
            //textBox1.Text = result;
            //SaveFile(result, "東方_美股一覽_總.html");
        }

        public static Task<string> GetWebPageAsync(string url)
        {
            CookieContainer cookies = new CookieContainer();
            Uri uri = new Uri("https://www.futunn.com");
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            HttpClient client = new HttpClient(handler);
            var a = client.GetAsync("https://www.futunn.com").Result;
            IEnumerable<Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();
            return client.GetStringAsync(url);
            //return HttpGetter.GetStringAsync(url);
        }

        public static string HtmlPost(string requestUrl, string data, string encoded)
        {
            StringContent queryString = new StringContent(data);
            HttpResponseMessage response = HttpGetter.PostAsync(requestUrl, queryString).Result;
            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.CharSet = encoded;
            return response.Content.ReadAsStringAsync().Result;
        }

        public static void SaveFile(string file, string fileName)
        {
            //組檔案路徑
            string path = Path.Combine(Environment.CurrentDirectory, "test", fileName);
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.Write(file);
            }
        }
    }
}
