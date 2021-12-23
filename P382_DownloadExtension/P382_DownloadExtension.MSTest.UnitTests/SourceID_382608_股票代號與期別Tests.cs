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
    /// SourceID_382608_股票代號與期別的單元測試
    /// </summary>
    [TestClass()]
    public class SourceID_382608_股票代號與期別Tests
    {
        /// <summary>
        /// 測試資料
        /// </summary>
        private const string TEST = @"
<html>
<head></head>
<link rel=""stylesheet"" href=""http://isin.twse.com.tw/isin/style1.css"" type=""text/css"">
<body background=""../image/back1.jpg"" bgproperties=fixed>

    <table  align=center>
  	<h2><strong><font class='h1'>證券編碼＿分類查詢結果</font></strong></h2>
    </table>
    <table class='h4' border=0 align=center bordercolor=#6495ed>
    <tr align=center>
      <td bgcolor=#D5FFD5>頁面編號</td>
      <td bgcolor=#D5FFD5>國際證券編碼</td>
      <td bgcolor=#D5FFD5>有價證券代號</td>
      <td bgcolor=#D5FFD5>有價證券名稱</td>
  
      <td bgcolor=#D5FFD5>市場別</td>

      <td bgcolor=#D5FFD5>有價證券別</td>
      <td bgcolor=#D5FFD5>產業別</td>
      <td bgcolor=#D5FFD5>公開發行/上市(櫃)/發行日</td>
      <td bgcolor=#D5FFD5>CFICode</td>
      <td bgcolor=#D5FFD5>備註</td>
    </tr>
      <tr>
        <td align=center bgcolor=#FAFAD2> 8 </td>
        <td bgcolor=#FAFAD2>TW0002881A00</td>
        <td align=center bgcolor=#FAFAD2>2881A</td>
        <td bgcolor=#FAFAD2>富邦特</td>

        <td bgcolor=#FAFAD2>上市 </td>
        <td bgcolor=#FAFAD2>特別股</td>
        <td bgcolor=#FAFAD2>金融保險業</td>

        <td bgcolor=#FAFAD2 align=center>2016/05/31</td>
        <td bgcolor=#FAFAD2>EPNRAR</td>
        
        <td bgcolor=#FAFAD2></td>
      </tr>

      <tr>
        <td align=center bgcolor=#FAFAD2> 9 </td>
        <td bgcolor=#FAFAD2>TW0002881B09</td>
        <td align=center bgcolor=#FAFAD2>2881B</td>
        <td bgcolor=#FAFAD2>富邦金乙特</td>

        <td bgcolor=#FAFAD2>上市 </td>
        <td bgcolor=#FAFAD2>特別股</td>
        <td bgcolor=#FAFAD2>金融保險業</td>

        <td bgcolor=#FAFAD2 align=center>2018/04/23</td>
        <td bgcolor=#FAFAD2>EPNRAR</td>
        
        <td bgcolor=#FAFAD2></td>
      </tr>

      <tr>
        <td align=center bgcolor=#FAFAD2> 10 </td>
        <td bgcolor=#FAFAD2>TW0002881C08</td>
        <td align=center bgcolor=#FAFAD2>2881C</td>
        <td bgcolor=#FAFAD2>富邦金丙特</td>

        <td bgcolor=#FAFAD2>上市 </td>
        <td bgcolor=#FAFAD2>特別股</td>
        <td bgcolor=#FAFAD2>金融保險業</td>

        <td bgcolor=#FAFAD2 align=center>2021/10/29</td>
        <td bgcolor=#FAFAD2>EPNRAR</td>
        
        <td bgcolor=#FAFAD2></td>
      </tr>

      <tr>
        <td align=center bgcolor=#FAFAD2> 11 </td>
        <td bgcolor=#FAFAD2>TW0002882A09</td>
        <td align=center bgcolor=#FAFAD2>2882A</td>
        <td bgcolor=#FAFAD2>國泰特</td>

        <td bgcolor=#FAFAD2>上市 </td>
        <td bgcolor=#FAFAD2>特別股</td>
        <td bgcolor=#FAFAD2>金融保險業</td>

        <td bgcolor=#FAFAD2 align=center>2017/01/17</td>
        <td bgcolor=#FAFAD2>EPNRAR</td>
        
        <td bgcolor=#FAFAD2></td>
      </tr>

      <tr>
        <td align=center bgcolor=#FAFAD2> 12 </td>
        <td bgcolor=#FAFAD2>TW0002882B08</td>
        <td align=center bgcolor=#FAFAD2>2882B</td>
        <td bgcolor=#FAFAD2>國泰金乙特</td>

        <td bgcolor=#FAFAD2>上市 </td>
        <td bgcolor=#FAFAD2>特別股</td>
        <td bgcolor=#FAFAD2>金融保險業</td>

        <td bgcolor=#FAFAD2 align=center>2018/08/08</td>
        <td bgcolor=#FAFAD2>EPNRAR</td>
        
        <td bgcolor=#FAFAD2></td>
      </tr>

      <tr>
        <td align=center bgcolor=#FAFAD2> 13 </td>
        <td bgcolor=#FAFAD2>TW0002887E00</td>
        <td align=center bgcolor=#FAFAD2>2887E</td>
        <td bgcolor=#FAFAD2>台新戊特</td>

        <td bgcolor=#FAFAD2>上市 </td>
        <td bgcolor=#FAFAD2>特別股</td>
        <td bgcolor=#FAFAD2>金融保險業</td>

        <td bgcolor=#FAFAD2 align=center>2017/02/10</td>
        <td bgcolor=#FAFAD2>EPNRAR</td>
        
        <td bgcolor=#FAFAD2></td>
      </tr>

      <tr>
        <td align=center bgcolor=#FAFAD2> 14 </td>
        <td bgcolor=#FAFAD2>TW0002887F09</td>
        <td align=center bgcolor=#FAFAD2>2887F</td>
        <td bgcolor=#FAFAD2>台新戊特二</td>

        <td bgcolor=#FAFAD2>上市 </td>
        <td bgcolor=#FAFAD2>特別股</td>
        <td bgcolor=#FAFAD2>金融保險業</td>

        <td bgcolor=#FAFAD2 align=center>2019/01/08</td>
        <td bgcolor=#FAFAD2>EPNRAR</td>
        
        <td bgcolor=#FAFAD2></td>
      </tr>

      <tr>
        <td align=center bgcolor=#FAFAD2> 15 </td>
        <td bgcolor=#FAFAD2>TW0002888A03</td>
        <td align=center bgcolor=#FAFAD2>2888A</td>
        <td bgcolor=#FAFAD2>新光金甲特</td>

        <td bgcolor=#FAFAD2>上市 </td>
        <td bgcolor=#FAFAD2>特別股</td>
        <td bgcolor=#FAFAD2>金融保險業</td>

        <td bgcolor=#FAFAD2 align=center>2019/11/08</td>
        <td bgcolor=#FAFAD2>EPNRAR</td>
        
        <td bgcolor=#FAFAD2></td>
      </tr>

      <tr>
        <td align=center bgcolor=#FAFAD2> 16 </td>
        <td bgcolor=#FAFAD2>TW0002888B02</td>
        <td align=center bgcolor=#FAFAD2>2888B</td>
        <td bgcolor=#FAFAD2>新光金乙特</td>

        <td bgcolor=#FAFAD2>上市 </td>
        <td bgcolor=#FAFAD2>特別股</td>
        <td bgcolor=#FAFAD2>金融保險業</td>

        <td bgcolor=#FAFAD2 align=center>2020/09/04</td>
        <td bgcolor=#FAFAD2>EPNRAR</td>
        
        <td bgcolor=#FAFAD2></td>
      </tr>
</table>
<br>
<br>
<DIV ALIGN=center>
  <a href=""./class_i.jsp?kind=1"">回查詢頁</a>
</DIV>
</body>
</html>";

        /// <summary>
        /// 傳入正常資料測試GetSecuritiesCode是否正常
        /// </summary>
        /// <param name="webContent">傳入資料</param>
        /// <param name="result">預期結果</param>
        [TestMethod()]
        [DataRow(TEST, new int[] { 2881, 2882, 2887, 2888 })]
        public void GetSecuritiesCode_CorrectWebContent_ReturnCorrect(string webContent, int[] result)
        {
            SourceID_382608_股票代號與期別 source = new SourceID_382608_股票代號與期別();
            CollectionAssert.AreEqual(source.GetSecuritiesCode(webContent), result.Select(id => id.ToString()).ToList());
        }

        /// <summary>
        /// 傳入不正常資料測試GetSecuritiesCode是否正常
        /// </summary>
        /// <param name="webContent">傳入資料</param>
        [TestMethod()]
        [DataRow("aaa")]
        public void GetSecuritiesCode_WorstWebContent_ReturnEmpty(string webContent)
        {
            SourceID_382608_股票代號與期別 source = new SourceID_382608_股票代號與期別();
            CollectionAssert.AreEqual(source.GetSecuritiesCode(webContent), new List<string>());
        }
    }
}