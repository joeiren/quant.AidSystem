namespace TestService
{
    partial class PaymentCheckAcctForm
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
            this.labelQueryDate = new System.Windows.Forms.Label();
            this.textBoxQueryDate = new System.Windows.Forms.TextBox();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.buttonSync = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelQueryDate
            // 
            this.labelQueryDate.AutoSize = true;
            this.labelQueryDate.Location = new System.Drawing.Point(27, 15);
            this.labelQueryDate.Name = "labelQueryDate";
            this.labelQueryDate.Size = new System.Drawing.Size(53, 12);
            this.labelQueryDate.TabIndex = 0;
            this.labelQueryDate.Text = "请求日期";
            // 
            // textBoxQueryDate
            // 
            this.textBoxQueryDate.Location = new System.Drawing.Point(86, 6);
            this.textBoxQueryDate.MaxLength = 8;
            this.textBoxQueryDate.Name = "textBoxQueryDate";
            this.textBoxQueryDate.Size = new System.Drawing.Size(100, 21);
            this.textBoxQueryDate.TabIndex = 1;
            // 
            // textBoxResult
            // 
            this.textBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxResult.Location = new System.Drawing.Point(12, 46);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxResult.Size = new System.Drawing.Size(809, 333);
            this.textBoxResult.TabIndex = 2;
            // 
            // buttonQuery
            // 
            this.buttonQuery.Location = new System.Drawing.Point(204, 3);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(75, 23);
            this.buttonQuery.TabIndex = 3;
            this.buttonQuery.Text = "查询";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.buttonQuery_Click);
            // 
            // buttonSync
            // 
            this.buttonSync.Location = new System.Drawing.Point(286, 2);
            this.buttonSync.Name = "buttonSync";
            this.buttonSync.Size = new System.Drawing.Size(75, 23);
            this.buttonSync.TabIndex = 4;
            this.buttonSync.Text = "同步调用";
            this.buttonSync.UseVisualStyleBackColor = true;
            this.buttonSync.Click += new System.EventHandler(this.buttonSync_Click);
            // 
            // PaymentCheckAcctForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 391);
            this.Controls.Add(this.buttonSync);
            this.Controls.Add(this.buttonQuery);
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.textBoxQueryDate);
            this.Controls.Add(this.labelQueryDate);
            this.Name = "PaymentCheckAcctForm";
            this.Text = "PaymentCheckAcctForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelQueryDate;
        private System.Windows.Forms.TextBox textBoxQueryDate;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Button buttonQuery;
        private System.Windows.Forms.Button buttonSync;
    }
}