namespace TestService
{
    partial class CoreAcctCheckForm
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
            this.groupBoxCommon = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxOrgNO = new System.Windows.Forms.TextBox();
            this.textBoxTellerNO = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonRcrdDB = new System.Windows.Forms.Button();
            this.buttonQuerySync = new System.Windows.Forms.Button();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.textBoxQueryOrg = new System.Windows.Forms.TextBox();
            this.textBoxBizFlowNO = new System.Windows.Forms.TextBox();
            this.textBoxQueryDate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimeCore = new System.Windows.Forms.DateTimePicker();
            this.groupBoxCommon.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxCommon
            // 
            this.groupBoxCommon.Controls.Add(this.dateTimeCore);
            this.groupBoxCommon.Controls.Add(this.label2);
            this.groupBoxCommon.Controls.Add(this.textBoxOrgNO);
            this.groupBoxCommon.Controls.Add(this.textBoxTellerNO);
            this.groupBoxCommon.Controls.Add(this.label3);
            this.groupBoxCommon.Location = new System.Drawing.Point(12, 12);
            this.groupBoxCommon.Name = "groupBoxCommon";
            this.groupBoxCommon.Size = new System.Drawing.Size(932, 59);
            this.groupBoxCommon.TabIndex = 15;
            this.groupBoxCommon.TabStop = false;
            this.groupBoxCommon.Text = "Common";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "机构号";
            // 
            // textBoxOrgNO
            // 
            this.textBoxOrgNO.Location = new System.Drawing.Point(58, 25);
            this.textBoxOrgNO.MaxLength = 6;
            this.textBoxOrgNO.Name = "textBoxOrgNO";
            this.textBoxOrgNO.Size = new System.Drawing.Size(100, 21);
            this.textBoxOrgNO.TabIndex = 0;
            this.textBoxOrgNO.Text = "802000";
            // 
            // textBoxTellerNO
            // 
            this.textBoxTellerNO.Location = new System.Drawing.Point(210, 25);
            this.textBoxTellerNO.MaxLength = 7;
            this.textBoxTellerNO.Name = "textBoxTellerNO";
            this.textBoxTellerNO.Size = new System.Drawing.Size(100, 21);
            this.textBoxTellerNO.TabIndex = 1;
            this.textBoxTellerNO.Text = "8021121";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(160, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "柜员号";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonRcrdDB);
            this.groupBox1.Controls.Add(this.buttonQuerySync);
            this.groupBox1.Controls.Add(this.textBoxResult);
            this.groupBox1.Controls.Add(this.buttonQuery);
            this.groupBox1.Controls.Add(this.textBoxQueryOrg);
            this.groupBox1.Controls.Add(this.textBoxBizFlowNO);
            this.groupBox1.Controls.Add(this.textBoxQueryDate);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(932, 291);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "核心对账信息查询";
            // 
            // buttonRcrdDB
            // 
            this.buttonRcrdDB.Location = new System.Drawing.Point(794, 20);
            this.buttonRcrdDB.Name = "buttonRcrdDB";
            this.buttonRcrdDB.Size = new System.Drawing.Size(132, 23);
            this.buttonRcrdDB.TabIndex = 9;
            this.buttonRcrdDB.Text = "查询并记录到数据库";
            this.buttonRcrdDB.UseVisualStyleBackColor = true;
            this.buttonRcrdDB.Click += new System.EventHandler(this.buttonRcrdDB_Click);
            // 
            // buttonQuerySync
            // 
            this.buttonQuerySync.Location = new System.Drawing.Point(713, 20);
            this.buttonQuerySync.Name = "buttonQuerySync";
            this.buttonQuerySync.Size = new System.Drawing.Size(75, 23);
            this.buttonQuerySync.TabIndex = 8;
            this.buttonQuerySync.Text = "同步查询";
            this.buttonQuerySync.UseVisualStyleBackColor = true;
            this.buttonQuerySync.Click += new System.EventHandler(this.buttonQuerySync_Click);
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(6, 47);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.Size = new System.Drawing.Size(920, 238);
            this.textBoxResult.TabIndex = 7;
            // 
            // buttonQuery
            // 
            this.buttonQuery.Location = new System.Drawing.Point(597, 21);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(110, 23);
            this.buttonQuery.TabIndex = 6;
            this.buttonQuery.Text = "查询并记录到文件";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.buttonQuery_Click);
            // 
            // textBoxQueryOrg
            // 
            this.textBoxQueryOrg.Location = new System.Drawing.Point(491, 23);
            this.textBoxQueryOrg.MaxLength = 6;
            this.textBoxQueryOrg.Name = "textBoxQueryOrg";
            this.textBoxQueryOrg.Size = new System.Drawing.Size(100, 21);
            this.textBoxQueryOrg.TabIndex = 5;
            // 
            // textBoxBizFlowNO
            // 
            this.textBoxBizFlowNO.Location = new System.Drawing.Point(279, 23);
            this.textBoxBizFlowNO.MaxLength = 32;
            this.textBoxBizFlowNO.Name = "textBoxBizFlowNO";
            this.textBoxBizFlowNO.Size = new System.Drawing.Size(147, 21);
            this.textBoxBizFlowNO.TabIndex = 4;
            // 
            // textBoxQueryDate
            // 
            this.textBoxQueryDate.Location = new System.Drawing.Point(67, 23);
            this.textBoxQueryDate.MaxLength = 10;
            this.textBoxQueryDate.Name = "textBoxQueryDate";
            this.textBoxQueryDate.Size = new System.Drawing.Size(100, 21);
            this.textBoxQueryDate.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(432, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "查询机构";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(184, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "查询资金流水号";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "查询日期";
            // 
            // dateTimeCore
            // 
            this.dateTimeCore.Location = new System.Drawing.Point(350, 24);
            this.dateTimeCore.Name = "dateTimeCore";
            this.dateTimeCore.Size = new System.Drawing.Size(200, 21);
            this.dateTimeCore.TabIndex = 4;
            // 
            // CoreAcctCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 396);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxCommon);
            this.Name = "CoreAcctCheckForm";
            this.Text = "CoreAcctCheckForm";
            this.groupBoxCommon.ResumeLayout(false);
            this.groupBoxCommon.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxCommon;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxOrgNO;
        private System.Windows.Forms.TextBox textBoxTellerNO;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Button buttonQuery;
        private System.Windows.Forms.TextBox textBoxQueryOrg;
        private System.Windows.Forms.TextBox textBoxBizFlowNO;
        private System.Windows.Forms.TextBox textBoxQueryDate;
        private System.Windows.Forms.Button buttonQuerySync;
        private System.Windows.Forms.Button buttonRcrdDB;
        private System.Windows.Forms.DateTimePicker dateTimeCore;
    }
}