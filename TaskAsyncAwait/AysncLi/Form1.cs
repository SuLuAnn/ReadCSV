using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AysncLi
{
    /// <summary>
    /// 為了反編譯用的 本身無特別之處
    /// </summary>
    public class Form1
    {
        private TextBox TextBox;
        private async void button1_Click(object sender, EventArgs e)
        {
            string fooStr = await GetStringAsync();
            TextBox.Text = fooStr;
        }

        public async Task<string> GetStringAsync()
        {
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync("http://www.google.com.tw");
            return result;

        }
    }

  
}
