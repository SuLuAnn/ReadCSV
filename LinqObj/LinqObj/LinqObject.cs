using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqObj
{
    /// <summary>
    /// 取得跌代器的物件
    /// </summary>
    public class LinqObject : IEnumerable<int>
    {
        /// <summary>
        /// 取得跌代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return new LinqIterator();
        }

        /// <summary>
        /// 取得跌代器
        /// </summary>
        /// <returns></returns>
        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            return new LinqIterator();
        }

        /// <summary>
        /// 取得1~100最大質數
        /// </summary>
        /// <returns></returns>
        public int GetMax()
        {
            return new LinqIterator().MaxNum;
        }

        /// <summary>
        /// 實作where方法
        /// </summary>
        /// <param name="predicate">委派的方法</param>
        /// <returns>符合委派方法判斷的數值</returns>
        public IEnumerable<int> MyWhere(Func<int, bool> predicate)
        {
            foreach (int i in this)
            {
                if (predicate(i))
                {
                    yield return i;
                }
            }
        }
    }
}
