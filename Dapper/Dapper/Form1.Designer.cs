namespace Dapper
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.sqlConnection1 = new System.Data.SqlClient.SqlConnection();
            this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
            this.sqlInsertCommand1 = new System.Data.SqlClient.SqlCommand();
            this.sqlUpdateCommand1 = new System.Data.SqlClient.SqlCommand();
            this.sqlDeleteCommand1 = new System.Data.SqlClient.SqlCommand();
            this.sqlDataAdapter1 = new System.Data.SqlClient.SqlDataAdapter();
            this.dataSet11 = new Dapper.DataSet1();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.非營業日DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.公司代號DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.基金統編DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.基金名稱DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.排序DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cTIMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mTIMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(29, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(551, 25);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(196, 98);
            this.textBox1.TabIndex = 1;
            // 
            // sqlConnection1
            // 
            this.sqlConnection1.ConnectionString = "Data Source=192.168.10.180;Initial Catalog=StockDB;User ID=test;Password=test";
            this.sqlConnection1.FireInfoMessageEventOnUserErrors = false;
            // 
            // sqlSelectCommand1
            // 
            this.sqlSelectCommand1.CommandText = "SELECT [非營業日]\r\n      ,[公司代號]\r\n      ,[基金統編]\r\n      ,[基金名稱]\r\n      ,[排序]\r\n      ,[" +
    "CTIME]\r\n      ,[MTIME]\r\n  FROM [dbo].[基金非營業日明細_luann]";
            this.sqlSelectCommand1.Connection = this.sqlConnection1;
            // 
            // sqlInsertCommand1
            // 
            this.sqlInsertCommand1.CommandText = resources.GetString("sqlInsertCommand1.CommandText");
            this.sqlInsertCommand1.Connection = this.sqlConnection1;
            this.sqlInsertCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@非營業日", System.Data.SqlDbType.Char, 0, "非營業日"),
            new System.Data.SqlClient.SqlParameter("@公司代號", System.Data.SqlDbType.Char, 0, "公司代號"),
            new System.Data.SqlClient.SqlParameter("@基金統編", System.Data.SqlDbType.VarChar, 0, "基金統編"),
            new System.Data.SqlClient.SqlParameter("@基金名稱", System.Data.SqlDbType.NVarChar, 0, "基金名稱"),
            new System.Data.SqlClient.SqlParameter("@排序", System.Data.SqlDbType.TinyInt, 0, "排序"),
            new System.Data.SqlClient.SqlParameter("@CTIME", System.Data.SqlDbType.DateTime, 0, "CTIME"),
            new System.Data.SqlClient.SqlParameter("@MTIME", System.Data.SqlDbType.BigInt, 0, "MTIME")});
            // 
            // sqlUpdateCommand1
            // 
            this.sqlUpdateCommand1.CommandText = resources.GetString("sqlUpdateCommand1.CommandText");
            this.sqlUpdateCommand1.Connection = this.sqlConnection1;
            this.sqlUpdateCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@非營業日", System.Data.SqlDbType.Char, 0, "非營業日"),
            new System.Data.SqlClient.SqlParameter("@公司代號", System.Data.SqlDbType.Char, 0, "公司代號"),
            new System.Data.SqlClient.SqlParameter("@基金統編", System.Data.SqlDbType.VarChar, 0, "基金統編"),
            new System.Data.SqlClient.SqlParameter("@基金名稱", System.Data.SqlDbType.NVarChar, 0, "基金名稱"),
            new System.Data.SqlClient.SqlParameter("@排序", System.Data.SqlDbType.TinyInt, 0, "排序"),
            new System.Data.SqlClient.SqlParameter("@CTIME", System.Data.SqlDbType.DateTime, 0, "CTIME"),
            new System.Data.SqlClient.SqlParameter("@MTIME", System.Data.SqlDbType.BigInt, 0, "MTIME"),
            new System.Data.SqlClient.SqlParameter("@Original_非營業日", System.Data.SqlDbType.Char, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "非營業日", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_公司代號", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "公司代號", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_公司代號", System.Data.SqlDbType.Char, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "公司代號", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_基金統編", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "基金統編", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_基金名稱", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "基金名稱", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_基金名稱", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "基金名稱", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_排序", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "排序", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_排序", System.Data.SqlDbType.TinyInt, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "排序", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_CTIME", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "CTIME", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_CTIME", System.Data.SqlDbType.DateTime, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CTIME", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_MTIME", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "MTIME", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_MTIME", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "MTIME", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlDeleteCommand1
            // 
            this.sqlDeleteCommand1.CommandText = resources.GetString("sqlDeleteCommand1.CommandText");
            this.sqlDeleteCommand1.Connection = this.sqlConnection1;
            this.sqlDeleteCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Original_非營業日", System.Data.SqlDbType.Char, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "非營業日", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_公司代號", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "公司代號", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_公司代號", System.Data.SqlDbType.Char, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "公司代號", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_基金統編", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "基金統編", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_基金名稱", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "基金名稱", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_基金名稱", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "基金名稱", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_排序", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "排序", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_排序", System.Data.SqlDbType.TinyInt, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "排序", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_CTIME", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "CTIME", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_CTIME", System.Data.SqlDbType.DateTime, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CTIME", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_MTIME", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "MTIME", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_MTIME", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "MTIME", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlDataAdapter1
            // 
            this.sqlDataAdapter1.DeleteCommand = this.sqlDeleteCommand1;
            this.sqlDataAdapter1.InsertCommand = this.sqlInsertCommand1;
            this.sqlDataAdapter1.SelectCommand = this.sqlSelectCommand1;
            this.sqlDataAdapter1.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "基金非營業日明細_luann", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("非營業日", "非營業日"),
                        new System.Data.Common.DataColumnMapping("公司代號", "公司代號"),
                        new System.Data.Common.DataColumnMapping("基金統編", "基金統編"),
                        new System.Data.Common.DataColumnMapping("基金名稱", "基金名稱"),
                        new System.Data.Common.DataColumnMapping("排序", "排序"),
                        new System.Data.Common.DataColumnMapping("CTIME", "CTIME"),
                        new System.Data.Common.DataColumnMapping("MTIME", "MTIME")})});
            this.sqlDataAdapter1.UpdateCommand = this.sqlUpdateCommand1;
            // 
            // dataSet11
            // 
            this.dataSet11.DataSetName = "DataSet1";
            this.dataSet11.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "基金非營業日明細_luann";
            this.bindingSource1.DataSource = this.dataSet11;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.非營業日DataGridViewTextBoxColumn,
            this.公司代號DataGridViewTextBoxColumn,
            this.基金統編DataGridViewTextBoxColumn,
            this.基金名稱DataGridViewTextBoxColumn,
            this.排序DataGridViewTextBoxColumn,
            this.cTIMEDataGridViewTextBoxColumn,
            this.mTIMEDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.bindingSource1;
            this.dataGridView1.Location = new System.Drawing.Point(234, 223);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(240, 150);
            this.dataGridView1.TabIndex = 2;
            // 
            // 非營業日DataGridViewTextBoxColumn
            // 
            this.非營業日DataGridViewTextBoxColumn.DataPropertyName = "非營業日";
            this.非營業日DataGridViewTextBoxColumn.HeaderText = "非營業日";
            this.非營業日DataGridViewTextBoxColumn.Name = "非營業日DataGridViewTextBoxColumn";
            // 
            // 公司代號DataGridViewTextBoxColumn
            // 
            this.公司代號DataGridViewTextBoxColumn.DataPropertyName = "公司代號";
            this.公司代號DataGridViewTextBoxColumn.HeaderText = "公司代號";
            this.公司代號DataGridViewTextBoxColumn.Name = "公司代號DataGridViewTextBoxColumn";
            // 
            // 基金統編DataGridViewTextBoxColumn
            // 
            this.基金統編DataGridViewTextBoxColumn.DataPropertyName = "基金統編";
            this.基金統編DataGridViewTextBoxColumn.HeaderText = "基金統編";
            this.基金統編DataGridViewTextBoxColumn.Name = "基金統編DataGridViewTextBoxColumn";
            // 
            // 基金名稱DataGridViewTextBoxColumn
            // 
            this.基金名稱DataGridViewTextBoxColumn.DataPropertyName = "基金名稱";
            this.基金名稱DataGridViewTextBoxColumn.HeaderText = "基金名稱";
            this.基金名稱DataGridViewTextBoxColumn.Name = "基金名稱DataGridViewTextBoxColumn";
            // 
            // 排序DataGridViewTextBoxColumn
            // 
            this.排序DataGridViewTextBoxColumn.DataPropertyName = "排序";
            this.排序DataGridViewTextBoxColumn.HeaderText = "排序";
            this.排序DataGridViewTextBoxColumn.Name = "排序DataGridViewTextBoxColumn";
            // 
            // cTIMEDataGridViewTextBoxColumn
            // 
            this.cTIMEDataGridViewTextBoxColumn.DataPropertyName = "CTIME";
            this.cTIMEDataGridViewTextBoxColumn.HeaderText = "CTIME";
            this.cTIMEDataGridViewTextBoxColumn.Name = "cTIMEDataGridViewTextBoxColumn";
            // 
            // mTIMEDataGridViewTextBoxColumn
            // 
            this.mTIMEDataGridViewTextBoxColumn.DataPropertyName = "MTIME";
            this.mTIMEDataGridViewTextBoxColumn.HeaderText = "MTIME";
            this.mTIMEDataGridViewTextBoxColumn.Name = "mTIMEDataGridViewTextBoxColumn";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataSet11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Data.SqlClient.SqlConnection sqlConnection1;
        private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
        private System.Data.SqlClient.SqlCommand sqlInsertCommand1;
        private System.Data.SqlClient.SqlCommand sqlUpdateCommand1;
        private System.Data.SqlClient.SqlCommand sqlDeleteCommand1;
        private System.Data.SqlClient.SqlDataAdapter sqlDataAdapter1;
        private DataSet1 dataSet11;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 非營業日DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 公司代號DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 基金統編DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 基金名稱DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 排序DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTIMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mTIMEDataGridViewTextBoxColumn;
    }
}

