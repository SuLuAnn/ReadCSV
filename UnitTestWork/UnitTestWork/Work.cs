using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestWork
{
    /// <summary>
    /// 單元測試作業
    /// </summary>
    public class Work
    {
        /// <summary>
        /// 判斷數字A減數字B的結果是否大於0，大於0回傳TRUE，否則回傳FALSE，為0回傳NULL
        /// </summary>
        /// <param name="numberA">數字A</param>
        /// <param name="numberB">數字B</param>
        /// <returns>是否大於0，為0回傳NULL</returns>
        public bool? ReduceNumber(decimal numberA, decimal numberB)
        {
            decimal result = numberA - numberB;
            if (result == 0)
            {
                return null;
            }
            return result > 0;
        }
    }
}
