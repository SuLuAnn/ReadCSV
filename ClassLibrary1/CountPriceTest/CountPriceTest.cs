using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.UnitTests
{
    [TestClass()]
    public class CountPriceTest
    {
        [TestMethod()]
        //[DataRow(10,10,0)]
        //[DataRow(0, 0, 0)]
        //[DataRow(5, 10, -50)]
        //[DataRow(0, 10, -100)]
        //[DataRow(-5, 10, -150)]
        //[DataRow(-5, -1, 400)]
        //[DataRow(5, -1, -600)]
        [DataRow(0, 0, null)]
        public void CountPrice_WorstResult_Zero(int now, int last, int targetResult)
        {
            List<decimal> a;
            switch (0)
            {
                case 0:
                    a = new List<decimal>() { (decimal)1.1,2m };
                    break;
            }
            Class1 class1 = new Class1();
            decimal result = class1.countPrice( now, last);
            Assert.AreEqual(result, targetResult);
        }
        ReduceNumber
            Work
        /// <summary>
        /// 單元測試，測試
        /// </summary>
        [TestClass()]
        public class CountPriceTest
        {
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
            public void CountPrice_WorstResult_Zero(int numberA, int numberB, bool? targetResult)
            {
                Work work = new Work();
                bool? result = work.ReduceNumber(numberA, numberB);
                Assert.AreEqual(result, targetResult);
            }
        }
}