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
            this.StockIDNameMenu = new System.Windows.Forms.TextBox();
            this.CycleDayMenu = new System.Windows.Forms.TextBox();
            this.DayButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.CycleDay = new System.Windows.Forms.TextBox();
            this.DayPERPage = new System.Windows.Forms.TabPage();
            this.CheckPERButton = new System.Windows.Forms.Button();
            this.TargetPERMenu = new System.Windows.Forms.NumericUpDown();
            this.PERIndustryMenu = new System.Windows.Forms.ComboBox();
            this.CycleMenu = new System.Windows.Forms.TextBox();
            this.ERPIndustryName = new System.Windows.Forms.TextBox();
            this.TargetPER = new System.Windows.Forms.TextBox();
            this.Cycle = new System.Windows.Forms.TextBox();
            this.ElectionStockPerformancePage = new System.Windows.Forms.TabPage();
            this.RiseFallTop5Button = new System.Windows.Forms.Button();
            this.Top5VolumeButton = new System.Windows.Forms.Button();
            this.SameRankingButton = new System.Windows.Forms.Button();
            this.RisingButton = new System.Windows.Forms.Button();
            this.DataTabControl = new System.Windows.Forms.TabControl();
            this.CommonPage = new System.Windows.Forms.TabPage();
            this.CommonTable = new System.Windows.Forms.DataGridView();
            this.PayMaxMinPage = new System.Windows.Forms.TabPage();
            this.RiseFallTop5Table = new System.Windows.Forms.DataGridView();
            this.OriginalDataCalculatedPage = new System.Windows.Forms.TabPage();
            this.OriginalDataTable = new System.Windows.Forms.DataGridView();
            this.DataNum = new System.Windows.Forms.TextBox();
            this.HeaderTabControl.SuspendLayout();
            this.IndustryDropDownPage.SuspendLayout();
            this.DayCheckChangePage.SuspendLayout();
            this.DayPERPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TargetPERMenu)).BeginInit();
            this.ElectionStockPerformancePage.SuspendLayout();
            this.DataTabControl.SuspendLayout();
            this.CommonPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CommonTable)).BeginInit();
            this.PayMaxMinPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RiseFallTop5Table)).BeginInit();
            this.OriginalDataCalculatedPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OriginalDataTable)).BeginInit();
            this.SuspendLayout();
            // 
            // DisplayTime
            // 
            this.DisplayTime.Location = new System.Drawing.Point(865, 8);
            this.DisplayTime.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DisplayTime.Multiline = true;
            this.DisplayTime.Name = "DisplayTime";
            this.DisplayTime.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DisplayTime.Size = new System.Drawing.Size(181, 119);
            this.DisplayTime.TabIndex = 0;
            this.DisplayTime.HandleCreated += new System.EventHandler(this.DisplayLoad);
            // 
            // HeaderTabControl
            // 
            this.HeaderTabControl.Controls.Add(this.IndustryDropDownPage);
            this.HeaderTabControl.Controls.Add(this.DayCheckChangePage);
            this.HeaderTabControl.Controls.Add(this.DayPERPage);
            this.HeaderTabControl.Controls.Add(this.ElectionStockPerformancePage);
            this.HeaderTabControl.Location = new System.Drawing.Point(8, 8);
            this.HeaderTabControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.HeaderTabControl.Name = "HeaderTabControl";
            this.HeaderTabControl.SelectedIndex = 0;
            this.HeaderTabControl.Size = new System.Drawing.Size(853, 117);
            this.HeaderTabControl.TabIndex = 1;
            // 
            // IndustryDropDownPage
            // 
            this.IndustryDropDownPage.Controls.Add(this.CheckButton);
            this.IndustryDropDownPage.Controls.Add(this.IndustryMenu);
            this.IndustryDropDownPage.Controls.Add(this.MarketMenu);
            this.IndustryDropDownPage.Controls.Add(this.IndustryName);
            this.IndustryDropDownPage.Controls.Add(this.MarketName);
            this.IndustryDropDownPage.Location = new System.Drawing.Point(4, 22);
            this.IndustryDropDownPage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.IndustryDropDownPage.Name = "IndustryDropDownPage";
            this.IndustryDropDownPage.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.IndustryDropDownPage.Size = new System.Drawing.Size(845, 91);
            this.IndustryDropDownPage.TabIndex = 0;
            this.IndustryDropDownPage.Text = "交易所產業分類下拉選單";
            this.IndustryDropDownPage.UseVisualStyleBackColor = true;
            // 
            // CheckButton
            // 
            this.CheckButton.Location = new System.Drawing.Point(567, 32);
            this.CheckButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CheckButton.Name = "CheckButton";
            this.CheckButton.Size = new System.Drawing.Size(53, 19);
            this.CheckButton.TabIndex = 4;
            this.CheckButton.Text = "查詢";
            this.CheckButton.UseVisualStyleBackColor = true;
            this.CheckButton.Click += new System.EventHandler(this.ClickCheckButton);
            // 
            // IndustryMenu
            // 
            this.IndustryMenu.FormattingEnabled = true;
            this.IndustryMenu.Location = new System.Drawing.Point(398, 32);
            this.IndustryMenu.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.IndustryMenu.Name = "IndustryMenu";
            this.IndustryMenu.Size = new System.Drawing.Size(151, 20);
            this.IndustryMenu.TabIndex = 3;
            // 
            // MarketMenu
            // 
            this.MarketMenu.FormattingEnabled = true;
            this.MarketMenu.Location = new System.Drawing.Point(144, 32);
            this.MarketMenu.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MarketMenu.Name = "MarketMenu";
            this.MarketMenu.Size = new System.Drawing.Size(137, 20);
            this.MarketMenu.TabIndex = 2;
            this.MarketMenu.SelectedIndexChanged += new System.EventHandler(this.DisplayIndustryMenu);
            this.MarketMenu.Click += new System.EventHandler(this.DisplayMarketMenu);
            // 
            // IndustryName
            // 
            this.IndustryName.BackColor = System.Drawing.Color.White;
            this.IndustryName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.IndustryName.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.IndustryName.Location = new System.Drawing.Point(315, 32);
            this.IndustryName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.IndustryName.Name = "IndustryName";
            this.IndustryName.ReadOnly = true;
            this.IndustryName.Size = new System.Drawing.Size(79, 20);
            this.IndustryName.TabIndex = 1;
            this.IndustryName.Text = "產業名稱：";
            // 
            // MarketName
            // 
            this.MarketName.BackColor = System.Drawing.Color.White;
            this.MarketName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MarketName.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.MarketName.Location = new System.Drawing.Point(45, 32);
            this.MarketName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MarketName.Name = "MarketName";
            this.MarketName.ReadOnly = true;
            this.MarketName.Size = new System.Drawing.Size(101, 20);
            this.MarketName.TabIndex = 0;
            this.MarketName.Text = "市場別名稱：";
            // 
            // DayCheckChangePage
            // 
            this.DayCheckChangePage.Controls.Add(this.StockIDNameMenu);
            this.DayCheckChangePage.Controls.Add(this.CycleDayMenu);
            this.DayCheckChangePage.Controls.Add(this.DayButton);
            this.DayCheckChangePage.Controls.Add(this.textBox1);
            this.DayCheckChangePage.Controls.Add(this.CycleDay);
            this.DayCheckChangePage.Location = new System.Drawing.Point(4, 22);
            this.DayCheckChangePage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DayCheckChangePage.Name = "DayCheckChangePage";
            this.DayCheckChangePage.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DayCheckChangePage.Size = new System.Drawing.Size(845, 91);
            this.DayCheckChangePage.TabIndex = 1;
            this.DayCheckChangePage.Text = "日收盤查詢及轉置";
            this.DayCheckChangePage.UseVisualStyleBackColor = true;
            // 
            // StockIDNameMenu
            // 
            this.StockIDNameMenu.Location = new System.Drawing.Point(462, 27);
            this.StockIDNameMenu.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.StockIDNameMenu.Name = "StockIDNameMenu";
            this.StockIDNameMenu.Size = new System.Drawing.Size(163, 22);
            this.StockIDNameMenu.TabIndex = 4;
            this.StockIDNameMenu.Text = "2330,2317,2610,台灣50";
            // 
            // CycleDayMenu
            // 
            this.CycleDayMenu.Location = new System.Drawing.Point(113, 27);
            this.CycleDayMenu.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CycleDayMenu.Name = "CycleDayMenu";
            this.CycleDayMenu.Size = new System.Drawing.Size(158, 22);
            this.CycleDayMenu.TabIndex = 3;
            this.CycleDayMenu.Text = "20150701-20151231";
            // 
            // DayButton
            // 
            this.DayButton.Location = new System.Drawing.Point(658, 24);
            this.DayButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DayButton.Name = "DayButton";
            this.DayButton.Size = new System.Drawing.Size(50, 23);
            this.DayButton.TabIndex = 2;
            this.DayButton.Text = "查詢";
            this.DayButton.UseVisualStyleBackColor = true;
            this.DayButton.Click += new System.EventHandler(this.ClickDayButton);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox1.Location = new System.Drawing.Point(314, 29);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(153, 23);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "股票代號或名稱：";
            // 
            // CycleDay
            // 
            this.CycleDay.BackColor = System.Drawing.Color.White;
            this.CycleDay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CycleDay.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.CycleDay.Location = new System.Drawing.Point(57, 29);
            this.CycleDay.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CycleDay.Name = "CycleDay";
            this.CycleDay.ReadOnly = true;
            this.CycleDay.Size = new System.Drawing.Size(67, 23);
            this.CycleDay.TabIndex = 0;
            this.CycleDay.Text = "週期：";
            // 
            // DayPERPage
            // 
            this.DayPERPage.Controls.Add(this.CheckPERButton);
            this.DayPERPage.Controls.Add(this.TargetPERMenu);
            this.DayPERPage.Controls.Add(this.PERIndustryMenu);
            this.DayPERPage.Controls.Add(this.CycleMenu);
            this.DayPERPage.Controls.Add(this.ERPIndustryName);
            this.DayPERPage.Controls.Add(this.TargetPER);
            this.DayPERPage.Controls.Add(this.Cycle);
            this.DayPERPage.Location = new System.Drawing.Point(4, 22);
            this.DayPERPage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DayPERPage.Name = "DayPERPage";
            this.DayPERPage.Size = new System.Drawing.Size(845, 91);
            this.DayPERPage.TabIndex = 2;
            this.DayPERPage.Text = "日收盤本益比優劣榜";
            this.DayPERPage.UseVisualStyleBackColor = true;
            // 
            // CheckPERButton
            // 
            this.CheckPERButton.Location = new System.Drawing.Point(667, 9);
            this.CheckPERButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CheckPERButton.Name = "CheckPERButton";
            this.CheckPERButton.Size = new System.Drawing.Size(91, 68);
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
            this.TargetPERMenu.Location = new System.Drawing.Point(452, 18);
            this.TargetPERMenu.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TargetPERMenu.Name = "TargetPERMenu";
            this.TargetPERMenu.Size = new System.Drawing.Size(80, 22);
            this.TargetPERMenu.TabIndex = 5;
            this.TargetPERMenu.Value = new decimal(new int[] {
            105,
            0,
            0,
            65536});
            // 
            // PERIndustryMenu
            // 
            this.PERIndustryMenu.FormattingEnabled = true;
            this.PERIndustryMenu.Location = new System.Drawing.Point(452, 55);
            this.PERIndustryMenu.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PERIndustryMenu.Name = "PERIndustryMenu";
            this.PERIndustryMenu.Size = new System.Drawing.Size(166, 20);
            this.PERIndustryMenu.TabIndex = 4;
            this.PERIndustryMenu.Click += new System.EventHandler(this.DisplayPERIndustryMenu);
            // 
            // CycleMenu
            // 
            this.CycleMenu.Location = new System.Drawing.Point(98, 18);
            this.CycleMenu.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CycleMenu.Name = "CycleMenu";
            this.CycleMenu.Size = new System.Drawing.Size(181, 22);
            this.CycleMenu.TabIndex = 3;
            this.CycleMenu.Text = "20150101-20151231";
            // 
            // ERPIndustryName
            // 
            this.ERPIndustryName.BackColor = System.Drawing.Color.White;
            this.ERPIndustryName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ERPIndustryName.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ERPIndustryName.Location = new System.Drawing.Point(357, 55);
            this.ERPIndustryName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ERPIndustryName.Name = "ERPIndustryName";
            this.ERPIndustryName.ReadOnly = true;
            this.ERPIndustryName.Size = new System.Drawing.Size(91, 23);
            this.ERPIndustryName.TabIndex = 2;
            this.ERPIndustryName.Text = "產業名稱：";
            // 
            // TargetPER
            // 
            this.TargetPER.BackColor = System.Drawing.Color.White;
            this.TargetPER.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TargetPER.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TargetPER.Location = new System.Drawing.Point(339, 18);
            this.TargetPER.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TargetPER.Name = "TargetPER";
            this.TargetPER.ReadOnly = true;
            this.TargetPER.Size = new System.Drawing.Size(108, 23);
            this.TargetPER.TabIndex = 1;
            this.TargetPER.Text = "目標本益比：";
            // 
            // Cycle
            // 
            this.Cycle.BackColor = System.Drawing.Color.White;
            this.Cycle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Cycle.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Cycle.Location = new System.Drawing.Point(40, 18);
            this.Cycle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Cycle.Name = "Cycle";
            this.Cycle.ReadOnly = true;
            this.Cycle.Size = new System.Drawing.Size(67, 23);
            this.Cycle.TabIndex = 0;
            this.Cycle.Text = "週期：";
            // 
            // ElectionStockPerformancePage
            // 
            this.ElectionStockPerformancePage.Controls.Add(this.RiseFallTop5Button);
            this.ElectionStockPerformancePage.Controls.Add(this.Top5VolumeButton);
            this.ElectionStockPerformancePage.Controls.Add(this.SameRankingButton);
            this.ElectionStockPerformancePage.Controls.Add(this.RisingButton);
            this.ElectionStockPerformancePage.Location = new System.Drawing.Point(4, 22);
            this.ElectionStockPerformancePage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ElectionStockPerformancePage.Name = "ElectionStockPerformancePage";
            this.ElectionStockPerformancePage.Size = new System.Drawing.Size(845, 91);
            this.ElectionStockPerformancePage.TabIndex = 3;
            this.ElectionStockPerformancePage.Text = "總統大選類股表現";
            this.ElectionStockPerformancePage.UseVisualStyleBackColor = true;
            // 
            // RiseFallTop5Button
            // 
            this.RiseFallTop5Button.Location = new System.Drawing.Point(553, 29);
            this.RiseFallTop5Button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RiseFallTop5Button.Name = "RiseFallTop5Button";
            this.RiseFallTop5Button.Size = new System.Drawing.Size(249, 20);
            this.RiseFallTop5Button.TabIndex = 3;
            this.RiseFallTop5Button.Text = "d.漲跌次數+漲跌連續天數是否在報酬率前五";
            this.RiseFallTop5Button.UseVisualStyleBackColor = true;
            this.RiseFallTop5Button.Click += new System.EventHandler(this.ClickRiseFallTop5Button);
            // 
            // Top5VolumeButton
            // 
            this.Top5VolumeButton.Location = new System.Drawing.Point(372, 29);
            this.Top5VolumeButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Top5VolumeButton.Name = "Top5VolumeButton";
            this.Top5VolumeButton.Size = new System.Drawing.Size(149, 20);
            this.Top5VolumeButton.TabIndex = 2;
            this.Top5VolumeButton.Text = "c.成交量前五大的個股";
            this.Top5VolumeButton.UseVisualStyleBackColor = true;
            this.Top5VolumeButton.Click += new System.EventHandler(this.ClickTop5VolumeButton);
            // 
            // SameRankingButton
            // 
            this.SameRankingButton.Location = new System.Drawing.Point(187, 29);
            this.SameRankingButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SameRankingButton.Name = "SameRankingButton";
            this.SameRankingButton.Size = new System.Drawing.Size(165, 20);
            this.SameRankingButton.TabIndex = 1;
            this.SameRankingButton.Text = "b.排名相同類股";
            this.SameRankingButton.UseVisualStyleBackColor = true;
            this.SameRankingButton.Click += new System.EventHandler(this.ClickSameRankingButton);
            // 
            // RisingButton
            // 
            this.RisingButton.Location = new System.Drawing.Point(15, 29);
            this.RisingButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RisingButton.Name = "RisingButton";
            this.RisingButton.Size = new System.Drawing.Size(145, 20);
            this.RisingButton.TabIndex = 0;
            this.RisingButton.Text = "a.上漲所有類股查詢";
            this.RisingButton.UseVisualStyleBackColor = true;
            this.RisingButton.Click += new System.EventHandler(this.ClickRisingButton);
            // 
            // DataTabControl
            // 
            this.DataTabControl.Controls.Add(this.CommonPage);
            this.DataTabControl.Controls.Add(this.PayMaxMinPage);
            this.DataTabControl.Controls.Add(this.OriginalDataCalculatedPage);
            this.DataTabControl.Location = new System.Drawing.Point(11, 129);
            this.DataTabControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DataTabControl.Name = "DataTabControl";
            this.DataTabControl.SelectedIndex = 0;
            this.DataTabControl.Size = new System.Drawing.Size(1035, 386);
            this.DataTabControl.TabIndex = 2;
            // 
            // CommonPage
            // 
            this.CommonPage.Controls.Add(this.CommonTable);
            this.CommonPage.Location = new System.Drawing.Point(4, 22);
            this.CommonPage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CommonPage.Name = "CommonPage";
            this.CommonPage.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CommonPage.Size = new System.Drawing.Size(1027, 360);
            this.CommonPage.TabIndex = 0;
            this.CommonPage.Text = "共用";
            this.CommonPage.UseVisualStyleBackColor = true;
            // 
            // CommonTable
            // 
            this.CommonTable.AllowUserToAddRows = false;
            this.CommonTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CommonTable.Location = new System.Drawing.Point(2, 2);
            this.CommonTable.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CommonTable.Name = "CommonTable";
            this.CommonTable.RowHeadersWidth = 62;
            this.CommonTable.RowTemplate.Height = 31;
            this.CommonTable.Size = new System.Drawing.Size(1023, 359);
            this.CommonTable.TabIndex = 0;
            // 
            // PayMaxMinPage
            // 
            this.PayMaxMinPage.Controls.Add(this.RiseFallTop5Table);
            this.PayMaxMinPage.Location = new System.Drawing.Point(4, 22);
            this.PayMaxMinPage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PayMaxMinPage.Name = "PayMaxMinPage";
            this.PayMaxMinPage.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PayMaxMinPage.Size = new System.Drawing.Size(1027, 360);
            this.PayMaxMinPage.TabIndex = 1;
            this.PayMaxMinPage.Text = "Q4-d-類股報酬率最高、最低前五名";
            this.PayMaxMinPage.UseVisualStyleBackColor = true;
            // 
            // RiseFallTop5Table
            // 
            this.RiseFallTop5Table.AllowUserToAddRows = false;
            this.RiseFallTop5Table.AllowUserToDeleteRows = false;
            this.RiseFallTop5Table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RiseFallTop5Table.Location = new System.Drawing.Point(4, 2);
            this.RiseFallTop5Table.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RiseFallTop5Table.Name = "RiseFallTop5Table";
            this.RiseFallTop5Table.ReadOnly = true;
            this.RiseFallTop5Table.RowHeadersWidth = 62;
            this.RiseFallTop5Table.RowTemplate.Height = 31;
            this.RiseFallTop5Table.Size = new System.Drawing.Size(1021, 359);
            this.RiseFallTop5Table.TabIndex = 0;
            // 
            // OriginalDataCalculatedPage
            // 
            this.OriginalDataCalculatedPage.Controls.Add(this.OriginalDataTable);
            this.OriginalDataCalculatedPage.Location = new System.Drawing.Point(4, 22);
            this.OriginalDataCalculatedPage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OriginalDataCalculatedPage.Name = "OriginalDataCalculatedPage";
            this.OriginalDataCalculatedPage.Size = new System.Drawing.Size(1027, 360);
            this.OriginalDataCalculatedPage.TabIndex = 2;
            this.OriginalDataCalculatedPage.Text = "Q4-d-計算後原始資料";
            this.OriginalDataCalculatedPage.UseVisualStyleBackColor = true;
            // 
            // OriginalDataTable
            // 
            this.OriginalDataTable.AllowUserToAddRows = false;
            this.OriginalDataTable.AllowUserToDeleteRows = false;
            this.OriginalDataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OriginalDataTable.Location = new System.Drawing.Point(3, 3);
            this.OriginalDataTable.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OriginalDataTable.Name = "OriginalDataTable";
            this.OriginalDataTable.ReadOnly = true;
            this.OriginalDataTable.RowHeadersWidth = 62;
            this.OriginalDataTable.RowTemplate.Height = 31;
            this.OriginalDataTable.Size = new System.Drawing.Size(1025, 360);
            this.OriginalDataTable.TabIndex = 0;
            // 
            // DataNum
            // 
            this.DataNum.BackColor = System.Drawing.SystemColors.Control;
            this.DataNum.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DataNum.Location = new System.Drawing.Point(11, 517);
            this.DataNum.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DataNum.Name = "DataNum";
            this.DataNum.ReadOnly = true;
            this.DataNum.Size = new System.Drawing.Size(67, 15);
            this.DataNum.TabIndex = 0;
            this.DataNum.Text = "0筆";
            // 
            // LINQTraning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1053, 539);
            this.Controls.Add(this.DataNum);
            this.Controls.Add(this.DataTabControl);
            this.Controls.Add(this.HeaderTabControl);
            this.Controls.Add(this.DisplayTime);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "LINQTraning";
            this.Text = "LINQ訓練";
            this.HeaderTabControl.ResumeLayout(false);
            this.IndustryDropDownPage.ResumeLayout(false);
            this.IndustryDropDownPage.PerformLayout();
            this.DayCheckChangePage.ResumeLayout(false);
            this.DayCheckChangePage.PerformLayout();
            this.DayPERPage.ResumeLayout(false);
            this.DayPERPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TargetPERMenu)).EndInit();
            this.ElectionStockPerformancePage.ResumeLayout(false);
            this.DataTabControl.ResumeLayout(false);
            this.CommonPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CommonTable)).EndInit();
            this.PayMaxMinPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RiseFallTop5Table)).EndInit();
            this.OriginalDataCalculatedPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OriginalDataTable)).EndInit();
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
        private System.Windows.Forms.ComboBox PERIndustryMenu;
        private System.Windows.Forms.TextBox CycleMenu;
        private System.Windows.Forms.TextBox CycleDay;
        private System.Windows.Forms.Button DayButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox StockIDNameMenu;
        private System.Windows.Forms.TextBox CycleDayMenu;
        private System.Windows.Forms.Button RiseFallTop5Button;
        private System.Windows.Forms.Button Top5VolumeButton;
        private System.Windows.Forms.Button SameRankingButton;
        private System.Windows.Forms.Button RisingButton;
        private System.Windows.Forms.DataGridView RiseFallTop5Table;
        private System.Windows.Forms.DataGridView OriginalDataTable;
    }
}

