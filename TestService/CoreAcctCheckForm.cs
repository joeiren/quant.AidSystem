using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xQuant.AidSystem.Communication;
using System.Net.Sockets;
using xQuant.AidSystem.CoreMessageData;
using xQuant.AidSystem.ClientSyncWrapper;
using xQuant.AidSystem.BizDataModel;

namespace TestService
{
    public partial class CoreAcctCheckForm : Form
    {
        public CoreAcctCheckForm()
        {
            InitializeComponent();
        }
        #region Common
        byte[] uLongText;
        MsgDispatchEAP _dispatchMsg = null;
        private void DispatchMsg(MessageData msgdata)
        {
            ICommunicationHandler handler;
            switch (msgdata.TragetPlatform)
            {
                case PlatformType.Encrypt:
                    handler = new EncryptCommunicationHandler();
                    break;
                case PlatformType.Payment:
                case PlatformType.PaymentDownload:
                    handler = new PayCommunicationHandler();
                    break;
                case PlatformType.Core:
                default:
                    handler = new CoreCommunicationHandler();
                    break;
            }

            if (_dispatchMsg != null)
            {

                lock (_dispatchMsg)
                {
                    _dispatchMsg.MsgHandler = handler;
                    _dispatchMsg.OnDispatchMsgAsync(msgdata, msgdata.MessageID);
                }
            }
            else
            {
                _dispatchMsg = new MsgDispatchEAP(handler);
                lock (_dispatchMsg)
                {
                    _dispatchMsg.DispatchCompleted += new DispatchCompletedEventHandler(DispatchMsg_DispatchCompleted);
                    _dispatchMsg.OnDispatchMsgAsync(msgdata, msgdata.MessageID);
                }
            }
        }

        void DispatchMsg_DispatchCompleted(object sender, TransmitCompletedEventArgs e)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                if (e.Cancelled)
                {
                    result.AppendLine();
                    result.Append(" Canceled!");
                }
                else if (e.Error != null)
                {
                    //result.AppendFormat("EorrCode:",e.Error.);
                    if (e.Error is SocketException)
                    {
                        SocketException sex = e.Error as SocketException;
                        result.AppendLine();
                        result.AppendFormat("{0}. ERROR CODE:{1}", e.Error.Message, sex.SocketErrorCode);
                    }
                    else
                    {
                        result.AppendLine();
                        result.Append(e.Error.Message);
                    }

                    //textBoxRetCstmResult.Text += result.ToString();
                }
                else
                {
                    if (e.MessageData.IsMultiPackage)
                    {
                        while (e.MessageData.RespPackageList.Count > 0)
                        {
                            result.Append(e.MessageData.GetRespMessage());
                            e.MessageData.RespPackageList.Dequeue();
                        }
                    }
                    else
                    {
                        byte[] rebytes = e.MessageData.GetRespMessage();

                        // 发送个MQ(MessageID, buffer)

                        // 跳过MQ
                        // ....

                        // web界面层
                        int realLen = rebytes.Length;
                        if (e.MessageData.TragetPlatform != PlatformType.Encrypt)
                        {
                            byte end = 0;
                            realLen = Array.IndexOf(rebytes, end);
                        }

                        byte[] buffer = new byte[realLen];
                        Array.Copy(rebytes, buffer, realLen);
                        BizMsgDataBase respData = null;

                        respData = MsgTransfer.DecodeMsg(e.MessageData.MessageID, buffer);
                        #region Test code
                        if (respData is AcctCheckData)
                        {
                            TestForAcctChecking(result, (AcctCheckData)respData);
                            
                        }
                       


                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                result.AppendLine();
                result.Append(ex.Message.ToString());
                //textBoxRespAcct.Text = result.ToString();
                //MessageBox.Show(ex.Message.ToString());
            }
        }
        #endregion

        private void buttonQuery_Click(object sender, EventArgs e)
        {

            try
            {
                String ouno = textBoxOrgNO.Text.TrimStart();
                String tellno = textBoxTellerNO.Text.TrimStart();
                DateTime _coreDate = DateTime.Parse(dateTimeCore.Text);
                DateTime querydate = DateTime.ParseExact(textBoxQueryDate.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);

                Guid messageID = MsgTransferUtility.AccountingCheck(tellno, ouno, _coreDate, querydate, textBoxBizFlowNO.Text.Trim(), textBoxQueryOrg.Text.Trim(), ref uLongText);
                MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core };
                msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
                msgdata.IsMultiPackage = false;
                DispatchMsg(msgdata);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TestForAcctChecking(StringBuilder result, AcctCheckData respdata)
        {
            if (respdata != null && respdata.OBDataList != null)
            {
                String path = @"e:\CoreAccts.del";
                List<string> contentlist = new List<string>();
                foreach (var item in respdata.OBDataList)
                {
                    contentlist.Add(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}",
                        item.TradeDate, item.BizFlowNO, string.Empty, item.OrgNO, item.TellerNO, item.TellerFlowNO, string.Empty, string.Empty, item.TradeAcctNO, item.OrgNOWithinAcct, item.Currency, item.CheckCode, item.DCFlag, item.RedBlueFlag, item.Amount, item.Status, string.Empty));
                }
                CommonMethods.WriteLocalGBKFile(path, contentlist.ToArray());
                MessageBox.Show(string.Format("下载完毕，结果已保存文件在{0}", path), "核心对账", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            
            {
                if (!String.IsNullOrEmpty(respdata.SyserrHandler.Message))
                {
                    textBoxResult.Text = respdata.SyserrHandler.Message;
                }
                else if (respdata.OmsgHandler.NUM_ENT > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in respdata.OmsgHandler.OMSGItemList)
                    {
                        
                        sb.AppendLine(string.Format("{0}:{1}",item.MSG_NO,item.MSG_TEXT));
                    }

                    textBoxResult.Text = sb.ToString();
                }
            }
        }
        
        private void buttonQuerySync_Click(object sender, EventArgs e)
        {
            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            DateTime _coreDate = DateTime.Parse(dateTimeCore.Text);
            DateTime querydate = DateTime.ParseExact(textBoxQueryDate.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            List<CoreCheckAcctInfo> list = null;
            String outmsg = "";
            if (!AidSysClientSyncWrapper.CoreAcctChecking(tellno, ouno, DateTime.Now, querydate, textBoxBizFlowNO.Text.Trim(), textBoxQueryOrg.Text.Trim(), out list, out outmsg))
            {
                MessageBox.Show(outmsg);
            }
            else
            {
                MessageBox.Show("同步查询接收");
            }
        }

        private void buttonRcrdDB_Click(object sender, EventArgs e)
        {
            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            DateTime _coreDate = DateTime.Parse(dateTimeCore.Text);
            DateTime querydate = DateTime.ParseExact(textBoxQueryDate.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            String outmsg = "";
            if (!AidSysClientSyncWrapper.CoreRollingAcct(tellno, ouno, _coreDate, querydate, textBoxBizFlowNO.Text.Trim(), textBoxQueryOrg.Text.Trim(), out outmsg))
            {
                MessageBox.Show(outmsg);
            }
            else
            {
                MessageBox.Show("已同步记录到DB");
            }
        }


    }
}
