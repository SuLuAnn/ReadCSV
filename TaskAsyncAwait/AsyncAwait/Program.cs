using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class Program
    {
        static void Main(string[] args)
        {
            Task<string> task = MyDownloadPageAsync("https://www.huanlintalk.com/");

            string content = task.Result; // 取得非同步工作的結果。

            Console.WriteLine("網頁內容總共為 {0} 個字元。", content.Length);
            Console.ReadKey();
        }
        static async Task<string> MyDownloadPageAsync(string url)
        {
            using (var webClient = new WebClient())
            {
                Task<string> task = webClient.DownloadStringTaskAsync(url);
                string content = await task;
                return content;
            }
        }
    }
}
