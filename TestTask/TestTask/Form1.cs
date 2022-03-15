using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestTask
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //string aaa = File.ReadAllText(@"\\Srv14\201原始資料存檔\中文新聞來源\美股\新浪\內文\2021\1.htm").Replace("\r\n", string.Empty);

            InitializeComponent();
            object a =new string[] {"aa", "bb" };
            string[] aa = (string[])a;
            string A = "京城銀行,EPS";
            Regex regex = new Regex(@"(?<StockId>[^,]+)");
            var matchCollection = regex.Matches(A);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task task = new Task(n => Sleep((int)n),100);//兩個參數好像會爆

            //啟動任務 
            //task.Start();
            //等待任務完成
            //task.Wait();

            //有回傳值得寫法
            Task<int> task1 = new Task<int>(n => Sleep1((int)n), 10);
            Task<int> task4 = new Task<int>(() => Sleep1(10));
            //取結果
            //int result = task1.Result;

            Task<int> task2 = Task.Factory.StartNew(n => Sleep1((int)n), 100);

            //Task<int> task3 = Task.Run(() => Sleep1(10));
            task1.Start();
            ////任務完成時執行處理
            task1.ContinueWith(t=> Sleep2(t.Result));

            //等待task1、task2執行完畢
            Task.WaitAll(task1, task2);

            Task<int[]> task5 = new Task<int[]>(state =>
            {
                textBox1.Text = state.ToString();
                int[] a = new int[2];
                new Task(() => a[0] = Sleep1(10),TaskCreationOptions.AttachedToParent).Start();
                return a;
            },"我是父代，子代結束後才執行完");
            task5.Start();

        }
        public void Sleep(int i)
        {
            Thread.Sleep(i);
        }
        public int Sleep1(int i)
        {
            Thread.Sleep(i);
            return i;
        }
        public void Sleep2(int i)
        {
            Thread.Sleep(i);
        }
    }
}
