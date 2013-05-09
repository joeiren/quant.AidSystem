using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xQuant.AidSystem.ClientSyncWrapper;
using xQuant.AidSystem.BizDataModel;
using System.Net.Sockets;
using xQuant.AidSystem.Communication;
using xQuant.AidSystem.CoreMessageData;

namespace TestService
{
    public partial class InterBankSettleForm : Form
    {
        public InterBankSettleForm()
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

                        if (respData is CoreBizMsgDataBase)
                        {
                            TestForReturn(respData as CoreBizMsgDataBase, result);
                        }
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
            textBoxPrepareResult.Text = result.ToString();
        }
        #endregion

        private void buttonSet_Click(object sender, EventArgs e)
        {
            byte[] rebytes = null;
            //Guid messageID = MsgTransferUtility.InterBankInterestSettle("8010001", "801000", DateTime.Now, GetSettleInfo(), ref _byteCollection);
            Guid messageID = MsgTransferUtility.InterBankInterestSettle("8010001", "801000", DateTime.Now, GetTestSettleInfoFix(), ref rebytes);

            MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core };
            short packageid = 1;
            foreach (var bytes in _byteCollection)
            {
                msgdata.ReqPackageList.Enqueue(new PackageData(packageid++, bytes));
            }
            //msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = true;
            DispatchMsg(msgdata);
        }

        public InterBankInterestInfo GetTestSettleInfoFix()
        {
            DateTime accountDate = new DateTime(2011, 10, 22);
            int settlecount = 2;//int.Parse(总比数.Text.Trim());
            InterBankInterestInfo info = new InterBankInterestInfo();
            InterBankInterestSummaryInfo summaryinfo = new InterBankInterestSummaryInfo();
            summaryinfo.KPSN = ClientUtility.GenerateBatchNO(accountDate);
            summaryinfo.BatchName = string.IsNullOrEmpty(批量名称.Text.Trim()) ? "定期" : 批量名称.Text.Trim();
            summaryinfo.TotalCount = settlecount;

            #region 1定期
            double totalInterest1 = 0.0;
            InterBankInterestSettleInfo settinfo = new InterBankInterestSettleInfo();
            settinfo.AccountNO = "201000081389961";//活期
            settinfo.InterestAccount = "203000016308272"; //定期
            settinfo.RecordDate = accountDate;
            settinfo.TermFlag = AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE.Fixed;
            settinfo.ValueDate = new DateTime(2011, 10, 20);
            DateTime beg = new DateTime(2011, 10, 20);
            
            InterBankInterestAIInfo aiinfo = new InterBankInterestAIInfo();
            aiinfo.BeginDate = beg;
            aiinfo.EndDate = new DateTime(2011, 10, 21);
            aiinfo.Aggregate = 29000;
            aiinfo.Rate = 0.00522;
            aiinfo.Interest = Math.Round(aiinfo.Aggregate * aiinfo.Rate/360, 2, MidpointRounding.AwayFromZero);
            totalInterest1 += aiinfo.Interest;
            settinfo.AICollection.Add(aiinfo);
            //beg = aiinfo.EndDate.AddDays(1);
            
            settinfo.Interest = totalInterest1;
            info.SettleCollection.Add(settinfo);
            #endregion

            #region 2定期
            double totalInterest2 = 0.0;
            InterBankInterestSettleInfo settinfo2 = new InterBankInterestSettleInfo();
            settinfo2.AccountNO = "201000081388801";//活期
            settinfo2.InterestAccount = "203000016306507";
            settinfo2.RecordDate = accountDate;
            settinfo2.TermFlag = AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE.Fixed;
            settinfo2.ValueDate = new DateTime(2011, 9, 21);
            DateTime beg2 = new DateTime(2011, 04, 15);
            
            InterBankInterestAIInfo aiinfo2 = new InterBankInterestAIInfo();
            aiinfo2.BeginDate = new DateTime(2011, 9, 21);
            aiinfo2.EndDate = new DateTime(2011, 10, 21);
            aiinfo2.Aggregate = 20000;
            aiinfo2.Rate = 0.02;
            aiinfo2.Interest = Math.Round(aiinfo2.Aggregate * aiinfo2.Rate / 360, 2, MidpointRounding.AwayFromZero);
            totalInterest2 += aiinfo2.Interest;
            settinfo2.AICollection.Add(aiinfo2);
            //beg2 = aiinfo2.EndDate.AddDays(1);
            
            settinfo2.Interest = totalInterest2;
            info.SettleCollection.Add(settinfo2);
            #endregion

            summaryinfo.TotalAmount = totalInterest1 + totalInterest2;
            info.SummaryInfo = summaryinfo;
            return info;
        }

        public InterBankInterestInfo GetTestSettleInfoCurrent()
        {
            DateTime accountDate = new DateTime(2011, 10, 22);
            int settlecount = 4;//int.Parse(总比数.Text.Trim());
            //int aicount = 2;
            InterBankInterestInfo info = new InterBankInterestInfo();
            InterBankInterestSummaryInfo summaryinfo = new InterBankInterestSummaryInfo();
            summaryinfo.KPSN = ClientUtility.GenerateBatchNO(accountDate);
            summaryinfo.BatchName = string.IsNullOrEmpty(批量名称.Text.Trim()) ? "活期" : 批量名称.Text.Trim();
            summaryinfo.TotalCount = settlecount;

            
            #region 3活期
            double totalInterest3 = 0.0;
            InterBankInterestSettleInfo settinfo3 = new InterBankInterestSettleInfo();
            settinfo3.AccountNO = "201000081387903";//活期
            settinfo3.InterestAccount = "201000081387903";
            settinfo3.RecordDate = accountDate;
            settinfo3.TermFlag = AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE.Current;
            settinfo3.ValueDate = new DateTime(2011, 9, 21);
            DateTime beg3 = new DateTime(2011, 9, 21);
            
            InterBankInterestAIInfo aiinfo3 = new InterBankInterestAIInfo();
            aiinfo3.BeginDate = beg3;
            aiinfo3.EndDate = new DateTime(2011,9, 30);
            aiinfo3.Aggregate = 1000069.50;
            aiinfo3.Rate = 0.02;
            aiinfo3.Interest = Math.Round(aiinfo3.Aggregate * aiinfo3.Rate / 360, 2, MidpointRounding.AwayFromZero);
            totalInterest3 += aiinfo3.Interest;
            settinfo3.AICollection.Add(aiinfo3);

            InterBankInterestAIInfo aiinfo31 = new InterBankInterestAIInfo();
            aiinfo31.BeginDate = new DateTime(2011, 10, 1); 
            aiinfo31.EndDate = new DateTime(2011, 10, 20);
            aiinfo31.Aggregate = 2000139.00 ;
            aiinfo31.Rate = 0.024;
            aiinfo31.Interest = Math.Round(aiinfo31.Aggregate * aiinfo31.Rate / 360, 2, MidpointRounding.AwayFromZero);
            totalInterest3 += aiinfo31.Interest;
            settinfo3.AICollection.Add(aiinfo31);

            settinfo3.Interest = totalInterest3;
            info.SettleCollection.Add(settinfo3);
            #endregion

            #region 4
            double totalInterest4 = 0.0;
            InterBankInterestSettleInfo settinfo4 = new InterBankInterestSettleInfo();
            settinfo4.AccountNO = "201000081388053";//活期
            settinfo4.InterestAccount = "201000081388053";
            settinfo4.RecordDate = accountDate;
            settinfo4.TermFlag = AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE.Current;
            settinfo4.ValueDate = new DateTime(2011, 9, 21);
            DateTime beg4 = new DateTime(2011, 9, 21);
            
            InterBankInterestAIInfo aiinfo4 = new InterBankInterestAIInfo();
            aiinfo4.BeginDate = beg4;
            aiinfo4.EndDate = new DateTime(2011, 9, 30);
            aiinfo4.Aggregate = 1000106.5;
            aiinfo4.Rate = 0.033333;
            aiinfo4.Interest = Math.Round(aiinfo4.Aggregate * aiinfo4.Rate / 360, 2, MidpointRounding.AwayFromZero);
            totalInterest4 += aiinfo4.Interest;
            settinfo4.AICollection.Add(aiinfo4);

            InterBankInterestAIInfo aiinfo41 = new InterBankInterestAIInfo();
            aiinfo41.BeginDate = new DateTime(2011, 10, 1);
            aiinfo41.EndDate = new DateTime(2011, 10, 20);
            aiinfo41.Aggregate = 2000213;
            aiinfo41.Rate = 0.035;
            aiinfo41.Interest = Math.Round(aiinfo41.Aggregate * aiinfo41.Rate / 360, 2, MidpointRounding.AwayFromZero);
            totalInterest4 += aiinfo41.Interest;
            settinfo4.AICollection.Add(aiinfo41);

            settinfo4.Interest = totalInterest4;
            info.SettleCollection.Add(settinfo4);
            #endregion

            #region 5
            double totalInterest5 = 0.0;
            InterBankInterestSettleInfo settinfo5 = new InterBankInterestSettleInfo();
            settinfo5.AccountNO = "201000081389113";//活期
            settinfo5.InterestAccount = "201000081389113";
            settinfo5.RecordDate = accountDate;
            settinfo5.TermFlag = AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE.Current;
            settinfo5.ValueDate = new DateTime(2011, 9, 21);
            DateTime beg5 = new DateTime(2011, 9, 21);

            InterBankInterestAIInfo aiinfo5 = new InterBankInterestAIInfo();
            aiinfo5.BeginDate = beg5;
            aiinfo5.EndDate = new DateTime(2011, 10, 20);
            aiinfo5.Aggregate = 1500000;
            aiinfo5.Rate = 0.0072;
            aiinfo5.Interest = Math.Round(aiinfo5.Aggregate * aiinfo5.Rate / 360, 2, MidpointRounding.AwayFromZero);
            totalInterest5 += aiinfo5.Interest;
            settinfo5.AICollection.Add(aiinfo5);

            settinfo5.Interest = totalInterest5;
            info.SettleCollection.Add(settinfo5);
            #endregion

            #region 6
            double totalInterest6 = 0.0;
            InterBankInterestSettleInfo settinfo6 = new InterBankInterestSettleInfo();
            settinfo6.AccountNO = "201000081389406";//活期
            settinfo6.InterestAccount = "201000081389406";
            settinfo6.RecordDate = accountDate;
            settinfo6.TermFlag = AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE.Current;
            settinfo6.ValueDate = new DateTime(2011, 9, 22);
            DateTime beg6 = new DateTime(2011, 9, 22);

            InterBankInterestAIInfo aiinfo6 = new InterBankInterestAIInfo();
            aiinfo6.BeginDate = beg6;
            aiinfo6.EndDate = new DateTime(2011, 10, 20);
            aiinfo6.Aggregate = 4686029;
            aiinfo6.Rate = 0;
            aiinfo6.Interest = Math.Round(aiinfo6.Aggregate * aiinfo6.Rate / 360, 2, MidpointRounding.AwayFromZero);
            totalInterest6 += aiinfo6.Interest;
            settinfo6.AICollection.Add(aiinfo6);

            settinfo6.Interest = totalInterest6;
            info.SettleCollection.Add(settinfo6);
            #endregion

            summaryinfo.TotalAmount = totalInterest3 + totalInterest4 + totalInterest5 + totalInterest6;
            info.SummaryInfo = summaryinfo;
            return info;
        }

        private void buttonPrepareMQ_Click(object sender, EventArgs e)
        {
            List<InterBankPreparedInfo> list = GetPreparedList();
            if (list.Count == 0)
            {
                MessageBox.Show("选择要记账的分录！", "记账提示", MessageBoxButtons.OK);
                return;
            }
            RegularResult result = AidSysClientSyncWrapper.InterBankAssetsPrepared(柜员号.Text.Trim(), 机构号.Text.Trim(), DateTime.Parse(交易日期.Text), list);
            if (result.Succeed)
            {
                textBoxPrepareResult.Text = "上送计提成功";
            }
            else
            {
                textBoxPrepareResult.Text = result.ExceptionMsg;
            }
        }

        private void buttonPrepare_Click(object sender, EventArgs e)
        {
            List<InterBankPreparedInfo> list = GetPreparedList();
            if (list.Count == 0)
            {
                MessageBox.Show("选择要记账的分录！", "记账提示", MessageBoxButtons.OK);
                 list = GetTestPreparedList();
            }
            
            
            byte[] buffer = null;

            Guid messageID = MsgTransferUtility.InterBankAssetsPrepared(柜员号.Text.Trim(), 机构号.Text.Trim(), DateTime.Parse(交易日期.Text), list, ref buffer);

            MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core };
            //short packageid = 1;
            //foreach (var bytes in _byteCollection)
            //{
            //    msgdata.ReqPackageList.Enqueue(new PackageData(packageid++, bytes));
            //}
            msgdata.ReqPackageList.Enqueue(new PackageData(1, buffer));
            msgdata.IsMultiPackage = false;
            DispatchMsg(msgdata);
        }

        private List<InterBankPreparedInfo> GetPreparedList()
        {
            int count = 0;
            List<InterBankPreparedInfo> list = new List<InterBankPreparedInfo>();
            string textCount = textBoxAvilabeCount.Text.Trim();
            if (string.IsNullOrEmpty(textCount) || !int.TryParse(textCount, out count))
            {
                return list;
            }

            for (var i = 0; i < count; i++)
            {
                InterBankPreparedInfo info = new InterBankPreparedInfo();
                Control[] controls = this.Controls.Find("定活标志" + (i + 1).ToString(), true);
                if (controls != null && controls.Length > 0)
                {
                    info.TermFlag = (AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE)Enum.Parse(typeof(AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE), controls[0].Text.Trim());
                }
                controls = this.Controls.Find("账号" + (i + 1).ToString(), true);
                if (controls != null && controls.Length > 0)
                {
                    info.AccountNO = controls[0].Text.Trim();
                }
                controls = this.Controls.Find("计提利息" + (i + 1).ToString(), true);
                if (controls != null && controls.Length > 0)
                {
                    info.PreparedInterest = double.Parse(controls[0].Text.Trim());
                }
                controls = this.Controls.Find("当前余额" + (i + 1).ToString(), true);
                if (controls != null && controls.Length > 0)
                {
                    info.CurrentBalance = double.Parse(controls[0].Text.Trim());
                }
                controls = this.Controls.Find("计提日期" + (i + 1).ToString(), true);
                if (controls != null && controls.Length > 0)
                {
                    info.PreparedDate = DateTime.Parse(controls[0].Text);
                }
                list.Add(info);
            }
            return list;
        }

        private List<InterBankPreparedInfo> GetTestPreparedList()
        {
            int testCount =100;
            List<InterBankPreparedInfo> list = new List<InterBankPreparedInfo>();


            for (var i = 0; i < testCount; i++)
            {
                InterBankPreparedInfo info = new InterBankPreparedInfo();
                info.TermFlag =  AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE.Current;
                info.AccountNO = "6666666"+ i;
                info.PreparedInterest = 980.5;
                info.CurrentBalance = 900000;
                info.PreparedDate = DateTime.Now.AddDays(i);
                
                list.Add(info);
            }
            return list;
        }

        private void TestForReturn(CoreBizMsgDataBase rData, StringBuilder result)
        {
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

        private void buttonSettle_Click(object sender, EventArgs e)
        {
            byte[] rebytes = null;
            Guid messageID = MsgTransferUtility.InterBankInterestSettle(柜员号.Text.Trim(), 机构号.Text.Trim(), DateTime.Parse(交易日期.Text), GetTestSettleInfoFix(), ref rebytes);
            MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, rebytes));
            DispatchMsg(msgdata);
        }

        private void buttonSettleMQ_Click(object sender, EventArgs e)
        {
            RegularResult result = AidSysClientSyncWrapper.InterBankSetInterest(柜员号.Text.Trim(), 机构号.Text.Trim(), DateTime.Parse(交易日期.Text), GetTestSettleInfoFix());
            if (result.Succeed)
            {
                textBoxPrepareResult.Text = "上送结息成功";
            }
            else
            {
                textBoxPrepareResult.Text = result.ExceptionMsg;
            }
            MessageBox.Show(textBoxPrepareResult.Text);

            RegularResult result1 = AidSysClientSyncWrapper.InterBankSetInterest(柜员号.Text.Trim(), 机构号.Text.Trim(), DateTime.Parse(交易日期.Text), GetTestSettleInfoCurrent());
            if (result1.Succeed)
            {
                textBoxPrepareResult.Text = "上送结息成功";
            }
            else
            {
                textBoxPrepareResult.Text = result1.ExceptionMsg;
            }
            MessageBox.Show(textBoxPrepareResult.Text);
        }
    }
}
