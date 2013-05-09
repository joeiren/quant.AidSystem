using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Threading;
using System.Net.Sockets;

namespace xQuant.AidSystem.Communication
{
    //public delegate void SocketCompletedEventHandler(object sender, SocketCompletedEventArgs e);

    public class CoreCommunicationHandler : ICommunicationHandler
    {
        public CoreCommunicationHandler()
        {
            //RequestMsg = new RequestMessage();
            //ResponseMsg = new ResponseMessage();
        }

        #region Property

        private static readonly String Host = ConfigurationSettings.AppSettings["CoreHost"];

        private static readonly Int32 Port = Convert.ToInt16(ConfigurationSettings.AppSettings["CorePort"]);
        private const Int32 RECEIVE_MAX_LENGTH = 10 * 1024;
        #endregion

        #region IMessageHandler Members

        public MessageData MessageHandler(MessageData reqmsg)
        {
            MessageData message = new MessageData { MessageID = reqmsg.MessageID, FirstTime = reqmsg.FirstTime, TragetPlatform = reqmsg.TragetPlatform, BizMsgID = reqmsg.BizMsgID, ReSentTime = reqmsg.ReSentTime, SourceIP = reqmsg.SourceIP, ReqPackageList = reqmsg.ReqPackageList };
            try
            {
                #region Sync-Socket

                AidLogHelper.Write(xQuant.Log4.LogLevel.Debug, "开始创建SocketClient（CoreCommunicationHandler），MessageID="+message.MessageID);
                using (SocketClient sca = new SocketClient(Host, Port))
                {
                    sca.Connect();
                    AidLogHelper.Write(xQuant.Log4.LogLevel.Debug, "SocketClient已连接（CoreCommunicationHandler），MessageID=" + message.MessageID);
                    byte[] returnmsg;
                    Int16 i = 0;
                    if (reqmsg.IsMultiPackage)
                    {
                        while (reqmsg.ReqPackageList.Count > 1)
                        {
                            sca.Send(reqmsg.ReqPackageList.Dequeue().PackageMessage);
                        }
                        returnmsg = sca.SendReceive(reqmsg.ReqPackageList.Dequeue().PackageMessage, RECEIVE_MAX_LENGTH);
                        if (returnmsg.Length > 0)
                        {
                            message.RespPackageList.Enqueue(new PackageData(i++, returnmsg));
                        }
                       
                    }
                    else
                    {
                        AidLogHelper.Write(xQuant.Log4.LogLevel.Debug, "开始收发（CoreCommunicationHandler，MessageID=" + message.MessageID);
                        returnmsg = sca.SendReceive(reqmsg.CurrentReqPackage.PackageMessage, RECEIVE_MAX_LENGTH);
                        message.RespPackageList.Enqueue(new PackageData(i, returnmsg));
                        AidLogHelper.Write(xQuant.Log4.LogLevel.Debug, string.Format("收发结束（CoreCommunicationHandler，MessageID={0};接收数据长度：{1}",message.MessageID,returnmsg != null?returnmsg.Length:0));
                    }

                    sca.DisConnect();
                    AidLogHelper.Write(xQuant.Log4.LogLevel.Debug, "断开连接（CoreCommunicationHandler，MessageID=" + message.MessageID);
                    return message;
                }
                #endregion
            }
            catch (SocketException sex)
            {
                StringBuilder sb = new StringBuilder("Socket Exception occured!");
                sb.AppendLine();
                sb.AppendFormat("ID:{0}; Biz ID:{1}; Platform Type:Core; Socket Error Code:{2}; Message:{3}.", message.MessageID, message.BizMsgID, sex.SocketErrorCode, sex.Message);
                //CommonHelper.AddEventLog(this.GetType().ToString(), sb.ToString());
                AidLogHelper.Write(xQuant.Log4.LogLevel.Error, sb.ToString());
                throw sex;

            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("Exception occured!");
                sb.AppendLine();
                sb.AppendFormat("ID:{0}; Biz ID:{1}; Platform Type:Core; Message:{2}.", message.MessageID, message.BizMsgID, ex.Message);
                sb.AppendLine();
                sb.AppendFormat("StackTrace:{0}.", ex.StackTrace);
                //CommonHelper.AddEventLog(this.GetType().ToString(), sb.ToString());
                AidLogHelper.Write(xQuant.Log4.LogLevel.Error, sb.ToString());
                throw ex;
            }

        }
        #endregion
        
    }
    
}
