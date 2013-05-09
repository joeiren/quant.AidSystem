using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xQuant.AidSystem.CoreMessageData;
using xQuant.AidSystem.Communication;
using System.Net.Sockets;
using xQuant.AidSystem.BizDataModel;
using xQuant.AidSystem.ClientSyncWrapper;

namespace TestService
{
    public partial class AccountRecordForm : Form
    {
        public AccountRecordForm()
        {
            InitializeComponent();
        }
        private int _dataCount = 10;
        long _begin;
        byte[] uLongText;
        MsgDispatchEAP _dispatchMsg = null;
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
            _dataCount--;
            StringBuilder result = new StringBuilder();
            try
            {
                if (e.Cancelled)
                {
                    result.AppendLine();
                    result.Append(" Canceled!");
                    if (_dataCount <= 0)
                    {
                        long end = DateTime.Now.Ticks;
                        TimeSpan span = new TimeSpan(end - _begin);

                        MessageBox.Show(span.TotalMilliseconds.ToString());
                    }

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
                    if (_dataCount <= 0)
                    {
                        long end = DateTime.Now.Ticks;
                        TimeSpan span = new TimeSpan(end - _begin);

                        MessageBox.Show(span.TotalMilliseconds.ToString());
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

                        if (respData is AcctRecordData)
                        {
                            TestForAcctRecord(result, (CoreBizMsgDataBase)respData);
                            textBoxRespAcct.Text = result.ToString();
                        }
                        else if (respData is AcctRetrieveData)
                        {
                            TestForRetrieveRecord(result, (CoreBizMsgDataBase)respData);
                            textBoxRespRetrieve.Text = result.ToString();
                        }
                        else if (respData is AcctEraseData)
                        {
                            TestForAcctErase(result, (CoreBizMsgDataBase)respData);
                            textBoxRespErase.Text = result.ToString();
                        }
                        
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                result.AppendLine();
                result.Append(ex.Message.ToString());
                textBoxRespAcct.Text = result.ToString();
                //MessageBox.Show(ex.Message.ToString());
            }
        }
        #endregion

        #region 记账
        private void radioButtonRemove1_CheckedChanged(object sender, EventArgs e)
        {
            textBoxRemove1.ReadOnly = !radioButtonRemove1.Checked;
            textBoxRemove1.Text = "";
        }

        private void radioButtonRemove_CheckedChanged(object sender, EventArgs e)
        {
            textBoxRemove.ReadOnly = !radioButtonRemove.Checked;
            textBoxRemove.Text = "";
        }

        private void buttonAccounting_Click(object sender, EventArgs e)
        {
            List<CoreBillRecord> list = GetAccoutingEntry();
            if (list.Count == 0)
            {
                MessageBox.Show("选择要记账的分录！", "记账提示", MessageBoxButtons.OK);
                return;
            }
            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            String flowno = textBoxFlowNO.Text.Trim();
            Guid messageID = MsgTransferUtility.AccountingRecord(tellno, ouno, DateTime.Now, flowno, list, ref uLongText);
            List<CoreAcctEntry> rlist = new List<CoreAcctEntry>();
            //string outmsg="";
            //AidSysClientSyncWrapper.CoreAcctRecord(ouno, tellno, DateTime.Now, flowno, list, rlist, ref outmsg);
            MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = false;
            DispatchMsg(msgdata);

        }

        private List<CoreBillRecord> GetAccoutingEntry()
        {
            List<CoreBillRecord> billList = new List<CoreBillRecord>();
            //记账1
            CoreBillRecord bill = new CoreBillRecord();
            bill.InnerSN = (billList.Count + 1).ToString();
            //bill.TradeAcct = textBoxTradeAcct.Text.Trim();
            bill.Currency = textBoxCurrency.Text.Trim();
            bill.InterAcctSN = textBoxInterAcctSN.Text.Trim();
            bill.Subject = textBoxSubject.Text.Trim();
            bill.OptAcct = textBoxOptAcct.Text.Trim();
            bill.TradeMoney = textBoxMoney.Text.Trim();
            bill.Opt = radioButtonBorrow.Checked ? "1" : "2";
            
            if (!radioButtonNor.Checked)
            {
                bill.IsEliminateFlag = radioButtonRecord.Checked ? "1" : "2";
                if (radioButtonRemove.Checked)
                {
                    bill.PendingAcctSN = textBoxRemove.Text;
                }
            }
            bill.OrgNO = textBoxOrgNO.Text.Trim();
            billList.Add(bill);     
            

            //记账2
            CoreBillRecord bill1 = new CoreBillRecord();            
            bill1.InnerSN = (billList.Count + 1).ToString();
            //bill1.TradeAcct = textBoxTradeAcct1.Text.Trim();
            bill1.Currency = textBoxCurrency1.Text.Trim();
            bill1.InterAcctSN = textBoxInterAcctSN1.Text.Trim();
            bill1.Subject = textBoxSubject1.Text.Trim();
            bill1.OptAcct = textBoxOptAcct1.Text.Trim();
            bill1.TradeMoney = textBoxMoney1.Text.Trim();
            bill1.Opt = radioButtonBorrow1.Checked ? "1" : "2";
            //bill.Opt = "2";
            if (!radioButtonNor1.Checked)
            {
                bill1.IsEliminateFlag = radioButtonRecord1.Checked ? "1" : "2";
                if (radioButtonRemove1.Checked)
                {
                    bill1.PendingAcctSN = textBoxRemove1.Text;
                }
            }
            bill1.OrgNO = textBoxOrgNO.Text.Trim();
            billList.Add(bill1);

            return billList;
        }

        private String TestForAcctRecord(StringBuilder result, CoreBizMsgDataBase respData)
        {
            AcctRecordData recorddata = respData as AcctRecordData;

            if (!String.IsNullOrEmpty(recorddata.RPhdrHandler.SEQ_NO))
            {
                result.AppendFormat("此交易流水号:{0}", recorddata.RPhdrHandler.SEQ_NO);
                result.AppendLine();
            }

            if (recorddata.OmsgHandler.NUM_ENT > 0)
            {
                result.AppendFormat("OMSG:{0} item(s)", recorddata.OmsgHandler.NUM_ENT);
                result.AppendLine();
                foreach (OMSG_Item_Handler omsgItem in recorddata.OmsgHandler.OMSGItemList)
                {
                    result.AppendFormat("MOD_ID:{0};MSG_NO:{1};MSG_TYPE:{2};MSG_TEXT:{3};", omsgItem.MOD_ID, omsgItem.MSG_NO, omsgItem.MSG_TYPE, omsgItem.MSG_TEXT);
                    result.AppendLine();
                }
            }
            if (recorddata.OData.OdataItemList.Count > 0)
            {
                result.Append("ODATA:");
                result.AppendLine();
                foreach (AcctRecordODATA_Item item in recorddata.OData.OdataItemList)
                {
                    result.AppendFormat("账号:{0};科目号:{1};币种:{2};借贷标志:{3};金额{4};分录序号:{5}.",item.ACCT, item.GL_NO, item.CCY, item.CD_IND, item.AMT, item.GL_SEQ);
                    result.AppendLine();
                }              
                
            }
            else
            {
                result.Append("ODATA:");
                result.AppendLine();
                result.Append(recorddata.OData.RespOdata);
            }
            if (!String.IsNullOrEmpty(recorddata.SyserrHandler.Message))
            {
                result.AppendFormat("SYSERR:{0}", recorddata.SyserrHandler.Message);
            }

            return result.ToString();
        }
        #endregion

        #region 记账查询
        private void buttonAcctRetrieve_Click(object sender, EventArgs e)
        {
            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            String flowcode = textBoxRetrieveFolwCode.Text.Trim();
            Guid messageID = MsgTransferUtility.RetrieveAccounting(tellno, ouno, new DateTime(2010,11,2), flowcode, ref uLongText);
            MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = false;
            DispatchMsg(msgdata);
        }
        private String TestForRetrieveRecord(StringBuilder result, CoreBizMsgDataBase respData)
        {
            AcctRetrieveData recorddata = respData as AcctRetrieveData;

            if (recorddata == null)
            {
                return result.ToString();
            }

            if (recorddata.OmsgHandler.NUM_ENT > 0)
            {
                result.AppendFormat("OMSG:{0} item(s)", recorddata.OmsgHandler.NUM_ENT);
                result.AppendLine();
                foreach (OMSG_Item_Handler omsgItem in recorddata.OmsgHandler.OMSGItemList)
                {
                    result.AppendFormat("MOD_ID:{0};MSG_NO:{1};MSG_TYPE:{2};MSG_TEXT:{3};", omsgItem.MOD_ID, omsgItem.MSG_NO, omsgItem.MSG_TYPE, omsgItem.MSG_TEXT);
                    result.AppendLine();
                }
            }
            if (recorddata.OData!= null)
            {
                result.Append("ODATA:");
                result.AppendLine();

                if (recorddata.OData.DB_BGO30800 != null && !String.IsNullOrEmpty(recorddata.OData.DB_BGO30800.CoreTradeSN))
                {
                    result.AppendFormat("BGO30800:");
                    result.AppendLine();
                    result.AppendFormat("核心交易流水号:{0};发起方系统代码{1};有会计流水标志{2};外系统跟踪号:{3};被抹账标志:{4}",
                        recorddata.OData.DB_BGO30800.CoreTradeSN, recorddata.OData.DB_BGO30800.SrcSystemCode, recorddata.OData.DB_BGO30800.AcctSNFlag, recorddata.OData.DB_BGO30800.OutsideSN, recorddata.OData.DB_BGO30800.ErasedFlag);
                    result.AppendLine();
                }

                if (recorddata.OData.DB_BGO30801_List.Count > 0)
                {
                    result.AppendFormat("BGO30801:{0} item(s)!", recorddata.OData.DB_BGO30801_List.Count);
                    result.AppendLine();
                }
                int i = 0;
                foreach (AcctRetrieveODATA_DB2 item in recorddata.OData.DB_BGO30801_List)
                {
                    result.AppendFormat("No.{0}:  核心交易流水号:{1};交易账号:{2};户名:{3};现转标志:{4};借贷标志:{5};金额:{6};凭证号码:{7};发起方系统代码:{8};入账配钞标志:{9}.",
                        i, item.CoreTradeSN, item.TradeAccount, item.AccountName, item.CashFalg, item.LoanFlag, item.Amount, item.VoucherNO, item.SrcSystemCode, item.PostingCashFlag);
                    result.AppendLine();
                    i++;
                }

                if (recorddata.OData.DB_BGO30802 != null)
                {
                    result.Append("BGO30802:");
                    result.AppendLine();
                    result.AppendFormat("是否配钞:{0};配钞笔数:{1}.", recorddata.OData.DB_BGO30802.IsQuotaMoney, recorddata.OData.DB_BGO30802.QuotaMoneyCount);
                    result.AppendLine();
                }
                if (recorddata.OData.DB_BGO30803_List.Count > 0)
                {
                    result.AppendFormat("BGO30803:{0} item(s)!", recorddata.OData.DB_BGO30803_List.Count);
                    result.AppendLine();
                }
                i = 0;
                foreach (AcctRetrieveODATA_DB4 item in recorddata.OData.DB_BGO30803_List)
                {
                    result.AppendFormat("No. {0}:  币种:{1};配钞金额:{2}", i, item.Currency, item.QuotaAmount);
                    result.AppendLine();
                    i++;
                }
            }
            
            if (!String.IsNullOrEmpty(recorddata.SyserrHandler.Message))
            {
                result.AppendFormat("SYSERR:{0}", recorddata.SyserrHandler.Message);
            }

            return result.ToString();
        }
        #endregion
        
        #region 抹账
        private void buttonEraseAcct_Click(object sender, EventArgs e)
        {
            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            String eraseno = textBoxEraseAcctNO.Text.Trim();
            Guid messageID = MsgTransferUtility.EraseAccounting(tellno, ouno, new DateTime(2010, 11, 2), eraseno, ref uLongText);
            MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = false;
            DispatchMsg(msgdata);
        }

        private String TestForAcctErase(StringBuilder result, CoreBizMsgDataBase respData)
        {
            AcctEraseData recorddata = respData as AcctEraseData;

            if (recorddata == null)
            {
                return result.ToString();
            }

            if (recorddata.OmsgHandler.NUM_ENT > 0)
            {
                result.AppendFormat("OMSG:{0} item(s)", recorddata.OmsgHandler.NUM_ENT);
                result.AppendLine();
                foreach (OMSG_Item_Handler omsgItem in recorddata.OmsgHandler.OMSGItemList)
                {
                    result.AppendFormat("MOD_ID:{0};MSG_NO:{1};MSG_TYPE:{2};MSG_TEXT:{3};", omsgItem.MOD_ID, omsgItem.MSG_NO, omsgItem.MSG_TYPE, omsgItem.MSG_TEXT);
                    result.AppendLine();
                }
            }
            if (recorddata.OData != null)
            {
                if (recorddata.OData.DB_BY999000 != null)
                {
                    result.Append("ODATA:");
                    result.AppendLine();
                    result.AppendFormat("BY999000:");
                    result.AppendLine();
                    result.AppendFormat("反交易时间:{0};反交易流水号:{1};交易日期:{2};交易时间:{3};交易柜员号:{4};交易码:{5};交易流水号:{6}.",
                        recorddata.OData.DB_BY999000.TIME, recorddata.OData.DB_BY999000.JNO, recorddata.OData.DB_BY999000.Date, recorddata.OData.DB_BY999000.TX_TIME, recorddata.OData.DB_BY999000.TEL_NO,recorddata.OData.DB_BY999000.TX_ID, recorddata.OData.DB_BY999000.TX_JNO);
                    result.AppendLine();
                }

                if (recorddata.OData.DB_BY999001_List.Count > 0)
                {
                    result.AppendFormat("BY999001:{0} item(s)!", recorddata.OData.DB_BY999001_List.Count);
                    result.AppendLine();
                }
                int i = 0;
                foreach (AcctEraseODATA_DB2 item in recorddata.OData.DB_BY999001_List)
                {
                    result.AppendFormat("No.{0}:  账号:{1};户名:{2};借/贷标志:{3};交易金额:{4};交易币种:{5}.",
                        i, item.ACC_NO, item.ACC_NAME, item.DRCR_IND, item.TX_AMT, item.TX_CCY);
                    result.AppendLine();
                    i++;
                }
            }

            if (!String.IsNullOrEmpty(recorddata.SyserrHandler.Message))
            {
                result.AppendFormat("SYSERR:{0}", recorddata.SyserrHandler.Message);
            }

            return result.ToString();
        }
        #endregion
    }
}
