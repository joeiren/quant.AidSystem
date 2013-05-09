using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using xQuant.MQ;
using xQuant.AidSystem.Communication;
using xQuant.AidSystem.CoreMessageData;
using System.Net.Sockets;
using System.Collections;
using System.Net;
using xQuant.AidSystem.ClientSyncWrapper;
using xQuant.AidSystem.BizDataModel;
using System.Threading;
using xQuant.AidSystem.DBAction;
using System.IO;
using System.Diagnostics;
using System.Management;
using System.Runtime.Remoting.Channels;

namespace TestService
{
    public partial class TestServiceForm : Form
    {
        public TestServiceForm()
        {
            InitializeComponent();
        }
        private int _dataCount = 10;
        long _begin;

        #region MQ test
        /// <summary>
        /// Testing MQ send 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSend_Click(object sender, EventArgs e)
        {
             try
            {
            MQSender mqSender = new MQSender();
            MQConnection mqcon = new MQConnection();
            mqcon.Init("1", ConfigurationSettings.AppSettings["MQHOST"], int.Parse(ConfigurationSettings.AppSettings["MQPORT"]), ConfigurationSettings.AppSettings["MQName"]);
            mqcon.Open();
            //mqSender.Init(ConfigurationSettings.AppSettings["Test_2591"], mqcon);
            mqSender.Init(ConfigurationSettings.AppSettings["Test_2591"], mqcon);

            MQMessage msg = new MQMessage();
            msg.HeaderMcd.McdType = MQMessage.MQHeaderMcd.Request;
            msg.HeaderUser.UserBaseDate.Value = DateTime.Now.ToShortDateString();
            msg.HeaderUser.UserServiceId.Value = "11";
            msg.HeaderUser.UserServiceStatus.Value = "0";
            msg.HeaderUser.UserServiceGuage.Value = "0";
            msg.HeaderUser.UserServiceGuageInfo.Value = String.Empty;
            msg.HeaderUser.UserUserId.Value = "12";
            msg.HeaderUser.UserTaskCode.Value = "1290";
            msg.Text = "MQ test 消息";
           
                mqSender.SendMessage(msg);
   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// MQ receive
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonReceive_Click(object sender, EventArgs e)
        {
            MQReceiver receiver = new MQReceiver();
            MQConnection mqcon = new MQConnection();
            mqcon.Init("1", ConfigurationSettings.AppSettings["MQHOST"], int.Parse(ConfigurationSettings.AppSettings["MQPORT"]), ConfigurationSettings.AppSettings["MQName"]);
            mqcon.Open();
            receiver.Init(ConfigurationSettings.AppSettings["Test_2591"], mqcon);
            receiver.RegisterMessageReceiver(MessageReceiver_Callback);
        }

        private void MessageReceiver_Callback(MQMessage aMessage)
        {
            try
            {
                switch (aMessage.HeaderMcd.McdType)
                {
                    case MQMessage.MQHeaderMcd.Response:
                        MessageBox.Show(aMessage.Text);
                        break;
                    case MQMessage.MQHeaderMcd.Result:
                        MessageBox.Show(aMessage.Text);
                        break;
                    case MQMessage.MQHeaderMcd.Request:
                    default:
                        return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        #endregion

        private void buttonSocketTest_Click(object sender, EventArgs e)
        {
            MessageData msgdata = new MessageData{MessageID=Guid.NewGuid(),FirstTime=DateTime.Now,TragetPlatform=PlatformType.Core};

            msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = false;
            DispatchMsg(msgdata);

        }
        #region 客户
        /// <summary>
        /// 查询客户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRetrieve_Click(object sender, EventArgs e)
        {
            // web界面层
            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            String internalID = textBoxInternalID.Text.TrimStart();

            Guid messageID = MsgTransferUtility.RetrieveCstm(tellno, ouno, DateTime.Now, internalID, ref uLongText);

            // 跳过MQ层
            // 从MQ获取到消息后的操作
            MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core, ReSentTime =0 };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = false;
     
            DispatchMsg(msgdata);
        }      

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            UpdateDeleteCstm(false);
        }
        private void buttonAddCstm_Click(object sender, EventArgs e)
        {
            String cName = textBoxCName.Text;
            String oName = textBoxOName.Text;
            String eName = textBoxEName.Text;
            String address = textBoxAddress.Text;
            String telNO = textBoxTelNO.Text;
            String mblNO = textBoxMblNO.Text;
            String zip = textBoxZip.Text;

            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            String interCode = textBoxInterID.Text.TrimStart();
            String cstmNO = textBoxCstmNO.Text.TrimStart();
            // 临时测试对象
            xQuant.AidSystem.BizDataModel.CoreCustomerBrief cstm = new xQuant.AidSystem.BizDataModel.CoreCustomerBrief
            {
                CstmNO = cstmNO,
                CName = cName,
                OName = oName,
                EName = eName,
                Address = address,
                TeleNO = telNO,
                MobileNO = mblNO,
                ZIP = zip
            };
            Guid messageID = MsgTransferUtility.AddCustomer(tellno, ouno, DateTime.Now, ref uLongText, cstm);

            //StringBuilder sb = new StringBuilder(uLongText.Length);
            //int len = EBCDICEncoder.EBCDICToWideChar(1388, uLongText, uLongText.Length, sb, sb.Capacity);
            //this.textBoxRespUpdtCstm.Text = sb.ToString();

            MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = false;
            DispatchMsg(msgdata);
        }
        private void buttonDeleteCstm_Click(object sender, EventArgs e)
        {
            UpdateDeleteCstm(true);
        }
        
        private void UpdateDeleteCstm(bool isdelete)
        {
            String cName = textBoxCName.Text;
            String oName = textBoxOName.Text;
            String eName = textBoxEName.Text;
            String address = textBoxAddress.Text;
            String telNO = textBoxTelNO.Text;
            String mblNO = textBoxMblNO.Text;
            String zip = textBoxZip.Text;

            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            String interCode = "83000044566";//textBoxInterID.Text.TrimStart();
            // 临时测试对象
            xQuant.AidSystem.BizDataModel.CoreCustomerBrief cstm = new xQuant.AidSystem.BizDataModel.CoreCustomerBrief
            {
                CName = cName,
                OName = oName,
                EName = eName,
                Address = address,
                TeleNO = telNO,
                MobileNO = mblNO,
                ZIP = zip
            };
            Guid messageID;
            string outmsg = "";
            if (isdelete)
            {
                messageID = MsgTransferUtility.DeleteCustomer(tellno, ouno, DateTime.Now, interCode, ref uLongText);
            }
            else
            {

                bool ret = AidSysClientSyncWrapper.UpdateCustomer(ouno, tellno, DateTime.Now, interCode, cstm, out outmsg);
                //messageID = MsgTransferUtility.UpdateCustomer(tellno, ouno, DateTime.Now, interCode, ref uLongText, cstm);
            }
            //MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core };
            //msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            //msgdata.IsMultiPackage = false;
            //DispatchMsg(msgdata);
        }

        private void buttonRetrieveBaseInfo_Click(object sender, EventArgs e)
        {
            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            string doctype = textBoxDocType.Text.TrimStart();
            String docno = textBoxDocNO.Text.TrimStart();
            Guid messageID = MsgTransferUtility.RetrieveCstmBaseInfo(tellno, ouno, DateTime.Now, doctype, docno, ref uLongText);
            MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core, ReSentTime = 0 };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = false;

            DispatchMsg(msgdata);
        }
        #endregion

        #region 登入
        private void buttonEncrypt_Click(object sender, EventArgs e)
        {
            TellerLogin1();
        }

        private void buttonMarksValidation_Click(object sender, EventArgs e)
        {
            try
            {
                String ouno = textBoxOrgNO.Text.TrimStart();
                String tellno = textBoxTellerNO.Text.TrimStart();
                String fingerinfo = textBoxMarkInfo.Text.TrimStart().TrimEnd();
                String device = textBoxDeviceType.Text.TrimStart().TrimEnd();

                Guid msgID = MsgTransferUtility.FingerMarksValidation(tellno, ouno, DateTime.Now, device, fingerinfo, ref uLongText);

                // 跳过MQ层
                // 从MQ获取到消息后的操作
                MessageData msgdata = new MessageData { MessageID = msgID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.FingerMarks, ReSentTime = 0 };
                msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
                msgdata.IsMultiPackage = false;

                DispatchMsg(msgdata);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonFingerMarkSync_Click(object sender, EventArgs e)
        {
            String outmsg = "";
            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            String fingerinfo = textBoxMarkInfo.Text.TrimStart().TrimEnd();
            String device = textBoxDeviceType.Text.TrimStart().TrimEnd();
            if (AidSysClientSyncWrapper.FingerMarkAuth(tellno, ouno, DateTime.Now, device, fingerinfo, out outmsg))
            {
                MessageBox.Show("指纹验证通过！");
                return;
            }
            else
            {
                MessageBox.Show(outmsg);
            }
             
        }
        private EncryptTellerAuth tellerLogin;
        private void TellerLogin1()
        {
            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            String pwd = textBoxEncryptInput.Text;

            Guid msgID = MsgTransferUtility.EncryptPassword(ouno, tellno, pwd, ref uLongText);

            // 跳过MQ层
            // 从MQ获取到消息后的操作
            MessageData msgdata = new MessageData { MessageID = msgID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Encrypt, ReSentTime = 0 };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = false;

            DispatchMsg(msgdata);

        }

        private void TellerLogin2(EncryptTellerAuth data)
        {
            if (data != null)
            {
                Guid msgID = MsgTransferUtility.AuthTeller(data.TellerData.RQDTL.ORG_NO, data.TellerData.RQDTL.STF_NO, data.TellerEncrypt.ReqPin, data.TellerEncrypt.RespPin, ref uLongText);
                //Guid msgID = MsgTransferUtility.AuthTeller(data, ref uLongText);
                MessageData msgdata = new MessageData { MessageID = msgID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core, ReSentTime = 0 };
                msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
                msgdata.IsMultiPackage = false;

                DispatchMsg(msgdata);
            }
        }
        #endregion

        private void buttonWebServiceTest_Click(object sender, EventArgs e)
        {
            //WebServiceTestForm serviceForm = new WebServiceTestForm();
            //serviceForm.Show();
        }
        #region 并发测试
        bool _isConCurrentTesting = false;

        public delegate void LoginDelegate(int index, StringBuilder sb);
        private void buttonPressionTest_Click(object sender, EventArgs e)
        {
            try
            {

                _isConCurrentTesting = true;
                textBoxRetCstmResult.Text = "";
                textBoxRespUpdtCstm.Text = "";
                int.TryParse(textBoxDataCount.Text.Trim(), out _dataCount);
                int testCount;
                if (_dataCount > 0)
                    testCount = _dataCount;
                else
                {
                    return;
                }
                Dictionary<Guid, MessageData> datalist = new Dictionary<Guid, MessageData>();
                StringBuilder retmsg = new StringBuilder();
                while (testCount > 0)
                {
                    //Random random = new Random();
                    //switch (random.Next(3))
                    //{
                    //    case 2:
                    //        KeyValuePair<Guid, MessageData> data2 = GetUpdateCstmData();
                    //        datalist.Add(data2.Key, data2.Value);
                    //        break;
                    //    default:
                    //    case 1:
                    //        KeyValuePair<Guid, MessageData> data1 = GetRetrieveCstmTestData();
                    //        datalist.Add(data1.Key, data1.Value);
                    //        break;
                    //}

                    //同步接口

                    LoginDelegate login = new LoginDelegate(OnLoginTest);
                    login.BeginInvoke(testCount, retmsg, null, null);
                    testCount--;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //_begin = DateTime.Now.Ticks;
            //foreach ( KeyValuePair<Guid, MessageData> data in datalist)
            //{
            //    DispatchMsg(data.Value);
            //}
         

        }

        private void OnLoginTest(int testCount, StringBuilder retmsg)
        {
            try
            {
                String outmsg = "";
                String duedate = "";
                bool login = AidSysClientSyncWrapper.TellerAuth("801000", "8010119", textBoxEncryptInput.Text.Trim(), out duedate, out outmsg);
                if (!string.IsNullOrEmpty(duedate))
                {
                    textBoxRetCstmResult.Text = retmsg.Append("#" + testCount.ToString() + ":" + duedate).ToString();
                }
                else
                {
                    textBoxRetCstmResult.Text = retmsg.Append("#" + testCount.ToString() + ":" + outmsg).ToString();
                }
                retmsg.AppendLine();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
 
        }

        private KeyValuePair<Guid, MessageData> GetRetrieveCstmTestData()
        {
            // web界面层
            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            String internalID = textBoxInternalID.Text.TrimStart();

            Guid messageID = MsgTransferUtility.RetrieveCstm(tellno, ouno, DateTime.Now, internalID, ref uLongText);

            MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core, ReSentTime = 0 };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = false;
            return new KeyValuePair<Guid, MessageData>(messageID, msgdata); 
        }

        private KeyValuePair<Guid, MessageData> GetLoginTestData()
        {
            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            String pwd = textBoxEncryptInput.Text;

            Guid msgID = MsgTransferUtility.EncryptPassword(ouno, tellno, pwd, ref uLongText);

            MessageData msgdata = new MessageData { MessageID = msgID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Encrypt, ReSentTime = 0 };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = false;
            return new KeyValuePair<Guid, MessageData>(msgID, msgdata); 
        }

        private KeyValuePair<Guid, MessageData> GetUpdateCstmData()
        {
            String cName = textBoxCName.Text;
            String oName = textBoxOName.Text;
            String eName = textBoxEName.Text;
            String address = textBoxAddress.Text;
            String telNO = textBoxTelNO.Text;
            String mblNO = textBoxMblNO.Text;
            String zip = textBoxZip.Text;

            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            String interCode = textBoxInterID.Text.TrimStart();
            // 临时测试对象
            xQuant.AidSystem.BizDataModel.CoreCustomerBrief cstm = new xQuant.AidSystem.BizDataModel.CoreCustomerBrief
            {
                CName = cName,
                OName = oName,
                EName = eName,
                Address = address,
                TeleNO = telNO,
                MobileNO = mblNO,
                ZIP = zip
            };
            Guid messageID = MsgTransferUtility.UpdateCustomer(tellno, ouno, DateTime.Now, interCode, ref uLongText, cstm);

            MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.Core };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
            msgdata.IsMultiPackage = false;

            return new KeyValuePair<Guid, MessageData>(messageID, msgdata);
        }
        #endregion

        #region 同步测试
        private void buttonLoginSync_Click(object sender, EventArgs e)
        {
            String outmsg = "";
            String duedate = "";
            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            String pwd = textBoxEncryptInput.Text;
          
                bool login = AidSysClientSyncWrapper.TellerAuth(ouno, tellno, pwd, out duedate, out outmsg);
                if (!login)
                {
                    MessageBox.Show(outmsg);
                }
                else
                {
                    StringBuilder sb = new StringBuilder(textBoxRetCstmResult.Text);
                    sb.AppendLine();
                    sb.Append("Login Sync succeed");
                    textBoxRetCstmResult.Text = sb.ToString();
                }
           
        }

        private void buttonCreateCstmSync_Click(object sender, EventArgs e)
        {

        }

        private void buttonRetrieveCstmSync_Click(object sender, EventArgs e)
        {
            String outmsg = "";
            CoreCustomer customer = null;
            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            String internalID = textBoxInternalID.Text.TrimStart();
            if (!AidSysClientSyncWrapper.RetrieveCustomer(ouno, tellno, DateTime.Now, internalID, out customer, out outmsg))
            {
                MessageBox.Show(outmsg);
            }
            else
            {
                StringBuilder sb = new StringBuilder(textBoxRetCstmResult.Text);
                sb.AppendLine();
                sb.Append("Retrieve Custom Sync succeed");
                textBoxRetCstmResult.Text = sb.ToString();
            }
            //MessageBox.Show((customer != null).ToString());
        }

        private void buttonAcctSync_Click(object sender, EventArgs e)
        {
            string outmsg = "";
            string flowNO = textBoxFlowNO.Text.Trim();
            #region 多借多贷同步

            List<CoreBillRecord> list = new List<CoreBillRecord>();
            CoreAcctResult outresult = new CoreAcctResult();

            CoreBillRecord bill = new CoreBillRecord();
            bill.Currency = "CNY";
            bill.InnerSN = "1";
            bill.InterAcctSN = "0003";
            bill.IsEliminateFlag = "0";
            bill.IsNote = null;
            bill.Opt = "2";
            bill.OptAcct = null;
            bill.OrgNO = "801000";
            bill.PendingAcctSN = null;
            bill.RedBlueFlag = "1";
            bill.Subject = "111299";
            bill.TradeAcctNO = null;
            bill.TradeMoney = "104840.00";
            list.Add(bill);

            CoreBillRecord bill1 = new CoreBillRecord();
            bill1.Currency = "CNY";
            bill1.InnerSN = "2";
            bill1.InterAcctSN = "0001";
            bill1.IsEliminateFlag = "0";
            bill1.IsNote = null;
            bill1.Opt = "1";
            bill1.OptAcct = null;
            bill1.OrgNO = "801000";
            bill1.PendingAcctSN = null;
            bill1.RedBlueFlag = "1";
            bill1.Subject = "143101";
            bill1.TradeAcctNO = null;
            bill1.TradeMoney = "102340.00";
            list.Add(bill1);

            CoreBillRecord bill2 = new CoreBillRecord();
            bill2.Currency = "CNY";
            bill2.InnerSN = "3";
            bill2.InterAcctSN = "0001";
            bill2.IsEliminateFlag = "0";
            bill2.IsNote = null;
            bill2.Opt = "1";
            bill2.OptAcct = null;
            bill2.OrgNO = "801000";
            bill2.PendingAcctSN = null;
            bill2.RedBlueFlag = "1";
            bill2.Subject = "132158";
            bill2.TradeAcctNO = null;
            bill2.TradeMoney = "2500.00";
            list.Add(bill2);

            //CoreBillRecord bill3 = new CoreBillRecord();
            //bill3.Currency = "CNY";
            //bill3.InnerSN = "3";
            //bill3.InterAcctSN = "0001";
            //bill3.IsEliminateFlag = "0";
            //bill3.IsNote = null;
            //bill3.Opt = "1";
            //bill3.OptAcct = null;
            //bill3.OrgNO = "801000";
            //bill3.PendingAcctSN = null;
            //bill3.RedBlueFlag = "1";
            //bill3.Subject = "145103";
            //bill3.TradeAcctNO = null;
            //bill3.TradeMoney = "20000";
            //list.Add(bill3);


            AidSysClientSyncWrapper.CoreAcctRecord("801000", "8010001", DateTime.Now, flowNO, list, out outresult, out outmsg);
            #endregion

        }

        private void buttonCoreErase_Click(object sender, EventArgs e)
        {
            string outmsg = "";
            string coresn = textBoxCoreSN.Text.Trim();
            #region 核心抹账

            CoreAcctErase outdEraseData;
            AidSysClientSyncWrapper.CoreAcctErase("801000", "8010001", DateTime.Now, coresn, out outdEraseData, out outmsg);
            MessageBox.Show(outmsg);
            #endregion
        }

        private void buttonBaseInfoSync_Click(object sender, EventArgs e)
        {
            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            String doctype = textBoxDocType.Text.TrimStart();
            String docno = textBoxDocNO.Text.TrimStart();
            List<CoreCustomBaseInfo> baseinfolist = null;
            String outmsg = "";
            if (!AidSysClientSyncWrapper.RetrieveCustomerBaseInfo(ouno, tellno, DateTime.Now, doctype, docno, out baseinfolist, out outmsg))
            {
                MessageBox.Show("查询失败：" + outmsg);
            }
            else
            {
                int i = 0;
                StringBuilder ret = new StringBuilder();
                foreach (var item in baseinfolist)
                {
                    ret.AppendFormat("#{0}", (++i).ToString());
                    ret.AppendLine();
                    ret.AppendFormat("客户内码：{0};客户名字：{1};地址序号：{2}; 地址：{3}", item.CustomerInnerCode, item.CustomerName, item.AddressSN, item.Address);
                    ret.AppendLine();
                    ret.AppendFormat("电话号码：{0};电话序号：{1}。", item.TelephoneNumber, item.TelephoneSN);
                }
                textBoxRetCstmResult.Text = ret.ToString();
            }
            
        }
        #endregion

        #region 消息处理
        #region Test code for displaying TestForm
        private String TestForRetrieveCstm(StringBuilder result, CoreBizMsgDataBase respData)
        {
            RetrieveCstmData cstmdata = respData as RetrieveCstmData;

            if (cstmdata.OmsgHandler.NUM_ENT > 0)
            {
                result.AppendFormat("OMSG:{0} item(s)", cstmdata.OmsgHandler.NUM_ENT);
                result.AppendLine();
                foreach (OMSG_Item_Handler omsgItem in cstmdata.OmsgHandler.OMSGItemList)
                {
                    result.AppendFormat("MOD_ID:{0};MSG_NO:{1};MSG_TYPE:{2};MSG_TEXT:{3};", omsgItem.MOD_ID, omsgItem.MSG_NO, omsgItem.MSG_TYPE, omsgItem.MSG_TEXT);
                    result.AppendLine();
                }
            }
            if (!String.IsNullOrEmpty(cstmdata.OData.CUS_CDE))
            {
                result.Append("ODATA:");
                result.AppendLine();
                result.AppendFormat("客户内码:{0}", cstmdata.OData.CUS_CDE);
                textBoxInterID.Text = cstmdata.OData.CUS_CDE;
                result.AppendLine();
                result.AppendFormat("客户号:{0}", cstmdata.OData.CUS_NO);
                result.AppendLine();
                result.AppendFormat("客户类型:{0}", cstmdata.OData.CUS_TYP);
                result.AppendLine();
                result.AppendFormat("客户名称:{0}", cstmdata.OData.CUS_NAM);
                textBoxCName.Text = cstmdata.OData.CUS_NAM;
                result.AppendLine();
                result.AppendFormat("其他名称:{0}", cstmdata.OData.CUS_ONAM);
                textBoxOName.Text = cstmdata.OData.CUS_ONAM;
                result.AppendLine();
                result.AppendFormat("国籍:{0}", cstmdata.OData.NATION);
                result.AppendLine();
                result.AppendFormat("贵宾类型:{0}", cstmdata.OData.VIP_TYP);
                result.AppendLine();
                result.AppendFormat("客户状态:{0}", cstmdata.OData.CUS_STS);
                result.AppendLine();
                result.AppendFormat("地址序号:{0}", cstmdata.OData.ADD_SN);
                result.AppendLine();
                result.AppendFormat("地址:{0}", cstmdata.OData.ADDR);
                textBoxAddress.Text = cstmdata.OData.ADDR;
                result.AppendLine();
                result.AppendFormat("电话序号:{0}", cstmdata.OData.TEL_SN);
                result.AppendLine();
                result.AppendFormat("电话号码:{0}", cstmdata.OData.TEL_NO);
                textBoxTelNO.Text = cstmdata.OData.TEL_NO;
                result.AppendLine();
                result.AppendFormat("手机号码:{0}", cstmdata.OData.MBL_NO);
                textBoxMblNO.Text = cstmdata.OData.MBL_NO;
                result.AppendLine();
                result.AppendFormat("邮编:{0}", cstmdata.OData.ZIP);
                textBoxZip.Text = cstmdata.OData.ZIP;
                result.AppendLine();
                result.AppendFormat("区域类型:{0}", cstmdata.OData.CMB_QYLX);
                result.AppendLine();
                result.AppendFormat("建立机构:{0}", cstmdata.OData.CRT_PDT);
                result.AppendLine();
                result.AppendFormat("建立柜员:{0}", cstmdata.OData.CRT_OPR);
                result.AppendLine();
                result.AppendFormat("建立日期:{0}", cstmdata.OData.CRT_DTE);
                result.AppendLine();
                result.AppendFormat("更新机构:{0}", cstmdata.OData.UP_PDT);
                result.AppendLine();
                result.AppendFormat("更新柜员:{0}", cstmdata.OData.UP_OPR);
                result.AppendLine();
                result.AppendFormat("更新日期:{0}", cstmdata.OData.UP_DTE);
                result.AppendLine();
            }
            if (!String.IsNullOrEmpty(cstmdata.SyserrHandler.Message))
            {
                result.AppendFormat("SYSERR:{0}", cstmdata.SyserrHandler.Message);
            }

            return result.ToString();
        }

        private String TestForUpdateCstm(StringBuilder result, CoreBizMsgDataBase respData)
        {
            UpdateCstmData cstmdata = respData as UpdateCstmData;

            if (cstmdata.OmsgHandler.NUM_ENT > 0)
            {
                result.AppendFormat("OMSG:{0} item(s)", cstmdata.OmsgHandler.NUM_ENT);
                result.AppendLine();
                foreach (OMSG_Item_Handler omsgItem in cstmdata.OmsgHandler.OMSGItemList)
                {
                    result.AppendFormat("MOD_ID:{0};MSG_NO:{1};MSG_TYPE:{2};MSG_TEXT:{3};", omsgItem.MOD_ID, omsgItem.MSG_NO, omsgItem.MSG_TYPE, omsgItem.MSG_TEXT);
                    result.AppendLine();
                }
            }
            if (!String.IsNullOrEmpty(cstmdata.OData.CUS_CDE))
            {
                result.Append("ODATA:");
                result.AppendLine();
                result.AppendFormat("客户内码:{0}", cstmdata.OData.CUS_CDE);
                result.AppendLine();
            }
            if (!String.IsNullOrEmpty(cstmdata.SyserrHandler.Message))
            {
                result.AppendFormat("SYSERR:{0}", cstmdata.SyserrHandler.Message);
            }

            return result.ToString();
        }

        private String TestForAddCstm(StringBuilder result, CoreBizMsgDataBase respData)
        {
            AddCstmData cstmdata = respData as AddCstmData;

            if (cstmdata.OmsgHandler.NUM_ENT > 0)
            {
                result.AppendFormat("OMSG:{0} item(s)", cstmdata.OmsgHandler.NUM_ENT);
                result.AppendLine();
                foreach (OMSG_Item_Handler omsgItem in cstmdata.OmsgHandler.OMSGItemList)
                {
                    result.AppendFormat("MOD_ID:{0};MSG_NO:{1};MSG_TYPE:{2};MSG_TEXT:{3};", omsgItem.MOD_ID, omsgItem.MSG_NO, omsgItem.MSG_TYPE, omsgItem.MSG_TEXT);
                    result.AppendLine();
                }
            }
            if (!String.IsNullOrEmpty(cstmdata.OData.CUS_CDE))
            {
                result.Append("ODATA:");
                result.AppendLine();
                result.AppendFormat("客户内码:{0}", cstmdata.OData.CUS_CDE);
                result.AppendLine();
            }
            if (!String.IsNullOrEmpty(cstmdata.SyserrHandler.Message))
            {
                result.AppendFormat("SYSERR:{0}", cstmdata.SyserrHandler.Message);
            }

            return result.ToString();
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
            if (!String.IsNullOrEmpty(recorddata.OData.RespOdata))
            {
                result.Append("ODATA:");
                result.AppendLine();
                result.Append(recorddata.OData.RespOdata);
                result.AppendLine();
            }
            if (!String.IsNullOrEmpty(recorddata.SyserrHandler.Message))
            {
                result.AppendFormat("SYSERR:{0}", recorddata.SyserrHandler.Message);
            }

            return result.ToString();
        }

        private String TestForLogin(StringBuilder result, EncryptTellerAuth respData)
        {
            if (respData == null)
            {
                return result.ToString();
            }

            //if (!String.IsNullOrEmpty(respData.RPhdrHandler.SEQ_NO))
            //{
            //    result.AppendFormat("此交易流水号:{0}", respData.RPhdrHandler.SEQ_NO);
            //    result.AppendLine();
            //}

            if (respData.TellerData.OmsgHandler.NUM_ENT > 0)
            {
                result.AppendFormat("OMSG:{0} item(s)", respData.OmsgHandler.NUM_ENT);
                result.AppendLine();
                foreach (OMSG_Item_Handler omsgItem in respData.OmsgHandler.OMSGItemList)
                {
                    result.AppendFormat("MOD_ID:{0};MSG_NO:{1};MSG_TYPE:{2};MSG_TEXT:{3};", omsgItem.MOD_ID, omsgItem.MSG_NO, omsgItem.MSG_TYPE, omsgItem.MSG_TEXT);
                    result.AppendLine();
                }
            }
            if (!String.IsNullOrEmpty(respData.TellerData.OData.DUE_DATE))
            {
                result.Append("ODATA:");
                result.AppendLine();
                result.Append(respData.TellerData.OData.DUE_DATE);
                result.AppendLine();
            }
            if (!String.IsNullOrEmpty(respData.SyserrHandler.Message))
            {
                result.AppendFormat("SYSERR:{0}", respData.TellerData.SyserrHandler.Message);
            }

            return result.ToString();
        }

        private String TestRetrieveCstmBaseInfo(StringBuilder result, BizMsgDataBase respData)
        {
            RetrieveCstmBaseInfoData cstmdata = respData as RetrieveCstmBaseInfoData;

            if (cstmdata.OmsgHandler.NUM_ENT > 0)
            {
                result.AppendFormat("OMSG:{0} item(s)", cstmdata.OmsgHandler.NUM_ENT);
                result.AppendLine();
                foreach (OMSG_Item_Handler omsgItem in cstmdata.OmsgHandler.OMSGItemList)
                {
                    result.AppendFormat("MOD_ID:{0};MSG_NO:{1};MSG_TYPE:{2};MSG_TEXT:{3};", omsgItem.MOD_ID, omsgItem.MSG_NO, omsgItem.MSG_TYPE, omsgItem.MSG_TEXT);
                    result.AppendLine();
                }
            }
            if (cstmdata.OData.CstmBaseInfoList.Count > 0)
            {
                result.Append("ODATA:");
                result.AppendLine();
                int i = 0;
                foreach (RetrieveCstmBaseInfoODATA_Item item in cstmdata.OData.CstmBaseInfoList)
                {
                    result.AppendFormat("#{0}", (++i).ToString());
                    result.AppendLine();
                    result.AppendFormat("客户内码:{0}", item.CUS_CDE);

                    result.AppendLine();
                    result.AppendFormat("客户名字:{0}", item.CUS_NAM);
                    result.AppendLine();
                    result.AppendFormat("地址:{0}", item.ADDR);
                    result.AppendLine();
                    result.AppendFormat("地址序号:{0}", item.ADSNPTSN);
                    result.AppendLine();
                    result.AppendFormat("电话号码:{0}", item.TEL_NO);

                    result.AppendLine();
                    result.AppendFormat("电话序号:{0}", item.TEL_SN);
                    result.AppendLine();
                }
            }
            if (!String.IsNullOrEmpty(cstmdata.SyserrHandler.Message))
            {
                result.AppendFormat("SYSERR:{0}", cstmdata.SyserrHandler.Message);
            }

            return result.ToString();
        }
        #endregion
        #region Async message handler
        MessageHandlerCompleteAsync _handlerCompleteAsync = null;
        void OnMessageHandleAsync(MessageData msgdata)
        {
            CommunicationHandlerAsyncBase handler = null;
            switch (msgdata.TragetPlatform)
            {
                case PlatformType.Encrypt:
                    //handler = new EncryptCommunicationHandler();
                    break;
                case PlatformType.Core:
                    handler = new CoreCommunicationHandlerAsync();
                    break;
                default:

                    break;
            }

            if (_handlerCompleteAsync != null)
            {
                //lock (_handlerCompleteAsync)
                {
                    handler.MessageAsyncHandler(msgdata, _handlerCompleteAsync);
                }
            }
            else
            {
                _handlerCompleteAsync = new MessageHandlerCompleteAsync(MessageHandleAsync_Completed);
                //lock (_handlerCompleteAsync)
                {
                    handler.MessageAsyncHandler(msgdata, _handlerCompleteAsync);
                }
            }
        }
        object _locker = new object();
        void MessageHandleAsync_Completed(MessageData data, Exception ex)
        {
            Interlocked.Decrement(ref _dataCount);
            if (_isConCurrentTesting)
            {
                //String eventSourceName = "xQuant.ConCurrentTesting";
                //StringBuilder sb = new StringBuilder();
                //sb.AppendFormat("Req:{0};    Begin:{1}    Type:{2}", data.MessageID, data.Begin.ToString("HH:mm:ss:fff"), respData.GetType().ToString());
                //sb.AppendLine();
                //sb.AppendFormat("Resp:{0};    End:{1}, Remain:{2}", data.RespMsgID, DateTime.Now.ToString("HH:mm:ss:fff"), _dataCount);
                //CommonHelper.AddEventLog(eventSourceName, sb.ToString());
                if (_dataCount <= 0)
                {
                    TimeSpan span = new TimeSpan(DateTime.Now.Ticks - _begin);
                    MessageBox.Show("Finish! Duration:" + span.TotalMilliseconds.ToString());
                }

                return;
            }

            StringBuilder result = new StringBuilder();
            if (ex != null)
            {
                return;
            }
            if (data == null)
            { }
            byte[] rebytes = data.GetRespMessage();

            // 发送个MQ(MessageID, buffer)

            // 跳过MQ
            // ....

            // web界面层
            int realLen = rebytes.Length;
            if (data.TragetPlatform != PlatformType.Encrypt)
            {
                byte end = 0;
                realLen = Array.IndexOf(rebytes, end);
            }

            byte[] buffer = new byte[realLen];
            Array.Copy(rebytes, buffer, realLen);
            BizMsgDataBase respData = null;

            respData = MsgTransfer.DecodeMsg(data.MessageID, buffer);

            
            #region Test code

            if (respData is RetrieveCstmData)
            {
                TestForRetrieveCstm(result, (CoreBizMsgDataBase)respData);
                result.AppendLine();
                if (_dataCount <= 0)
                {
                    long end = DateTime.Now.Ticks;
                    TimeSpan span = new TimeSpan(end - _begin);

                    MessageBox.Show(span.TotalMilliseconds.ToString());
                }
                //textBoxRetCstmResult.Text += result.ToString();

            }
            else if (respData is RetrieveCstmBaseInfoData)
            {
                TestRetrieveCstmBaseInfo(result, respData);
                textBoxRetCstmResult.Text = result.ToString();
            }
            else if (respData is UpdateCstmData)
            {
                TestForUpdateCstm(result, (CoreBizMsgDataBase)respData);
                result.AppendLine();
                if (_dataCount <= 0)
                {
                    long end = DateTime.Now.Ticks;
                    TimeSpan span = new TimeSpan(end - _begin);

                    MessageBox.Show(span.TotalMilliseconds.ToString());
                }
                //textBoxRespUpdtCstm.Text +=  result.ToString();
            }
            else if (respData is AddCstmData)
            {
                TestForAddCstm(result, (CoreBizMsgDataBase)respData);
                textBoxRespUpdtCstm.Text = result.ToString();
            }
            else if (respData is AcctRecordData)
            {
                //TestForAcctRecord(result, (CoreBizMsgDataBase)respData);
                //textBoxRespAcct.Text = result.ToString();
            }
            else if (respData is EncryptTellerAuth)
            {
                EncryptTellerAuth encryptTeller = respData as EncryptTellerAuth;
                if (encryptTeller.AfterEncrypt)
                {
                    TestForLogin(result, encryptTeller);
                    textBoxEncryptOutput.Text = result.ToString();
                    //if (DialogResult.OK == MessageBox.Show(_dataCount.ToString(), "Test", MessageBoxButtons.OK))
                    {

                        if (_dataCount <= 0)
                        {
                            long end = DateTime.Now.Ticks;
                            TimeSpan span = new TimeSpan(end - _begin);

                            MessageBox.Show(span.TotalMilliseconds.ToString());
                        }
                    }

                }
                else
                {
                    TellerLogin2((EncryptTellerAuth)respData);
                }
            }

            #endregion
        }
        #endregion

        bool _isAsyncSocket = true;

        #region Message Handler
        MsgDispatchEAP _dispatchMsg = null;
        byte[] uLongText;
        private void DispatchMsg(MessageData msgdata)
        {
            _isAsyncSocket = checkBoxAsyncSock.Checked;
            if (_isAsyncSocket)
            {
                OnMessageHandleAsync(msgdata);
            }
            else
            {
                ICommunicationHandler handler;
                switch (msgdata.TragetPlatform)
                {
                    case PlatformType.Payment:
                    case PlatformType.PaymentDownload:
                        handler = new PayCommunicationHandler();
                        break;
                    case PlatformType.Encrypt:
                        handler = new EncryptCommunicationHandler();
                        break;
                    case PlatformType.FingerMarks:
                        handler = new MarkComminicationHandler();
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
        }
        void DispatchMsg_DispatchCompleted(object sender, TransmitCompletedEventArgs e)
        {
            _dataCount--;

            StringBuilder result = new StringBuilder();
            result.Append("begin completed");
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
                    result.AppendLine("e.Error --:");
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
                        result.AppendLine("e.Error.Message:");
                        result.Append(e.Error.Message);
                    }
                    if (_dataCount <= 0)
                    {
                        //long end = DateTime.Now.Ticks;
                        //TimeSpan span = new TimeSpan(end - _begin);

                        //MessageBox.Show(span.TotalMilliseconds.ToString());
                    }
                    textBoxRetCstmResult.Text += result.ToString();
                }
                else
                {
                    if (_isConCurrentTesting)
                    {

                        //String eventSourceName = "xQuant.ConCurrentTesting";
                        //StringBuilder sb = new StringBuilder();
                        //sb.AppendFormat("Req:{0};    Begin:{1}    Type:{2}", e.MessageData.MessageID, e.MessageData.Begin.ToString("HH:mm:ss:fff"), e.MessageData.GetType().ToString());
                        //sb.AppendLine();
                        //sb.AppendFormat("Resp:{0};    End:{1}, Remain:{2}", e.MessageData.RespMsgID, DateTime.Now.ToString("HH:mm:ss:fff"), _dataCount);
                        //CommonHelper.AddEventLog(eventSourceName, sb.ToString());
                        if (_dataCount <= 0)
                        {
                            TimeSpan span = new TimeSpan(DateTime.Now.Ticks - _begin);
                            MessageBox.Show("Finish! Duration:" + span.TotalMilliseconds.ToString());
                        }

                        return;
                    }

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
                        result.AppendLine("realLen:" + realLen.ToString());
                        if (e.MessageData.TragetPlatform != PlatformType.Encrypt)
                        {
                            byte end = 0;
                            realLen = Array.IndexOf(rebytes, end);
                        }

                        if (realLen <= 0)
                        {
                            MessageBox.Show("返回数据为空！");
                            return;
                        }

                        byte[] buffer = new byte[realLen];
                        Array.Copy(rebytes, buffer, realLen);
                        BizMsgDataBase respData = null;

                        respData = MsgTransfer.DecodeMsg(e.MessageData.MessageID, buffer);
                        #region Test code

                        if (respData is RetrieveCstmData)
                        {
                            TestForRetrieveCstm(result, (CoreBizMsgDataBase)respData);
                            result.AppendLine();
                            if (_dataCount <= 0)
                            {
                                long end = DateTime.Now.Ticks;
                                TimeSpan span = new TimeSpan(end - _begin);

                                MessageBox.Show(span.TotalMilliseconds.ToString());
                            }
                            //textBoxRetCstmResult.Text += result.ToString();

                        }
                        else if (respData is UpdateCstmData)
                        {
                            TestForUpdateCstm(result, (CoreBizMsgDataBase)respData);
                            result.AppendLine();
                            if (_dataCount <= 0)
                            {
                                long end = DateTime.Now.Ticks;
                                TimeSpan span = new TimeSpan(end - _begin);

                                MessageBox.Show(span.TotalMilliseconds.ToString());
                            }
                            //textBoxRespUpdtCstm.Text +=  result.ToString();
                        }
                        else if (respData is AddCstmData)
                        {
                            TestForAddCstm(result, (CoreBizMsgDataBase)respData);
                            textBoxRespUpdtCstm.Text = result.ToString();
                        }
                        else if (respData is AcctRecordData)
                        {
                            //TestForAcctRecord(result, (CoreBizMsgDataBase)respData);
                            //textBoxRespAcct.Text = result.ToString();
                        }
                        else if (respData is RetrieveCstmBaseInfoData)
                        {
                            TestRetrieveCstmBaseInfo(result, respData);
                            textBoxRetCstmResult.Text = result.ToString();
                        }
                        else if (respData is EncryptTellerAuth)
                        {
                            EncryptTellerAuth encryptTeller = respData as EncryptTellerAuth;
                            if (encryptTeller.AfterEncrypt)
                            {
                                TestForLogin(result, encryptTeller);
                                textBoxEncryptOutput.Text = result.ToString();
                                //if (DialogResult.OK == MessageBox.Show(_dataCount.ToString(), "Test", MessageBoxButtons.OK))
                                {

                                    if (_dataCount <= 0)
                                    {
                                        //long end = DateTime.Now.Ticks;
                                        //TimeSpan span = new TimeSpan(end - _begin);

                                        //MessageBox.Show(span.TotalMilliseconds.ToString());
                                    }
                                }

                            }
                            else
                            {
                                TellerLogin2((EncryptTellerAuth)respData);
                            }
                        }
                        else if (respData is PayBankInfoData)
                        {
                            result.AppendLine("respData is PayBankInfoData!");
                            TestForBanksInfoDownload((PayBankInfoData)respData);
                        }
                       
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                result.AppendLine();
                result.Append(ex.Message.ToString());
                textBoxRetCstmResult.Text = "DispatchMsg_DispatchCompleted Exception:" + result.ToString();
                //MessageBox.Show(ex.Message.ToString());
            }
        }
        #endregion
        #endregion

        #region 显示记账Form
        private void buttonTestAcctRecord_Click(object sender, EventArgs e)
        {
            new AccountRecordForm().Show();
        }
        #endregion

        #region 显示支付平台Form
        private void buttonPaymentTest_Click(object sender, EventArgs e)
        {
            new PaymentTestForm().Show();
        }
        #endregion

        #region 数据库测试
        private void buttonDBAction_Click(object sender, EventArgs e)
        {

        }

        private void DBActionTest(MessageData msgdata)
        {
            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            String internalID = textBoxInternalID.Text.TrimStart();

            TTRD_AIDSYS_MSG_LOG row = new TTRD_AIDSYS_MSG_LOG();
            row.M_ID = msgdata.MessageID.ToString();
            row.M_S_CONTENT = msgdata.CurrentReqPackage.PackageMessage;
            row.M_ISSINGLE = msgdata.IsMultiPackage ? "0" : "1";
            row.M_ERROR = "Test DB msg";
            row.M_SENDDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            row.M_STATE = "1";
            row.M_SUBID = 1;
            row.I_ID = ouno;
            row.U_ID = tellno;
            row.M_PLATTYPE = (int)msgdata.TragetPlatform;
            TTRD_AIDSYS_MSG_LOG_Manager manager = new TTRD_AIDSYS_MSG_LOG_Manager();
            manager.LogInsert(row);

        }
        #endregion

        #region 编码解码工具
        private void buttonCodeTools_Click(object sender, EventArgs e)
        {
            new CoderTool().Show();
        }
        #endregion

        #region 同业结息
        private void buttonInterBankSettle_Click(object sender, EventArgs e)
        {
            new InterBankSettleForm().ShowDialog();
        }
        #endregion

        private void buttonFundRetrieve_Click(object sender, EventArgs e)
        {
            new FundRetrieveForm().Show();
        }

        private void btnCoreAcctNew_Click(object sender, EventArgs e)
        {
            new Multi2OneAcctRecordForm().Show();
        }

        private void buttonGenerateAcct_Click(object sender, EventArgs e)
        {
            new InnerAcctForm().Show();
        }

        private void buttonPayAcctCheck_Click(object sender, EventArgs e)
        {
            new PaymentCheckAcctForm().Show();
        }

        #region 银行信息下载
        private void btnBankInfoDownload_Click(object sender, EventArgs e)
        {
            try
            {
                Guid messageID = MsgTransferUtility.PayRetrieveBanksInfo(ref uLongText);
                MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.PaymentDownload };
                msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
                msgdata.IsMultiPackage = false;
                DispatchMsg(msgdata);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "银行信息下载 出错信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
        }

        private void TestForBanksInfoDownload(PayBankInfoData respdata)
        {
            
            if (respdata != null && respdata.RPData != null)
            {
                string path = @"c:\BanksInfo.del";

                List<string> contentlist = new List<string>();
                foreach (var item in respdata.RPData.BankList)
                {
                    contentlist.Add(string.Format("{0},{1}",
                        CommonDataHelper.StrTrimer(item.BankNO, null), CommonDataHelper.StrTrimer(item.BankName, null)));
                }
                CommonMethods.WriteLocalGBKFile(path, contentlist.ToArray());

                // Create a file to write to.
                //using (StreamWriter sw = File.CreateText(path))
                //{
                //    //sw.WriteLine(string.Format("RetCode：{0}", respdata.RPData.RetCode.TrimStart().TrimEnd()));
                //    //sw.WriteLine(string.Format("RetMsg:{0}", respdata.RPData.RetMsg.TrimStart().TrimEnd()));
                //    //sw.WriteLine(string.Format("Count:{0}", respdata.RPData.AccoutCount.ToString()));
                //    //sw.WriteLine(string.Format("记录时间：{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                //    if (respdata.RPData.BankList.Count > 0)
                //    {
                //        foreach (var item in respdata.RPData.BankList)
                //        {
                //            //sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6}",CommonDataHelper.StrTrimer(item.BankNO, null), CommonDataHelper.StrTrimer(item.BankName, null), CommonDataHelper.StrTrimer(item.DirectParticipator, null), CommonDataHelper.StrTrimer(item.NodeCode, null), CommonDataHelper.StrTrimer(item.CityCode, null), CommonDataHelper.StrTrimer(item.TelephoneNO, null), CommonDataHelper.StrTrimer(item.Address, null)));
                //            sw.WriteLine(string.Format("{0},{1}",));
                //        }
                //    }

                //}
                MessageBox.Show(string.Format("下载完毕，结果已保存文件在{0}",path), "银行信息下载", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
        #endregion

        private void buttonAcctCheck_Click(object sender, EventArgs e)
        {
            new CoreAcctCheckForm().Show();
        }

        public delegate void MethodDelegate(Guid guid);
        private void button1_Click(object sender, EventArgs e)
        {
            List<Guid> list = new List<Guid>();

            string ex = "";
            DataTable tb = MQMsgCommonHelper.QueryMQLog("1=1",out ex);
            if (tb != null)
            {
                foreach (DataRow row in tb.Rows)
                {
                    //list.Add(new Guid(row["GUID"].ToString()));
                    MethodDelegate test = new MethodDelegate(OnMultiTest);
                    test.BeginInvoke(new Guid(row["GUID"].ToString()), null, null);
                }
            }
            

        }

        private void OnMultiTest(Guid id)
        {
            string ex = "";
            Thread thread = new Thread(ThreadMethod);
            MQMsgCommonHelper.UpdateMQLog(id, "1", MsgLogState.SendSucceed, out ex);
            MQMsgCommonHelper.UpdateMQLog(id, "2", MsgLogState.PayTimeOut, out ex);
        }

        private void ThreadMethod()
        {
        }

        private void btn开户_Click(object sender, EventArgs e)
        {
            InterBankAccount form = new InterBankAccount();
            form.Show();
        }

        private void btn销户_Click(object sender, EventArgs e)
        {
            InterBankDelete form = new InterBankDelete();
            form.Show();
        }

        private void btn通知单查询_Click(object sender, EventArgs e)
        {
            QueryNoticeForm form = new QueryNoticeForm();
            form.Show();
        }

        private void 自动转存_Click(object sender, EventArgs e)
        {
            InterBankRedepoForm form = new InterBankRedepoForm();
            form.Show();
        }

        private void 活期余额查询_Click(object sender, EventArgs e)
        {
            InterBankRetrieveBalance form = new InterBankRetrieveBalance();
            form.Show();
        }

        #region 密码修改
        private void buttonChangePwd_Click(object sender, EventArgs e)
        {
            String outmsg = "";
            String duedate = "2011-11-01";
            String ouno = textBoxOrgNO.Text.TrimStart();
            String tellno = textBoxTellerNO.Text.TrimStart();
            //String pwd = textBoxEncryptInput.Text;
            String oldPwd = textBoxOldPwd.Text;
            String newPwd = textBoxNewPwd.Text;

            bool change = AidSysClientSyncWrapper.TellerChangePwd(tellno,ouno, DateTime.Parse("2011-11-01"), oldPwd, newPwd, out outmsg);
            if (!change)
            {
                MessageBox.Show(outmsg);
            }
            else
            {
                StringBuilder sb = new StringBuilder(textBoxRetCstmResult.Text);
                sb.AppendLine();
                sb.Append("ChangePwd Sync succeed");
                textBoxRetCstmResult.Text = sb.ToString();
            }

        }
        #endregion

        private void TestServiceForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
            Dispose();
            Close();
        }
    }   

}
