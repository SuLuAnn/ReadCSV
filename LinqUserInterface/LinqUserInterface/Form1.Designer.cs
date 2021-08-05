namespace LinqUserInterface
{
    partial class LINQTraning
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
            this.DisplayTime = new System.Windows.Forms.TextBox();
            this.HeaderTabControl = new System.Windows.Forms.TabControl();
            this.IndustryDropDownPage = new System.Windows.Forms.TabPage();
            this.CheckButton = new System.Windows.Forms.Button();
            this.IndustryMenu = new System.Windows.Forms.ComboBox();
            this.MarketMenu = new System.Windows.Forms.ComboBox();
            this.IndustryName = new System.Windows.Forms.TextBox();
            this.MarketName = new System.Windows.Forms.TextBox();
            this.DayCheckChangePage = new System.Windows.Forms.TabPage();
            this.DayPERPage = new System.Windows.Forms.TabPage();
            this.CheckPERButton = new System.Windows.Forms.Button();
            this.TargetPERMenu = new System.Windows.Forms.NumericUpDown();
            this.ERPIndustryMenu = new System.Windows.Forms.ComboBox();
            this.CycleMenu = new System.Windows.Forms.TextBox();
            this.ERPIndustryName = new System.Windows.Forms.TextBox();
            this.TargetPER = new System.Windows.Forms.TextBox();
            this.Cycle = new System.Windows.Forms.TextBox();
            this.ElectionStockPerformancePage = new System.Windows.Forms.TabPage();
            this.DataTabControl = new System.Windows.Forms.TabControl();
            this.CommonPage = new System.Windows.Forms.TabPage();
            this.CommonTable = new System.Windows.Forms.DataGridView();
            this.PayMaxMinPage = new System.Windows.Forms.TabPage();
            this.OriginalDataCalculatedPage = new System.Windows.Forms.TabPage();
            this.DataNum = new System.Windows.Forms.TextBox();
            this.HeaderTabControl.SuspendLayout();
            this.IndustryDropDownPage.SuspendLayout();
            this.DayPERPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TargetPERMenu)).BeginInit();
            this.DataTabControl.SuspendLayout();
            this.CommonPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CommonTable)).BeginInit();
            this.SuspendLayout();
            // 
            // DisplayTime
            // 
            this.DisplayTime.Location = new System.Drawing.Point(1298, 12);
            this.DisplayTime.Multiline = true;
            this.DisplayTime.Name = "DisplayTime";
            this.DisplayTime.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DisplayTime.Size = new System.Drawing.Size(270, 176);
            this.DisplayTime.TabIndex = 0;
            this.DisplayTime.HandleCreated += new System.EventHandler(this.DisplayLoad);
            // 
            // HeaderTabControl
            // 
            this.HeaderTabControl.Controls.Add(this.IndustryDropDownPage);
            this.HeaderTabControl.Controls.Add(this.DayCheckChangePage);
            this.HeaderTabControl.Controls.Add(this.DayPERPage);
            this.HeaderTabControl.Controls.Add(this.ElectionStockPerformancePage);
            this.HeaderTabControl.Location = new System.Drawing.Point(12, 12);
            this.HeaderTabControl.Name = "HeaderTabControl";
            this.HeaderTabControl.SelectedIndex = 0;
            this.HeaderTabControl.Size = new System.Drawing.Size(1280, 176);
            this.HeaderTabControl.TabIndex = 1;
            // 
            // IndustryDropDownPage
            // 
            this.IndustryDropDownPage.Controls.Add(this.CheckButton);
            this.IndustryDropDownPage.Controls.Add(this.IndustryMenu);
            this.IndustryDropDownPage.Controls.Add(this.MarketMenu);
            this.IndustryDropDownPage.Controls.Add(this.IndustryName);
            this.IndustryDropDownPage.Controls.Add(this.MarketName);
            this.IndustryDropDownPage.Location = new System.Drawing.Point(4, 28);
            this.IndustryDropDownPage.Name = "IndustryDropDownPage";
            this.IndustryDropDownPage.Padding = new System.Windows.Forms.Padding(3);
            this.IndustryDropDownPage.Size = new System.Drawing.Size(1272, 144);
            this.IndustryDropDownPage.TabIndex = 0;
            this.IndustryDropDownPage.Text = "交易所產業分類下拉選單";
            this.IndustryDropDownPage.UseVisualStyleBackColor = true;
            // 
            // CheckButton
            // 
            this.CheckButton.Location = new System.Drawing.Point(851, 48);
            this.CheckButton.Name = "CheckButton";
            this.CheckButton.Size = new System.Drawing.Size(79, 29);
            this.CheckButton.TabIndex = 4;
            this.CheckButton.Text = "查詢";
            this.CheckButton.UseVisualStyleBackColor = true;
            this.CheckButton.Click += new System.EventHandler(this.ClickCheckButton);
            // 
            // IndustryMenu
            // 
            this.IndustryMenu.FormattingEnabled = true;
            this.IndustryMenu.Location = new System.Drawing.Point(597, 48);
            this.IndustryMenu.Name = "IndustryMenu";
            this.IndustryMenu.Size = new System.Drawing.Size(224, 26);
            this.IndustryMenu.TabIndex = 3;
            // 
            // MarketMenu
            // 
            this.MarketMenu.FormattingEnabled = true;
            this.MarketMenu.Location = new System.Drawing.Point(216, 48);
            this.MarketMenu.Name = "MarketMenu";
            this.MarketMenu.Size = new System.Drawing.Size(204, 26);
            this.MarketMenu.TabIndex = 2;
            this.MarketMenu.SelectedIndexChanged += new System.EventHandler(this.DisplayIndustryMenu);
            this.MarketMenu.Click += new System.EventHandler(this.DisplayMarketMenu);
            // 
            // IndustryName
            // 
            this.IndustryName.BackColor = System.Drawing.Color.White;
            this.IndustryName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.IndustryName.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.IndustryName.Location = new System.Drawing.Point(472, 48);
            this.IndustryName.Name = "IndustryName";
            this.IndustryName.ReadOnly = true;
            this.IndustryName.Size = new System.Drawing.Size(119, 29);
            this.IndustryName.TabIndex = 1;
            this.IndustryName.Text = "產業名稱：";
            // 
            // MarketName
            // 
            this.MarketName.BackColor = System.Drawing.Color.White;
            this.MarketName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MarketName.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.MarketName.Location = new System.Drawing.Point(68, 48);
            this.MarketName.Name = "MarketName";
            this.MarketName.ReadOnly = true;
            this.MarketName.Size = new System.Drawing.Size(152, 29);
            this.MarketName.TabIndex = 0;
            this.MarketName.Text = "市場別名稱：";
            // 
            // DayCheckChangePage
            // 
            this.DayCheckChangePage.Location = new System.Drawing.Point(4, 28);
            this.DayCheckChangePage.Name = "DayCheckChangePage";
            this.DayCheckChangePage.Padding = new System.Windows.Forms.Padding(3);
            this.DayCheckChangePage.Size = new System.Drawing.Size(1272, 144);
            this.DayCheckChangePage.TabIndex = 1;
            this.DayCheckChangePage.Text = "日收盤查詢及轉置";
            this.DayCheckChangePage.UseVisualStyleBackColor = true;
            // 
            // DayPERPage
            // 
            this.DayPERPage.Controls.Add(this.CheckPERButton);
            this.DayPERPage.Controls.Add(this.TargetPERMenu);
            this.DayPERPage.Controls.Add(this.ERPIndustryMenu);
            this.DayPERPage.Controls.Add(this.CycleMenu);
            this.DayPERPage.Controls.Add(this.ERPIndustryName);
            this.DayPERPage.Controls.Add(this.TargetPER);
            this.DayPERPage.Controls.Add(this.Cycle);
            this.DayPERPage.Location = new System.Drawing.Point(4, 28);
            this.DayPERPage.Name = "DayPERPage";
            this.DayPERPage.Size = new System.Drawing.Size(1272, 144);
            this.DayPERPage.TabIndex = 2;
            this.DayPERPage.Text = "日收盤本益比優劣榜";
            this.DayPERPage.UseVisualStyleBackColor = true;
            // 
            // CheckPERButton
            // 
            this.CheckPERButton.Location = new System.Drawing.Point(1001, 14);
            this.CheckPERButton.Name = "CheckPERButton";
            this.CheckPERButton.Size = new System.Drawing.Size(136, 102);
            this.CheckPERButton.TabIndex = 6;
            this.CheckPERButton.Text = "查詢";
            this.CheckPERButton.UseVisualStyleBackColor = true;
            this.CheckPERButton.Click += new System.EventHandler(this.ClickCheckPERButton);
            // 
            // TargetPERMenu
            // 
            this.TargetPERMenu.DecimalPlaces = 1;
            this.TargetPERMenu.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.TargetPERMenu.Location = new System.Drawing.Point(678, 27);
            this.TargetPERMenu.Name = "TargetPERMenu";
            this.TargetPERMenu.Size = new System.Drawing.Size(120, 29);
            this.TargetPERMenu.TabIndex = 5;
            this.TargetPERMenu.Value = new decimal(new int[] {
            105,
            0,
            0,
            65536});
            // 
            // ERPIndustryMenu
            // 
            this.ERPIndustryMenu.FormattingEnabled = true;
            this.ERPIndustryMenu.Location = new System.Drawing.Point(678, 82);
            this.ERPIndustryMenu.Name = "ERPIndustryMenu";
            this.ERPIndustryMenu.Size = new System.Drawing.Size(247, 26);
            this.ERPIndustryMenu.TabIndex = 4;
            this.ERPIndustryMenu.Click += new System.EventHandler(this.DisplayERPIndustryMenu);
            // 
            // CycleMenu
            // 
            this.CycleMenu.Location = new System.Drawing.Point(147, 27);
            this.CycleMenu.Name = "CycleMenu";
            this.CycleMenu.Size = new System.Drawing.Size(270, 29);
            this.CycleMenu.TabIndex = 3;
            this.CycleMenu.Text = "20150101-20151231";
            // 
            // ERPIndustryName
            // 
            this.ERPIndustryName.BackColor = System.Drawing.Color.White;
            this.ERPIndustryName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ERPIndustryName.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ERPIndustryName.Location = new System.Drawing.Point(535, 82);
            this.ERPIndustryName.Name = "ERPIndustryName";
            this.ERPIndustryName.ReadOnly = true;
            this.ERPIndustryName.Size = new System.Drawing.Size(137, 34);
            this.ERPIndustryName.TabIndex = 2;
            this.ERPIndustryName.Text = "產業名稱：";
            // 
            // TargetPER
            // 
            this.TargetPER.BackColor = System.Drawing.Color.White;
            this.TargetPER.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TargetPER.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TargetPER.Location = new System.Drawing.Point(509, 27);
            this.TargetPER.Name = "TargetPER";
            this.TargetPER.ReadOnly = true;
            this.TargetPER.Size = new System.Drawing.Size(162, 34);
            this.TargetPER.TabIndex = 1;
            this.TargetPER.Text = "目標本益比：";
            // 
            // Cycle
            // 
            this.Cycle.BackColor = System.Drawing.Color.White;
            this.Cycle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Cycle.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Cycle.Location = new System.Drawing.Point(60, 27);
            this.Cycle.Name = "Cycle";
            this.Cycle.ReadOnly = true;
            this.Cycle.Size = new System.Drawing.Size(100, 34);
            this.Cycle.TabIndex = 0;
            this.Cycle.Text = "週期：";
            // 
            // ElectionStockPerformancePage
            // 
            this.ElectionStockPerformancePage.Location = new System.Drawing.Point(4, 28);
            this.ElectionStockPerformancePage.Name = "ElectionStockPerformancePage";
            this.ElectionStockPerformancePage.Size = new System.Drawing.Size(1272, 144);
            this.ElectionStockPerformancePage.TabIndex = 3;
            this.ElectionStockPerformancePage.Text = "總統大選類股表現";
            this.ElectionStockPerformancePage.UseVisualStyleBackColor = true;
            // 
            // DataTabControl
            // 
            this.DataTabControl.Controls.Add(this.CommonPage);
            this.DataTabControl.Controls.Add(this.PayMaxMinPage);
            this.DataTabControl.Controls.Add(this.OriginalDataCalculatedPage);
            this.DataTabControl.Location = new System.Drawing.Point(16, 194);
            this.DataTabControl.Name = "DataTabControl";
            this.DataTabControl.SelectedIndex = 0;
            this.DataTabControl.Size = new System.Drawing.Size(1552, 579);
            this.DataTabControl.TabIndex = 2;
            // 
            // CommonPage
            // 
            this.CommonPage.Controls.Add(this.CommonTable);
            this.CommonPage.Location = new System.Drawing.Point(4, 28);
            this.CommonPage.Name = "CommonPage";
            this.CommonPage.Padding = new System.Windows.Forms.Padding(3);
            this.CommonPage.Size = new System.Drawing.Size(1544, 547);
            this.CommonPage.TabIndex = 0;
            this.CommonPage.Text = "共用";
            this.CommonPage.UseVisualStyleBackColor = true;
            // 
            // CommonTable
            // 
            this.CommonTable.AllowUserToAddRows = false;
            this.CommonTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CommonTable.Location = new System.Drawing.Point(3, 3);
            this.CommonTable.Name = "CommonTable";
            this.CommonTable.RowHeadersWidth = 62;
            this.CommonTable.RowTemplate.Height = 31;
            this.CommonTable.Size = new System.Drawing.Size(1535, 538);
            this.CommonTable.TabIndex = 0;
            // 
            // PayMaxMinPage
            // 
            this.PayMaxMinPage.Location = new System.Drawing.Point(4, 28);
            this.PayMaxMinPage.Name = "PayMaxMinPage";
            this.PayMaxMinPage.Padding = new System.Windows.Forms.Padding(3);
            this.PayMaxMinPage.Size = new System.Drawing.Size(1544, 547);
            this.PayMaxMinPage.TabIndex = 1;
            this.PayMaxMinPage.Text = "Q4-d-類股報酬率最高、最低前五名";
            this.PayMaxMinPage.UseVisualStyleBackColor = true;
            // 
            // OriginalDataCalculatedPage
            // 
            this.OriginalDataCalculatedPage.Location = new System.Drawing.Point(4, 28);
            this.OriginalDataCalculatedPage.Name = "OriginalDataCalculatedPage";
            this.OriginalDataCalculatedPage.Size = new System.Drawing.Size(1544, 547);
            this.OriginalDataCalculatedPage.TabIndex = 2;
            this.OriginalDataCalculatedPage.Text = "Q4-d-計算後原始資料";
            this.OriginalDataCalculatedPage.UseVisualStyleBackColor = true;
            // 
            // DataNum
            // 
            this.DataNum.BackColor = System.Drawing.SystemColors.Control;
            this.DataNum.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DataNum.Location = new System.Drawing.Point(16, 775);
            this.DataNum.Name = "DataNum";
            this.DataNum.ReadOnly = true;
            this.DataNum.Size = new System.Drawing.Size(100, 22);
            this.DataNum.TabIndex = 0;
            this.DataNum.Text = "0筆";
            // 
            // LINQTraning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1580, 809);
            this.Controls.Add(this.DataNum);
            this.Controls.Add(this.DataTabControl);
            this.Controls.Add(this.HeaderTabControl);
            this.Controls.Add(this.DisplayTime);
            this.Name = "LINQTraning";
            this.Text = "LINQ訓練";
            this.HeaderTabControl.ResumeLayout(false);
            this.IndustryDropDownPage.ResumeLayout(false);
            this.IndustryDropDownPage.PerformLayout();
            this.DayPERPage.ResumeLayout(false);
            this.DayPERPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TargetPERMenu)).EndInit();
            this.DataTabControl.ResumeLayout(false);
            this.CommonPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CommonTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox DisplayTime;
        private System.Windows.Forms.TabControl HeaderTabControl;
        private System.Windows.Forms.TabPage IndustryDropDownPage;
        private System.Windows.Forms.TabPage DayCheckChangePage;
        private System.Windows.Forms.TabControl DataTabControl;
        private System.Windows.Forms.TabPage CommonPage;
        private System.Windows.Forms.TabPage PayMaxMinPage;
        private System.Windows.Forms.TabPage DayPERPage;
        private System.Windows.Forms.TabPage ElectionStockPerformancePage;
        private System.Windows.Forms.TabPage OriginalDataCalculatedPage;
        private System.Windows.Forms.TextBox DataNum;
        private System.Windows.Forms.Button CheckButton;
        private System.Windows.Forms.ComboBox IndustryMenu;
        private System.Windows.Forms.ComboBox MarketMenu;
        private System.Windows.Forms.TextBox IndustryName;
        private System.Windows.Forms.TextBox MarketName;
        private System.Windows.Forms.DataGridView CommonTable;
        private System.Windows.Forms.TextBox Cycle;
        private System.Windows.Forms.TextBox ERPIndustryName;
        private System.Windows.Forms.TextBox TargetPER;
        private System.Windows.Forms.Button CheckPERButton;
        private System.Windows.Forms.NumericUpDown TargetPERMenu;
        private System.Windows.Forms.ComboBox ERPIndustryMenu;
        private System.Windows.Forms.TextBox CycleMenu;
    }
}

