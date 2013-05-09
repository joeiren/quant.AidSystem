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
using xQuant.AidSystem.CoreMessageData;
using System.Net.Sockets;
using xQuant.AidSystem.Communication;

namespace TestService
{
    public partial class InterBankDelete : Form
    {
        public InterBankDelete()
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

                        if (respData is InterBankDeleteAcctData)
                        {
                            ForTestReturn( (InterBankDeleteAcctData)respData, result);
                            //textBoxRespAcct.Text = result.ToString();
                        }
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
                MessageBox.Show(ex.Message.ToString());
            }
        }
        #endregion

        private void InterBankDelete_Load(object sender, EventArgs e)
        {

        }
        private void ForTestReturn(InterBankDeleteAcctData ibData, StringBuilder result)
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
            MessageBox.Show(result.ToString());

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RegularResult result = AidSysClientSyncWrapper.InterBankDeleteAccountOrExtract(机构号.Text.Trim(), "8010130", DateTime.Parse(业务交易日.Text), GetModel(true));
            if (!result.Succeed)
            {
                MessageBox.Show(result.ExceptionMsg);
            }
            else
            {
                MessageBox.Show("操作成功");
            }
        }

        private InterBankDeleteAcctInfo GetModel(bool isAdd)
        {
            InterBankDeleteAcctInfo info = new InterBankDeleteAcctInfo();
            //info.ACCOUNT_DATE = DateTime.Parse(会计日期.Text);
            info.AMOUNT = double.Parse(交易金额.Text);
            //info.APPROVE_ORGNAZTION = 审批机构.Text;
            //info.APPROVE_TELLER = "8010130";
            //info.HANDLE_ORGNAZTION = 经办机构.Text;
            //info.HANDLE_TELLER = "8010130";
            info.TERM_TYPE = 业务类型.Text == "1"? AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE.Current : AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE.Fixed;
            info.NOTICE_NO = 通知单编号.Text;
            info.OPERATION_TYPE = isAdd ? AidTypeDefine.INTER_BANK_OPERATION_TYPE.CreateNew : AidTypeDefine.INTER_BANK_OPERATION_TYPE.Cancel;
            //info.TELLER_NO = 柜员号.Text;
            //info.TRADE_DATE = DateTime.Parse(业务交易日.Text);
            //info.TRADE_ORGNAZTION = 机构号.Text;
            info.ACCOUNT = 账号.Text;
            info.INTEREST_ACCOUNT = 收息账号.Text.Trim();
            info.INTEREST = double.Parse(利息金额.Text);
            info.NOTICE_TYPE = 通知单类型.Text == "2" ? AidTypeDefine.INTER_BANK_NOTICE_TYPE.DeleteAccount : AidTypeDefine.INTER_BANK_NOTICE_TYPE.PartWithdraw;

            info.DETAILS=new List<InterestAccrualInfo>();
            info.DETAILS = GetDetailInfos();
            //InterestAccrualInfo detail=new InterestAccrualInfo();
            //detail.CHARGE_NUMBER = double.Parse(积数1.Text);
            //detail.INTEREST = double.Parse(利息1.Text);
            //detail.MATURITY_DATE = DateTime.Parse(到期日期1.Text);
            //detail.RATE = double.Parse(利率1.Text);
            //detail.VALUE_DATE = DateTime.Parse(起息日期1.Text);
            //info.DETAILS.Add(detail);

            return info;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            RegularResult result = AidSysClientSyncWrapper.InterBankDeleteAccountOrExtract(机构号.Text.Trim(), "8010130",DateTime.Parse(业务交易日.Text), GetModel(false));
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
                Guid messageID = MsgTransferUtility.DeleteAccount(机构号.Text.Trim(), "8010130", DateTime.Parse(业务交易日.Text), GetModel(true), ref codemsg);

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

        private void btnCancelOfCustom_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] codemsg = null;
                Guid messageID = MsgTransferUtility.DeleteAccount(机构号.Text.Trim(), "8010130", DateTime.Parse(业务交易日.Text), GetModel(false), ref codemsg);

                MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core };

                msgdata.ReqPackageList.Enqueue(new PackageData(1, codemsg));
                msgdata.IsMultiPackage = false;
                DispatchMsg(msgdata);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private List<InterestAccrualInfo> GetDetailInfos()
        {
            int count = 0;
            if (string.IsNullOrEmpty(this.textAICount.Text.Trim()) || !int.TryParse(this.textAICount.Text.Trim(), out count))
            {
                return null;
            }            
            List<InterestAccrualInfo> list = new List<InterestAccrualInfo>();
            for (var i = 0; i < count; i++)
            {
                InterestAccrualInfo info = new InterestAccrualInfo();
                Control[] controls = this.Controls.Find("起息日期" + (i + 1).ToString(), true);
                if (controls != null && controls.Length > 0)
                {
                    info.VALUE_DATE = DateTime.Parse(controls[0].Text);
                }
                controls = this.Controls.Find("到期日期" + (i + 1).ToString(), true);
                if (controls != null && controls.Length > 0)
                {
                    info.MATURITY_DATE = DateTime.Parse(controls[0].Text);
                }
                controls = this.Controls.Find("利率" + (i + 1).ToString(), true);
                if (controls != null && controls.Length > 0)
                {
                    info.RATE = double.Parse(controls[0].Text);
                }

                controls = this.Controls.Find("积数" + (i + 1).ToString(), true);
                if (controls != null && controls.Length > 0)
                {
                    info.CHARGE_NUMBER = double.Parse(controls[0].Text);
                }
                controls = this.Controls.Find("利息" + (i + 1).ToString(), true);
                if (controls != null && controls.Length > 0)
                {
                    info.INTEREST = double.Parse(controls[0].Text);
                }
                list.Add(info);
            }
            return list;
        }
        

    }
}
