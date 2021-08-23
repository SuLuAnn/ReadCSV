using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularExpressions
{
    /// <summary>
    /// 存靜態常數,enum之類
    /// </summary>
    public class Global
    {
        /// <summary>
        /// 身份證確認轉數值時要乘的值
        /// </summary>
        public static readonly int[] Multiplier = { 1, 9, 8, 7, 6, 5, 4, 3, 2, 1, 1 };

        /// <summary>
        /// 身份證字號首字母代表的數字
        /// </summary>
        public enum ID : int
        {
            A = 10,
            B = 11,
            C = 12,
            D = 13,
            E = 14,
            F = 15,
            G = 16,
            H = 17,
            I = 34,
            J = 18,
            K = 19,
            L = 20,
            M = 21,
            N = 22,
            O = 35,
            P = 23,
            Q = 24,
            R = 25,
            S = 26,
            T = 27,
            U = 28,
            V = 29,
            W = 32,
            X = 30,
            Y = 31,
            Z = 33
        }
    }
}
