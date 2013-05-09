using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xQuant.BizCore;
using xQuant.MQ;
using xQuant.AidSystem.Communication;
using System.Net.Sockets;
using xQuant.AidSystem.DBAction;

namespace AidSystemService
{
    /// <summary>
    /// 同步接口MQ队列处理
    /// </summary>
    public class MQHandlerSync : BaseCore
    {
        #region 成员变量
        private static MQHandlerSync _mqHandler;
        //MQ发送队列
        private MQSender _mqSender = null;
        //MQ接受队列
        private MQReceiver _mqReceiver = null;

        private const String EventSourceName = "xQuant.AidService.MQHandlerSync";
        #endregion

        #region private 成员函数
        /// <summary>
        /// 
        /// </summary>
        static MQHandlerSync()
        {
            _mqHandler = new MQHandlerSync();
        }

        /// <summary>
        /// 
        /// </summary>
        protected MQHandlerSync()
            : base()
        {
            _mqSender = new MQSender();
            _mqReceiver = new MQReceiver();

        }
        #endregion

        #region MessageReceiver
        /// <summary>
        /// 处理消息反馈
        /// </summary>
        /// <param name="aMessage"></param>
        public void OnMessageReceiver_CallBack(MQMessage aMessage)
        {
            
            MessageData msgdata = ConvertMessage(aMessage);
            AidLogHelper.Write(xQuant.Log4.LogLevel.Debug, string.Format("MQ已接收到消息（MQHandlerSync）,GUID={0}", msgdata.MessageID));
            if (!Inited)
            {
                Init();
            }
            try
            {
                bool isAsyncSocket = false;
                
                if (msgdata != null)
                {
                    msgdata.FirstTime = DateTime.Now;
                    if (isAsyncSocket)
                    {
                        OnMessageHandleAsync(msgdata);
                    }
                    else
                    {
                        DispatchMsg(msgdata);
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    StringBuilder sb = new StringBuilder("从MQ接收到消息后处理发生异常！(MQHandlerSync)");
                    SendToMQ(msgdata, new byte[1] { 0 }, sb.ToString());
                    sb.AppendLine();
                    sb.AppendFormat("GUID={0},Exception.Message:{1}.", msgdata.MessageID, ex.Message);
                    sb.AppendLine();
                    sb.AppendFormat("Stack Trace:{0}.", ex.StackTrace);
                    //CommonHelper.AddEventLog(EventSourceName, sb.ToString());
                    AidLogHelper.Write(xQuant.Log4.LogLevel.Error, sb.ToString());
                }
                catch
                {
                }
            }
        }
        #endregion

        #region public成员函数
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MQHandlerSync GetSingleton()
        {
            if (_mqHandler != null)
                return _mqHandler;
            lock (typeof(MQHandlerSync))
            {
                if (_mqHandler != null)
                    return _mqHandler;
                _mqHandler = new MQHandlerSync();
                return _mqHandler;
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns>结果</returns>
        public override bool Init()
        {
            return Init(ServerConfiguration.MQHost, ServerConfiguration.MQName, ServerConfiguration.AidSysResSync, ServerConfiguration.AidSysReqSync);
        }

        /// <summary>
        /// 初始化队列
        /// </summary>
        /// <param name="aQmgrHostName">MQ主机</param>
        /// <param name="aQmgrName">MQ队列管理器名称</param>
        /// <param name="aPayReq">MQ发送队列</param>
        /// <param name="aPayRes">MQ接收队列</param>
        /// <returns>结果</returns>
        public bool Init(string aQmgrHostName, string aQmgrName, string aAidSysReq, string aAidSysRes)
        {
            if (Inited)
                return true;

            try
            {
                // 初始化发送者
                _mqSender.Init(aAidSysReq);

                // 初始化接收者
                _mqReceiver.Init(aAidSysRes);

                // 注册回调接口
                _mqReceiver.RegisterMessageReceiver(this.OnMessageReceiver_CallBack);

                bool ret = base.Init();

                return ret;
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("MQ Init()初始化异常！");
                sb.AppendLine();
                sb.AppendFormat("Exception.Message:{0}.", ex.Message);
                //CommonHelper.AddEventLog(EventSourceName, sb.ToString());
                xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Error, String.Format("{0}.Init失败:\n{1}", GetType().FullName, sb.ToString()));
                return false;
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public override void Close()
        {
            base.Close();
            _mqSender.Close();
            _mqReceiver.Close();

        }

        /// <summary>
        /// 服务标题
        /// </summary>
        public override string Caption
        {
            get
            {
                return "外围系统服务";
            }
        }
        #endregion

        #region 公布函数

        #endregion

        #region 消息处理（从MQ取回发送到后台）
        #region Sync message handler
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
                    handler = new CoreCommunicationHandler();
                    break;
                case PlatformType.FingerMarks:
                    handler = new MarkComminicationHandler();
                    break;
                default:
                    handler = new NullableHandler();
                    break;
                    
            }

            AidLogHelper.Write(xQuant.Log4.LogLevel.Debug, "开始分发（MQHandlerSync）,GUID=" + msgdata.MessageID);
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
            AidLogHelper.Write(xQuant.Log4.LogLevel.Debug, "分发结束（MQHandlerSync）,GUID="+ msgdata.MessageID);
        }

        void DispatchMsg_DispatchCompleted(object sender, TransmitCompletedEventArgs e)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                if (e.Cancelled)
                {
                    AidLogHelper.Write(xQuant.Log4.LogLevel.Debug, string.Format("分发结束返回处理（DispatchMsg_DispatchCompleted）：GUID={0},e.Cancelled", e.MessageData.MessageID));
                    result.AppendLine();
                    result.Append(" Canceled!");
                }
                else if (e.Error != null)
                {
                    AidLogHelper.Write(xQuant.Log4.LogLevel.Debug, string.Format("分发结束返回处理（DispatchMsg_DispatchCompleted）：GUID={0},e.Error.Message={1}",e.MessageData.MessageID,e.Error.Message));
                    if (e.Error is SocketException)
                    {
                        SocketException sex = e.Error as SocketException;
                        result.AppendLine();
                        result.AppendFormat("Socket错误:{0}. 错误代码:{1}", e.Error.Message, sex.SocketErrorCode);
                    }
                    else
                    {
                        result.AppendLine();
                        result.Append(e.Error.Message);
                    }
                    byte[] rebytes = e.MessageData.GetRespMessage();
                    if (rebytes == null)
                    {
                        rebytes = new byte[] { 0 };
                    }
                    SendToMQ(e.MessageData, rebytes, result.ToString()); 
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
                        AidLogHelper.Write(xQuant.Log4.LogLevel.Debug, string.Format("分发结束返回处理（DispatchMsg_DispatchCompleted）：GUID={0};正常；返回字节长度={1}",e.MessageData.MessageID,rebytes.Length));
                        SendToMQ(e.MessageData, rebytes, "");                     
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    StringBuilder sb = new StringBuilder("AidSystemService DispatchMsg_DispatchCompleted发生异常！(MQHandlerSync)");
                    sb.AppendLine();
                    sb.AppendFormat("GUID={0},Exception.Message:{1}.", e.MessageData.MessageID, ex.Message);
                    SendToMQ(e.MessageData, new byte[1] { 0 }, sb.ToString());
                    
                    AidLogHelper.Write(xQuant.Log4.LogLevel.Error, "分发结束返回处理（DispatchMsg_DispatchCompleted）发生异常！" + ex.Message);
                    //CommonHelper.AddEventLog(EventSourceName, sb.ToString());
                }
                catch (Exception innerex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("GUID={0},Exception.Message{1}.StackTrace:{2}.", e.MessageData.MessageID, innerex.Message, innerex.StackTrace);
                    AidLogHelper.Write(xQuant.Log4.LogLevel.Error, "分发结束返回处理（DispatchMsg_DispatchCompleted）异常处理内部有问题！" + sb.ToString());
                    //CommonHelper.AddEventLog(EventSourceName, sb.ToString());
                }
            }
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
                    handler = new EncryptCommunicationHandlerAsync();
                    break;
                case PlatformType.Payment:
                case PlatformType.PaymentDownload:
                    handler = new PayCommunicationHandlerAsync();
                    break;
                case PlatformType.Core:
                    handler = new CoreCommunicationHandlerAsync();
                    break;
                case PlatformType.FingerMarks:
                    //handler = new MarkComminicationHandler();
                    break;
                default:
                    handler = new NullableHandlerAsync();
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
        void MessageHandleAsync_Completed(MessageData respData, Exception ex)
        {
            
            if (respData == null)
            {
                //CommonHelper.AddEventLog(EventSourceName, "返回数据为空！");
                return;
            }
            if (ex != null)
            {
                //CommonHelper.AddEventLog(EventSourceName, ex.Message);
                RecordDB(ex.Message, respData);
                MQMessage exmq = GetMessage(respData.BizMsgID, respData.MessageID, respData.TragetPlatform, !respData.IsMultiPackage, respData.SourceIP, ex.Message);
                
                return;
            }
            byte[] rebytes = respData.GetRespMessage();

            // 发送个MQ(MessageID, buffer)
            MQMessage msg = GetMessage(respData.BizMsgID, respData.MessageID, respData.TragetPlatform, !respData.IsMultiPackage, respData.SourceIP, "");
            int realLen = rebytes.Length;
            if (respData.TragetPlatform != PlatformType.Encrypt)
            {
                byte end = 0;
                realLen = Array.IndexOf(rebytes, end);
            }

            byte[] buffer = new byte[realLen];
            Array.Copy(rebytes, buffer, realLen);
            msg.Byte = buffer;
            lock (typeof(MQHandlerSync))
            {
                _mqSender.SendMessage(msg);
            }
            AidLogHelper.Write(xQuant.Log4.LogLevel.Debug, "socket处理完，MQ发回消息（MQHandlerSync）");
        }
        #endregion

        private void RecordDB(String result, MessageData msg)
        {
            if (msg != null)
            {
                TTRD_AIDSYS_MSG_LOG logrow = new TTRD_AIDSYS_MSG_LOG();
                logrow.M_ID = msg.MessageID.ToString();
                logrow.M_ERROR = result;
                logrow.M_STATE = "1";
                logrow.M_S_CONTENT = msg.CurrentReqPackage.PackageMessage;
                logrow.M_SUBID = msg.CurrentReqPackage.PackageID;
                logrow.M_SERIALNO = msg.BizMsgID;
                logrow.M_ISSINGLE = msg.IsMultiPackage ? "0" : "1";
                logrow.M_PLATTYPE = (int)msg.TragetPlatform;
                logrow.M_SENDDATE = msg.FirstTime.ToString("yyyy-MM-dd HH:mm:ss");
                TTRD_AIDSYS_MSG_LOG_Manager manager = new TTRD_AIDSYS_MSG_LOG_Manager();
                manager.LogInsert(logrow);
            }
        }

        private void SendToMQ(MessageData msgdata, byte[] recBytes, String definedError)
        {
            if (msgdata == null)
            {
                return;
            }
            if (recBytes == null || recBytes.Length == 0)
            {
                definedError = string.IsNullOrEmpty(definedError) ? "MQ发送返回数据为空" : definedError;
            }
            MQMessage msg = GetMessage(msgdata.BizMsgID, msgdata.MessageID, msgdata.TragetPlatform, msgdata.IsMultiPackage, msgdata.SourceIP, definedError);
            int realLen = 0;
            if (recBytes != null)
            {
                realLen = recBytes.Length;
                if (msgdata.TragetPlatform != PlatformType.Encrypt)
                {
                    byte end = 0;
                    realLen = Array.IndexOf(recBytes, end);
                }
                if (realLen >= 0)
                {
                    byte[] buffer = new byte[realLen];
                    Array.Copy(recBytes, buffer, realLen);
                    msg.Byte = buffer;
                }
            }
            try
            {
                if (!Inited)
                {
                    Init();
                }
                lock (typeof(MQHandlerSync))
                {
                    _mqSender.SendMessage(msg);
                }
            }
            catch (Exception ex)
            {
                if (msg != null)
                {
                    AidLogHelper.Write(xQuant.Log4.LogLevel.Error, string.Format("_mqSender.SendMessage()发生异常!Exception：{0}!\r\nStackTrace:{1}!",ex.Message, ex.StackTrace));
                    //try
                    //{
                    //    Close();
                    //    Init();
                    //}
                    //catch (Exception e)
                    //{
                    //    AidLogHelper.Write(xQuant.Log4.LogLevel.Error, string.Format("重新开始初始化发生异常！Exception：{0}!\r\nStackTrace:{1}!", e.Message, e.StackTrace));
                    //}
                }
            }
            
        }
        #endregion

        #region MQ请求的XML
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        private MQMessage GetMessage(String msgId, Guid id, PlatformType targetSys, bool isSinglePkg, String sourceIP, String definedError)
        {
            MQMessage msg = new MQMessage();
            msg.HeaderUser.UserServiceId.Value = msgId;
            msg.HeaderMcd.McdType = MQMessage.MQHeaderMcd.Request;
            //msg.HeaderUser.UserBaseDate.Value = DateTimeHelper.NowDateToString;
            //msg.HeaderUser.UserServiceId.Value = id.ToString();
            msg.HeaderUser.UserServiceStatus.Value = "0";
            msg.HeaderUser.UserServiceGuage.Value = "0";
            msg.HeaderUser.UserServiceGuageInfo.Value = "";
            msg.HeaderUser.UserTaskCode.Value = "";
            msg.HeaderUser.UserUserId.Value = "";            

            msg.HeaderUser.UserDefined.Add(new MQParameter<string>("TARGETSYS", targetSys.ToString()));
            msg.HeaderUser.UserDefined.Add(new MQParameter<string>("PKGTYPE", isSinglePkg.ToString()));
            msg.HeaderUser.UserDefined.Add(new MQParameter<string>("GUID", id.ToString()));
            msg.HeaderUser.UserDefined.Add(new MQParameter<string>("ERROR", definedError));
            msg.HeaderUser.UserDefined.Add(new MQParameter<string>("IP", sourceIP));

            return msg;
        }

        private MessageData ConvertMessage(MQMessage mqmsg)
        {
            if (mqmsg == null)
            {
                return null;
            }
            MessageData data = new MessageData();
            string msgid = FindDefinedParameterValue(mqmsg.HeaderUser.UserDefined, "GUID");
            if (!String.IsNullOrEmpty(msgid))
            {
                data.MessageID = new Guid(msgid);
            }
            else
            {
                return null;
            }
            
            data.BizMsgID = mqmsg.HeaderUser.UserServiceId.Value;
            bool isSingle = true;
            string mpkgFlag = FindDefinedParameterValue(mqmsg.HeaderUser.UserDefined, "PKGTYPE");
            if (!String.IsNullOrEmpty(mpkgFlag))
            {
                if (bool.TryParse(mpkgFlag, out isSingle))
                {
                    data.IsMultiPackage = !isSingle;
                }
                else
                {
                    data.IsMultiPackage = false;
                }
            }
            else
            {
                return null;
            }

            string platform = FindDefinedParameterValue(mqmsg.HeaderUser.UserDefined, "TARGETSYS");
            if (!String.IsNullOrEmpty(platform))
            {
                data.TragetPlatform = (PlatformType)Enum.Parse(typeof(PlatformType), platform);
            }
            else
            {
                return null;
            }

            string sourceIP = FindDefinedParameterValue(mqmsg.HeaderUser.UserDefined, "IP");
            if (!String.IsNullOrEmpty(sourceIP))
            {
                data.SourceIP = sourceIP;
            }            

            data.ReqPackageList.Enqueue(new PackageData(1, mqmsg.Byte));

            return data;  
        }

        private string FindDefinedParameterValue(List<MQParameter<string>> mqParameterList, string parameterName)
        {
            if (mqParameterList == null)
            {
                return String.Empty;
            }
            foreach (MQParameter<String> mqParameter in mqParameterList)
            {
                if (mqParameter.Name.CompareTo(parameterName) == 0)
                {
                    return mqParameter.Value;
                }
            }
            return String.Empty;
        }

        #endregion
    }
}
