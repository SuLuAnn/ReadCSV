﻿using System;
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
            var idNumber = Regex.Match(id, @"^(?<id>[A-Z]){1}(?<number>[1-2]{1}[0-9]{8})$");
            if (idNumber.Success)
            {
                string value = $"{TransformToNumber(idNumber.Groups["id"].Value)}{idNumber.Groups["number"]}";
                char[] numbers = value.ToCharArray();
                IDNumberDisplay.Text = IsIDNumber(numbers).ToString();
            }
            else
            {
                IDNumberDisplay.Text = idNumber.Success.ToString();
            }
        }

        /// <summary>
        /// 將身分證首字母轉換為對應的數字字串
        /// </summary>
        /// <param name="id">身分證首字母</param>
        /// <returns>對應的數字字串</returns>
        private string TransformToNumber(string id)
        {
            return ((int)Enum.Parse(typeof(Global.ID), id)).ToString();
        }

        /// <summary>
        /// 確認身分證是否合法，將傳入的字元轉為數字依序乘上特定數字%10為0的即為合法身分證
        /// </summary>
        /// <param name="numbers">身分證數值組成的字元</param>
        /// <returns>是否是合法身分證字號</returns>
        private bool IsIDNumber(char[] numbers)
        {
            //被%的值
            double mod = 10;
            double total = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                //每個char轉數值乘上特定數值相加
                total += char.GetNumericValue(numbers[i]) * Global.Multiplier[i];
            }
            return total % mod == 0;
        }

        /// <summary>
        /// 按"密碼確認"，輸入密碼必須是8個字元以上，不得為特殊字元
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
        /// 讀檔
        /// </summary>
        /// <param name="fileName">檔名</param>
        /// <returns>讀出來的資料</returns>
        private string ReadFile(string fileName)
        {
            //檔案路徑
            string path = $"{Environment.CurrentDirectory}/{fileName}.htm";
            //讀出來的資料
            string datas;
            //讀檔
            using (StreamReader streamReader = new StreamReader(path, System.Text.Encoding.GetEncoding("Big5")))
            {
                datas = streamReader.ReadToEnd();
            }
            return datas;
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
            MatchCollection items = Regex.Matches(datas, @"(?<=<h4>)(?<date>[^&nbsp;]*)?[&nbsp;]*(?<name>\S*?)<\/h4>\s*?<div class=\""fl_txt\""><p>[NT$&nbsp;]*(?<num1>[-]?\d+?)[,]?(?<num2>[.]?\d*?)[,]?(?<num3>\d*?)(?=<\/p>)");
            foreach (Match item in items)
            {
                H4ReadDisplay.Text += $"{item.Groups["date"].Value}{item.Groups["name"].Value}:{item.Groups["num1"].Value}{item.Groups["num2"].Value}{item.Groups["num3"].Value}{Environment.NewLine}";
            }
        }

        /// <summary>
        /// Q5.htm裡的資料轉成DataTable
        /// </summary>
        /// <param name="sender">觸發物件</param>
        /// <param name="e">觸發事件</param>
        private void ClickH5ReadButton(object sender, EventArgs e)
        {
            string datas = ReadFile("Q5");
            DataTable table = new DataTable();
            AddColumn(datas, table);
            //取要入的資料
            MatchCollection items = Regex.Matches(datas, @"(?<=<td align='center'>)(?<year>\d{3}|\d{3}\/\d{2}\/\d{2})(?=<\/td>)|(?<=<td align='center'>|<td>|<td align='right'>)[&nbsp;]*(?<data1>\S*?)\s*?(?<data2>\S*?)(?=<\/td>)");
            //每17筆(欄位數)是1列資料，cell是紀錄取資料的index取到第幾筆
            int cell = items.Count / table.Columns.Count;
            DataRow row;
            for (int i = 0; i < cell; i++)
            {
                row = table.NewRow();
                for (int k = 0; k < table.Columns.Count; k++)
                {
                    int index = (i * table.Columns.Count) + k;
                    //i * table.Columns.Count + k是目前要取的資料的index,資料要取代掉&nbsp;和空白
                    if (items[index].Groups["year"].Success)
                    {
                        //將民國轉西元
                        row[k] = $"{(int.Parse(items[index].Value.Substring(0, 3)) + 1911)}{items[index].Value.Substring(3)}";
                    }
                    else
                    {
                        row[k] = $"{ items[index].Groups["data1"].Value}{ items[index].Groups["data2"].Value}";
                    }
                }
                table.Rows.Add(row);
            }
            DataView.DataSource = table;
        }

        /// <summary>
        /// Q5加欄位
        /// </summary>
        /// <param name="datas">Q5資料</param>
        /// <param name="table">要被家欄位的table</param>
        private void AddColumn(string datas, DataTable table)
        {
            //先取出所有欄位資料
            MatchCollection data = Regex.Matches(datas, @"(?<=<th rowspan='2'>)(?<column>.*?)(?=<\/th>)|(?<=<th>)(?<downColumn>.*?)(?=<\/th>)");
            //要放入欄位的資料順序:0~3,10~16,4~9
            int[] indexs = Enumerable.Range(0, 4).Concat(Enumerable.Range(10, 7)).Concat(Enumerable.Range(4, 6)).ToArray();
            foreach (var index in indexs)
            {
                table.Columns.Add(data[index].Value);
            }
        }

        //測試
        //private void ClickIDButton(object sender, EventArgs e)
        //{
        //    string id = IDNumberBox.Text;
        //    //開頭一位A-Z，第二位1或2,在8位數字
        //    string test1 = Regex.IsMatch(id, @"[A-z]").ToString();
        //    //相反順序的 [x-y] 範圍。'
        //    //string test2 = Regex.IsMatch(id, @"[a-Z]").ToString(); 
        //    //string test5 = Regex.IsMatch(id, @"[a-1]").ToString();
        //    //string test6 = Regex.IsMatch(id, @"[A-1]").ToString();
        //    string test3 = Regex.IsMatch(id, @"[1-Z]").ToString();
        //    string test4 = Regex.IsMatch(id, @"[1-z]").ToString();
        //    IDNumberDisplay.Text = $"[A - z]:{test1}, [1-Z]:{test3}, [1-z]:{test4}";
        //}
    }
}
