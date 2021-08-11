using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqObj
{
    /// <summary>
    /// 跌代器
    /// </summary>
    public class LinqIterator : IEnumerator<int>
    {
        /// <summary>
        /// 當前質數
        /// </summary>
        private int CurrentNum;

        /// <summary>
        /// 1~100最大質數
        /// </summary>
        public int MaxNum { get; set; }

        /// <summary>
        /// 建構子，初始化物件
        /// </summary>
        public LinqIterator()
        {
            CurrentNum = 1;
            GetMaxNum();
        }

        /// <summary>
        /// 取得當前數值
        /// </summary>
        public object Current => CurrentNum;

        /// <summary>
        /// 取得當前數值
        /// </summary>
        int IEnumerator<int>.Current => CurrentNum;

        /// <summary>
        /// 實作IEnumerator的方法之一，釋放資源
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// 確認是否有下一個數值，如果有則放入當前數值
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (CurrentNum >= MaxNum)
            {
                Reset();
                return false;
            }
            for (int i = CurrentNum + 1; i <= MaxNum; i++)
            {
                if (IsPrimeNum(i))
                {
                    CurrentNum = i;
                    break;
                }
            }
            return true;
        }

        /// <summary>
        /// 取得1~100最大的質數
        /// </summary>
        private void GetMaxNum()
        {
            for (int i = 100; i > 0; i--)
            {
                if (IsPrimeNum(i))
                {
                    MaxNum = i;
                    break;
                }
            }
        }

        /// <summary>
        /// 確認是否為質數
        /// </summary>
        /// <param name="num">要確認的數值</param>
        /// <returns>是否為質數</returns>
        private bool IsPrimeNum(int num)
        {
            bool result = true;
            if (num == 1)
            {
                return false;
            }
            for (int i = 2; i < num; i++)
            {
                if (num % i == 0)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 回到未使用的時候
        /// </summary>
        public void Reset()
        {
            CurrentNum = 0;
        }
    }
}
