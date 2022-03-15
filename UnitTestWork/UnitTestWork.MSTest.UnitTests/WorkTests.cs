using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestWork.MSTest.UnitTests
{
    /// <summary>
    /// 單元測試，測試
    /// </summary>
    [TestClass()]
    public class WorkTests
    {
        /// <summary>
        /// ReduceNumber的測試方法，判斷數字A減數字B的結果是否大於0，大於0回傳TRUE，否則回傳FALSE，為0回傳NULL
        /// </summary>
        /// <param name="numberA">數字A</param>
        /// <param name="numberB">數字B</param>
        /// <param name="targetResult">是否大於0，為0回傳NULL</param>
        [TestMethod()]
        [DataRow(1, 1, null)]
        [DataRow(-1, -1, null)]
        [DataRow(-10, 2, false)]
        [DataRow(10, -200, true)]
        [DataRow(0, -200, true)]
        [DataRow(10, 0, true)]
        [DataRow(0, 0, null)]
        [DataRow(-50, 0, false)]
        [DataRow(0, 8, false)]
        public void ReduceNumber_RightResult_Boolean(int numberA, int numberB, bool? targetResult)
        {
            Work work = new Work();
            bool? result = work.ReduceNumber(numberA, numberB);
            Assert.AreEqual(result, targetResult);
        }
    }
}
