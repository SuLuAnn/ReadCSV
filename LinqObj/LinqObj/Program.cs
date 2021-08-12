using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqObj
{
    /// <summary>
    /// 主程式
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// 程式進入點
        /// </summary>
        public static void Main()
        {
            LinqObject linqObject = new LinqObject();
            foreach (var obj in linqObject)
            {
                Console.WriteLine(obj);
            }
            Console.WriteLine("_______________________________________");
            Console.WriteLine(linqObject.GetMaxNum());
            Console.WriteLine("_______________________________________");
            var list = linqObject.MyWhere(i => i < 50);
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// LinqObject的擴充方法，可以做到1~100最大質數+1
        /// </summary>
        /// <param name="item">LinqObject物件</param>
        /// <returns></returns>
        public static int GetMaxNum(this LinqObject item)
        {
            return item.GetMax() + 1;
        }
    }
}
