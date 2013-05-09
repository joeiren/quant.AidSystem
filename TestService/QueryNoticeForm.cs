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
    public partial class QueryNoticeForm : Form
    {
        public QueryNoticeForm()
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
                        if (respData is InterBankNoticeLetterData)
                        {
                            TestForQuery(respData as InterBankNoticeLetterData);
                        }
                        //if (respData is AcctRecordData)
                        //{
                        //    TestForAcctRecord(result, (CoreBizMsgDataBase)respData);
                        //    textBoxRespAcct.Text = result.ToString();
                        //}
                        //else if (respData is AcctRecordMulti2OneData)
                        //{
                        //    TestForMulti2One(result, (CoreBizMsgDataBase)respData);
                        //    textBoxRespAcct.Text = result.ToString();
                        //}


                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                result.AppendLine();
                result.Append(ex.Message.ToString());

                //MessageBox.Show(ex.Message.ToString());
            }
        }
        #endregion

        private void TestForQuery(InterBankNoticeLetterData rData)
        {
            StringBuilder result = new StringBuilder();
            if (rData == null)
            {
                MessageBox.Show("返回数据为空！");
            }
            if (rData.RPhdrHandler != null)
            {
                result.AppendFormat("返回数据状态：{0};", rData.RPhdrHandler.STATUS);
            }

            if (rData.OmsgHandler != null && rData.OmsgHandler.OMSGItemList != null && rData.OmsgHandler.OMSGItemList.Count > 0)
            {
                result.AppendLine();
                result.AppendFormat("OMSG:{0};", rData.OmsgHandler.OMSGItemList[0].MSG_TEXT);
            }
            if (rData.SyserrHandler != null && !string.IsNullOrEmpty(rData.SyserrHandler.Message))
            {
                result.AppendLine();
                result.AppendFormat("SYSERR:{0};", rData.SyserrHandler.Message);
            }
            if (rData.OData != null)
            {
                result.AppendLine();
                result.Append("ODATA:");
                result.AppendFormat("会计日期:{0};通知单编号:{1};通知单类型:{2};业务类型:{3};", rData.OData.ACCOUNT_DATE, rData.OData.NOTICE_NO, rData.OData.NOTICE_TYPE, rData.OData.BUSINESS_TYPE);
                result.AppendLine();
                result.AppendFormat("账号:{0};币种:{1};外币客户类别:{2};钞汇属性:{3};", rData.OData.ACCOUNT_NO, rData.OData.CURRENCY, rData.OData.CUSTOMER_TYPE, rData.OData.CASH_PROPERTY);
                result.AppendLine();
                result.AppendFormat("产品类别:{0};产品代码:{1};存款种类:{2};账户性质:{3};帐户类型:{4}", rData.OData.PRODUCT_TYPE, rData.OData.PRODUCT_CODE, rData.OData.DEPOSIT_TYPE, rData.OData.ACCOUNT_PROPERTY, rData.OData.ACCOUNT_TYPE);
                result.AppendLine();
                result.AppendFormat("资金来源活期账号:{0};收息账号:{1};定期部提新账号:{2};凭证号码:{3};新凭证号码:{4};", rData.OData.CURRENT_ACCOUNT, rData.OData.INTEREST_ACCOUNT, rData.OData.FIXED_NEW_ACCOUNT, rData.OData.VOUCHER_NO, rData.OData.VOUCHER_NEW_NO);
                result.AppendLine();
                result.AppendFormat("自动转存:{0};定期存期:{1};起息日期:{2};到期日期:{3};交易金额:{4};", rData.OData.AUTO_REDEPO, rData.OData.DEPOSIT_TERM, rData.OData.VALUE_DATE, rData.OData.MATURITY_DATE, rData.OData.AMOUNT);
                result.AppendLine();
                result.AppendFormat("利率:{0};计息方式:{1};利息金额:{2};客户内码:{3};银行集团企业标志:{4};", rData.OData.RATE, rData.OData.INTEREST_BEARING_MANNER, rData.OData.INTEREST, rData.OData.CUSTOMER_CODE, rData.OData.BANK_FLAG);
                result.AppendLine();
                result.AppendFormat("经办机构:{0};审批机构:{1};操作柜员:{2};审核柜员:{3};", rData.OData.HANDLE_ORGNAZTION, rData.OData.APPROVE_ORGNAZTION, rData.OData.HANDLE_TELLER, rData.OData.APPROVE_TELLER);
                result.AppendLine();
                result.AppendFormat("处理标志:{0};原交易流水号:{1};子交易流水号:{2};柜员流水号:{3};记录状态:{4};存单折类型:{5}", rData.OData.HANDLE_FLAG, rData.OData.TRADE_FLOW_NO, rData.OData.CHILD_TRADE_FLOW_NO, rData.OData.TELLER_FLOW_NO, rData.OData.RECORD_FLAG, rData.OData.BOOK_TYPE);
                result.AppendLine();
                result.AppendFormat("户名:{0};余额:{1};本息合计:{2};备用:{3}", rData.OData.ACCOUNT_NAME, rData.OData.BALANCE, rData.OData.SUM_AMOUNT, rData.OData.RESERVE);
            }
            textResult.Text = result.ToString();
        }


        private void TestForQuery(InterBankNoticeLetterInfo rData)
        {
            StringBuilder result = new StringBuilder();
            if (rData == null)
            {
                MessageBox.Show("返回数据为空！");
            }
            else
            {
                result.AppendLine();
                result.Append("ODATA:");
                result.AppendFormat("会计日期:{0};通知单编号:{1};通知单类型:{2};业务类型:{3};", rData.ACCOUNT_DATE, rData.NOTICE_NO, rData.NOTICE_TYPE, rData.BUSINESS_TYPE);
                result.AppendLine();
                result.AppendFormat("账号:{0};币种:{1};外币客户类别:{2};钞汇属性:{3};", rData.ACCOUNT, rData.CURRENCY, rData.CUSTOMER_TYPE, rData.CASH_PROPERTY);
                result.AppendLine();
                result.AppendFormat("产品类别:{0};产品代码:{1};存款种类:{2};账户性质:{3};帐户类型:{4}", rData.PRODUCT_TYPE, rData.PRODUCT_CODE, rData.DEPOSIT_TYPE, rData.ACCOUNT_PROPERTY, rData.ACCOUNT_TYPE);
                result.AppendLine();
                result.AppendFormat("资金来源活期账号:{0};收息账号:{1};定期部提新账号:{2};凭证号码:{3};新凭证号码:{4};", rData.CURRENT_ACCOUNT, rData.INTEREST_ACCOUNT, rData.FIXED_NEW_ACCOUNT, rData.VOUCHER_NO, rData.VOUCHER_NEW_NO);
                result.AppendLine();
                result.AppendFormat("自动转存:{0};定期存期:{1};起息日期:{2};到期日期:{3};交易金额:{4};", rData.AUTO_REDEPO, rData.DEPOSIT_TERM, rData.VALUE_DATE, rData.MATURITY_DATE, rData.AMOUNT);
                result.AppendLine();
                result.AppendFormat("利率:{0};计息方式:{1};利息金额:{2};客户内码:{3};银行集团企业标志:{4};", rData.RATE, rData.INTEREST_BEARING_MANNER, rData.INTEREST, rData.CUSTOMER_CODE, rData.BANK_FLAG);
                result.AppendLine();
                result.AppendFormat("经办机构:{0};审批机构:{1};操作柜员:{2};审核柜员:{3};", rData.HANDLE_ORGNAZTION, rData.APPROVE_ORGNAZTION, rData.HANDLE_TELLER, rData.APPROVE_TELLER);
                result.AppendLine();
                result.AppendFormat("处理标志:{0};原交易流水号:{1};子交易流水号:{2};柜员流水号:{3};记录状态:{4};存单折类型:{5}", rData.HANDLE_FLAG, rData.TRADE_FLOW_NO, rData.CHILD_TRADE_FLOW_NO, rData.TELLER_FLOW_NO, rData.RECORD_FLAG, rData.BOOK_TYPE);
                result.AppendLine();
                result.AppendFormat("户名:{0};余额:{1};本息合计:{2};备用:{3}", rData.ACCOUNT_NAME, rData.BALANCE, rData.SUM_AMOUNT, rData.RESERVE);
            }
            textResult.Text = result.ToString();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            InterBankNoticeQueryInfo info = new InterBankNoticeQueryInfo() 
            {
                AccountDate = DateTime.Parse(会计日期.Text),
                NoticeID = 通知单编号.Text,
                NoticeType = (AidTypeDefine.INTER_BANK_NOTICE_TYPE)Enum.Parse(typeof(AidTypeDefine.INTER_BANK_NOTICE_TYPE),通知单类型.Text),
                BizType =  (AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE) Enum.Parse(typeof(AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE), 业务类型.Text)
            };
            TupleResult<RegularResult, InterBankNoticeLetterInfo> resultset = AidSysClientSyncWrapper.InterBankRetrieveNotice(机构号.Text, 柜员号.Text, DateTime.Parse(业务交易日.Text), info);
            if (!resultset.First.Succeed)
            {
                MessageBox.Show(resultset.First.ExceptionMsg);
            }
            else
            {
                TestForQuery(resultset.Second);
            }
        }

        private void btnQueryOfCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] codemsg = null;
                InterBankNoticeQueryInfo info = new InterBankNoticeQueryInfo();
                info.BizType = (AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE)Enum.Parse(typeof(AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE), 业务类型.Text.Trim());
                info.AccountDate = DateTime.Parse(会计日期.Text);
                info.NoticeID = 通知单编号.Text.Trim();
                info.NoticeType = (AidTypeDefine.INTER_BANK_NOTICE_TYPE) Enum.Parse(typeof(AidTypeDefine.INTER_BANK_NOTICE_TYPE), 通知单类型.Text.Trim());
                Guid messageID = MsgTransferUtility.RetrieveNoticeLetter(DateTime.Parse(业务交易日.Text), 机构号.Text, 柜员号.Text, info, ref codemsg);

                MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core };
                msgdata.ReqPackageList.Enqueue(new PackageData(1, codemsg));
                msgdata.IsMultiPackage = false;
                DispatchMsg(msgdata);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
    }
}
