using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using xQuant.AidSystem.Communication;
using xQuant.AidSystem.CoreMessageData;
using xQuant.AidSystem.ClientSyncWrapper;

namespace TestService
{
    public partial class InterBankRetrieveBalance : Form
    {
        public InterBankRetrieveBalance()
        {
            InitializeComponent();
        }
        MsgDispatchEAP _dispatchMsg = null;

        List<byte[]> _byteCollection = new List<byte[]>();
        #region Common
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

                        if (respData is InterBankAcctInfoData)
                        {
                            //ForTestReturn(respData as InterBankAcctInfoData, result);
                        }



                        #endregion
                    }
                }
                MessageBox.Show(result.ToString());
            }
            catch (Exception ex)
            {
                result.AppendLine();
                result.Append(ex.Message.ToString());

                //MessageBox.Show(ex.Message.ToString());
            }
        }
        #endregion
        private void buttonQuery_Click(object sender, EventArgs e)
        {
            TupleResult<RegularResult, double> result = AidSysClientSyncWrapper.InterBankRetrieveAccount(textBoxOrg.Text.Trim(), textBoxTeller.Text.Trim(), DateTime.Parse(textBoxTradeDate.Text.Trim()), textBoxAccountNO.Text.Trim());
            if (!result.First.Succeed)
            {
                textBoxResult.Text = result.First.ExceptionMsg;
            }
            else
            {
                textBoxResult.Text = string.Format("操作成功!\r\n{0}", result.Second);
            }
        }
    }
}
