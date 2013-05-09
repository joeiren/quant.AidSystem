using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xQuant.AidSystem.CoreMessageData;
using xQuant.AidSystem.DBAction;
using xQuant.AidSystem.Communication;

namespace TestService
{
    public partial class CoderTool : Form
    {
        public CoderTool()
        {
            InitializeComponent();
            this.groupBoxDataBase.Enabled = radioButtonDB.Checked;
            this.groupBoxText.Enabled = radioButtonText.Checked;
        }

        private void buttonDecode_Click(object sender, EventArgs e)
        {
            if (this.radioButtonDB.Checked)
            {
                GetDBInfo();
            }
            else
            {
                GetTextInfo();
            }
        }

        private void GetTextInfo()
        {
            String input = textBoxInput.Text.TrimEnd();
            input = input.TrimStart();

            bool isHex = checkBoxHex.Checked;
            if (!String.IsNullOrEmpty(input))
            {
                String[] inputarray = input.Split(' ');
                if (inputarray.Length > 0)
                {
                    try
                    {
                        byte[] buffer = new byte[inputarray.Length];
                        int i = 0;
                        if (isHex)
                        {
                            foreach (String s in inputarray)
                            {
                                string s1 = s.TrimStart(new char[] { '0', 'x' });
                                s1 = s1.TrimStart(new char[] { '0', 'X' });
                                buffer[i] = Byte.Parse(s1, System.Globalization.NumberStyles.HexNumber);
                                i++;
                            }
                        }
                        else
                        {
                            foreach (String s in inputarray)
                            {

                                buffer[i] = Byte.Parse(s, System.Globalization.NumberStyles.Number);
                                i++;
                            }
                        }
                        DoTranslate(buffer);
                        //StringBuilder sb = new StringBuilder(inputarray.Length);
                        //int len = EBCDICEncoder.EBCDICToWideChar(EBCDICEncoder.CCSID_IBM_1388, buffer, inputarray.Length, sb, sb.Capacity);
                        //textBoxResult.Text = sb.ToString();
                    }
                    catch (Exception ex)
                    {
                        textBoxResult.Text = ex.Message;
                    }

                }

            }
        }

        private void GetDBInfo()
        {

            string guid = textBoxGUID.Text.TrimStart().TrimEnd();
            string flowno = textBoxFlowNO.Text.TrimStart().TrimEnd();
            string hostflowno = textBoxHostFlowNO.Text.TrimStart().TrimEnd();

            if (string.IsNullOrEmpty(guid) && string.IsNullOrEmpty(flowno) && string.IsNullOrEmpty(hostflowno))
            {
                MessageBox.Show("至少输入一个数据库的查询字段");
                return;
            }
            string sqlwhere = "";
            if (!string.IsNullOrEmpty(guid))
            {
                sqlwhere = string.Format("GUID='{0}'", guid);
            }
            if (!string.IsNullOrEmpty(flowno))
            {
                if (string.IsNullOrEmpty(sqlwhere))
                {
                    sqlwhere = string.Format("FLOW_NO='{0}'", flowno);
                }
                else
                {
                    sqlwhere = string.Format("{0} AND FLOW_NO={1} ", sqlwhere, flowno);
                }
            }
            if (!string.IsNullOrEmpty(hostflowno))
            {
                if (string.IsNullOrEmpty(sqlwhere))
                {
                    sqlwhere = string.Format("HOSTFLOW_NO={0}", hostflowno);
                }
                else
                {
                    sqlwhere = string.Format("{0} AND HOSTFLOW_NO={1}", sqlwhere, hostflowno);
                }
            }
            DataTable dt = TTRD_SET_MSG_LOG_Controller.Query(sqlwhere);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                byte[] buf;
                if (radioButtonSend.Checked)
                {
                    buf = row["SEND_CONTENT"] as byte[];
                }
                else
                {
                    buf = row["RECV_CONTENT"] as byte[];
                }
                // string content = CommonDataHelper.GBKTOWideChar(row["SEND_CONTENT"]);
                if (buf != null)
                {
                    DoTranslate(buf);
                }
            }
            return;
        }

        private void DoTranslate(byte[] buffer)
        {
            if (radioButtonCore.Checked)
            {
                StringBuilder sb = new StringBuilder(buffer.Length * 2);
                int len = EBCDICEncoder.EBCDICToWideChar(EBCDICEncoder.CCSID_IBM_1388, buffer, buffer.Length, sb, sb.Capacity);
                textBoxResult.Text = sb.ToString();
            }
            else if (radioButtonPay.Checked)
            {
                textBoxResult.Text = CommonDataHelper.GBKTOWideChar(buffer);
            }
        }

        private void radioButtonDB_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBoxDataBase.Enabled = radioButtonDB.Checked;
            this.groupBoxText.Enabled = radioButtonText.Checked;
        }
    }
}
