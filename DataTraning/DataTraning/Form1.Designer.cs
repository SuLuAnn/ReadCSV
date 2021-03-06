namespace DataTraning
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
            this.VoteDayText = new System.Windows.Forms.TextBox();
            this.VoteDayAddButton = new System.Windows.Forms.Button();
            this.VoteDayDeleteButton = new System.Windows.Forms.Button();
            this.VoteDataAddButton = new System.Windows.Forms.Button();
            this.VoteDataDeleteButton = new System.Windows.Forms.Button();
            this.FundDetailAddButton = new System.Windows.Forms.Button();
            this.FundDetailDeleteButton = new System.Windows.Forms.Button();
            this.FundStatisticAddButton = new System.Windows.Forms.Button();
            this.FundStatisticDeleteButton = new System.Windows.Forms.Button();
            this.FuturesPriceAddButton = new System.Windows.Forms.Button();
            this.FuturesPriceDeleteButton = new System.Windows.Forms.Button();
            this.FuturesStatisticAddButton = new System.Windows.Forms.Button();
            this.FuturesStatisticDeleteButton = new System.Windows.Forms.Button();
            this.VoteDataText = new System.Windows.Forms.TextBox();
            this.FundDetailText = new System.Windows.Forms.TextBox();
            this.FundStatisticText = new System.Windows.Forms.TextBox();
            this.FuturesPriceText = new System.Windows.Forms.TextBox();
            this.FuturesStatisticText = new System.Windows.Forms.TextBox();
            this.TimeText = new System.Windows.Forms.TextBox();
            this.SQLConnection = new System.Data.SqlClient.SqlConnection();
            this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
            this.sqlInsertCommand1 = new System.Data.SqlClient.SqlCommand();
            this.sqlUpdateCommand1 = new System.Data.SqlClient.SqlCommand();
            this.sqlDeleteCommand1 = new System.Data.SqlClient.SqlCommand();
            this.SQLDataAdapter = new System.Data.SqlClient.SqlDataAdapter();
            this.StockVoteSet = new DataTraning.StockVoteSet();
            this.BindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.StockVoteSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // VoteDayText
            // 
            this.VoteDayText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.VoteDayText.Location = new System.Drawing.Point(13, 13);
            this.VoteDayText.Name = "VoteDayText";
            this.VoteDayText.ReadOnly = true;
            this.VoteDayText.Size = new System.Drawing.Size(103, 15);
            this.VoteDayText.TabIndex = 0;
            this.VoteDayText.Text = "股東會投票日明細";
            // 
            // VoteDayAddButton
            // 
            this.VoteDayAddButton.Location = new System.Drawing.Point(12, 34);
            this.VoteDayAddButton.Name = "VoteDayAddButton";
            this.VoteDayAddButton.Size = new System.Drawing.Size(135, 23);
            this.VoteDayAddButton.TabIndex = 1;
            this.VoteDayAddButton.Text = "根據來源新增、修改";
            this.VoteDayAddButton.UseVisualStyleBackColor = true;
            this.VoteDayAddButton.Click += new System.EventHandler(this.ClickVoteDayAddButton);
            // 
            // VoteDayDeleteButton
            // 
            this.VoteDayDeleteButton.Location = new System.Drawing.Point(153, 34);
            this.VoteDayDeleteButton.Name = "VoteDayDeleteButton";
            this.VoteDayDeleteButton.Size = new System.Drawing.Size(140, 23);
            this.VoteDayDeleteButton.TabIndex = 2;
            this.VoteDayDeleteButton.Text = "刪除該表資料";
            this.VoteDayDeleteButton.UseVisualStyleBackColor = true;
            this.VoteDayDeleteButton.Click += new System.EventHandler(this.ClickVoteDayDeleteButton);
            // 
            // VoteDataAddButton
            // 
            this.VoteDataAddButton.Location = new System.Drawing.Point(314, 34);
            this.VoteDataAddButton.Name = "VoteDataAddButton";
            this.VoteDataAddButton.Size = new System.Drawing.Size(129, 23);
            this.VoteDataAddButton.TabIndex = 3;
            this.VoteDataAddButton.Text = "根據來源新增、修改";
            this.VoteDataAddButton.UseVisualStyleBackColor = true;
            this.VoteDataAddButton.Click += new System.EventHandler(this.ClickVoteDataAddButton);
            // 
            // VoteDataDeleteButton
            // 
            this.VoteDataDeleteButton.Location = new System.Drawing.Point(449, 34);
            this.VoteDataDeleteButton.Name = "VoteDataDeleteButton";
            this.VoteDataDeleteButton.Size = new System.Drawing.Size(125, 23);
            this.VoteDataDeleteButton.TabIndex = 4;
            this.VoteDataDeleteButton.Text = "刪除該表資料";
            this.VoteDataDeleteButton.UseVisualStyleBackColor = true;
            this.VoteDataDeleteButton.Click += new System.EventHandler(this.ClickVoteDataDeleteButton);
            // 
            // FundDetailAddButton
            // 
            this.FundDetailAddButton.Location = new System.Drawing.Point(12, 100);
            this.FundDetailAddButton.Name = "FundDetailAddButton";
            this.FundDetailAddButton.Size = new System.Drawing.Size(135, 23);
            this.FundDetailAddButton.TabIndex = 5;
            this.FundDetailAddButton.Text = "根據來源新增、修改";
            this.FundDetailAddButton.UseVisualStyleBackColor = true;
            this.FundDetailAddButton.Click += new System.EventHandler(this.ClickFundDetailAddButton);
            // 
            // FundDetailDeleteButton
            // 
            this.FundDetailDeleteButton.Location = new System.Drawing.Point(153, 100);
            this.FundDetailDeleteButton.Name = "FundDetailDeleteButton";
            this.FundDetailDeleteButton.Size = new System.Drawing.Size(140, 23);
            this.FundDetailDeleteButton.TabIndex = 6;
            this.FundDetailDeleteButton.Text = "刪除該表資料";
            this.FundDetailDeleteButton.UseVisualStyleBackColor = true;
            this.FundDetailDeleteButton.Click += new System.EventHandler(this.ClickFundDetailDeleteButton);
            // 
            // FundStatisticAddButton
            // 
            this.FundStatisticAddButton.Location = new System.Drawing.Point(314, 100);
            this.FundStatisticAddButton.Name = "FundStatisticAddButton";
            this.FundStatisticAddButton.Size = new System.Drawing.Size(129, 23);
            this.FundStatisticAddButton.TabIndex = 7;
            this.FundStatisticAddButton.Text = "根據來源新增、修改";
            this.FundStatisticAddButton.UseVisualStyleBackColor = true;
            this.FundStatisticAddButton.Click += new System.EventHandler(this.ClickFundStatisticAddButton);
            // 
            // FundStatisticDeleteButton
            // 
            this.FundStatisticDeleteButton.Location = new System.Drawing.Point(449, 100);
            this.FundStatisticDeleteButton.Name = "FundStatisticDeleteButton";
            this.FundStatisticDeleteButton.Size = new System.Drawing.Size(125, 23);
            this.FundStatisticDeleteButton.TabIndex = 8;
            this.FundStatisticDeleteButton.Text = "刪除該表資料";
            this.FundStatisticDeleteButton.UseVisualStyleBackColor = true;
            this.FundStatisticDeleteButton.Click += new System.EventHandler(this.ClickFundStatisticDeleteButton);
            // 
            // FuturesPriceAddButton
            // 
            this.FuturesPriceAddButton.Location = new System.Drawing.Point(13, 171);
            this.FuturesPriceAddButton.Name = "FuturesPriceAddButton";
            this.FuturesPriceAddButton.Size = new System.Drawing.Size(134, 23);
            this.FuturesPriceAddButton.TabIndex = 9;
            this.FuturesPriceAddButton.Text = "根據來源新增、修改";
            this.FuturesPriceAddButton.UseVisualStyleBackColor = true;
            this.FuturesPriceAddButton.Click += new System.EventHandler(this.ClickFuturesPriceAddButton);
            // 
            // FuturesPriceDeleteButton
            // 
            this.FuturesPriceDeleteButton.Location = new System.Drawing.Point(153, 171);
            this.FuturesPriceDeleteButton.Name = "FuturesPriceDeleteButton";
            this.FuturesPriceDeleteButton.Size = new System.Drawing.Size(140, 23);
            this.FuturesPriceDeleteButton.TabIndex = 10;
            this.FuturesPriceDeleteButton.Text = "刪除該表資料";
            this.FuturesPriceDeleteButton.UseVisualStyleBackColor = true;
            this.FuturesPriceDeleteButton.Click += new System.EventHandler(this.ClickFuturesPriceDeleteButton);
            // 
            // FuturesStatisticAddButton
            // 
            this.FuturesStatisticAddButton.Location = new System.Drawing.Point(314, 171);
            this.FuturesStatisticAddButton.Name = "FuturesStatisticAddButton";
            this.FuturesStatisticAddButton.Size = new System.Drawing.Size(129, 23);
            this.FuturesStatisticAddButton.TabIndex = 11;
            this.FuturesStatisticAddButton.Text = "根據來源新增、修改";
            this.FuturesStatisticAddButton.UseVisualStyleBackColor = true;
            this.FuturesStatisticAddButton.Click += new System.EventHandler(this.ClickFuturesStatisticAddButton);
            // 
            // FuturesStatisticDeleteButton
            // 
            this.FuturesStatisticDeleteButton.Location = new System.Drawing.Point(449, 171);
            this.FuturesStatisticDeleteButton.Name = "FuturesStatisticDeleteButton";
            this.FuturesStatisticDeleteButton.Size = new System.Drawing.Size(125, 23);
            this.FuturesStatisticDeleteButton.TabIndex = 12;
            this.FuturesStatisticDeleteButton.Text = "刪除該表資料";
            this.FuturesStatisticDeleteButton.UseVisualStyleBackColor = true;
            this.FuturesStatisticDeleteButton.Click += new System.EventHandler(this.ClickFuturesStatisticDeleteButton);
            // 
            // VoteDataText
            // 
            this.VoteDataText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.VoteDataText.Location = new System.Drawing.Point(314, 12);
            this.VoteDataText.Name = "VoteDataText";
            this.VoteDataText.ReadOnly = true;
            this.VoteDataText.Size = new System.Drawing.Size(100, 15);
            this.VoteDataText.TabIndex = 13;
            this.VoteDataText.Text = "股東會投票資料表";
            // 
            // FundDetailText
            // 
            this.FundDetailText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FundDetailText.Location = new System.Drawing.Point(13, 79);
            this.FundDetailText.Name = "FundDetailText";
            this.FundDetailText.ReadOnly = true;
            this.FundDetailText.Size = new System.Drawing.Size(100, 15);
            this.FundDetailText.TabIndex = 14;
            this.FundDetailText.Text = "基金非營業日明細";
            // 
            // FundStatisticText
            // 
            this.FundStatisticText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FundStatisticText.Location = new System.Drawing.Point(314, 79);
            this.FundStatisticText.Name = "FundStatisticText";
            this.FundStatisticText.ReadOnly = true;
            this.FundStatisticText.Size = new System.Drawing.Size(100, 15);
            this.FundStatisticText.TabIndex = 15;
            this.FundStatisticText.Text = "基金非營業日統計";
            // 
            // FuturesPriceText
            // 
            this.FuturesPriceText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FuturesPriceText.Location = new System.Drawing.Point(12, 143);
            this.FuturesPriceText.Name = "FuturesPriceText";
            this.FuturesPriceText.ReadOnly = true;
            this.FuturesPriceText.Size = new System.Drawing.Size(100, 15);
            this.FuturesPriceText.TabIndex = 16;
            this.FuturesPriceText.Text = "日期貨盤後行情表";
            // 
            // FuturesStatisticText
            // 
            this.FuturesStatisticText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FuturesStatisticText.Location = new System.Drawing.Point(314, 143);
            this.FuturesStatisticText.Name = "FuturesStatisticText";
            this.FuturesStatisticText.ReadOnly = true;
            this.FuturesStatisticText.Size = new System.Drawing.Size(100, 15);
            this.FuturesStatisticText.TabIndex = 17;
            this.FuturesStatisticText.Text = "日期貨盤後統計表";
            // 
            // TimeText
            // 
            this.TimeText.Location = new System.Drawing.Point(593, 34);
            this.TimeText.Multiline = true;
            this.TimeText.Name = "TimeText";
            this.TimeText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TimeText.Size = new System.Drawing.Size(195, 89);
            this.TimeText.TabIndex = 18;
            // 
            // SQLConnection
            // 
            this.SQLConnection.ConnectionString = "Data Source=192.168.10.180;Initial Catalog=StockDB;User ID=test;Password=test";
            this.SQLConnection.FireInfoMessageEventOnUserErrors = false;
            // 
            // sqlSelectCommand1
            // 
            this.sqlSelectCommand1.CommandText = "SELECT [投票日期]\r\n      ,[證券代號]\r\n      ,[證券名稱]\r\n      ,[召集人]\r\n      ,[股東會日期]\r\n      " +
    ",[發行代理機構]\r\n      ,[聯絡電話]\r\n      ,[CTIME]\r\n      ,[MTIME]\r\n  FROM [dbo].[股東會投票日明細" +
    "_luann];\r\n";
            this.sqlSelectCommand1.Connection = this.SQLConnection;
            // 
            // sqlInsertCommand1
            // 
            this.sqlInsertCommand1.CommandText = resources.GetString("sqlInsertCommand1.CommandText");
            this.sqlInsertCommand1.Connection = this.SQLConnection;
            this.sqlInsertCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@投票日期", System.Data.SqlDbType.Char, 0, "投票日期"),
            new System.Data.SqlClient.SqlParameter("@證券代號", System.Data.SqlDbType.VarChar, 0, "證券代號"),
            new System.Data.SqlClient.SqlParameter("@證券名稱", System.Data.SqlDbType.NVarChar, 0, "證券名稱"),
            new System.Data.SqlClient.SqlParameter("@召集人", System.Data.SqlDbType.NVarChar, 0, "召集人"),
            new System.Data.SqlClient.SqlParameter("@股東會日期", System.Data.SqlDbType.Char, 0, "股東會日期"),
            new System.Data.SqlClient.SqlParameter("@發行代理機構", System.Data.SqlDbType.NVarChar, 0, "發行代理機構"),
            new System.Data.SqlClient.SqlParameter("@聯絡電話", System.Data.SqlDbType.Char, 0, "聯絡電話"),
            new System.Data.SqlClient.SqlParameter("@CTIME", System.Data.SqlDbType.DateTime, 0, "CTIME"),
            new System.Data.SqlClient.SqlParameter("@MTIME", System.Data.SqlDbType.BigInt, 0, "MTIME")});
            // 
            // sqlUpdateCommand1
            // 
            this.sqlUpdateCommand1.CommandText = resources.GetString("sqlUpdateCommand1.CommandText");
            this.sqlUpdateCommand1.Connection = this.SQLConnection;
            this.sqlUpdateCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@投票日期", System.Data.SqlDbType.Char, 0, "投票日期"),
            new System.Data.SqlClient.SqlParameter("@證券代號", System.Data.SqlDbType.VarChar, 0, "證券代號"),
            new System.Data.SqlClient.SqlParameter("@證券名稱", System.Data.SqlDbType.NVarChar, 0, "證券名稱"),
            new System.Data.SqlClient.SqlParameter("@召集人", System.Data.SqlDbType.NVarChar, 0, "召集人"),
            new System.Data.SqlClient.SqlParameter("@股東會日期", System.Data.SqlDbType.Char, 0, "股東會日期"),
            new System.Data.SqlClient.SqlParameter("@發行代理機構", System.Data.SqlDbType.NVarChar, 0, "發行代理機構"),
            new System.Data.SqlClient.SqlParameter("@聯絡電話", System.Data.SqlDbType.Char, 0, "聯絡電話"),
            new System.Data.SqlClient.SqlParameter("@CTIME", System.Data.SqlDbType.DateTime, 0, "CTIME"),
            new System.Data.SqlClient.SqlParameter("@MTIME", System.Data.SqlDbType.BigInt, 0, "MTIME"),
            new System.Data.SqlClient.SqlParameter("@Original_投票日期", System.Data.SqlDbType.Char, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "投票日期", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_證券代號", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "證券代號", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_證券名稱", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "證券名稱", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_證券名稱", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "證券名稱", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_召集人", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "召集人", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_召集人", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "召集人", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_股東會日期", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "股東會日期", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_股東會日期", System.Data.SqlDbType.Char, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "股東會日期", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_發行代理機構", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "發行代理機構", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_發行代理機構", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "發行代理機構", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_聯絡電話", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "聯絡電話", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_聯絡電話", System.Data.SqlDbType.Char, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "聯絡電話", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_CTIME", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "CTIME", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_CTIME", System.Data.SqlDbType.DateTime, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CTIME", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_MTIME", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "MTIME", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_MTIME", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "MTIME", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlDeleteCommand1
            // 
            this.sqlDeleteCommand1.CommandText = resources.GetString("sqlDeleteCommand1.CommandText");
            this.sqlDeleteCommand1.Connection = this.SQLConnection;
            this.sqlDeleteCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Original_投票日期", System.Data.SqlDbType.Char, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "投票日期", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_證券代號", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "證券代號", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_證券名稱", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "證券名稱", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_證券名稱", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "證券名稱", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_召集人", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "召集人", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_召集人", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "召集人", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_股東會日期", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "股東會日期", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_股東會日期", System.Data.SqlDbType.Char, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "股東會日期", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_發行代理機構", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "發行代理機構", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_發行代理機構", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "發行代理機構", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_聯絡電話", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "聯絡電話", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_聯絡電話", System.Data.SqlDbType.Char, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "聯絡電話", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_CTIME", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "CTIME", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_CTIME", System.Data.SqlDbType.DateTime, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CTIME", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_MTIME", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "MTIME", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_MTIME", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "MTIME", System.Data.DataRowVersion.Original, null)});
            // 
            // SQLDataAdapter
            // 
            this.SQLDataAdapter.DeleteCommand = this.sqlDeleteCommand1;
            this.SQLDataAdapter.InsertCommand = this.sqlInsertCommand1;
            this.SQLDataAdapter.SelectCommand = this.sqlSelectCommand1;
            this.SQLDataAdapter.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "股東會投票日明細_luann", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("投票日期", "投票日期"),
                        new System.Data.Common.DataColumnMapping("證券代號", "證券代號"),
                        new System.Data.Common.DataColumnMapping("證券名稱", "證券名稱"),
                        new System.Data.Common.DataColumnMapping("召集人", "召集人"),
                        new System.Data.Common.DataColumnMapping("股東會日期", "股東會日期"),
                        new System.Data.Common.DataColumnMapping("發行代理機構", "發行代理機構"),
                        new System.Data.Common.DataColumnMapping("聯絡電話", "聯絡電話"),
                        new System.Data.Common.DataColumnMapping("CTIME", "CTIME"),
                        new System.Data.Common.DataColumnMapping("MTIME", "MTIME")})});
            this.SQLDataAdapter.UpdateCommand = this.sqlUpdateCommand1;
            // 
            // StockVoteSet
            // 
            this.StockVoteSet.DataSetName = "StockVoteSet";
            this.StockVoteSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // BindingSource
            // 
            this.BindingSource.DataMember = "股東會投票日明細_luann";
            this.BindingSource.DataSource = this.StockVoteSet;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 214);
            this.Controls.Add(this.TimeText);
            this.Controls.Add(this.FuturesStatisticText);
            this.Controls.Add(this.FuturesPriceText);
            this.Controls.Add(this.FundStatisticText);
            this.Controls.Add(this.FundDetailText);
            this.Controls.Add(this.VoteDataText);
            this.Controls.Add(this.FuturesStatisticDeleteButton);
            this.Controls.Add(this.FuturesStatisticAddButton);
            this.Controls.Add(this.FuturesPriceDeleteButton);
            this.Controls.Add(this.FuturesPriceAddButton);
            this.Controls.Add(this.FundStatisticDeleteButton);
            this.Controls.Add(this.FundStatisticAddButton);
            this.Controls.Add(this.FundDetailDeleteButton);
            this.Controls.Add(this.FundDetailAddButton);
            this.Controls.Add(this.VoteDataDeleteButton);
            this.Controls.Add(this.VoteDataAddButton);
            this.Controls.Add(this.VoteDayDeleteButton);
            this.Controls.Add(this.VoteDayAddButton);
            this.Controls.Add(this.VoteDayText);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.StockVoteSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox VoteDayText;
        private System.Windows.Forms.Button VoteDayAddButton;
        private System.Windows.Forms.Button VoteDayDeleteButton;
        private System.Windows.Forms.Button VoteDataAddButton;
        private System.Windows.Forms.Button VoteDataDeleteButton;
        private System.Windows.Forms.Button FundDetailAddButton;
        private System.Windows.Forms.Button FundDetailDeleteButton;
        private System.Windows.Forms.Button FundStatisticAddButton;
        private System.Windows.Forms.Button FundStatisticDeleteButton;
        private System.Windows.Forms.Button FuturesPriceAddButton;
        private System.Windows.Forms.Button FuturesPriceDeleteButton;
        private System.Windows.Forms.Button FuturesStatisticAddButton;
        private System.Windows.Forms.Button FuturesStatisticDeleteButton;
        private System.Windows.Forms.TextBox VoteDataText;
        private System.Windows.Forms.TextBox FundDetailText;
        private System.Windows.Forms.TextBox FundStatisticText;
        private System.Windows.Forms.TextBox FuturesPriceText;
        private System.Windows.Forms.TextBox FuturesStatisticText;
        private System.Windows.Forms.TextBox TimeText;
        private System.Data.SqlClient.SqlConnection SQLConnection;
        private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
        private System.Data.SqlClient.SqlCommand sqlInsertCommand1;
        private System.Data.SqlClient.SqlCommand sqlUpdateCommand1;
        private System.Data.SqlClient.SqlCommand sqlDeleteCommand1;
        private System.Data.SqlClient.SqlDataAdapter SQLDataAdapter;
        private StockVoteSet StockVoteSet;
        private System.Windows.Forms.BindingSource BindingSource;
    }
}

