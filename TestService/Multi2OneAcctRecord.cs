using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xQuant.AidSystem.BizDataModel;
using xQuant.AidSystem.CoreMessageData;
using xQuant.AidSystem.Communication;
using System.Net.Sockets;
using xQuant.AidSystem.ClientSyncWrapper;

namespace TestService
{
    public partial class Multi2OneAcctRecordForm : Form
    {
        public Multi2OneAcctRecordForm()
        {
            InitializeComponent();
        }

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
                        else if (respData is AcctRecordMulti2OneData)
                        {
                            TestForMulti2One(result, (CoreBizMsgDataBase)respData);
                            textBoxRespAcct.Text = result.ToString();
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

        #region Event method
        private void radioButtonRemove_CheckedChanged(object sender, EventArgs e)
        {
            textBoxRemove.ReadOnly = !radioButtonRemove.Checked;
            textBoxRemove.Text = "";
        }

        private void radioButtonRemove1_CheckedChanged(object sender, EventArgs e)
        {
            textBoxRemove1.ReadOnly = !radioButtonRemove1.Checked;
            textBoxRemove1.Text = "";
        }

        private void radioButtonRemove2_CheckedChanged(object sender, EventArgs e)
        {
            textBoxRemove2.ReadOnly = !radioButtonRemove2.Checked;
            textBoxRemove2.Text = "";
        }

        private void radioButtonRemove3_CheckedChanged(object sender, EventArgs e)
        {
            textBoxRemove3.ReadOnly = !radioButtonRemove3.Checked;
            textBoxRemove3.Text = "";
        }

        private void radioButtonRemove4_CheckedChanged(object sender, EventArgs e)
        {
            textBoxRemove4.ReadOnly = !radioButtonRemove4.Checked;
            textBoxRemove4.Text = "";
        }

        private void radioButtonRemove5_CheckedChanged(object sender, EventArgs e)
        {
            textBoxRemove5.ReadOnly = !radioButtonRemove5.Checked;
            textBoxRemove5.Text = "";
        }

        private void radioButtonRemove6_CheckedChanged(object sender, EventArgs e)
        {
            textBoxRemove6.ReadOnly = !radioButtonRemove6.Checked;
            textBoxRemove6.Text = "";
        }

        private void radioButtonRemove7_CheckedChanged(object sender, EventArgs e)
        {
            textBoxRemove7.ReadOnly = !radioButtonRemove7.Checked;
            textBoxRemove7.Text = "";
        }

        private void radioButtonRemove8_CheckedChanged(object sender, EventArgs e)
        {
            textBoxRemove8.ReadOnly = !radioButtonRemove8.Checked;
            textBoxRemove8.Text = "";
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
            Guid messageID = Guid.Empty;
            string outmsg="";
            if (rdoM2M.Checked)
            {
                CoreAcctResult acctResult = null;
                //messageID = MsgTransferUtility.AccountingRecord(tellno, ouno, DateTime.Now, flowno, list, ref uLongText);
                if (AidSysClientSyncWrapper.CoreAcctRecord(ouno, tellno, DateTime.Now, flowno, list, out acctResult, out outmsg))
                {
                   
                }
                else
                {
                    MessageBox.Show(outmsg);
                }
            }
            else if (rdoMultiLend.Checked)
            {
                messageID = MsgTransferUtility.AcctRecordMultiLendOneLoan(tellno, ouno, DateTime.Now, flowno, list, ref uLongText);
            }
            else
            {
                messageID = MsgTransferUtility.AcctRecordMultiLoanOneLend(tellno, ouno, DateTime.Now, flowno, list, ref uLongText);
            }

            MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = false;
            DispatchMsg(msgdata);
        }
        #endregion

        private List<CoreBillRecord> GetAccoutingEntry()
        {
            List<CoreBillRecord> billList = new List<CoreBillRecord>();
            #region 分录1
            if (!string.IsNullOrEmpty(textBoxInterAcctSN.Text.Trim()) && !string.IsNullOrEmpty(textBoxSubject.Text.Trim()) || !string.IsNullOrEmpty(textBoxTradeAcct.Text.Trim()))
            {
                CoreBillRecord bill = new CoreBillRecord();
                bill.InnerSN = (billList.Count + 1).ToString();
                bill.TradeAcctNO = textBoxTradeAcct.Text.Trim(); //交易客户账
                if (bill.TradeAcctNO.Length > 0)
                {
                    bill.IsNote = "1"; // 钞汇属性
                }
                bill.Currency = textBoxCurrency.Text.Trim();
                bill.InterAcctSN = textBoxInterAcctSN.Text.Trim();
                bill.Subject = textBoxSubject.Text.Trim();
                bill.OptAcct = textBoxTradeAcct.Text.Trim();
                bill.TradeMoney = textBoxMoney.Text.Trim();
                bill.Opt = radioButtonBorrow.Checked ? "1" : "2";
                string flag = "1";
                if (radioButtonFlagRed.Checked)
                {
                    flag = "2";
                }
                else if (radioButtonFlagBlue.Checked)
                {
                    flag = "3";
                }
                bill.RedBlueFlag = flag;

                if (!radioButtonNor.Checked)
                {
                    bill.IsEliminateFlag = radioButtonRecord.Checked ? "1" : "2";
                    if (radioButtonRemove.Checked)
                    {
                        bill.PendingAcctSN = textBoxRemove.Text;
                    }
                }
                if (string.IsNullOrEmpty(textBoxOppOrg.Text.Trim()))
                {
                    bill.OrgNO = textBoxOrgNO.Text.Trim();
                }
                else
                {
                    bill.OrgNO = textBoxOppOrg.Text.Trim();
                }
                billList.Add(bill);
            }
            #endregion

            #region 分录2
            //记账2
            if (!string.IsNullOrEmpty(textBoxInterAcctSN1.Text.Trim()) && !string.IsNullOrEmpty(textBoxSubject1.Text.Trim()))
            {
                CoreBillRecord bill1 = new CoreBillRecord();
                bill1.InnerSN = (billList.Count + 1).ToString();
                //bill1.TradeAcct = textBoxTradeAcct1.Text.Trim();
                bill1.Currency = textBoxCurrency1.Text.Trim();
                bill1.InterAcctSN = textBoxInterAcctSN1.Text.Trim();
                bill1.Subject = textBoxSubject1.Text.Trim();
                bill1.OptAcct = textBoxOptAcct1.Text.Trim();
                bill1.TradeMoney = textBoxMoney1.Text.Trim();
                bill1.Opt = radioButtonBorrow1.Checked ? "1" : "2";
                string flag = "1";
                if (radioButtonFlagRed1.Checked)
                {
                    flag = "2";
                }
                else if (radioButtonFlagBlue1.Checked)
                {
                    flag = "3";
                }
                bill1.RedBlueFlag = flag;
                //bill.Opt = "2";
                if (!radioButtonNor1.Checked)
                {
                    bill1.IsEliminateFlag = radioButtonRecord1.Checked ? "1" : "2";
                    if (radioButtonRemove1.Checked)
                    {
                        bill1.PendingAcctSN = textBoxRemove1.Text;
                    }
                }
                if (string.IsNullOrEmpty(textBoxOppOrg1.Text.Trim()))
                {
                    bill1.OrgNO = textBoxOrgNO.Text.Trim();
                }
                else
                {
                    bill1.OrgNO = textBoxOppOrg1.Text.Trim();
                }
                billList.Add(bill1);
            }
            #endregion

            #region 分录3
            if (!string.IsNullOrEmpty(textBoxInterAcctSN2.Text.Trim()) && !string.IsNullOrEmpty(textBoxSubject2.Text.Trim()))
            {
                CoreBillRecord bill = new CoreBillRecord();
                bill.InnerSN = (billList.Count + 1).ToString();
                //bill1.TradeAcct = textBoxTradeAcct1.Text.Trim();
                bill.Currency = textBoxCurrency2.Text.Trim();
                bill.InterAcctSN = textBoxInterAcctSN2.Text.Trim();
                bill.Subject = textBoxSubject2.Text.Trim();
                bill.OptAcct = textBoxOptAcct2.Text.Trim();
                bill.TradeMoney = textBoxMoney2.Text.Trim();
                bill.Opt = radioButtonBorrow2.Checked ? "1" : "2";
                string flag = "1";
                if (radioButtonFlagRed2.Checked)
                {
                    flag = "2";
                }
                else if (radioButtonFlagBlue2.Checked)
                {
                    flag = "3";
                }
                bill.RedBlueFlag = flag;
                //bill.Opt = "2";
                if (!radioButtonNor2.Checked)
                {
                    bill.IsEliminateFlag = radioButtonRecord2.Checked ? "1" : "2";
                    if (radioButtonRemove2.Checked)
                    {
                        bill.PendingAcctSN = textBoxRemove2.Text;
                    }
                }
                if (string.IsNullOrEmpty(textBoxOppOrg2.Text.Trim()))
                {
                    bill.OrgNO = textBoxOrgNO.Text.Trim();
                }
                else
                {
                    bill.OrgNO = textBoxOppOrg2.Text.Trim();
                }
                billList.Add(bill);
            }
            #endregion

            #region 分录4
            if (!string.IsNullOrEmpty(textBoxInterAcctSN3.Text.Trim()) && !string.IsNullOrEmpty(textBoxSubject3.Text.Trim()))
            {
                CoreBillRecord bill = new CoreBillRecord();
                bill.InnerSN = (billList.Count + 1).ToString();
                bill.Currency = textBoxCurrency3.Text.Trim();
                bill.InterAcctSN = textBoxInterAcctSN3.Text.Trim();
                bill.Subject = textBoxSubject3.Text.Trim();
                bill.OptAcct = textBoxOptAcct3.Text.Trim();
                bill.TradeMoney = textBoxMoney3.Text.Trim();
                bill.Opt = radioButtonBorrow3.Checked ? "1" : "2";
                string flag = "1";
                if (radioButtonFlagRed3.Checked)
                {
                    flag = "2";
                }
                else if (radioButtonFlagBlue3.Checked)
                {
                    flag = "3";
                }
                bill.RedBlueFlag = flag;
                if (!radioButtonNor3.Checked)
                {
                    bill.IsEliminateFlag = radioButtonRecord3.Checked ? "1" : "2";
                    if (radioButtonRemove3.Checked)
                    {
                        bill.PendingAcctSN = textBoxRemove3.Text;
                    }
                }
                if (string.IsNullOrEmpty(textBoxOppOrg3.Text.Trim()))
                {
                    bill.OrgNO = textBoxOrgNO.Text.Trim();
                }
                else
                {
                    bill.OrgNO = textBoxOppOrg3.Text.Trim();
                }
                billList.Add(bill);
            }
            #endregion
            #region 分录5
            if (!string.IsNullOrEmpty(textBoxInterAcctSN4.Text.Trim()) && !string.IsNullOrEmpty(textBoxSubject4.Text.Trim()))
            {
                CoreBillRecord bill = new CoreBillRecord();
                bill.InnerSN = (billList.Count + 1).ToString();
                bill.Currency = textBoxCurrency4.Text.Trim();
                bill.InterAcctSN = textBoxInterAcctSN4.Text.Trim();
                bill.Subject = textBoxSubject4.Text.Trim();
                bill.OptAcct = textBoxOptAcct4.Text.Trim();
                bill.TradeMoney = textBoxMoney4.Text.Trim();
                bill.Opt = radioButtonBorrow4.Checked ? "1" : "2";
                string flag = "1";
                if (radioButtonFlagRed4.Checked)
                {
                    flag = "2";
                }
                else if (radioButtonFlagBlue4.Checked)
                {
                    flag = "3";
                }
                bill.RedBlueFlag = flag;
                if (!radioButtonNor4.Checked)
                {
                    bill.IsEliminateFlag = radioButtonRecord4.Checked ? "1" : "2";
                    if (radioButtonRemove4.Checked)
                    {
                        bill.PendingAcctSN = textBoxRemove4.Text;
                    }
                }
                if (string.IsNullOrEmpty(textBoxOppOrg4.Text.Trim()))
                {
                    bill.OrgNO = textBoxOrgNO.Text.Trim();
                }
                else
                {
                    bill.OrgNO = textBoxOppOrg4.Text.Trim();
                }
                billList.Add(bill);
            }
            #endregion

            #region 分录6
            if (!string.IsNullOrEmpty(textBoxInterAcctSN5.Text.Trim()) && !string.IsNullOrEmpty(textBoxSubject5.Text.Trim()))
            {
                CoreBillRecord bill = new CoreBillRecord();
                bill.InnerSN = (billList.Count + 1).ToString();
                bill.Currency = textBoxCurrency5.Text.Trim();
                bill.InterAcctSN = textBoxInterAcctSN5.Text.Trim();
                bill.Subject = textBoxSubject5.Text.Trim();
                bill.OptAcct = textBoxOptAcct5.Text.Trim();
                bill.TradeMoney = textBoxMoney5.Text.Trim();
                bill.Opt = radioButtonBorrow5.Checked ? "1" : "2";
                string flag = "1";
                if (radioButtonFlagRed5.Checked)
                {
                    flag = "2";
                }
                else if (radioButtonFlagBlue5.Checked)
                {
                    flag = "3";
                }
                bill.RedBlueFlag = flag;
                if (!radioButtonNor5.Checked)
                {
                    bill.IsEliminateFlag = radioButtonRecord5.Checked ? "1" : "2";
                    if (radioButtonRemove5.Checked)
                    {
                        bill.PendingAcctSN = textBoxRemove5.Text;
                    }
                }
                if (string.IsNullOrEmpty(textBoxOppOrg5.Text.Trim()))
                {
                    bill.OrgNO = textBoxOrgNO.Text.Trim();
                }
                else
                {
                    bill.OrgNO = textBoxOppOrg5.Text.Trim();
                }
                billList.Add(bill);
            }
            #endregion
            #region 分录7
            if (!string.IsNullOrEmpty(textBoxInterAcctSN6.Text.Trim()) && !string.IsNullOrEmpty(textBoxSubject6.Text.Trim()))
            {
                CoreBillRecord bill = new CoreBillRecord();
                bill.InnerSN = (billList.Count + 1).ToString();
                bill.Currency = textBoxCurrency6.Text.Trim();
                bill.InterAcctSN = textBoxInterAcctSN6.Text.Trim();
                bill.Subject = textBoxSubject6.Text.Trim();
                bill.OptAcct = textBoxOptAcct6.Text.Trim();
                bill.TradeMoney = textBoxMoney6.Text.Trim();
                bill.Opt = radioButtonBorrow6.Checked ? "1" : "2";

                if (!radioButtonNor6.Checked)
                {
                    bill.IsEliminateFlag = radioButtonRecord6.Checked ? "1" : "2";
                    if (radioButtonRemove6.Checked)
                    {
                        bill.PendingAcctSN = textBoxRemove6.Text;
                    }
                }
                bill.OrgNO = textBoxOrgNO.Text.Trim();
                billList.Add(bill);
            }
            #endregion

            #region 分录8
            if (!string.IsNullOrEmpty(textBoxInterAcctSN7.Text.Trim()) && !string.IsNullOrEmpty(textBoxSubject7.Text.Trim()))
            {
                CoreBillRecord bill = new CoreBillRecord();
                bill.InnerSN = (billList.Count + 1).ToString();
                bill.Currency = textBoxCurrency7.Text.Trim();
                bill.InterAcctSN = textBoxInterAcctSN7.Text.Trim();
                bill.Subject = textBoxSubject7.Text.Trim();
                bill.OptAcct = textBoxOptAcct7.Text.Trim();
                bill.TradeMoney = textBoxMoney7.Text.Trim();
                bill.Opt = radioButtonBorrow7.Checked ? "1" : "2";
                if (!radioButtonNor7.Checked)
                {
                    bill.IsEliminateFlag = radioButtonRecord7.Checked ? "1" : "2";
                    if (radioButtonRemove7.Checked)
                    {
                        bill.PendingAcctSN = textBoxRemove7.Text;
                    }
                }
                bill.OrgNO = textBoxOrgNO.Text.Trim();
                billList.Add(bill);
            }
            #endregion

            #region 分录9
            if (!string.IsNullOrEmpty(textBoxInterAcctSN8.Text.Trim()) && !string.IsNullOrEmpty(textBoxSubject8.Text.Trim()))
            {
                CoreBillRecord bill = new CoreBillRecord();
                bill.InnerSN = (billList.Count + 1).ToString();
                bill.Currency = textBoxCurrency8.Text.Trim();
                bill.InterAcctSN = textBoxInterAcctSN8.Text.Trim();
                bill.Subject = textBoxSubject8.Text.Trim();
                bill.OptAcct = textBoxOptAcct8.Text.Trim();
                bill.TradeMoney = textBoxMoney8.Text.Trim();
                bill.Opt = radioButtonBorrow8.Checked ? "1" : "2";
                if (!radioButtonNor8.Checked)
                {
                    bill.IsEliminateFlag = radioButtonRecord8.Checked ? "1" : "2";
                    if (radioButtonRemove8.Checked)
                    {
                        bill.PendingAcctSN = textBoxRemove8.Text;
                    }
                }
                bill.OrgNO = textBoxOrgNO.Text.Trim();
                billList.Add(bill);
            }
            #endregion

            return billList;
        }
        private void TestForAcctRecord(StringBuilder result, CoreBizMsgDataBase respData)
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
                result.Append("返回的分录:");
                result.AppendLine();
                foreach (AcctRecordODATA_Item item in recorddata.OData.OdataItemList)
                {
                    result.AppendFormat("账号:{0};科目号:{1};币种:{2};借贷标志:{3};金额{4};分录序号:{5}.", item.ACCT, item.GL_NO, item.CCY, item.CD_IND, item.AMT, item.GL_SEQ);
                    result.AppendLine();
                }

            }
            if (recorddata.OData.OdataPendingList.Count > 0)
            {
                result.Append("返回的挂账记录:");
                result.AppendLine();
                foreach (AcctRecordODATA_PendingItem item in recorddata.OData.OdataPendingList)
                {
                    result.AppendFormat("资金业务系统流水号{0};套内序号:{1};挂账账号:{2};挂账序号:{3};挂账金额:{4}.",item.FlowNO, item.InnerSN, item.PendingAccount, item.PendingSN, item.PendingAmount);
                    result.AppendLine();
                }
            }

            if (recorddata.OData.OdataItemList.Count == 0 && recorddata.OData.OdataPendingList.Count == 0)
            {
                result.Append("ODATA:");
                result.AppendLine();
                result.Append(recorddata.OData.RespOdata);
            }
            if (!String.IsNullOrEmpty(recorddata.SyserrHandler.Message))
            {
                result.AppendFormat("SYSERR:{0}", recorddata.SyserrHandler.Message);
            }
        }
        private void TestForMulti2One(StringBuilder result, CoreBizMsgDataBase respData)
        {
            AcctRecordMulti2OneData recorddata = respData as AcctRecordMulti2OneData;

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
                result.Append("返回的分录:");
                result.AppendLine();
                foreach (AcctRecordMulti2OneODATA_Item item in recorddata.OData.OdataItemList)
                {
                    result.AppendFormat("账号:{0};科目号:{1};币种:{2};借贷标志:{3};金额{4};分录序号:{5}.", item.ACCT, item.GL_NO, item.CCY, item.CD_IND, item.AMT, item.GL_SEQ);
                    result.AppendLine();
                }

            }
            if (recorddata.OData.OdataPendingList.Count > 0)
            {
                result.Append("返回的挂账记录:");
                result.AppendLine();
                foreach (AcctRecordMulti2OneODATA_PendingItem item in recorddata.OData.OdataPendingList)
                {
                    result.AppendFormat("套内序号:{0};挂账账号:{1};挂账序号:{2};挂账金额:{3}.", item.InnerSN, item.PendingAccount, item.PendingSN, item.PendingAmount);
                    result.AppendLine();
                }
            }

            if (recorddata.OData.OdataItemList.Count == 0 && recorddata.OData.OdataPendingList.Count == 0)
            {
                result.Append("ODATA:");
                result.AppendLine();
                result.Append(recorddata.OData.RespOdata);
            }
            if (!String.IsNullOrEmpty(recorddata.SyserrHandler.Message))
            {
                result.AppendFormat("SYSERR:{0}", recorddata.SyserrHandler.Message);
            }
        }


   
  
    }
}
