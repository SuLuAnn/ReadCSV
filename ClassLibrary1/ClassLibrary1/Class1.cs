using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Class1
    {
        public decimal countPrice(decimal now, decimal last)
        {
            if (last == 0)
            {
                return 0;
            }
            return (now - last) / last * 100m;
        }
    }
}
