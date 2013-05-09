namespace TestService
{
    partial class InterBankRetrieveBalance
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label20 = new System.Windows.Forms.Label();
            this.textBoxTeller = new System.Windows.Forms.TextBox();
            this.textBoxOrg = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.textBoxTradeDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxAccountNO = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(479, 16);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(41, 12);
            this.label20.TabIndex = 54;
            this.label20.Text = "柜员号";
            // 
            // textBoxTeller
            // 
            this.textBoxTeller.Location = new System.Drawing.Point(546, 13);
            this.textBoxTeller.Name = "textBoxTeller";
            this.textBoxTeller.Size = new System.Drawing.Size(100, 21);
            this.textBoxTeller.TabIndex = 53;
            this.textBoxTeller.Text = "8010130";
            // 
            // textBoxOrg
            // 
            this.textBoxOrg.Location = new System.Drawing.Point(365, 13);
            this.textBoxOrg.Name = "textBoxOrg";
            this.textBoxOrg.Size = new System.Drawing.Size(100, 21);
            this.textBoxOrg.TabIndex = 52;
            this.textBoxOrg.Text = "801180";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(318, 16);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(41, 12);
            this.label19.TabIndex = 51;
            this.label19.Text = "机构号";
            // 
            // textBoxTradeDate
            // 
            this.textBoxTradeDate.Location = new System.Drawing.Point(91, 12);
            this.textBoxTradeDate.Name = "textBoxTradeDate";
            this.textBoxTradeDate.Size = new System.Drawing.Size(200, 21);
            this.textBoxTradeDate.TabIndex = 50;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 49;
            this.label1.Text = "业务交易日";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 56;
            this.label2.Text = "账号";
            // 
            // textBoxAccountNO
            // 
            this.textBoxAccountNO.Location = new System.Drawing.Point(91, 39);
            this.textBoxAccountNO.MaxLength = 22;
            this.textBoxAccountNO.Name = "textBoxAccountNO";
            this.textBoxAccountNO.Size = new System.Drawing.Size(131, 21);
            this.textBoxAccountNO.TabIndex = 55;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 57;
            this.label3.Text = "结果";
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(91, 79);
            this.textBoxResult.MaxLength = 22;
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.Size = new System.Drawing.Size(713, 165);
            this.textBoxResult.TabIndex = 58;
            // 
            // buttonQuery
            // 
            this.buttonQuery.Location = new System.Drawing.Point(729, 50);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(75, 23);
            this.buttonQuery.TabIndex = 59;
            this.buttonQuery.Text = "查询";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.buttonQuery_Click);
            // 
            // InterBankRetrieveBalance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 266);
            this.Controls.Add(this.buttonQuery);
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxAccountNO);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.textBoxTeller);
            this.Controls.Add(this.textBoxOrg);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.textBoxTradeDate);
            this.Controls.Add(this.label1);
            this.Name = "InterBankRetrieveBalance";
            this.Text = "同业存放活期余额查询";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox textBoxTeller;
        private System.Windows.Forms.TextBox textBoxOrg;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.DateTimePicker textBoxTradeDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxAccountNO;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Button buttonQuery;
    }
}