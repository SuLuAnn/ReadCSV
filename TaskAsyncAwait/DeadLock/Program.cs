using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadLock
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        public ActionResult Action()
        {
            var data = GetDataAsync().Result;
            return View(data);
        }

        private async Task<string> GetDataAsync()
        {
            var result = await MyWebService.GetDataAsync();
            return result.ToString();
        }
    }
}
