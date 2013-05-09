using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net.Sockets;

namespace xQuant.AidSystem.Communication
{
    public class PayCommunicationHandler : ICommunicationHandler
    {
        #region Property

        private static readonly String Host = ConfigurationSettings.AppSettings["PaymentHost"];

        private static readonly Int32 Port = Convert.ToInt32(ConfigurationSettings.AppSettings["PaymentPort"]);

        private static readonly String DownloadHost = ConfigurationSettings.AppSettings["PaymentDownloadHost"];

        private static readonly Int32 DownloadPort = Convert.ToInt32(ConfigurationSettings.AppSettings["PaymentDownloadPort"]);
        private const Int32 RECEIVE_MAX_LENGTH = 4 * 1024;
        #endregion

        #region IMessageHandler Members

        public MessageData MessageHandler(MessageData reqmsg)
        {
            MessageData message = new MessageData { MessageID = reqmsg.MessageID, FirstTime = reqmsg.FirstTime, TragetPlatform = reqmsg.TragetPlatform, BizMsgID = reqmsg.BizMsgID, ReSentTime = reqmsg.ReSentTime, SourceIP = reqmsg.SourceIP, ReqPackageList = reqmsg.ReqPackageList };
            try
            {
                #region Sync-Socket
                
                if(message.TragetPlatform == PlatformType.PaymentDownload)
                {
                    AidLogHelper.Write(xQuant.Log4.LogLevel.Debug, "开始创建SocketClient（PayCommunicationHandler），MessageID=" + message.MessageID);
                    using (SocketClient sca = new SocketClient(DownloadHost, DownloadPort))
                    {
                        sca.ReceiveTimeout = 10 * SocketClient.ONE_MINUTE;
                        return DoSendReceived( sca,reqmsg,message);
                    }
                }
                else
                {
                    AidLogHelper.Write(xQuant.Log4.LogLevel.Debug, "开始创建SocketClient（PayCommunicationHandler），MessageID=" + message.MessageID);
                    using (SocketClient sca = new SocketClient(Host, Port))
                    {
                        return DoSendReceived( sca,reqmsg,message);
                    }
                }
                
                #endregion
            }
            catch (SocketException sex)
            {
                //if (sex.ErrorCode == (int)SocketError.TimedOut && message.ReSentTime < 5)
                //{
                //    StringBuilder sb = new StringBuilder(sex.SocketErrorCode.ToString());
                //    sb.AppendLine();
                //    message.ReSentTime++;
                //    sb.AppendFormat("ID:{0}; Platform Type:Payment; 第{1}次重发！", message.MessageID, message.ReSentTime);
                //    CommonHelper.AddEventLog(this.GetType().ToString(), sb.ToString());
                //    LogHelper.Write(xQuant.Log4.LogLevel.Error, sb.ToString());
                //    return MessageHandler(message);
                //}
                //else
                {
                    StringBuilder sb = new StringBuilder("Socket Exception occured!");
                    sb.AppendLine();
                    sb.AppendFormat("ID:{0}; Biz ID:{1}; Platform Type:Payment; Socket Error Code:{2}; Message:{3}.", message.MessageID, message.BizMsgID, sex.SocketErrorCode, sex.Message);
                    //CommonHelper.AddEventLog(this.GetType().ToString(), sb.ToString());
                    AidLogHelper.Write(xQuant.Log4.LogLevel.Error, sb.ToString());
                    throw sex;
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("Exception occured!");
                sb.AppendLine();
                sb.AppendFormat("ID:{0};Biz ID:{1}; Platform Type:Payment; Message:{2};", message.MessageID, message.BizMsgID, ex.Message);
                sb.AppendLine();
                sb.AppendFormat("StackTrace:{0}.", ex.StackTrace);
                //CommonHelper.AddEventLog(this.GetType().ToString(), sb.ToString());
                AidLogHelper.Write(xQuant.Log4.LogLevel.Error, sb.ToString());
                throw ex;
            }

        }


        private  MessageData DoSendReceived(SocketClient sca,MessageData reqmsg,MessageData message)
        {
            try
            {
                sca.Connect();
                AidLogHelper.Write(xQuant.Log4.LogLevel.Debug, "SocketClient已连接（PayCommunicationHandler），MessageID=" + message.MessageID);
                byte[] returnmsg;
                Int16 i = 0;

                if (reqmsg.IsMultiPackage)
                {
                    foreach (PackageData packmsg in reqmsg.ReqPackageList)
                    {
                        returnmsg = sca.SendReceive(packmsg.PackageMessage, RECEIVE_MAX_LENGTH);
                        if (returnmsg.Length > 0)
                        {
                            message.RespPackageList.Enqueue(new PackageData(i++, returnmsg));
                        }
                    }
                }
                else
                {
                    AidLogHelper.Write(xQuant.Log4.LogLevel.Debug, "开始收发（PayCommunicationHandler），MessageID=" + message.MessageID);
                    returnmsg = sca.SendReceive(reqmsg.CurrentReqPackage.PackageMessage, RECEIVE_MAX_LENGTH);
                    message.RespPackageList.Enqueue(new PackageData(i, returnmsg));
                    AidLogHelper.Write(xQuant.Log4.LogLevel.Debug, string.Format("收发结束（PayCommunicationHandler，MessageID={0};接收数据长度：{1}", message.MessageID, returnmsg != null ? returnmsg.Length : 0));
                }

                sca.DisConnect();
                AidLogHelper.Write(xQuant.Log4.LogLevel.Debug, "断开连接（PayCommunicationHandler），MessageID=" + message.MessageID);
                return message;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
