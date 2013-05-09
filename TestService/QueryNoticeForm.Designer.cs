namespace TestService
{
    partial class QueryNoticeForm
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
            this.柜员号 = new System.Windows.Forms.TextBox();
            this.机构号 = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.业务交易日 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.业务类型 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.通知单编号 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.会计日期 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.btnQuery = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.通知单类型 = new System.Windows.Forms.TextBox();
            this.btnQueryOfCustomer = new System.Windows.Forms.Button();
            this.textResult = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(16, 69);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(41, 12);
            this.label20.TabIndex = 60;
            this.label20.Text = "柜员号";
            // 
            // 柜员号
            // 
            this.柜员号.Location = new System.Drawing.Point(83, 66);
            this.柜员号.Name = "柜员号";
            this.柜员号.Size = new System.Drawing.Size(100, 21);
            this.柜员号.TabIndex = 59;
            this.柜员号.Text = "8010130";
            // 
            // 机构号
            // 
            this.机构号.Location = new System.Drawing.Point(357, 17);
            this.机构号.Name = "机构号";
            this.机构号.Size = new System.Drawing.Size(100, 21);
            this.机构号.TabIndex = 58;
            this.机构号.Text = "801000";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(310, 20);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(41, 12);
            this.label19.TabIndex = 57;
            this.label19.Text = "机构号";
            // 
            // 业务交易日
            // 
            this.业务交易日.Location = new System.Drawing.Point(83, 16);
            this.业务交易日.Name = "业务交易日";
            this.业务交易日.Size = new System.Drawing.Size(200, 21);
            this.业务交易日.TabIndex = 56;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 55;
            this.label1.Text = "业务交易日";
            // 
            // 业务类型
            // 
            this.业务类型.Location = new System.Drawing.Point(511, 115);
            this.业务类型.Name = "业务类型";
            this.业务类型.Size = new System.Drawing.Size(100, 21);
            this.业务类型.TabIndex = 54;
            this.业务类型.Text = "1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(302, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(203, 12);
            this.label4.TabIndex = 53;
            this.label4.Text = "业务类型（1-同业活期 2-同业定期）";
            // 
            // 通知单编号
            // 
            this.通知单编号.Location = new System.Drawing.Point(83, 115);
            this.通知单编号.Name = "通知单编号";
            this.通知单编号.Size = new System.Drawing.Size(142, 21);
            this.通知单编号.TabIndex = 52;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 51;
            this.label3.Text = "通知单编号";
            // 
            // 会计日期
            // 
            this.会计日期.Location = new System.Drawing.Point(357, 69);
            this.会计日期.Name = "会计日期";
            this.会计日期.Size = new System.Drawing.Size(200, 21);
            this.会计日期.TabIndex = 50;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(298, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 49;
            this.label2.Text = "会计日期";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(342, 157);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 61;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(12, 157);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(197, 12);
            this.label21.TabIndex = 95;
            this.label21.Text = "通知单类型(1-开户 2-销户 3-部提)";
            // 
            // 通知单类型
            // 
            this.通知单类型.Location = new System.Drawing.Point(215, 154);
            this.通知单类型.Name = "通知单类型";
            this.通知单类型.Size = new System.Drawing.Size(100, 21);
            this.通知单类型.TabIndex = 94;
            this.通知单类型.Text = "1";
            // 
            // btnQueryOfCustomer
            // 
            this.btnQueryOfCustomer.Location = new System.Drawing.Point(437, 157);
            this.btnQueryOfCustomer.Name = "btnQueryOfCustomer";
            this.btnQueryOfCustomer.Size = new System.Drawing.Size(120, 23);
            this.btnQueryOfCustomer.TabIndex = 96;
            this.btnQueryOfCustomer.Text = "QueryOfCustomer";
            this.btnQueryOfCustomer.UseVisualStyleBackColor = true;
            this.btnQueryOfCustomer.Click += new System.EventHandler(this.btnQueryOfCustomer_Click);
            // 
            // textResult
            // 
            this.textResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textResult.Location = new System.Drawing.Point(18, 192);
            this.textResult.Multiline = true;
            this.textResult.Name = "textResult";
            this.textResult.Size = new System.Drawing.Size(777, 305);
            this.textResult.TabIndex = 97;
            // 
            // QueryNoticeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 509);
            this.Controls.Add(this.textResult);
            this.Controls.Add(this.btnQueryOfCustomer);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.通知单类型);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.柜员号);
            this.Controls.Add(this.机构号);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.业务交易日);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.业务类型);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.通知单编号);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.会计日期);
            this.Controls.Add(this.label2);
            this.Name = "QueryNoticeForm";
            this.Text = "QueryNoticeForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox 柜员号;
        private System.Windows.Forms.TextBox 机构号;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.DateTimePicker 业务交易日;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox 业务类型;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox 通知单编号;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker 会计日期;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox 通知单类型;
        private System.Windows.Forms.Button btnQueryOfCustomer;
        private System.Windows.Forms.TextBox textResult;
    }
}