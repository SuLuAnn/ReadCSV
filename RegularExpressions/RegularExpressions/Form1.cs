using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegularExpressions
{
    /// <summary>
    /// 主要程式邏輯
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 按"身分證確認"比對出正確的身分證字號
        /// </summary>
        /// <param name="sender">觸發物件</param>
        /// <param name="e">觸發事件</param>
        private void ClickIDButton(object sender, EventArgs e)
        {
            string id = IDNumberBox.Text;
            //開頭一位A-Z，第二位1或2,在8位數字
            IDNumberDisplay.Text = Regex.IsMatch(id, @"^[A-Z]{1}[1-2]{1}[0-9]{8}$").ToString();

        }

        /// <summary>
        /// 按"密碼確認"，輸入密碼必須是8個字元以上，不得為+-*/.!@#$%^&*()等特殊字元
        /// </summary>
        /// <param name="sender">觸發物件</param>
        /// <param name="e">觸發事件</param>
        private void ClickPassword(object sender, EventArgs e)
        {
            string password = PasswordBox.Text;
            PasswordDisplay.Text = Regex.IsMatch(password, @"^\w{8,}$").ToString();
        }

        /// <summary>
        /// cmoney員工的email帳號，要取英文的姓名
        /// </summary>
        /// <param name="sender">觸發物件</param>
        /// <param name="e">觸發事件</param>
        private void ClickEnglishNameButton(object sender, EventArgs e)
        {
            string englishName = EnglishNameBox.Text;
            //@cmoney前面的英文名
            EnglishNameDisplay.Text = Regex.Match(englishName, @"(^\S*)(?=@cmoney)").Value;
        }

        /// <summary>
        /// Q4.htm裡的欄位與數值
        /// </summary>
        /// <param name="sender">觸發物件</param>
        /// <param name="e">觸發事件</param>
        private void ClickH4ReadButton(object sender, EventArgs e)
        {
            string path = $"{Environment.CurrentDirectory}/Q4.htm"; //檔案路徑
            string datas = ReadFile("Q4"); //讀出來的資料
            //欄位在<h4>標籤裡，數值在<p>標籤裡
            MatchCollection items = Regex.Matches(datas, @"(?<=<h4>)(?<name>\S*?)(?=<\/h4>)|(?<=<div class=\""fl_txt\""><p>)[NT$&nbsp;]*(?<number>\S*)(?=<\/p>)");
            for (int i = 0; i < items.Count; i += 2)
            {
                H4ReadDisplay.Text += $"{Regex.Replace($"{items[i].Groups["name"].Value}:{items[i + 1].Groups["number"].Value}", @"&nbsp;|,", string.Empty)}{Environment.NewLine}";
            }
        }

        /// <summary>
        /// Q5.htm裡的資料轉成DataTable
        /// </summary>
        /// <param name="sender">觸發物件</param>
        /// <param name="e">觸發事件</param>
        //private void ClickH5ReadButton(object sender, EventArgs e)
        //{
        //    string datas = ReadFile("Q5");
        //    DataTable table = new DataTable();
        //    // var data = Regex.Matches(datas, @"(?<=<th rowspan='2'>)(?<column>.*?)(?=<\/th>)|(?<=<th>)(?<downColumn>.*?)(?=<\/th>)");
        //    //先取出所有欄位資料
        //    MatchCollection data = Regex.Matches(datas, @"(?<=<tr class='tblHead'>)(.*?)(?=</tr>)");
        //    //index0的資料會是大欄位
        //    MatchCollection columns = Regex.Matches(data[0].Value, @"(?<=<th)(.*?)(?=</th>)");
        //    //index1的資料會是在大欄位下的小欄位
        //    MatchCollection downColumn = Regex.Matches(data[1].Value, @"(?<=<th>)(.*?)(?=</th>)");
        //    int index = 0;
        //    for (int i = 0; i < columns.Count; i++)
        //    {
        //        string num = Regex.Match(columns[i].Value, @"(?<==')(.*?)(?=')").Value;
        //        if (num == "2")
        //        {
        //            //2的欄位直接交入
        //            string column = Regex.Match(columns[i].Value, @"(?<='>)(.*?)$").Value;
        //            table.Columns.Add(column);
        //            continue;
        //        }
        //        //不是2的要取與num相當數量的小欄位加入
        //        int max =index + Convert.ToInt32(num);
        //        for (int k = index; k < max; k++)
        //        {
        //            table.Columns.Add(downColumn[k].Value);
        //        }
        //        //排除小欄位中已被加入的index，防止下次重複取
        //        index = max;
        //    }
        //    //取要入的資料
        //    MatchCollection items = Regex.Matches(datas, @"(?<=<td align='center'>|<td>|<td align='right'>)(.*?)(?=<\/td>)");
        //    //每17筆(欄位數)是1列資料，cell是紀錄取資料的index取到第幾筆
        //    int cell = items.Count /table.Columns.Count;
        //    DataRow row;
        //    for (int i = 0; i < cell; i++)
        //    {
        //        row = table.NewRow();
        //        for (int k = 0; k < table.Columns.Count; k++) 
        //        {
        //            //i * table.Columns.Count + k是目前要取的資料的index,資料要取代掉&nbsp;和空白
        //            row[k] = Regex.Replace( items[i * table.Columns.Count + k].Value, @"&nbsp;| ", string.Empty);
        //        }
        //        table.Rows.Add(row);
        //    }
        //    DataView.DataSource = table;
        //}

        /// <summary>
        /// 讀檔
        /// </summary>
        /// <param name="fileName">檔名</param>
        /// <returns>讀出來的資料</returns>
        private string ReadFile(string fileName)
        {
            string path = $"{Environment.CurrentDirectory}/{fileName}.htm";//檔案路徑
            string datas;//讀出來的資料
            //讀檔
            using (StreamReader streamReader = new StreamReader(path, System.Text.Encoding.GetEncoding("Big5")))
            {
                datas = streamReader.ReadToEnd();
            }
            return datas;
        }

        private void ClickH5ReadButton(object sender, EventArgs e)
        {
            string datas = ReadFile("Q5");
            DataTable table = new DataTable();
            //先取出所有欄位資料
            MatchCollection data = Regex.Matches(datas, @"(?<=<th rowspan='2'>)(?<column>.*?)(?=<\/th>)|(?<=<th colspan='(\d)'>)|(?<=<th>)(?<downColumn>.*?)(?=<\/th>)");
            
            for (int i = 0; i < data.Count; i++)
            {
                
                var a = data[i].Groups.Count;
                H4ReadDisplay.Text += a;
            }
            DataView.DataSource = table;
        }
    }
}
