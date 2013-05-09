using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xQuant.MQ;
using xQuant.BizCore;
using xQuant.AidSystem.Communication;
using xQuant.AidSystem.CoreMessageData;

namespace xQuant.AidSystem.ClientSyncWrapper
{
    internal class MQMsgHandlerEntry
    {
        private static bool Inited = false;
        private static MQExternalHandler _handler = null;
        private static void Init()
        {
            try
            {
                lock (typeof(MQMsgHandlerEntry))
                {
                    if (InitMQConnection())
                    {
                        _handler = MQExternalHandler.GetSingleton();
                        Inited = _handler.Init();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static MessageData DeliverMessage(MessageData reqmsg, byte[] bytes, out bool hasSent)
        {
            if (bytes != null)
            {
                List<byte[]> list = new List<byte[]>();
                list.Add(bytes);
                return PostMessage(reqmsg, list, out hasSent);
            }
            else
            {
                hasSent = false;
                return null;
            }
        }


        public static MessageData DeliverMultiMessage(MessageData reqmsg, List<byte[]> bytes, out bool hasSent)
        { 
            return PostMessage( reqmsg,  bytes, out hasSent);
        }

        public static MessageData PostMessage(MessageData reqmsg, List<byte[]> bytes, out bool hasSent)
        {
            if (!Inited)
            {
                MQMsgHandlerEntry.Init();
                if (!Inited || _handler == null)
                {
                    throw new AidException(MsgHandlerEntry.MQ_Exception_Title, "初始化MQ连接失败！(MQMsgHandlerEntry.DeliverMessage)", null);
                }
            }
            if (_handler != null)
            {
                try
                {
                    return _handler.GetSyncResult(reqmsg, bytes, out hasSent);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new AidException(MsgHandlerEntry.MQ_Exception_Title, "无法得到MQExternalHandler的singleton实例！(MQMsgHandlerEntry.DeliverMessage)", null);
            }
        }

        /// <summary>
        /// 初始化MQ连接
        /// </summary>
        private static bool InitMQConnection()
        {
            if (!MQConnection.Default.Connected)
            {
                try
                {
                    MQConnection.Default.Init(MQRepository.GetSingleton().MQClientIdentifier, MQRepository.GetSingleton().MQHost, MQRepository.GetSingleton().MQPort, MQRepository.GetSingleton().MQName);
                    MQConnection.Default.Open();
                }
                catch (Exception ex)
                {
                    //AddEventLog(ex.Message);
                    throw ex;
                }
            }
            if (!MQConnection.Default.Connected)
            {
                //xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Error, "mq连接失败！");
                //AddEventLog("mq连接失败！");
                return false;
            }
            return true;
        }
    }
}
