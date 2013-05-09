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
using xQuant.AidSystem.BizDataModel;
using xQuant.AidSystem.ClientSyncWrapper;

namespace TestService
{
    public partial class PaymentTestForm : Form
    {
        public PaymentTestForm()
        {
            InitializeComponent();
        }

        private void PaymentTestForm_Load(object sender, EventArgs e)
        {
            textBoxOrigDate.Text = DateTime.Now.ToString("yyyyMMdd");
        }

        #region Common
        byte[] uLongText;
        MsgDispatchEAP _dispatchMsg = null;
        private void DispatchMsg(MessageData msgdata)
        {
            try
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
            catch (Exception ex)
            { }
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

                        //if (e.MessageData.ReSentTime < 5)
                        //{
                        //    e.MessageData.ReSentTime++;

                        //    e.MessageData.RespPackageList = new Queue<PackageData>();
                        //    e.MessageData.IsMultiPackage = e.MessageData.ReqPackageList.Count > 1 ? true : false;

                        //    result.AppendLine();
                        //    result.AppendFormat("正在第{0}次重发数据: {1}", e.MessageData.ReSentTime, e.MessageData.MessageID);
                        //    result.AppendLine();
                        //    DispatchMsg(e.MessageData);
                        //}
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

                        if (respData is PayRegisterRetrievedData)
                        {
                            TestForRegisterRetrieved(result, (PayRegisterRetrievedData)respData);
                            textBoxRespRegRetrieved.Text = result.ToString();
                        }
                        else if (respData is PayFundTransferData)
                        {
                            TestForFundTransfer(result, (PayFundTransferData)respData);
                            textBoxRespFundTransfer.Text = result.ToString();
                        }
                        else if (respData is PayOutcomeAcctEraseData)
                        {
                            TestForPayAcctErase(result, (PayOutcomeAcctEraseData)respData);
                            textBoxRespEraseAcct.Text = result.ToString();
                        }
                        else if (respData is PayVostroAcctEliminateData)
                        {
                            TestForVostroAcctEliminate(result, (PayVostroAcctEliminateData)respData);
                            textBoxRespEl.Text = result.ToString();
                        }
                        else if (respData is PayInterBankData)
                        {
                            TestForInterBank(result, (PayInterBankData)respData);
                            textBoxRespInterBank.Text = result.ToString();
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

        #region 登记簿查询
        private void buttonRegRetrieved_Click(object sender, EventArgs e)
        {
            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            String flowcode = textBoxFlowNO.Text.Trim();
            String queryorg = textBoxRegQueryOrg.Text.Trim();
            Guid messageID = MsgTransferUtility.PayRegisterRetrieved(tellno, ouno, flowcode, DateTime.Now, ref uLongText);
            MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Payment };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = false;
            DispatchMsg(msgdata);
        }

        private String TestForRegisterRetrieved(StringBuilder result, PayRegisterRetrievedData respData)
        {
            if (respData == null || respData.RPData == null)
            {
                return result.ToString();
            }
            result.AppendFormat("交易结果:{0};交易结果描述:{1};渠道标志:{2};状态:{3}", respData.RPData.RetCode, respData.RPData.RetMsg, respData.RPData.PackageChannelType, respData.RPData.AccountStatus);
            return result.ToString();
        }
        #endregion

        #region 资金划拨
        private void buttonFundTransfer_Click(object sender, EventArgs e)
        {
            PayFundTransfer data = new PayFundTransfer();
            data.Teller = textBoxTellerNO.Text.Trim();
            data.TransferFlowNo = textBoxFundFlowNO.Text.Trim();
            data.PayBank = textBoxOrgNO.Text.Trim();
            data.TranDate = DateTime.Now.ToString("yyyyMMdd");
            data.PayAccount = textBoxPayAccount.Text.Trim();
            data.PayAccountName = textBoxPayName.Text.Trim();
            data.RecAccount = textBoxRecAccount.Text.Trim();
            data.RecAccountName = textBoxRecName.Text.Trim();
            data.RecAccountBanks = textBoxRecBankNO.Text.Trim();
            data.PackageChannelType = textBoxChanelType.Text.Trim();
            //data.CurrencyType = textBoxCurrency.Text.Trim();
            data.PayAmount = textBoxTradeAmount.Text.Trim();
            //data.BizType = textBoxBizType.Text.Trim();
            data.Fee = textBoxFee.Text.Trim();
            data.Remark = textBoxRemark.Text.Trim();
            //data.ChannelId = textBoxChannelID.Text.Trim();
            data.PendingSN = textBoxPendingSN.Text.Trim();
            Guid messageID = MsgTransferUtility.PayFundTransfer(data.Teller, data.PayBank, DateTime.Now, data, ref uLongText);
            MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Payment };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = false;
            DispatchMsg(msgdata);
        }

        private String TestForFundTransfer(StringBuilder result,PayFundTransferData respData)
        {
            if (respData == null || respData.RPData == null)
            {
                return result.ToString();
            }
            result.AppendFormat("交易结果:{0};返回码:{1};返回信息:{2};主机交易流水号:{3};支付交易序号:{4}", respData.RPData.RetCode, respData.RPData.HostReturnCode, respData.RPData.HostReturnMessage, respData.RPData.HostTranFlowNo, respData.RPData.TransSeq);
            return result.ToString();
        }
        #endregion

        #region 抹账交易
        private void buttonEraseAcct_Click(object sender, EventArgs e)
        {
            try
            {
                String ouno = textBoxOrgNO.Text.TrimStart();
                String tellno = textBoxTellerNO.Text.TrimStart();
                String flowcode = textBoxEraseFlowNO.Text.Trim();
                String hostflow = textBoxHostFlowNO.Text.Trim();

                Guid messageID = MsgTransferUtility.PayEraseAccounting(tellno, ouno, flowcode, DateTime.Now, hostflow, ref uLongText);
                MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Payment };
                msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
                msgdata.IsMultiPackage = false;
                DispatchMsg(msgdata);
            }
            catch (Exception ex)
            { }
        }

        private String TestForPayAcctErase(StringBuilder result, PayOutcomeAcctEraseData data)
        {
            if (data == null || data.RPData == null)
            {
                return result.ToString();
            }
            result.AppendFormat("交易结果:{0};交易结果描述:{1};", data.RPData.RetCode, data.RPData.RetMsg);
            return result.ToString();
        }
        #endregion

        #region 来账的销账
        private void buttonElAccount_Click(object sender, EventArgs e)
        {
            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            PayVostroAcctEliminate data = new PayVostroAcctEliminate();
            data.PayBank = textBoxOrgNO.Text.Trim();
            data.Operator = textBoxTellerNO.Text.Trim();
            data.PackageChannelType = textBoxElChannelType.Text.Trim();
            //data.BizType = textBoxElBizType.Text.Trim();
            data.TransSeq = textBoxElTransSeq.Text.Trim();
            data.AccountBanks = textBoxElAccountBanks.Text.Trim();
            data.DelegateDate = textBoxOrigDate.Text.Trim();//DateTime.Now.ToString("yyyyMMdd");
            //data.FundDest = textBoxElFundDest.Text.Trim();
            data.Amount = textBoxElAmount.Text.Trim();
            data.PostAcount = textBoxElPostAccount.Text.Trim();
            data.PostAccountName = textBoxElPostAccountName.Text.Trim();
            data.PostBank = textBoxElPostBank.Text.Trim();
            data.PostBankName = textBoxElPostBankName.Text.Trim();

            Guid messageID = MsgTransferUtility.PayVostroAccountEliminate(tellno, ouno, data.DelegateDate, data, ref uLongText);
            MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Payment };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = false;
            DispatchMsg(msgdata);

        }
        private String TestForVostroAcctEliminate(StringBuilder result, PayVostroAcctEliminateData respData)
        {
            if (respData == null || respData.RPData == null)
            {
                return result.ToString();
            }
            result.AppendFormat("交易结果:{0};返回码:{1};返回信息:{2};主机交易流水号:{3};支付交易序号:{4};挂账序号:{5}", respData.RPData.RetCode, respData.RPData.HostReturnCode, respData.RPData.HostReturnMessage, respData.RPData.HostTranFlowNo, respData.RPData.TransSeq, respData.RPData.PendingSN.TrimEnd());
            return result.ToString();
        }
        #endregion

        #region 同业拆借
        private void buttonInterBank_Click(object sender, EventArgs e)
        {
            PayInterBank paydata = new PayInterBank();
            paydata.TransferFlowNo = textBoxIBFlowNO.Text.Trim();
            paydata.PayBank = textBoxOrgNO.Text.Trim();
            paydata.TranDate = DateTime.Now.ToString("yyyyMMdd");
            paydata.PayAccount = textBoxIBPayAccount.Text.Trim();
            paydata.PayAccountName = textBoxIBPayAccountName.Text.Trim();
            paydata.OnAccountSN = textBoxIBOnAccountSN.Text.Trim();
            paydata.RecAccount = textBoxIBRecAccount.Text.Trim();
            paydata.RecAccountName = textBoxIBRecAccountName.Text.Trim();
            paydata.RecAccountBanks = textBoxIBRecAccountBanks.Text.Trim();
            paydata.PackageChannelType = textBoxIBChannelType.Text.Trim();
            paydata.CurrencyType = textBoxIBCurrency.Text.Trim();
            paydata.PayAmount = textBoxIBPayAmount.Text.Trim();
            paydata.Rate = textBoxIBRate.Text.Trim();
            paydata.TimeLimit = textBoxIBTimeLimit.Text.Trim();
            paydata.BizType = textBoxIBBizType.Text.Trim();
            paydata.Fee = textBoxIBFee.Text.Trim();
            paydata.Remark = textBoxIBRemark.Text.Trim();
            paydata.Teller = textBoxTellerNO.Text.Trim();
            paydata.AuthTeller = textBoxIBAuthTeller.Text.Trim();
            //paydata.ChannelId = textBoxIBChannelID.Text.Trim();

            Guid messageID = MsgTransferUtility.PayInterBankBiz(paydata.Teller, paydata.PayBank, DateTime.Now, paydata, ref uLongText);
            MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Payment };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = false;
            DispatchMsg(msgdata);
        }

        private String TestForInterBank(StringBuilder result, PayInterBankData respData)
        {
            if (respData == null || respData.RPData == null)
            {
                return result.ToString();
            }
            result.AppendFormat("交易结果:{0};返回码:{1};返回信息:{2};主机交易流水号:{3};支付交易序号:{4}", respData.RPData.RetCode, respData.RPData.HostReturnCode, respData.RPData.HostReturnMessage, respData.RPData.HostTranFlowNo, respData.RPData.TransSeq);
            return result.ToString();
        }
        #endregion

        private void buttonIncomeErase_Click(object sender, EventArgs e)
        {
            string srcBankNO = textBoxSrcBankNO.Text.Trim();
            string hostFlowNO = textBoxIncomeHostNO.Text.Trim();
            string paySN = textBoxPaySN.Text.Trim();
            PaymentResult outdata = null;
            string outmsg = "";
            DateTime delegateDate = DateTime.ParseExact(textBoxOrigDate.Text.Trim(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            if (AidSysClientSyncWrapper.PayIncomeEraseAccounting("", "", srcBankNO, paySN, delegateDate, "1", hostFlowNO, out outdata, out outmsg))
            {
                textBoxRespIncome.Text = string.Format("Return Code:{0}; Return Msg:{1}", outdata.HostReturnCode, outdata.HostReturnMessage);
            }
            else
            {
                if (outdata != null)
                {
                    textBoxRespIncome.Text = string.Format("Return Code:{0}; Return Msg:{1}", outdata.HostReturnCode, outdata.HostReturnMessage);
                }
            }
        }
    }
    
}
