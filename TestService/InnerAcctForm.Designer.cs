namespace TestService
{
    partial class InnerAcctForm
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
            this.txtOrgNO = new System.Windows.Forms.TextBox();
            this.txtCurrency = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCheckCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtInnerAcctSN = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonGo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "机构号";
            // 
            // txtOrgNO
            // 
            this.txtOrgNO.Location = new System.Drawing.Point(95, 21);
            this.txtOrgNO.Name = "txtOrgNO";
            this.txtOrgNO.Size = new System.Drawing.Size(100, 21);
            this.txtOrgNO.TabIndex = 1;
            // 
            // txtCurrency
            // 
            this.txtCurrency.Location = new System.Drawing.Point(95, 48);
            this.txtCurrency.Name = "txtCurrency";
            this.txtCurrency.Size = new System.Drawing.Size(100, 21);
            this.txtCurrency.TabIndex = 3;
            this.txtCurrency.Text = "01";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "币种";
            // 
            // txtCheckCode
            // 
            this.txtCheckCode.Location = new System.Drawing.Point(95, 75);
            this.txtCheckCode.Name = "txtCheckCode";
            this.txtCheckCode.Size = new System.Drawing.Size(100, 21);
            this.txtCheckCode.TabIndex = 5;
            this.txtCheckCode.Text = "262120";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "核算码";
            // 
            // txtInnerAcctSN
            // 
            this.txtInnerAcctSN.Location = new System.Drawing.Point(95, 102);
            this.txtInnerAcctSN.Name = "txtInnerAcctSN";
            this.txtInnerAcctSN.Size = new System.Drawing.Size(100, 21);
            this.txtInnerAcctSN.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "内部帐顺序号";
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(95, 154);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(100, 21);
            this.txtResult.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(36, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "生成账号";
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(232, 133);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(75, 23);
            this.buttonGo.TabIndex = 10;
            this.buttonGo.Text = "go";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // InnerAcctForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 266);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtInnerAcctSN);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCheckCode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtCurrency);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtOrgNO);
            this.Controls.Add(this.label1);
            this.Name = "InnerAcctForm";
            this.Text = "InnerAcctForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOrgNO;
        private System.Windows.Forms.TextBox txtCurrency;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCheckCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtInnerAcctSN;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonGo;
    }
}