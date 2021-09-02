namespace DataTraning2
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
            this.TimeText = new System.Windows.Forms.TextBox();
            this.AddReviseButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.DropDownMenu = new System.Windows.Forms.ComboBox();
            this.SQLConnection = new System.Data.SqlClient.SqlConnection();
            this.DeleteWorker = new System.ComponentModel.BackgroundWorker();
            this.AddReviseWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // TimeText
            // 
            this.TimeText.Location = new System.Drawing.Point(286, 80);
            this.TimeText.Multiline = true;
            this.TimeText.Name = "TimeText";
            this.TimeText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TimeText.Size = new System.Drawing.Size(263, 101);
            this.TimeText.TabIndex = 0;
            // 
            // AddReviseButton
            // 
            this.AddReviseButton.Location = new System.Drawing.Point(306, 22);
            this.AddReviseButton.Name = "AddReviseButton";
            this.AddReviseButton.Size = new System.Drawing.Size(123, 36);
            this.AddReviseButton.TabIndex = 1;
            this.AddReviseButton.Text = "新增、修改";
            this.AddReviseButton.UseVisualStyleBackColor = true;
            this.AddReviseButton.Click += new System.EventHandler(this.ClickAddReviseButton);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(435, 21);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(114, 37);
            this.DeleteButton.TabIndex = 2;
            this.DeleteButton.Text = "刪除";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.ClickDeleteButton);
            // 
            // DropDownMenu
            // 
            this.DropDownMenu.FormattingEnabled = true;
            this.DropDownMenu.Location = new System.Drawing.Point(12, 31);
            this.DropDownMenu.Name = "DropDownMenu";
            this.DropDownMenu.Size = new System.Drawing.Size(273, 20);
            this.DropDownMenu.TabIndex = 3;
            this.DropDownMenu.SelectedIndexChanged += new System.EventHandler(this.ChangedDropDownMenu);
            // 
            // SQLConnection
            // 
            this.SQLConnection.ConnectionString = "Data Source=192.168.10.180;Initial Catalog=StockDB;User ID=test;Password=test";
            this.SQLConnection.FireInfoMessageEventOnUserErrors = false;
            // 
            // DeleteWorker
            // 
            this.DeleteWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.WorkDelete);
            this.DeleteWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Completed);
            // 
            // AddReviseWorker
            // 
            this.AddReviseWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.WorkAddRevise);
            this.AddReviseWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Completed);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 191);
            this.Controls.Add(this.DropDownMenu);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.AddReviseButton);
            this.Controls.Add(this.TimeText);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TimeText;
        private System.Windows.Forms.Button AddReviseButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.ComboBox DropDownMenu;
        private System.Data.SqlClient.SqlConnection SQLConnection;
        private System.ComponentModel.BackgroundWorker DeleteWorker;
        private System.ComponentModel.BackgroundWorker AddReviseWorker;
    }
}

