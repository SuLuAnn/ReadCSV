using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait2
{
    public class AsyncTest
    {
        private Stopwatch StopWatch;

        public AsyncTest()
        {
            StopWatch = new Stopwatch();
        }

        public async void DoJob()
        {
            StopWatch.Start();
            ConsoleMessage($"程式開始");
            await HttpGetStringLengthAsync();
            ConsoleMessage($"程式結束");
        }

       
        private async Task HttpGetStringLengthAsync()
        {
            StopWatch.Start();
            string a = $"即將進入GetStringAsync方法";
            ConsoleMessage(a);
            string getString = await GetStringAsync(); 
            ConsoleMessage($"物件中字元的數目:{getString.Length.ToString()}");
            ConsoleMessage($"GetStringAsync完成");
        }

        private void HttpGetStringLengthNormal()
        {
            StopWatch.Start();
            ConsoleMessage($"即將進入GetStringNormal方法");
            string getString = GetStringAsync().Result;
            ConsoleMessage($"物件中字元的數目:{getString.Length.ToString()}");
            ConsoleMessage($"GetStringNormal完成");
        }


        private Task HttpGetStringLengthTask()
        {
            StopWatch.Start();
            ConsoleMessage($"即將進入GetStringTask方法");
            return  Task<string>.Run(
                () =>
                {
                    string getString = GetStringAsync().Result;
                    ConsoleMessage($"物件中字元的數目:{getString.Length.ToString()}");                   
                }).ContinueWith(result => { ConsoleMessage($"GetStringTask完成"); });
        }

        private Task<string> GetStringAsync()
        {
            HttpClient httpclent = new HttpClient();
            ConsoleMessage($"進入網頁");           
            return httpclent.GetStringAsync("https://www.google.com.tw");
        }

        private void ConsoleMessage(string message)
        {
            Console.WriteLine($"thread:{Thread.CurrentThread.ManagedThreadId},{message},{StopWatch.Elapsed}");
        }

    }
}
