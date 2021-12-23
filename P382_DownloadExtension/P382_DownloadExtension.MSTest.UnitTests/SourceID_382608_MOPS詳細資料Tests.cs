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
    /// SourceID_382608_MOPS詳細資料的單元測試
    /// </summary>
    [TestClass()]
    public class SourceID_382608_MOPS詳細資料Tests
    {
        /// <summary>
        /// 測試資料
        /// </summary>
        private const string TEST = @"
<html>
<head>
	<title>公開資訊觀測站</title>
	<link href=""css/css1.css"" rel=""stylesheet"" type=""text/css"" Media=""Screen""/> 
<!--	<script type=""text/javascript"" src=""js/mops1.js""></script> -->
</head>

<body>
<table class='noBorder' align='center'>
<tr><td class='reportName' colspan='3'><center><h2>特別股權利基本資料查詢</h2></center></td></tr></table>
<table class='noBorder'><tr><td class='compName' align='center'>
<b>
本資料由　<span style='color:blue;'>台灣人壽</span>
　公司提供</b>
</td></tr></table>
<table class='noBorder' align='center'>
<td class='reportCont'>市場別：全部公司
</td></tr></table>
<form name='t51sb04_form3' action='/mops/web/ajax_t47sb12' method='post' onsubmit='return false;'>
<input type='hidden' name='firstin' value='true' />
<input type='hidden' name='TYPEK' value='all'>
<input type='hidden' name='step' value='2'><input type='hidden' name='summaries' value='Y'>
<input type='hidden' name='co_id'><input type='hidden' name='seq_no'><table class='hasBorder' style='width:50%;'>
<tr class='tblHead'><th nowrap>股票代號</th><th nowrap>期別</th><th nowrap>特別股名稱</th><th nowrap>&nbsp;</th></tr>
<tr class='even'>
<td style='text-align:left !important;'>2833A</td>
<td style='text-align:left !important;'>1</td>
<td style='text-align:left !important;'>台壽甲</td><td align='center'><input type='button' value='詳細資料' onclick=""document.t51sb04_form3.seq_no.value='1';document.t51sb04_form3.co_id.value='2833A';openWindow(this.form ,'');""></td>
</tr>
<tr class='odd'>
<td style='text-align:left !important;'>2833A</td>
<td style='text-align:left !important;'>2</td>
<td style='text-align:left !important;'>台壽甲</td><td align='center'><input type='button' value='詳細資料' onclick=""document.t51sb04_form3.seq_no.value='2';document.t51sb04_form3.co_id.value='2833A';openWindow(this.form ,'');""></td>
</tr>
</table></form>

</body>
</html>
";

        /// <summary>
        /// 測試結果的代號
        /// </summary>
        public enum TestResultModol : int
        {
            CONTAIN_WORST_DATA = 1,
            MAX_PERIOD = 2
        }

        /// <summary>
        /// 測試傳入資料有問題的狀況
        /// </summary>
        /// <param name="webContent">傳入資料</param>
        [TestMethod()]
        [DataRow("")]
        [DataRow("asdfghjk")]
        [DataRow("seq_no.value='A'.*co_id.value='B'")]
        public void GetStockIDs_WorstWebContent_ReturnEmpty(string webContent)
        {
            SourceID_382608_MOPS詳細資料 source = new SourceID_382608_MOPS詳細資料();
            CollectionAssert.AreEqual(source.GetStockIDs(webContent), new Dictionary<string, string>());
        }

        /// <summary>
        /// 測試傳入正常資料的狀況
        /// </summary>
        /// <param name="webContent">傳入資料</param>
        /// <param name="modol">測試應相等的結果</param>
        [TestMethod()]
        [DataRow("seq_no.value='A'.*co_id.value='B'seq_no.value='10'.*co_id.value='B'", TestResultModol.CONTAIN_WORST_DATA)]
        [DataRow(TEST, TestResultModol.MAX_PERIOD)]
        public void GetStockIDs_CorrectWebContent_ReturnCorrect(string webContent, TestResultModol modol)
        {
            SourceID_382608_MOPS詳細資料 source = new SourceID_382608_MOPS詳細資料();
            CollectionAssert.AreEqual(source.GetStockIDs(webContent), FakeDictionary(modol));
        }

        /// <summary>
        /// 取得測試比較的結果
        /// </summary>
        /// <param name="modol">結果代號</param>
        /// <returns>測試比較的結果</returns>
        private Dictionary<string, string> FakeDictionary(TestResultModol modol)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            switch (modol)
            {
                case TestResultModol.CONTAIN_WORST_DATA:
                    result = new Dictionary<string, string>()
                    {
                        { "B", "10" }
                    };
                    break;
                case TestResultModol.MAX_PERIOD:
                    result = new Dictionary<string, string>()
                    {
                        { "2833A", "2" }
                    };
                    break;
            }

            return result;
        }
    }
}