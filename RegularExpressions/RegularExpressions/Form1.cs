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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ClickIDButton(object sender, EventArgs e)
        {
            string id = IDNumberBox.Text;
            IDNumberDisplay.Text = Regex.IsMatch(id, @"^[A-Z]{1}[1-2]{1}[0-9]{8}$").ToString();

        }

        private void ClickPassword(object sender, EventArgs e)
        {
            string password = PasswordBox.Text;
            PasswordDisplay.Text = Regex.IsMatch(password, @"^\w{8,}$").ToString();
        }

        private void ClickEnglishNameButton(object sender, EventArgs e)
        {
            string englishName = EnglishNameBox.Text;
            EnglishNameDisplay.Text = Regex.Split(englishName, @"@cmoney").First();
        }

        private void ClickH4ReadButton(object sender, EventArgs e)
        {
            string path = $"{Environment.CurrentDirectory}/Q4.htm";
            string datas;
            using (StreamReader streamReader = new StreamReader(path, System.Text.Encoding.GetEncoding("Big5")))
            {
                datas = streamReader.ReadToEnd();
            }
            var names = Regex.Matches(datas, @"(?<=<h4>)(\S*?)(?=</h4>)");
            var numbers = Regex.Matches(datas, @"(?<=<p>)(\S*?)(?=</p>)");
            for (int i = 0; i < names.Count; i++)
            {
                string name = Regex.Replace(names[i].Value, @"(&nbsp;)", @" ");
                string number = Regex.Replace(numbers[i + 1].Value, @"&nbsp;", @" ");
                number = Regex.Replace(number, @",", string.Empty);
                number = Regex.Replace(number, @"\$", string.Empty);
                number = Regex.Replace(number, @"[A-z]", string.Empty);
                H4ReadDisplay.Text += $"{name}:{number}{Environment.NewLine}";
            }
        }

        private void ClickH5ReadButton(object sender, EventArgs e)
        {
            string path = $"{Environment.CurrentDirectory}/Q5.htm";
            DataTable table = new DataTable();
            string datas;
            using (StreamReader streamReader = new StreamReader(path, System.Text.Encoding.GetEncoding("Big5")))
            {
                datas = streamReader.ReadToEnd();
            }
            var data = Regex.Matches(datas, @"(?<=<tr class='tblHead'>)(.*?)(?=</tr>)");
            var downColumn = Regex.Matches(data[1].Value, @"(?<=<th>)(.*?)(?=</th>)");
            var columns = Regex.Matches(data[0].Value, @"(?<=<th)(.*?)(?=</th>)");
            int index = 0;
            for (int i = 0; i < columns.Count; i++)
            {
                string num = Regex.Match(columns[i].Value, @"(?<==')(.*?)(?=')").Value;
                if (num == "2")
                {
                    var column = Regex.Match(columns[i].Value, @"(?<='>)(.*?)$").Value;
                    table.Columns.Add(column);
                    continue;
                }
                int max =index + Convert.ToInt32(num);
                for (int k = index; k < max; k++)
                {
                    table.Columns.Add(downColumn[k].Value);
                }
                index = max;
            }
            var items = Regex.Matches(datas, @"(?<=<td)(.*?)(?=/td>)");
            int cell = 0 ;
            for (int i = 0; i < items.Count; i++)
            {
                var item = Regex.Match(items[i].Value, @"(?<=>)(.*?)(?=<)").Value.Trim();
                if (string.IsNullOrEmpty(item))
                {

                    cell++;
                }
            }
            DataView.DataSource = table;
        }
    }
}
