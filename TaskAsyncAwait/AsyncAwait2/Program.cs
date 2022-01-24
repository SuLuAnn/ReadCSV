using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait2
{
    class Program
    {
        static Stopwatch stopWatch = new Stopwatch();
        static void Main(string[] args)
        {
            AsyncTest test = new AsyncTest();
            test.DoJob();
            Console.ReadKey();
        }
    }
}
