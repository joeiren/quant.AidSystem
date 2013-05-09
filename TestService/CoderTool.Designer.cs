namespace TestService
{
    partial class CoderTool
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
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.buttonDecode = new System.Windows.Forms.Button();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.checkBoxHex = new System.Windows.Forms.CheckBox();
            this.labelCondition = new System.Windows.Forms.Label();
            this.textBoxGUID = new System.Windows.Forms.TextBox();
            this.radioButtonSend = new System.Windows.Forms.RadioButton();
            this.radioButtonRecv = new System.Windows.Forms.RadioButton();
            this.groupBoxDataBase = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFlowNO = new System.Windows.Forms.TextBox();
            this.textBoxHostFlowNO = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxText = new System.Windows.Forms.GroupBox();
            this.radioButtonDB = new System.Windows.Forms.RadioButton();
            this.radioButtonText = new System.Windows.Forms.RadioButton();
            this.groupBoxStyle = new System.Windows.Forms.GroupBox();
            this.groupBoxMsg = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonCore = new System.Windows.Forms.RadioButton();
            this.radioButtonPay = new System.Windows.Forms.RadioButton();
            this.groupBoxDataBase.SuspendLayout();
            this.groupBoxText.SuspendLayout();
            this.groupBoxStyle.SuspendLayout();
            this.groupBoxMsg.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxInput
            // 
            this.textBoxInput.Location = new System.Drawing.Point(6, 19);
            this.textBoxInput.Multiline = true;
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxInput.Size = new System.Drawing.Size(705, 138);
            this.textBoxInput.TabIndex = 0;
            // 
            // buttonDecode
            // 
            this.buttonDecode.Location = new System.Drawing.Point(515, 12);
            this.buttonDecode.Name = "buttonDecode";
            this.buttonDecode.Size = new System.Drawing.Size(75, 23);
            this.buttonDecode.TabIndex = 1;
            this.buttonDecode.Text = "解码";
            this.buttonDecode.UseVisualStyleBackColor = true;
            this.buttonDecode.Click += new System.EventHandler(this.buttonDecode_Click);
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(12, 330);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxResult.Size = new System.Drawing.Size(792, 137);
            this.textBoxResult.TabIndex = 2;
            // 
            // checkBoxHex
            // 
            this.checkBoxHex.AutoSize = true;
            this.checkBoxHex.Location = new System.Drawing.Point(726, 19);
            this.checkBoxHex.Name = "checkBoxHex";
            this.checkBoxHex.Size = new System.Drawing.Size(60, 16);
            this.checkBoxHex.TabIndex = 3;
            this.checkBoxHex.Text = "16进制";
            this.checkBoxHex.UseVisualStyleBackColor = true;
            // 
            // labelCondition
            // 
            this.labelCondition.AutoSize = true;
            this.labelCondition.Location = new System.Drawing.Point(37, 21);
            this.labelCondition.Name = "labelCondition";
            this.labelCondition.Size = new System.Drawing.Size(29, 12);
            this.labelCondition.TabIndex = 4;
            this.labelCondition.Text = "GUID";
            // 
            // textBoxGUID
            // 
            this.textBoxGUID.Location = new System.Drawing.Point(72, 18);
            this.textBoxGUID.Name = "textBoxGUID";
            this.textBoxGUID.Size = new System.Drawing.Size(377, 21);
            this.textBoxGUID.TabIndex = 5;
            // 
            // radioButtonSend
            // 
            this.radioButtonSend.AutoSize = true;
            this.radioButtonSend.Checked = true;
            this.radioButtonSend.Location = new System.Drawing.Point(16, 20);
            this.radioButtonSend.Name = "radioButtonSend";
            this.radioButtonSend.Size = new System.Drawing.Size(71, 16);
            this.radioButtonSend.TabIndex = 6;
            this.radioButtonSend.TabStop = true;
            this.radioButtonSend.Text = "发送报文";
            this.radioButtonSend.UseVisualStyleBackColor = true;
            // 
            // radioButtonRecv
            // 
            this.radioButtonRecv.AutoSize = true;
            this.radioButtonRecv.Location = new System.Drawing.Point(16, 49);
            this.radioButtonRecv.Name = "radioButtonRecv";
            this.radioButtonRecv.Size = new System.Drawing.Size(71, 16);
            this.radioButtonRecv.TabIndex = 7;
            this.radioButtonRecv.Text = "接收报文";
            this.radioButtonRecv.UseVisualStyleBackColor = true;
            // 
            // groupBoxDataBase
            // 
            this.groupBoxDataBase.Controls.Add(this.groupBoxMsg);
            this.groupBoxDataBase.Controls.Add(this.label2);
            this.groupBoxDataBase.Controls.Add(this.textBoxHostFlowNO);
            this.groupBoxDataBase.Controls.Add(this.textBoxFlowNO);
            this.groupBoxDataBase.Controls.Add(this.label1);
            this.groupBoxDataBase.Controls.Add(this.textBoxGUID);
            this.groupBoxDataBase.Controls.Add(this.labelCondition);
            this.groupBoxDataBase.Location = new System.Drawing.Point(12, 69);
            this.groupBoxDataBase.Name = "groupBoxDataBase";
            this.groupBoxDataBase.Size = new System.Drawing.Size(792, 86);
            this.groupBoxDataBase.TabIndex = 8;
            this.groupBoxDataBase.TabStop = false;
            this.groupBoxDataBase.Text = "数据库获取原报文";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "FLOW_NO";
            // 
            // textBoxFlowNO
            // 
            this.textBoxFlowNO.Location = new System.Drawing.Point(72, 49);
            this.textBoxFlowNO.Name = "textBoxFlowNO";
            this.textBoxFlowNO.Size = new System.Drawing.Size(155, 21);
            this.textBoxFlowNO.TabIndex = 9;
            // 
            // textBoxHostFlowNO
            // 
            this.textBoxHostFlowNO.Location = new System.Drawing.Point(310, 49);
            this.textBoxHostFlowNO.Name = "textBoxHostFlowNO";
            this.textBoxHostFlowNO.Size = new System.Drawing.Size(139, 21);
            this.textBoxHostFlowNO.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(233, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "HOSTFLOW_NO";
            // 
            // groupBoxText
            // 
            this.groupBoxText.Controls.Add(this.textBoxInput);
            this.groupBoxText.Controls.Add(this.checkBoxHex);
            this.groupBoxText.Location = new System.Drawing.Point(12, 161);
            this.groupBoxText.Name = "groupBoxText";
            this.groupBoxText.Size = new System.Drawing.Size(792, 163);
            this.groupBoxText.TabIndex = 9;
            this.groupBoxText.TabStop = false;
            this.groupBoxText.Text = "文本原报文";
            // 
            // radioButtonDB
            // 
            this.radioButtonDB.AutoSize = true;
            this.radioButtonDB.Checked = true;
            this.radioButtonDB.Location = new System.Drawing.Point(20, 20);
            this.radioButtonDB.Name = "radioButtonDB";
            this.radioButtonDB.Size = new System.Drawing.Size(83, 16);
            this.radioButtonDB.TabIndex = 10;
            this.radioButtonDB.TabStop = true;
            this.radioButtonDB.Text = "数据库方式";
            this.radioButtonDB.UseVisualStyleBackColor = true;
            this.radioButtonDB.CheckedChanged += new System.EventHandler(this.radioButtonDB_CheckedChanged);
            // 
            // radioButtonText
            // 
            this.radioButtonText.AutoSize = true;
            this.radioButtonText.Location = new System.Drawing.Point(150, 20);
            this.radioButtonText.Name = "radioButtonText";
            this.radioButtonText.Size = new System.Drawing.Size(71, 16);
            this.radioButtonText.TabIndex = 11;
            this.radioButtonText.Text = "文本方式";
            this.radioButtonText.UseVisualStyleBackColor = true;
            // 
            // groupBoxStyle
            // 
            this.groupBoxStyle.Controls.Add(this.radioButtonText);
            this.groupBoxStyle.Controls.Add(this.radioButtonDB);
            this.groupBoxStyle.Location = new System.Drawing.Point(18, 12);
            this.groupBoxStyle.Name = "groupBoxStyle";
            this.groupBoxStyle.Size = new System.Drawing.Size(340, 51);
            this.groupBoxStyle.TabIndex = 12;
            this.groupBoxStyle.TabStop = false;
            this.groupBoxStyle.Text = "数据提取方式";
            // 
            // groupBoxMsg
            // 
            this.groupBoxMsg.Controls.Add(this.radioButtonRecv);
            this.groupBoxMsg.Controls.Add(this.radioButtonSend);
            this.groupBoxMsg.Location = new System.Drawing.Point(455, 12);
            this.groupBoxMsg.Name = "groupBoxMsg";
            this.groupBoxMsg.Size = new System.Drawing.Size(107, 68);
            this.groupBoxMsg.TabIndex = 12;
            this.groupBoxMsg.TabStop = false;
            this.groupBoxMsg.Text = "报文类型";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonPay);
            this.groupBox1.Controls.Add(this.radioButtonCore);
            this.groupBox1.Location = new System.Drawing.Point(364, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(132, 56);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "平台类型";
            // 
            // radioButtonCore
            // 
            this.radioButtonCore.AutoSize = true;
            this.radioButtonCore.Checked = true;
            this.radioButtonCore.Location = new System.Drawing.Point(7, 15);
            this.radioButtonCore.Name = "radioButtonCore";
            this.radioButtonCore.Size = new System.Drawing.Size(47, 16);
            this.radioButtonCore.TabIndex = 0;
            this.radioButtonCore.TabStop = true;
            this.radioButtonCore.Text = "核心";
            this.radioButtonCore.UseVisualStyleBackColor = true;
            // 
            // radioButtonPay
            // 
            this.radioButtonPay.AutoSize = true;
            this.radioButtonPay.Location = new System.Drawing.Point(7, 35);
            this.radioButtonPay.Name = "radioButtonPay";
            this.radioButtonPay.Size = new System.Drawing.Size(47, 16);
            this.radioButtonPay.TabIndex = 1;
            this.radioButtonPay.Text = "支付";
            this.radioButtonPay.UseVisualStyleBackColor = true;
            // 
            // CoderTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 473);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxStyle);
            this.Controls.Add(this.groupBoxText);
            this.Controls.Add(this.groupBoxDataBase);
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.buttonDecode);
            this.Name = "CoderTool";
            this.Text = "CoderTool";
            this.groupBoxDataBase.ResumeLayout(false);
            this.groupBoxDataBase.PerformLayout();
            this.groupBoxText.ResumeLayout(false);
            this.groupBoxText.PerformLayout();
            this.groupBoxStyle.ResumeLayout(false);
            this.groupBoxStyle.PerformLayout();
            this.groupBoxMsg.ResumeLayout(false);
            this.groupBoxMsg.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.Button buttonDecode;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.CheckBox checkBoxHex;
        private System.Windows.Forms.Label labelCondition;
        private System.Windows.Forms.TextBox textBoxGUID;
        private System.Windows.Forms.RadioButton radioButtonSend;
        private System.Windows.Forms.RadioButton radioButtonRecv;
        private System.Windows.Forms.GroupBox groupBoxDataBase;
        private System.Windows.Forms.TextBox textBoxFlowNO;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxHostFlowNO;
        private System.Windows.Forms.GroupBox groupBoxText;
        private System.Windows.Forms.RadioButton radioButtonDB;
        private System.Windows.Forms.RadioButton radioButtonText;
        private System.Windows.Forms.GroupBox groupBoxStyle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonPay;
        private System.Windows.Forms.RadioButton radioButtonCore;
        private System.Windows.Forms.GroupBox groupBoxMsg;
    }
}