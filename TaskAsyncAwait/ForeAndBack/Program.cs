using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ForeAndBack
{
    class Program
    {
        static void Main(string[] args)
        {
            //Thread thread = new Thread(Loop);
            //thread.IsBackground = true;
            //thread.Start();
            ThreadPool.GetMaxThreads(out int a,out int b);
        }
        static void Loop()
        {
            Thread.Sleep(10000);
        }
    }
}
