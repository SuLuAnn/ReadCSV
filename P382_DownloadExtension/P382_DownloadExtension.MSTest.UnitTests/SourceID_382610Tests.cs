using Microsoft.VisualStudio.TestTools.UnitTesting;
using P382_DownloadExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P382_DownloadExtension.MSTest.UnitTests
{
    /// <summary>
    /// SourceID_382610的單元測試
    /// </summary>
    [TestClass()]
    public class SourceID_382610Tests
    {
        /// <summary>
        /// 測試GetCycle的傳入值是否符合預期結果
        /// </summary>
        /// <param name="webContent">傳入值</param>
        /// <param name="result">預期結果</param>
        [TestMethod()]
        [DataRow(@"""TranDate"":""\/Date(1640019600000)", "20211221")]
        [DataRow(@"""TranDate"":""\/Date(1640041200000)", "20211221")]
        [DataRow(@"""TranDate"":""\/Date(1640098800000)", "20211221")]
        [DataRow(@"""TranDate"":""\/Dat)", "")]
        [DataRow("", "")]
        public void GetCycle_CheckWebContent_ReturnNormal(string webContent, string result)
        {
            SourceID_382610 source = new SourceID_382610();
            Assert.AreEqual(source.GetCycle(webContent), result);
        }
    }
}