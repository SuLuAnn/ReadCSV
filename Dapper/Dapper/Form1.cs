using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Common;

namespace Dapper
{
    public partial class Form1 : Form
    {

        private DbConnection _conn { get; set; }
        public Form1()
        {
            InitializeComponent();
            this._conn = SqlClientFactory.Instance.CreateConnection();
            //指派預設DB連線字串
            this._conn.ConnectionString = ConfigurationManager.ConnectionStrings["StockDB"].ConnectionString;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string querystr = "SELECT 證券代號 FROM 股東會投票日明細_luann";
            var result = _conn.Query(querystr).ToList();
            textBox1.Text = result.First();
        }
    }
}
