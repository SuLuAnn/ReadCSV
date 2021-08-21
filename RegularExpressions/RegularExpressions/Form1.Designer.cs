namespace RegularExpressions
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
            this.IDNumberBox = new System.Windows.Forms.TextBox();
            this.IDNumberDisplay = new System.Windows.Forms.TextBox();
            this.IDButton = new System.Windows.Forms.Button();
            this.PasswordDisplay = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.EnglishNameBox = new System.Windows.Forms.TextBox();
            this.PasswordBox = new System.Windows.Forms.TextBox();
            this.PasswordButton = new System.Windows.Forms.Button();
            this.EnglishNameButton = new System.Windows.Forms.Button();
            this.EnglishNameDisplay = new System.Windows.Forms.TextBox();
            this.H4ReadButton = new System.Windows.Forms.Button();
            this.H4ReadDisplay = new System.Windows.Forms.TextBox();
            this.H5ReadButton = new System.Windows.Forms.Button();
            this.DataView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).BeginInit();
            this.SuspendLayout();
            // 
            // IDNumberBox
            // 
            this.IDNumberBox.Location = new System.Drawing.Point(36, 32);
            this.IDNumberBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.IDNumberBox.Name = "IDNumberBox";
            this.IDNumberBox.Size = new System.Drawing.Size(302, 29);
            this.IDNumberBox.TabIndex = 0;
            // 
            // IDNumberDisplay
            // 
            this.IDNumberDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.IDNumberDisplay.Location = new System.Drawing.Point(36, 74);
            this.IDNumberDisplay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.IDNumberDisplay.Name = "IDNumberDisplay";
            this.IDNumberDisplay.ReadOnly = true;
            this.IDNumberDisplay.Size = new System.Drawing.Size(304, 22);
            this.IDNumberDisplay.TabIndex = 1;
            // 
            // IDButton
            // 
            this.IDButton.Location = new System.Drawing.Point(363, 32);
            this.IDButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.IDButton.Name = "IDButton";
            this.IDButton.Size = new System.Drawing.Size(128, 34);
            this.IDButton.TabIndex = 2;
            this.IDButton.Text = "身分證確認";
            this.IDButton.UseVisualStyleBackColor = true;
            this.IDButton.Click += new System.EventHandler(this.ClickIDButton);
            // 
            // PasswordDisplay
            // 
            this.PasswordDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PasswordDisplay.Location = new System.Drawing.Point(36, 160);
            this.PasswordDisplay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PasswordDisplay.Name = "PasswordDisplay";
            this.PasswordDisplay.ReadOnly = true;
            this.PasswordDisplay.Size = new System.Drawing.Size(304, 22);
            this.PasswordDisplay.TabIndex = 3;
            // 
            // textBox4
            // 
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Location = new System.Drawing.Point(36, 244);
            this.textBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(304, 22);
            this.textBox4.TabIndex = 4;
            // 
            // EnglishNameBox
            // 
            this.EnglishNameBox.Location = new System.Drawing.Point(36, 202);
            this.EnglishNameBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EnglishNameBox.Name = "EnglishNameBox";
            this.EnglishNameBox.Size = new System.Drawing.Size(302, 29);
            this.EnglishNameBox.TabIndex = 5;
            // 
            // PasswordBox
            // 
            this.PasswordBox.Location = new System.Drawing.Point(36, 118);
            this.PasswordBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.Size = new System.Drawing.Size(302, 29);
            this.PasswordBox.TabIndex = 6;
            // 
            // PasswordButton
            // 
            this.PasswordButton.Location = new System.Drawing.Point(363, 116);
            this.PasswordButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PasswordButton.Name = "PasswordButton";
            this.PasswordButton.Size = new System.Drawing.Size(112, 34);
            this.PasswordButton.TabIndex = 8;
            this.PasswordButton.Text = "密碼確認";
            this.PasswordButton.UseVisualStyleBackColor = true;
            this.PasswordButton.Click += new System.EventHandler(this.ClickPassword);
            // 
            // EnglishNameButton
            // 
            this.EnglishNameButton.Location = new System.Drawing.Point(363, 202);
            this.EnglishNameButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EnglishNameButton.Name = "EnglishNameButton";
            this.EnglishNameButton.Size = new System.Drawing.Size(112, 34);
            this.EnglishNameButton.TabIndex = 9;
            this.EnglishNameButton.Text = "取英文姓名";
            this.EnglishNameButton.UseVisualStyleBackColor = true;
            this.EnglishNameButton.Click += new System.EventHandler(this.ClickEnglishNameButton);
            // 
            // EnglishNameDisplay
            // 
            this.EnglishNameDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.EnglishNameDisplay.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.EnglishNameDisplay.Location = new System.Drawing.Point(36, 244);
            this.EnglishNameDisplay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EnglishNameDisplay.Name = "EnglishNameDisplay";
            this.EnglishNameDisplay.ReadOnly = true;
            this.EnglishNameDisplay.Size = new System.Drawing.Size(304, 22);
            this.EnglishNameDisplay.TabIndex = 10;
            // 
            // H4ReadButton
            // 
            this.H4ReadButton.Location = new System.Drawing.Point(1128, 74);
            this.H4ReadButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.H4ReadButton.Name = "H4ReadButton";
            this.H4ReadButton.Size = new System.Drawing.Size(112, 34);
            this.H4ReadButton.TabIndex = 12;
            this.H4ReadButton.Text = "H4讀取";
            this.H4ReadButton.UseVisualStyleBackColor = true;
            this.H4ReadButton.Click += new System.EventHandler(this.ClickH4ReadButton);
            // 
            // H4ReadDisplay
            // 
            this.H4ReadDisplay.Location = new System.Drawing.Point(576, 75);
            this.H4ReadDisplay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.H4ReadDisplay.Multiline = true;
            this.H4ReadDisplay.Name = "H4ReadDisplay";
            this.H4ReadDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.H4ReadDisplay.Size = new System.Drawing.Size(541, 158);
            this.H4ReadDisplay.TabIndex = 13;
            // 
            // H5ReadButton
            // 
            this.H5ReadButton.Location = new System.Drawing.Point(1128, 118);
            this.H5ReadButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.H5ReadButton.Name = "H5ReadButton";
            this.H5ReadButton.Size = new System.Drawing.Size(112, 34);
            this.H5ReadButton.TabIndex = 14;
            this.H5ReadButton.Text = "H5讀取";
            this.H5ReadButton.UseVisualStyleBackColor = true;
            this.H5ReadButton.Click += new System.EventHandler(this.ClickH5ReadButton);
            // 
            // DataView
            // 
            this.DataView.AllowUserToAddRows = false;
            this.DataView.AllowUserToDeleteRows = false;
            this.DataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataView.Location = new System.Drawing.Point(13, 282);
            this.DataView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DataView.Name = "DataView";
            this.DataView.ReadOnly = true;
            this.DataView.RowHeadersWidth = 62;
            this.DataView.RowTemplate.Height = 24;
            this.DataView.Size = new System.Drawing.Size(1239, 476);
            this.DataView.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1265, 771);
            this.Controls.Add(this.DataView);
            this.Controls.Add(this.H5ReadButton);
            this.Controls.Add(this.H4ReadDisplay);
            this.Controls.Add(this.H4ReadButton);
            this.Controls.Add(this.EnglishNameDisplay);
            this.Controls.Add(this.EnglishNameButton);
            this.Controls.Add(this.PasswordButton);
            this.Controls.Add(this.PasswordBox);
            this.Controls.Add(this.EnglishNameBox);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.PasswordDisplay);
            this.Controls.Add(this.IDButton);
            this.Controls.Add(this.IDNumberDisplay);
            this.Controls.Add(this.IDNumberBox);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox IDNumberBox;
        private System.Windows.Forms.TextBox IDNumberDisplay;
        private System.Windows.Forms.Button IDButton;
        private System.Windows.Forms.TextBox PasswordDisplay;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox EnglishNameBox;
        private System.Windows.Forms.TextBox PasswordBox;
        private System.Windows.Forms.Button PasswordButton;
        private System.Windows.Forms.Button EnglishNameButton;
        private System.Windows.Forms.TextBox EnglishNameDisplay;
        private System.Windows.Forms.Button H4ReadButton;
        private System.Windows.Forms.TextBox H4ReadDisplay;
        private System.Windows.Forms.Button H5ReadButton;
        private System.Windows.Forms.DataGridView DataView;
    }
}

