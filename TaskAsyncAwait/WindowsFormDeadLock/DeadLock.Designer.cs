namespace WindowsFormDeadLock
{
    partial class DeadLock
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
            this.Dead1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Dead2 = new System.Windows.Forms.Button();
            this.Dead3 = new System.Windows.Forms.Button();
            this.Dead4 = new System.Windows.Forms.Button();
            this.Dead5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Dead1
            // 
            this.Dead1.Location = new System.Drawing.Point(613, 61);
            this.Dead1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Dead1.Name = "Dead1";
            this.Dead1.Size = new System.Drawing.Size(178, 78);
            this.Dead1.TabIndex = 0;
            this.Dead1.Text = "Dead1";
            this.Dead1.UseVisualStyleBackColor = true;
            this.Dead1.Click += new System.EventHandler(this.Dead1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(99, 88);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(370, 380);
            this.textBox1.TabIndex = 1;
            // 
            // Dead2
            // 
            this.Dead2.Location = new System.Drawing.Point(613, 174);
            this.Dead2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Dead2.Name = "Dead2";
            this.Dead2.Size = new System.Drawing.Size(178, 78);
            this.Dead2.TabIndex = 2;
            this.Dead2.Text = "Dead2";
            this.Dead2.UseVisualStyleBackColor = true;
            this.Dead2.Click += new System.EventHandler(this.Dead2_Click_1);
            // 
            // Dead3
            // 
            this.Dead3.Location = new System.Drawing.Point(613, 273);
            this.Dead3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Dead3.Name = "Dead3";
            this.Dead3.Size = new System.Drawing.Size(178, 78);
            this.Dead3.TabIndex = 3;
            this.Dead3.Text = "Dead3";
            this.Dead3.UseVisualStyleBackColor = true;
            this.Dead3.Click += new System.EventHandler(this.Dead3_Click);
            // 
            // Dead4
            // 
            this.Dead4.Location = new System.Drawing.Point(613, 374);
            this.Dead4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Dead4.Name = "Dead4";
            this.Dead4.Size = new System.Drawing.Size(178, 78);
            this.Dead4.TabIndex = 4;
            this.Dead4.Text = "Dead4";
            this.Dead4.UseVisualStyleBackColor = true;
            this.Dead4.Click += new System.EventHandler(this.Dead4_Click);
            // 
            // Dead5
            // 
            this.Dead5.Location = new System.Drawing.Point(613, 481);
            this.Dead5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Dead5.Name = "Dead5";
            this.Dead5.Size = new System.Drawing.Size(178, 78);
            this.Dead5.TabIndex = 5;
            this.Dead5.Text = "Dead5";
            this.Dead5.UseVisualStyleBackColor = true;
            this.Dead5.Click += new System.EventHandler(this.Dead5_Click);
            // 
            // DeadLock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 610);
            this.Controls.Add(this.Dead5);
            this.Controls.Add(this.Dead4);
            this.Controls.Add(this.Dead3);
            this.Controls.Add(this.Dead2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Dead1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "DeadLock";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Dead1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button Dead2;
        private System.Windows.Forms.Button Dead3;
        private System.Windows.Forms.Button Dead4;
        private System.Windows.Forms.Button Dead5;
    }
}

