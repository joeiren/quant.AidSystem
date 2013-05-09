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

namespace TestService
{
    public partial class InterBankRedepoForm : Form
    {
        public InterBankRedepoForm()
        {
            InitializeComponent();
        }
        #region Common
        MsgDispatchEAP _dispatchMsg = null;
        private void DispatchMsg(MessageData msgdata)
        {
            ICommunicationHandler handler;
            switch (msgdata.TragetPlatform)
            {
                case PlatformType.Encrypt:
                    handler = new EncryptCommunicationHandler();
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
                        result.AppendLine();
                        result.Append(sex.StackTrace);

                    }
                    else
                    {
                        result.AppendLine();
                        result.Append(e.Error.Message);

                    }

                    MessageBox.Show(result.ToString());
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

                        if (respData is InterBankAutoRedepoData)
                        {
                            InterBankAutoRedepoData rData = respData as InterBankAutoRedepoData;
                            if (rData == null)
                            {
                                result.AppendFormat("The Core's result object is null!");
                            }
                            else
                            {
                                result.AppendFormat("Core Status:{0}", rData.RPhdrHandler.STATUS);
                                if (rData.SyserrHandler.Message != null)
                                {
                                    result.AppendLine();
                                    result.AppendFormat("SYSERROR:{0};", rData.SyserrHandler.Message);
                                }
                                if (rData.OmsgHandler.OMSGItemList != null && rData.OmsgHandler.OMSGItemList.Count > 0)
                                {
                                    result.AppendLine();
                                    result.AppendFormat("OMSG:{0};", rData.OmsgHandler.OMSGItemList[0].MSG_TEXT);
                                }
                            }
                        }
                        #endregion
                    }
                }
                textBoxResult.Text = result.ToString();
            }
            catch (Exception ex)
            {
                result.AppendLine();
                result.Append(ex.Message.ToString());

                //MessageBox.Show(ex.Message.ToString());
            }
        }
        #endregion
        private void buttonRedepo_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] codemsg = null;
                Guid messageID = MsgTransferUtility.InterBankAutoRedepo(机构号.Text.Trim(), 柜员号.Text.Trim(), DateTime.Parse(交易日.Text.Trim()), 账号.Text.Trim(), DateTime.Parse(新起息日.Text), DateTime.Parse(新到期日.Text), ref codemsg);

                MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, IsMultiPackage = false, TragetPlatform = PlatformType.Core };
                msgdata.ReqPackageList.Enqueue(new PackageData(1, codemsg));
                DispatchMsg(msgdata);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonRedepoEx_Click(object sender, EventArgs e)
        {
            RegularResult result = AidSysClientSyncWrapper.InterBankAutoRedepo(机构号.Text.Trim(), 柜员号.Text.Trim(), DateTime.Parse(交易日.Text.Trim()), 账号.Text.Trim(), DateTime.Parse(新起息日.Text), DateTime.Parse(新到期日.Text), "s");
            if (!result.Succeed)
            {
                MessageBox.Show(result.ExceptionMsg);
            }
            else
            {
                MessageBox.Show("操作成功");
            }
        }
    }
}
