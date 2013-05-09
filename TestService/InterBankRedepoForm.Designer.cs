namespace TestService
{
    partial class InterBankRedepoForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.机构号 = new System.Windows.Forms.TextBox();
            this.柜员号 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.账号 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.交易日 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.新起息日 = new System.Windows.Forms.DateTimePicker();
            this.新到期日 = new System.Windows.Forms.DateTimePicker();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.buttonRedepoEx = new System.Windows.Forms.Button();
            this.buttonRedepo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "机构号";
            // 
            // 机构号
            // 
            this.机构号.Location = new System.Drawing.Point(60, 18);
            this.机构号.MaxLength = 6;
            this.机构号.Name = "机构号";
            this.机构号.Size = new System.Drawing.Size(100, 21);
            this.机构号.TabIndex = 1;
            // 
            // 柜员号
            // 
            this.柜员号.Location = new System.Drawing.Point(222, 18);
            this.柜员号.MaxLength = 7;
            this.柜员号.Name = "柜员号";
            this.柜员号.Size = new System.Drawing.Size(100, 21);
            this.柜员号.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(175, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "柜员号";
            // 
            // 账号
            // 
            this.账号.Location = new System.Drawing.Point(60, 59);
            this.账号.MaxLength = 22;
            this.账号.Name = "账号";
            this.账号.Size = new System.Drawing.Size(100, 21);
            this.账号.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "账号";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(338, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "交易日";
            // 
            // 交易日
            // 
            this.交易日.Checked = false;
            this.交易日.Location = new System.Drawing.Point(385, 18);
            this.交易日.Name = "交易日";
            this.交易日.Size = new System.Drawing.Size(200, 21);
            this.交易日.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(175, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "新起息日";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(450, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "新到期日";
            // 
            // 新起息日
            // 
            this.新起息日.Location = new System.Drawing.Point(234, 59);
            this.新起息日.Name = "新起息日";
            this.新起息日.Size = new System.Drawing.Size(200, 21);
            this.新起息日.TabIndex = 12;
            // 
            // 新到期日
            // 
            this.新到期日.Location = new System.Drawing.Point(509, 59);
            this.新到期日.Name = "新到期日";
            this.新到期日.Size = new System.Drawing.Size(200, 21);
            this.新到期日.TabIndex = 13;
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(12, 121);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.Size = new System.Drawing.Size(724, 172);
            this.textBoxResult.TabIndex = 14;
            // 
            // buttonRedepoEx
            // 
            this.buttonRedepoEx.Location = new System.Drawing.Point(27, 92);
            this.buttonRedepoEx.Name = "buttonRedepoEx";
            this.buttonRedepoEx.Size = new System.Drawing.Size(97, 23);
            this.buttonRedepoEx.TabIndex = 15;
            this.buttonRedepoEx.Text = "自动转存（MQ）";
            this.buttonRedepoEx.UseVisualStyleBackColor = true;
            this.buttonRedepoEx.Click += new System.EventHandler(this.buttonRedepoEx_Click);
            // 
            // buttonRedepo
            // 
            this.buttonRedepo.Location = new System.Drawing.Point(141, 92);
            this.buttonRedepo.Name = "buttonRedepo";
            this.buttonRedepo.Size = new System.Drawing.Size(75, 23);
            this.buttonRedepo.TabIndex = 16;
            this.buttonRedepo.Text = "自动转存";
            this.buttonRedepo.UseVisualStyleBackColor = true;
            this.buttonRedepo.Click += new System.EventHandler(this.buttonRedepo_Click);
            // 
            // InterBankRedepoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 305);
            this.Controls.Add(this.buttonRedepo);
            this.Controls.Add(this.buttonRedepoEx);
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.新到期日);
            this.Controls.Add(this.新起息日);
            this.Controls.Add(this.交易日);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.账号);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.柜员号);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.机构号);
            this.Controls.Add(this.label1);
            this.Name = "InterBankRedepoForm";
            this.Text = "InterBankRedepoForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox 机构号;
        private System.Windows.Forms.TextBox 柜员号;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox 账号;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker 交易日;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker 新起息日;
        private System.Windows.Forms.DateTimePicker 新到期日;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Button buttonRedepoEx;
        private System.Windows.Forms.Button buttonRedepo;
    }
}