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
using xQuant.AidSystem.ClientSyncWrapper;
using xQuant.AidSystem.BizDataModel;
using xQuant.AidSystem.DBAction;

namespace TestService
{
    public partial class PaymentCheckAcctForm : Form
    {
        public PaymentCheckAcctForm()
        {
            InitializeComponent();
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
            {
                MessageBox.Show(ex.Message);
                return;
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

                        if (respData is PayAcctCheckData)
                        {
                            TestForCheckAcct(result, (PayAcctCheckData)respData);
                            textBoxResult.Text = result.ToString();
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

        private String TestForCheckAcct(StringBuilder result, PayAcctCheckData data)
        {
            if (data == null || data.RPData == null)
            {
                return result.ToString();
            }

            result.AppendFormat("交易结果:{0};交易结果描述:{1};记录条数:{2}", data.RPData.RetCode, data.RPData.RetMsg, data.RPData.AccoutCount);
            if (data.RPData.AccoutCount <= 0)
            {
                return result.ToString();
            }

            int i = 1;
            foreach (var item in data.RPData.AccountList)
            {
                result.AppendLine();
                result.AppendFormat("#{0}", i++);
                result.AppendLine();
                result.AppendFormat("平台日期:{0};平台流水号:{1};渠道标志:{2};资金流水号{3};", item.PlatDate, item.SeqNO, item.ChanelFlag, item.SysSeqNO);
                result.AppendLine();
                result.AppendFormat("交易机构号:{0};报文编号:{1};付款账号:{2};付款方户名:{3};付款行行号:{4};付款行名称:{5};",item.OrganNO, item.MsgNO,item.PayAccount,item.PayName, item.PayBankNO, item.PayBankName);
                result.AppendLine();
                result.AppendFormat("收款账号:{0};收款方户名:{1};收款行行号:{2};收款行名称:{3};",item.RecvAccount,item.RecvName,item.RecvBankNO,item.RecvBankName);
                result.AppendLine();
                result.AppendFormat("币种代码:{0};交易金额:{1};拆借利率:{2};拆借期限:{3};业务种类:{4};",item.Currency,item.Amount, item.IBORate,item.IBOLimit, item.BizType);
                result.AppendLine();
                result.AppendFormat("备注:{0};",item.Note);
                result.AppendLine();
                result.AppendFormat("操作员:{0};上送渠道:{1};上主机记账标志:{2};主机流水号:{3};主机交易日期:{4}",item.Operator, item.UploadChannel, item.AccountFlag, item.HostFlowNO, item.HostTradeDate);
                result.AppendLine();
                result.AppendFormat("手续费:{0};主机响应码:{1};主机响应信息:{2};交易日期:{3};交易时间:{4}",item.Fee, item.HostRespCode,item.HostRespMsg,item.TradeDate,item.TradeTime);

            }
            List<string> contentlist = new List<string>();
            foreach (var item in data.RPData.AccountList)
            {
                contentlist.Add(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29}",
                    item.PlatDate, item.SeqNO, item.ChanelFlag, item.SysSeqNO, item.OrganNO, item.MsgNO, item.PayAccount, item.PayName, item.PayBankNO, item.PayBankName, item.RecvAccount, item.RecvName, item.RecvBankNO, item.RecvBankName, item.Currency, item.Amount,
                    item.IBORate, item.IBOLimit, item.BizType, item.Note,item.Operator, item.UploadChannel, item.AccountFlag, item.HostFlowNO, item.HostTradeDate, item.Fee,item.HostRespCode, item.HostRespMsg,item.TradeDate,item.TradeTime));
            }
            string path = @"e:\PayAcctCheck.del";
            CommonMethods.WriteLocalGBKFile(path, contentlist.ToArray());
            MessageBox.Show(string.Format("下载完毕，结果已保存文件在{0}", path), "支付对账", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return result.ToString();
        }

        private void buttonQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string strDate = textBoxQueryDate.Text;

                DateTime querydate = DateTime.ParseExact(strDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture); 
                //List<PayCheckAcct> outlist;
                //string outmsg;
                //AidSysClientSyncWrapper.PayCheckAccount(querydate, out outlist, out outmsg);

                Guid messageID = MsgTransferUtility.PayCheckAccount(querydate, ref uLongText);
                MessageData msgdata = new MessageData { MessageID = messageID, FirstTime = DateTime.Now, TragetPlatform = PlatformType.PaymentDownload };
                msgdata.ReqPackageList.Enqueue(new PackageData(1, uLongText));
                msgdata.IsMultiPackage = false;
                DispatchMsg(msgdata);
            }
            catch (Exception ex)
            {
                textBoxResult.Text = ex.Message;
                return;
 
            }
        }

        private void buttonSync_Click(object sender, EventArgs e)
        {
            string strDate = textBoxQueryDate.Text;

            DateTime querydate = DateTime.ParseExact(strDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            string outmsg;
            if (!AidSysClientSyncWrapper.PayRollingAcct(querydate, out outmsg))
            {
                MessageBox.Show(outmsg);
            }
            else
            {
                MessageBox.Show("查询结束，数据记录到DB！");
            }
        }
    }
}
