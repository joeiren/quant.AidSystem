using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xQuant.BizCore;
using xQuant.MQ;
using xQuant.AidSystem.Communication;
using System.Net.Sockets;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Net;
using xQuant.AidSystem.DBAction;
using System.Configuration;
using xQuant.AidSystem.CoreMessageData;
using xIR.Framework.Transactions;

namespace xQuant.AidSystem.ClientSyncWrapper
{
    /// <summary>
    /// 为同步接口服务的MQ收发处理器
    /// </summary>
    class MQExternalHandler: BaseCore
    {
        #region 成员变量
        private static MQExternalHandler _mqHandler;
        //MQ发送队列
        private MQSender _mqSender = null;
        //MQ接受队列
        private MQReceiver _mqReceiver = null;
        // 同步MQ模式
        private bool _isSyncReceiver = Convert.ToBoolean(ConfigurationSettings.AppSettings["SyncMQModel"]??"True"); 
        // 发送者的locker
        private object _senderLocker = new object();
        // 异步接收管理
        private AsyncSemaphoreManager _receiverManager = new AsyncSemaphoreManager();
        #endregion

        #region private 成员函数
        /// <summary>
        /// 
        /// </summary>
        static MQExternalHandler()
        {
            _mqHandler = new MQExternalHandler();
        }

        /// <summary>
        /// 
        /// </summary>
        protected MQExternalHandler()
            : base()
        {
            _mqSender = new MQSender();
            _mqReceiver = new MQReceiver();

        }
        #endregion

        #region public成员函数
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MQExternalHandler GetSingleton()
        {
            if (_mqHandler != null)
                return _mqHandler;
            lock (typeof(MQExternalHandler))
            {
                if (_mqHandler != null)
                    return _mqHandler;
                _mqHandler = new MQExternalHandler();
                return _mqHandler;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns>结果</returns>
        public override bool Init()
        {
            return Init(ServerConfiguration.MQHost, ServerConfiguration.MQName, ServerConfiguration.AidSysReqSync, ServerConfiguration.AidSysResSync);
        }

        /// <summary>
        /// 初始化队列
        /// </summary>
        /// <param name="aQmgrHostName">MQ主机</param>
        /// <param name="aQmgrName">MQ队列管理器名称</param>
        /// <param name="aAidSysReq">MQ发送队列</param>
        /// <param name="aAidSysRes">MQ接收队列</param>
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

                if (!_isSyncReceiver)
                {
                    // 注册回调接口
                    _mqReceiver.RegisterMessageReceiver(this.OnMessageReceiver_CallBack, "IP='" + MQMsgCommonHelper.GetHostIP() + "' ");
                }
                bool ret = base.Init();
                xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Debug, string.Format("MQ Init结束！结果：{0}！",ret.ToString()));
                return ret;
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("MQ Init()初始化异常！");
                sb.AppendLine();
                sb.AppendFormat("Exception.Message:{0}.", ex.Message);
                xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Error, sb.ToString());
                throw new AidException(MsgHandlerEntry.MQ_Exception_Title, ex.Message, ex);
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
                return "外围系统同步MQExternal服务";
            }
        }

        /// <summary>
        /// 获取调用接口的结果
        /// </summary>
        /// <param name="msgdata"></param>
        /// <param name="recBytes"></param>
        /// <param name="hasSent"></param>
        /// <returns></returns>
        public MessageData GetSyncResult(MessageData msgdata, List<byte[]> recBytes, out bool hasSent)
        {
            hasSent = false;
            if (msgdata == null)
            {
                return null;
            }
            try
            {
                if (!_isSyncReceiver)
                {
                    if (!_receiverManager.InsertSemaphore(msgdata.MessageID, false))
                    {
                        throw new AidException(MsgHandlerEntry.MQ_Exception_Title, "加入异步等待信号量错误！请重试...", null);
                    }
                }

                MessageData data = null;
                lock (_senderLocker)
                {
                    data = SendToMQInService(msgdata, recBytes, out hasSent);
                }
                //同步接收
                if (_isSyncReceiver)
                {
                    return data;
                }
                else
                {
                    if (_receiverManager.WaitSemaphore(msgdata.MessageID, 1000 * 70))
                    {
                        xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Debug, string.Format("<{0}>:已接收MQ返回！", msgdata.MessageID));
                        return AfterReleaseAsync(msgdata.MessageID);
                    }
                    else
                    {
                        xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Debug, string.Format("<{0}>:接收MQ返回超时！", msgdata.MessageID));
                        // 等待接收超时视为“发送成功”
                        ReleaseAsyncTimeout(msgdata.MessageID, "接收MQ返回超时！", MsgLogState.SendSucceed);
                        throw new AidException(MsgHandlerEntry.MQ_Exception_Title, "接收MQ返回超时", null);
                    }
                }
            }
            catch (AidException aidex)
            {
                throw aidex;
            }
            catch (Exception ex)
            {
                throw new AidException(MsgHandlerEntry.MQ_Exception_Title, ex.Message, ex);
            }
            finally
            {
                CleanUpAsync(msgdata.MessageID);
            }
        }
        #endregion

        #region 异步辅助函数，处理等待信号量和异步接收结果

        private void AfterReceivedAsync(MessageData data, string error, MsgLogState state)
        {
            if (data == null || data.MessageID == Guid.Empty)
            {
                return;
            }
            string dbexception = "";
            // 先更新数据库再释放等待信号量，保证多线程更新数据库前后顺序
            MQMsgCommonHelper.UpdateMQLog(data, "", error, state, out dbexception);
            _receiverManager.InsertAsyncResult(data.MessageID, state == MsgLogState.RecvSucceed? (object)data : (object)error);
            _receiverManager.ReleaseSemaphore(data.MessageID);
        }

        private MessageData AfterReleaseAsync(Guid id)
        {
            if (id != Guid.Empty)
            {
                object data = _receiverManager.GetAsyncResult(id);
                _receiverManager.RemoveSemaphore(id);
                _receiverManager.RemoveAsyncResult(id);
                if (data is MessageData)
                {
                    return data as MessageData;
                }
                else
                {
                    throw new AidException(MsgHandlerEntry.MQ_Exception_Title, data.ToString(), null);
                }
            }
            return null;
        }

        private void ReleaseAsyncTimeout(Guid id, string error, MsgLogState state)
        {
            if (id != Guid.Empty)
            {
                string dbexception = "";
                MQMsgCommonHelper.UpdateMQLog(id, error, state, out dbexception);
                _receiverManager.RemoveSemaphore(id);
            }
        }

        private void CleanUpAsync(Guid id)
        {
            if (id != Guid.Empty)
            {
                _receiverManager.RemoveSemaphore(id);
                _receiverManager.RemoveAsyncResult(id);
            }
        }

        #endregion

        #region 消息处理
        /// <summary>
        /// 发送处理
        /// </summary>
        /// <param name="msgdata"></param>
        /// <param name="recBytes"></param>
        /// <param name="sent"></param>
        /// <returns></returns>
        public MessageData SendToMQInService(MessageData msgdata, List<byte[]> recBytes, out bool sent)
        {
            string dbexception = "";
            
            sent = false;
            try
            {
                if (!Inited)
                {
                    Init();
                }
                MQMessage msg = MQMsgCommonHelper.ToMQMessage(msgdata.BizMsgID, msgdata.MessageID, msgdata.TragetPlatform, !msgdata.IsMultiPackage, ""/*USER_CODE*/, "");
                if (msgdata.IsMultiPackage)
                {
                    int realLen = (from item in recBytes
                                   select item.Length).Sum();
                    byte[] buffer = new byte[realLen];
                    int offset = 0;
                    StringBuilder arrayoffset = new StringBuilder();
                    foreach (var item in recBytes)
                    {
                        Array.Copy(item, 0, buffer, offset, item.Length);
                        offset += item.Length;
                        arrayoffset.AppendFormat("{0},", offset);
                    }
                    msg.HeaderUser.UserDefined.Add(new MQParameter<string>("ByteOffset", arrayoffset.ToString()));
                    msg.Byte = buffer;
                }
                else
                {
                    if (recBytes.Count > 0)
                    {
                        int realLen = recBytes[0].Length;

                        byte[] buffer = new byte[realLen];
                        Array.Copy(recBytes[0], buffer, realLen);
                        msg.Byte = buffer;
                    }                   
                }
               
                using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew)) // 独立事务
                {
                    if (MQMsgCommonHelper.ExistedByFlowNO(msgdata.BizMsgID) > 0)
                    {
                        MQMsgCommonHelper.UpdateMQLogByFlowNO(msgdata, " ", "再次发送！", MsgLogState.SendSucceed, out dbexception); 
                    }
                    else
                    {
                        MQMsgCommonHelper.AddMQLog(msgdata, msg.Byte, MsgLogState.SendSucceed, out dbexception);
                    }
                    _mqSender.SendMessage(msg);                    
                    trans.Complete();
                    sent = true;
                    xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Debug, string.Format("<{0}>:已发送到MQ", msgdata.MessageID));
                }
                if (_isSyncReceiver && sent)
                {
                    MQMessage recmqMsg = null;
                    bool succeed = false;
                    MessageData resultdata = null;
                    while (!succeed)
                    {
                        recmqMsg = _mqReceiver.ReceiveMessage(60 * 1000 * 1, "IP='" + MQMsgCommonHelper.GetHostIP() + "' ");
                        if (recmqMsg == null)
                        {
                            MQMsgCommonHelper.UpdateMQLog(msgdata, "", "消息队列响应超时！", MsgLogState.SendSucceed, out dbexception);
                            throw new AidException(MsgHandlerEntry.MQ_Exception_Title, "消息队列响应超时！", null);
                            //Exception timeout = new Exception("消息队列响应超时！");
                            //timeout.Data.Add("MQ_TIMEOUT", "消息队列响应超时");
                            //throw timeout;
                        }
                        string error = "";
                        MessageData temp = PerformReceiverSync(recmqMsg, out error);
                        succeed = temp.MessageID == msgdata.MessageID;
                        xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Debug, string.Format("<{0}>:已接收MQ返回", temp.MessageID));
                        if (succeed)
                        {
                            if (string.IsNullOrEmpty(error))
                            {
                                MQMsgCommonHelper.UpdateMQLog(temp, "", "", MsgLogState.RecvSucceed, out dbexception);
                                resultdata = temp;
                            }
                            else
                            {
                                MQMsgCommonHelper.UpdateMQLog(temp, "", error, MsgLogState.RecvFailed, out dbexception);
                                throw new AidException(MsgHandlerEntry.MQ_Exception_Title, error, null);
                            }
                        }
                        else
                        {
                            MQMsgCommonHelper.UpdateMQLog(temp, "", "该消息已过期!", MsgLogState.RecvOverdue, out dbexception);
                        }
                    }
                    resultdata.OrgNO = msgdata.OrgNO;
                    resultdata.MsgBizType = msgdata.MsgBizType;
                    return resultdata;
                }
                return null;
            }
            catch (AidException aidex)
            {
                throw aidex;
            }
            catch (Exception ex)
            {
                throw new AidException(MsgHandlerEntry.MQ_Exception_Title, ex.Message, ex);
            }
        }

        /// <summary>
        /// 同步接收后处理
        /// </summary>
        /// <param name="recmq"></param>
        /// <returns></returns>
        private MessageData PerformReceiverSync(MQMessage recmq, out string error)
        {
            try
            {
                error = "";
                MessageData msgdata = MQMsgCommonHelper.FromMQMessage(recmq, out error);
                return msgdata;
            }
            catch (Exception ex)
            {
                throw new AidException(MsgHandlerEntry.MQ_Exception_Title, ex.Message, ex);
            }
        }

        /// <summary>
        /// 异步回调接收后的处理
        /// </summary>
        /// <param name="mqmsg"></param>
        private void PerformReceiverAsync(MQMessage mqmsg)
        {
            string error = "";
            MessageData msgdata = MQMsgCommonHelper.FromMQMessage(mqmsg, out error);
         
            xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Debug, string.Format("异步接收MQ返回！GUID:{0}.Error:{1}.", msgdata.MessageID, error));
            if (_receiverManager.GetSemaphore(msgdata.MessageID) == null)
            {
                //过期消息
                AfterReceivedAsync(msgdata, error, MsgLogState.RecvOverdue);
            }
            else
            {
                if (string.IsNullOrEmpty(error))
                {
                    //正常接收并无错误
                    AfterReceivedAsync(msgdata, "已接收到报文！", MsgLogState.RecvSucceed);
                }
                else
                {
                    //接收过程中，在服务端发生错误
                    AfterReceivedAsync(msgdata, error, MsgLogState.RecvFailed);
                }
            }
        }

        #region MessageReceiver
        /// <summary>
        /// 处理从同步MQ结束后的消息反馈
        /// </summary>
        /// <param name="aMessage"></param>
        public void OnMessageReceiver_CallBack(MQMessage aMessage)
        {
            try
            {
                PerformReceiverAsync(aMessage);
            }
            catch (Exception ex)
            {
                xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Error, string.Format("接收处理后发生异常！Exception:{0}!\r\nStackTrace:{1}!", ex.Message,ex.StackTrace));
            }
        }
        #endregion
        #endregion
    }
}
