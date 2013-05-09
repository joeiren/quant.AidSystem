using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xQuant.AidSystem.CoreMessageData;
using xQuant.AidSystem.BizDataModel;
using xQuant.AidSystem.Communication;
using System.Net.Sockets;

namespace TestService
{
    public partial class FundRetrieveForm : Form
    {
        public FundRetrieveForm()
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
                        if (respData is InterAcctRetrieveData)
                        {
                            TestForInterAcctRetrieved(result, (InterAcctRetrieveData)respData);
                            textBoxRespInterAcct.Text = result.ToString();
                        }
                        else if (respData is SuperiorCurrentAcctData)
                        {
                            TestForClearingCenterRetrieved(result, (SuperiorCurrentAcctData)respData);
                            textBoxRespClearing.Text = result.ToString();
                        }
                        else if (respData is DepositClearingFundData)
                        {
                            TestForDepositClearingFundRetrieved(result, (DepositClearingFundData)respData);
                            textBoxDepositResult.Text = result.ToString();
                        }
                        else if (respData is RetrieveAcctCrntBalanceData)
                        {
                            TestForCrntAcctBalanceRetrieved(result, (RetrieveAcctCrntBalanceData)respData);
                            textBoxRetCrntAcct.Text = result.ToString();
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

        #region 内部帐
        private void buttonInterAcct_Click(object sender, EventArgs e)
        {
            String txtTell = textBoxTellerNO.Text.Trim();
            String txtOrgn = textBoxOrgNO.Text.Trim();
            InternalAcctCondtion condition = new InternalAcctCondtion();
            condition.CheckCode = textBoxCheckCode.Text.Trim();
            condition.BalanceQueryType = textBoxQueryType.Text.Trim();
            condition.Currcency = textBoxCurrency.Text.Trim();
            condition.OrgnaztionNO = txtOrgn;
            condition.SequenceNO = textBoxSequenceNO.Text.Trim();

            Guid messageID = MsgTransferUtility.RetrieveInternalAcct(txtTell, txtOrgn, new DateTime(2013,12,5), condition, ref uLongText);
            MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = false;
            DispatchMsg(msgdata);
        }

        private String TestForInterAcctRetrieved(StringBuilder result, InterAcctRetrieveData respData)
        {
            if (respData == null)
            {
                return result.ToString();
            }
            if (respData.OmsgHandler.NUM_ENT > 0)
            {
                result.AppendFormat("OMSG:{0} item(s)", respData.OmsgHandler.NUM_ENT);
                result.AppendLine();
                foreach (OMSG_Item_Handler omsgItem in respData.OmsgHandler.OMSGItemList)
                {
                    result.AppendFormat("MOD_ID:{0};MSG_NO:{1};MSG_TYPE:{2};MSG_TEXT:{3};", omsgItem.MOD_ID, omsgItem.MSG_NO, omsgItem.MSG_TYPE, omsgItem.MSG_TEXT);
                    result.AppendLine();
                }
            }
            if (!String.IsNullOrEmpty(respData.SyserrHandler.Message))
            {
                result.AppendFormat("SYSERR:{0}", respData.SyserrHandler.Message);
            }
            if (!String.IsNullOrEmpty(respData.OData.OrgnaztionNO))
            {
                result.AppendFormat(
                    "账号:{0};归属机构号:{1};币种:{2};科目号:{3};顺序号:{4};户名:{5};上一交易日:{6};昨日余额方向:{7};昨日余额:{8};余额方向:{9};当前余额:{10};",
                    respData.OData.AccountNO, respData.OData.OrgnaztionNO, respData.OData.Currcency, respData.OData.SubjectNO, respData.OData.SequenceNO,
                    respData.OData.AccountName, respData.OData.PreviousTradeDate, respData.OData.PreviousBalanceDirection, respData.OData.PreviousBalance,
                    respData.OData.BalanceDirection, respData.OData.CurrentBalance);
                result.AppendLine();
                result.AppendFormat(
                    "透支限额:{0};利率:{1};透支利率:{2};利息积数:{3};透支积数:{4};使用范围:{5};是否采用销账管理:{6};是否收付现:{7};结息方式:{8};结息是否入账:{9};",
                    respData.OData.OverdraftLimitation, respData.OData.Rate, respData.OData.OverdraftRate, respData.OData.InterestCumulation, respData.OData.OverdraftCumulation,
                    respData.OData.UsageRange, respData.OData.IsEliminate, respData.OData.IsExchangePay, respData.OData.SettlementType, respData.OData.IsSettlementWithAccount);
                result.AppendLine();
                result.AppendFormat(
                    "收息账号:{0};付息账号:{1};账户状态:{2};开户机构:{3};归属总账机构:{4};通兑级别:{5};开户日期:{6};开户柜员:{7};",
                    respData.OData.IncomeAccount, respData.OData.PaymentAccount, respData.OData.AccountStatus, respData.OData.AccountOrgan, respData.OData.HeadOrgan,
                    respData.OData.CirculateExchangeLevel, respData.OData.AccountDate, respData.OData.AccountTeller);
            }
            return result.ToString();
        }
        #endregion

        #region 存放省内上级机构活期款项查询
        private void buttonClearingCenter_Click(object sender, EventArgs e)
        {
            String txtTell = textBoxTellerNO.Text.Trim();
            String txtOrgn = textBoxOrgNO.Text.Trim();
            String currency = textBoxClearingCurrency.Text.Trim();
            Guid messageID = MsgTransferUtility.RetrieveSuperiorCurrentAcct(txtTell, txtOrgn, new DateTime(2013, 12, 5), currency, ref uLongText);
            MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = false;
            DispatchMsg(msgdata);
        }
        private String TestForClearingCenterRetrieved(StringBuilder result, SuperiorCurrentAcctData respData)
        {
            if (respData == null)
            {
                return result.ToString();
            }

            if (!string.IsNullOrEmpty(respData.SyserrHandler.Message))
            {
                result.AppendFormat("SYSERR:{0}", respData.SyserrHandler.Message);
            }

            if (respData.OmsgHandler.NUM_ENT > 0)
            {
                result.AppendFormat("OMSG:{0} item(s).", respData.OmsgHandler.NUM_ENT);
                result.AppendLine();
                foreach (OMSG_Item_Handler omsgItem in respData.OmsgHandler.OMSGItemList)
                {
                    result.AppendFormat("MOD_ID:{0};MSG_NO:{1};MSG_TYPE:{2};MSG_TEXT:{3};", omsgItem.MOD_ID, omsgItem.MSG_NO, omsgItem.MSG_TYPE, omsgItem.MSG_TEXT);
                    result.AppendLine();
                }
            }
            int i = 0;
            foreach (var item in respData.OData.CrntAcctList)
            {
                result.AppendFormat("#{0}", ++i);
                result.AppendFormat(
                "机构号:{0};币种:{1};科目:{2};上日余额:{3};本日借方发生额:{4};本日贷方发生额:{5};当前余额:{6};下限金额:{7};轧差金额:{8};",
                item.OrgNO, item.Currency, item.Subject, item.PerviousBalance, item.DebitAmount,
                item.CreditAmount, item.CurrentBalance, item.FloorAmount, item.OffsetBalance);
                result.AppendLine();
            }
            
           
            return result.ToString();
        }
        #endregion

        #region 县级行社上存清算资金查询
        private void buttonDepositQuery_Click(object sender, EventArgs e)
        {
            String txtTell = textBoxTellerNO.Text.Trim();
            String txtOrgn = textBoxOrgNO.Text.Trim();
            String currency = textBoxClearingCurrency.Text.Trim();
            Guid messageID = MsgTransferUtility.RetrieveDepositClearingFund(txtTell, txtOrgn, DateTime.Now, this.textBoxCurrency1.Text.Trim(), this.radioButtonAll.Checked ? "1" : "2", ref uLongText);
            MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = false;
            DispatchMsg(msgdata);
        }

        private String TestForDepositClearingFundRetrieved(StringBuilder result, DepositClearingFundData respData)
        {
            if (respData == null)
            {
                return result.ToString();
            }
            if (!string.IsNullOrEmpty(respData.SyserrHandler.Message))
            {
                result.AppendFormat("SYSERR:{0}", respData.SyserrHandler.Message);
            }

            if (respData.OmsgHandler.NUM_ENT > 0)
            {
                result.AppendFormat("OMSG:{0} item(s).", respData.OmsgHandler.NUM_ENT);
                result.AppendLine();
                foreach (OMSG_Item_Handler omsgItem in respData.OmsgHandler.OMSGItemList)
                {
                    result.AppendFormat("MOD_ID:{0};MSG_NO:{1};MSG_TYPE:{2};MSG_TEXT:{3};", omsgItem.MOD_ID, omsgItem.MSG_NO, omsgItem.MSG_TYPE, omsgItem.MSG_TEXT);
                    result.AppendLine();
                }
            }

            int i =0;
            foreach (var item in respData.OData.BalanceInfoList)
            {
                result.AppendFormat("#{0}", ++i);
                result.AppendFormat(
                "机构号:{0};币种:{1};科目:{2};上日余额:{3};本日借方发生额:{4};本日贷方发生额:{5};当前余额:{6};下限金额:{7};轧差金额:{8};",
                item.OrgNO, item.Currency, item.Subject, item.PerviousBalance, item.DebitAmount,
                item.CreditAmount, item.CurrentBalance, item.FloorAmount, item.OffsetBalance);
                result.AppendLine();

            }
            
            return result.ToString();
        }

        #endregion

        #region 资金业务账号余额查询

        private void buttonCrntAcctQuery_Click(object sender, EventArgs e)
        {
            String txtTell = textBoxTellerNO.Text.Trim();
            String txtOrgn = textBoxOrgNO.Text.Trim();

            List<CoreAcctCrntBalance> list = new List<CoreAcctCrntBalance>();
            CoreAcctCrntBalance item1 = new CoreAcctCrntBalance();
            item1.AcctNO = "99930201112411000108";
            item1.AcctProperty = "1";
            item1.Currency = "CNY";
            list.Add(item1);

            CoreAcctCrntBalance item2 = new CoreAcctCrntBalance();
            item2.AcctNO = "82100001112401000190";
            item2.AcctProperty = "1";
            item2.Currency = "CNY";
            list.Add(item2);


            CoreAcctCrntBalance item3= new CoreAcctCrntBalance();
            item3.AcctNO = "99900001232301082172";
            item3.AcctProperty = "1";
            item3.Currency = "CNY";
            list.Add(item3);

            //CoreAcctCrntBalance item2 = new CoreAcctCrntBalance();
            //item2.AcctNO = "87100001910199110111";
            //item2.AcctProperty = "1";
            //item2.Currency = "CNY";
            //list.Add(item2);

            //CoreAcctCrntBalance item3 = new CoreAcctCrntBalance();
            //item3.AcctNO = "101003829324539";
            //item3.AcctProperty = "1";
            //item3.Currency = "CNY";
            //list.Add(item3);

            //CoreAcctCrntBalance item4 = new CoreAcctCrntBalance();
            //item4.AcctNO = "87100001910199160287";
            //item4.AcctProperty = "1";
            //item4.Currency = "CNY";
            //list.Add(item4);


            //-----
            //Guid messageID = MsgTransferUtility.RetrieveAcctCrntBalance(txtTell, txtOrgn, new DateTime(2010,10,31), list, ref uLongText);
            //MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core };
            //msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            //msgdata.IsMultiPackage = false;
            //DispatchMsg(msgdata);
            //
            //---
            List<FundCrntAcctBalance> outlist;
            string outmsg;
            //if (xQuant.AidSystem.ClientSyncWrapper.AidSysClientSyncWrapper.RetrieveAcctCrntBalance("8010019", "801000", new DateTime(2011, 10, 20), list, out outlist, out outmsg))
            if (xQuant.AidSystem.ClientSyncWrapper.AidSysClientSyncWrapper.RetrieveAcctCrntBalance(txtTell, txtOrgn, DateTime.Now, list, out outlist, out outmsg))
            {
                MessageBox.Show("成功");
            }
            else
            {
                MessageBox.Show(outmsg);
            }

        }

        private String TestForCrntAcctBalanceRetrieved(StringBuilder result, RetrieveAcctCrntBalanceData respData)
        {
            if (respData == null)
            {
                return result.ToString();
            }
            if (!string.IsNullOrEmpty(respData.SyserrHandler.Message))
            {
                result.AppendFormat("SYSERR:{0}", respData.SyserrHandler.Message);
            }

            if (respData.OmsgHandler.NUM_ENT > 0)
            {
                result.AppendFormat("OMSG:{0} item(s).", respData.OmsgHandler.NUM_ENT);
                result.AppendLine();
                foreach (OMSG_Item_Handler omsgItem in respData.OmsgHandler.OMSGItemList)
                {
                    result.AppendFormat("MOD_ID:{0};MSG_NO:{1};MSG_TYPE:{2};MSG_TEXT:{3};", omsgItem.MOD_ID, omsgItem.MSG_NO, omsgItem.MSG_TYPE, omsgItem.MSG_TEXT);
                    result.AppendLine();
                }
            }

            int i = 0;
            foreach (var item in respData.OData.BalanceList)
            {
                result.AppendFormat("#{0}", ++i);
                result.AppendLine();
                result.AppendFormat(
                "账号:{0};账号性质:{1};结果标志:{2};余额:{3};余额方向:{4};",
                item.AcctNO, item.AcctProperty, item.ResultFlag, item.Balance, item.BalanceDirection);
                result.AppendLine();

            }

            return result.ToString();
        }
        #endregion

    }
}
