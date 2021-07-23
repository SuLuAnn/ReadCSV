
using System.ComponentModel;

namespace Readcsv2020LuAnn
{
    partial class ReadCSV
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
            this.ReadButton = new System.Windows.Forms.Button();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.Prompt = new System.Windows.Forms.TextBox();
            this.StockList = new System.Windows.Forms.DataGridView();
            this.TextDisplay = new System.Windows.Forms.TextBox();
            this.DropMenu = new System.Windows.Forms.ComboBox();
            this.StockCheck = new System.Windows.Forms.Button();
            this.BuySellTop50 = new System.Windows.Forms.Button();
            this.ReadData = new System.ComponentModel.BackgroundWorker();
            this.TotalList = new System.Windows.Forms.DataGridView();
            this.Top50List = new System.Windows.Forms.DataGridView();
            this.TotalData = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.StockList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TotalList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Top50List)).BeginInit();
            this.SuspendLayout();
            // 
            // ReadButton
            // 
            this.ReadButton.Location = new System.Drawing.Point(763, 10);
            this.ReadButton.Name = "ReadButton";
            this.ReadButton.Size = new System.Drawing.Size(108, 30);
            this.ReadButton.TabIndex = 0;
            this.ReadButton.Text = "讀取檔案";
            this.ReadButton.UseVisualStyleBackColor = true;
            this.ReadButton.Click += new System.EventHandler(this.ClickReadButton);
            // 
            // InputBox
            // 
            this.InputBox.Location = new System.Drawing.Point(15, 10);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(742, 29);
            this.InputBox.TabIndex = 1;
            // 
            // Prompt
            // 
            this.Prompt.BackColor = System.Drawing.Color.PaleTurquoise;
            this.Prompt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Prompt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Prompt.Location = new System.Drawing.Point(894, 18);
            this.Prompt.Name = "Prompt";
            this.Prompt.ReadOnly = true;
            this.Prompt.Size = new System.Drawing.Size(79, 22);
            this.Prompt.TabIndex = 2;
            this.Prompt.Text = "讀取狀態";
            // 
            // StockList
            // 
            this.StockList.AllowUserToAddRows = false;
            this.StockList.AllowUserToDeleteRows = false;
            this.StockList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StockList.Location = new System.Drawing.Point(15, 100);
            this.StockList.Name = "StockList";
            this.StockList.ReadOnly = true;
            this.StockList.RowHeadersWidth = 62;
            this.StockList.RowTemplate.Height = 31;
            this.StockList.Size = new System.Drawing.Size(1000, 340);
            this.StockList.TabIndex = 0;
            // 
            // TextDisplay
            // 
            this.TextDisplay.Location = new System.Drawing.Point(1024, 13);
            this.TextDisplay.Multiline = true;
            this.TextDisplay.Name = "TextDisplay";
            this.TextDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextDisplay.Size = new System.Drawing.Size(420, 80);
            this.TextDisplay.TabIndex = 5;
            this.TextDisplay.TextChanged += new System.EventHandler(this.ChangTextDisplay);
            // 
            // DropMenu
            // 
            this.DropMenu.FormattingEnabled = true;
            this.DropMenu.Location = new System.Drawing.Point(15, 56);
            this.DropMenu.Name = "DropMenu";
            this.DropMenu.Size = new System.Drawing.Size(742, 26);
            this.DropMenu.TabIndex = 6;
            // 
            // StockCheck
            // 
            this.StockCheck.Location = new System.Drawing.Point(763, 53);
            this.StockCheck.Name = "StockCheck";
            this.StockCheck.Size = new System.Drawing.Size(108, 30);
            this.StockCheck.TabIndex = 7;
            this.StockCheck.Text = "股票查詢";
            this.StockCheck.UseVisualStyleBackColor = true;
            this.StockCheck.Click += new System.EventHandler(this.ClickCheckStock);
            // 
            // BuySellTop50
            // 
            this.BuySellTop50.Location = new System.Drawing.Point(877, 52);
            this.BuySellTop50.Name = "BuySellTop50";
            this.BuySellTop50.Size = new System.Drawing.Size(122, 30);
            this.BuySellTop50.TabIndex = 8;
            this.BuySellTop50.Text = "買賣超Top50";
            this.BuySellTop50.UseVisualStyleBackColor = true;
            this.BuySellTop50.Click += new System.EventHandler(this.ClickTop50Button);
            // 
            // ReadData
            // 
            this.ReadData.WorkerReportsProgress = true;
            this.ReadData.WorkerSupportsCancellation = true;
            this.ReadData.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ReadCSVData);
            this.ReadData.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.RunWorkerCompleted);
            // 
            // TotalList
            // 
            this.TotalList.AllowUserToAddRows = false;
            this.TotalList.AllowUserToDeleteRows = false;
            this.TotalList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TotalList.Location = new System.Drawing.Point(15, 455);
            this.TotalList.Name = "TotalList";
            this.TotalList.ReadOnly = true;
            this.TotalList.RowHeadersWidth = 62;
            this.TotalList.RowTemplate.Height = 31;
            this.TotalList.Size = new System.Drawing.Size(1000, 340);
            this.TotalList.TabIndex = 9;
            // 
            // Top50List
            // 
            this.Top50List.AllowUserToAddRows = false;
            this.Top50List.AllowUserToDeleteRows = false;
            this.Top50List.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Top50List.Location = new System.Drawing.Point(1024, 100);
            this.Top50List.Name = "Top50List";
            this.Top50List.ReadOnly = true;
            this.Top50List.RowHeadersWidth = 62;
            this.Top50List.RowTemplate.Height = 31;
            this.Top50List.Size = new System.Drawing.Size(420, 695);
            this.Top50List.TabIndex = 10;
            // 
            // TotalData
            // 
            this.TotalData.DoWork += new System.ComponentModel.DoWorkEventHandler(this.WriteTotalData);
            this.TotalData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.DisplayTime);
            // 
            // ReadCSV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.PaleTurquoise;
            this.ClientSize = new System.Drawing.Size(1462, 809);
            this.Controls.Add(this.Top50List);
            this.Controls.Add(this.TotalList);
            this.Controls.Add(this.BuySellTop50);
            this.Controls.Add(this.StockCheck);
            this.Controls.Add(this.DropMenu);
            this.Controls.Add(this.TextDisplay);
            this.Controls.Add(this.Prompt);
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.ReadButton);
            this.Controls.Add(this.StockList);
            this.Name = "ReadCSV";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 10, 10);
            this.Text = "ReadCSV";
            ((System.ComponentModel.ISupportInitialize)(this.StockList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TotalList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Top50List)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ReadButton;
        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.TextBox Prompt;
        private System.Windows.Forms.TextBox TextDisplay;
        private System.Windows.Forms.ComboBox DropMenu;
        private System.Windows.Forms.Button StockCheck;
        private System.Windows.Forms.Button BuySellTop50;
        private System.ComponentModel.BackgroundWorker ReadData;
        private System.Windows.Forms.DataGridView StockList;
        private System.Windows.Forms.DataGridView TotalList;
        private System.Windows.Forms.DataGridView Top50List;
        private BackgroundWorker TotalData;
    }
}

