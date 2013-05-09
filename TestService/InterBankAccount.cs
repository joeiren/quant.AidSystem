using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xQuant.AidSystem.BizDataModel;
using xQuant.AidSystem.ClientSyncWrapper;
using xQuant.AidSystem.Communication;
using xQuant.AidSystem.CoreMessageData;
using System.Net.Sockets;

namespace TestService
{
    public partial class InterBankAccount : Form
    {
        public InterBankAccount()
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

                        if (respData is InterBankOpenAcctData)
                        {
                            ForTestReturn(respData as InterBankOpenAcctData, result);
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

        private void ForTestReturn(InterBankOpenAcctData ibData, StringBuilder result)
        {
            if (ibData == null)
            {
                result.AppendFormat("The Core's result object is null!");
            }
            else
            {
                result.AppendFormat("Core Status:{0}", ibData.RPhdrHandler.STATUS);
                if (ibData.SyserrHandler.Message != null)
                {
                    result.AppendLine();
                    result.AppendFormat("SYSERROR:{0};", ibData.SyserrHandler.Message);
                }
                if (ibData.OmsgHandler.OMSGItemList != null && ibData.OmsgHandler.OMSGItemList.Count > 0)
                {
                    result.AppendLine();
                    result.AppendFormat("OMSG:{0};", ibData.OmsgHandler.OMSGItemList[0].MSG_TEXT);
                }
            }
        
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RegularResult result = AidSysClientSyncWrapper.InterBankOpenAccount(机构号.Text, 柜员号.Text, DateTime.Parse(业务交易日.Text), GetModel(true));
            if(!result.Succeed)
            {
                MessageBox.Show(result.ExceptionMsg);
            }
            else
            {
                MessageBox.Show("操作成功");
            }
            
        }

        private InterBankOpenAcctInfo GetModel(bool isAdd)
        {
            InterBankOpenAcctInfo info=new InterBankOpenAcctInfo();
            info.AMOUNT = double.Parse(string.IsNullOrEmpty(交易金额.Text) ? "0" : 交易金额.Text);
            info.AUTO_REDEPO = 定期自动转存标志.Text == "1" ? true : false;
            info.CURRENT_ACCOUNT = 资金来源活期账号.Text;
            info.CUSTOMER_CODE = 客户内码.Text;
            info.DEPOSIT_TYPE = (AidTypeDefine.INTER_BANK_DEPOSITING_CATEGORY)Enum.Parse(typeof(AidTypeDefine.INTER_BANK_DEPOSITING_CATEGORY), 存款种类.Text);
            info.INTEREST_ACCOUNT = 收息账号.Text;
            info.INTEREST_BEARING_MANNER = (AidTypeDefine.INTER_BANK_COUPON_TYPE)Enum.Parse(typeof(AidTypeDefine.INTER_BANK_COUPON_TYPE), 计息方式.Text);
            info.BIZ_TERM_TYPE = 业务类型.Text == "1" ? AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE.Current : AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE.Fixed;
            info.MATURITY_DATE =string.IsNullOrEmpty(到期日期.Text) ? DateTime.Now : DateTime.Parse(到期日期.Text);
            info.NOTICE_NO = 通知单编号.Text;
            info.OPERATION_TYPE = isAdd ? AidTypeDefine.INTER_BANK_OPERATION_TYPE.CreateNew : AidTypeDefine.INTER_BANK_OPERATION_TYPE.Cancel;
            info.RATE = double.Parse(string.IsNullOrEmpty(利率.Text) ? "0" : 利率.Text);
            info.VALUE_DATE =string.IsNullOrEmpty(起息日期.Text.Trim()) ? DateTime.Now : DateTime.Parse(起息日期.Text);
            return info;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            RegularResult result = AidSysClientSyncWrapper.InterBankOpenAccount(机构号.Text, 柜员号.Text, DateTime.Parse(业务交易日.Text), GetModel(false));
            if (!result.Succeed)
            {
                MessageBox.Show(result.ExceptionMsg);
            }
            else
            {
                MessageBox.Show("操作成功");
            }
        }

        private void btnAddOfCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] codemsg = null;
                Guid messageID = MsgTransferUtility.OpenAccount(机构号.Text, 柜员号.Text, DateTime.Parse(业务交易日.Text), GetModel(true), ref codemsg);

                MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, IsMultiPackage = false, TragetPlatform = PlatformType.Core };
                msgdata.ReqPackageList.Enqueue(new PackageData(1, codemsg));
                DispatchMsg(msgdata);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancelOfCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] codemsg = null;
                Guid messageID = MsgTransferUtility.OpenAccount(机构号.Text, 柜员号.Text, DateTime.Parse(业务交易日.Text), GetModel(false), ref codemsg);

                MessageData msgdata = new MessageData { MessageID = messageID, IsMultiPackage = false, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core };
                msgdata.ReqPackageList.Enqueue(new PackageData(1, codemsg));
                DispatchMsg(msgdata);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
