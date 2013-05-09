using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xQuant.AidSystem.Communication;
using xQuant.AidSystem.DBAction;
using System.Data;
using System.Net.Sockets;
using xQuant.MQ;

namespace AidSystemService
{
    public class LoadResendLog
    {

        private List<MessageData> _msgList;
        private MQSender _mqSender;
        private MQReceiver _mqReceiver;

        public LoadResendLog()
        {
            _msgList = new List<MessageData>();
        }

        public bool InitMQ(MQSender sender, MQReceiver receiver)
        {
            if (sender != null && receiver != null)
            {
                _mqSender = sender;
                _mqReceiver = receiver;
                return true;
            }
            return false;
        }
        public void GetResendMessages()
        {
            try
            {
                String condition = "M_STATE='1'";
                TTRD_AIDSYS_MSG_LOG_Manager manager = new TTRD_AIDSYS_MSG_LOG_Manager();
                DataTable table = manager.LogQuery(condition);
                
                foreach (DataRow row in table.Rows)
                {
                    try
                    {
                        MessageData data = new MessageData();
                        data.MessageID = new Guid(row["M_ID"].ToString());
                        data.BizMsgID = row["M_SERIALNO"].ToString();
                        data.FirstTime = Convert.ToDateTime(row["M_SENDDATE"].ToString());
                        data.TragetPlatform = (PlatformType)Convert.ToInt32(row["M_PLATTYPE"].ToString());
                        data.IsMultiPackage = Convert.ToInt32(row["M_ISSINGLE"]) != 1;
                        data.ReqPackageList.Enqueue(new PackageData(Convert.ToInt16(row["M_SUBID"]), (byte[])row["M_S_CONTENT"]));
                        _msgList.Add(data);

                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void OnSend()
        {
            bool isAsyncSocket = false;
            if (isAsyncSocket)
            {
                foreach (MessageData resenddata in _msgList)
                {
                    OnMessageHandleAsync(resenddata);
                }
            }
            else
            {
                foreach (MessageData resenddata in _msgList)
                {
                    DispatchMsg(resenddata);
                }
            }
        }

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
                    //CommonHelper.AddEventLog(EventSourceName, result.ToString());
                    //RecordDB(result.ToString(), e.MessageData);
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
                        MQMessage msg = GetMessage(e.MessageData.BizMsgID, e.MessageData.MessageID, e.MessageData.TragetPlatform, e.MessageData.IsMultiPackage);
                        int realLen = rebytes.Length;
                        if (e.MessageData.TragetPlatform != PlatformType.Encrypt)
                        {
                            byte end = 0;
                            realLen = Array.IndexOf(rebytes, end);
                        }
                        
                        byte[] buffer = new byte[realLen];
                        Array.Copy(rebytes, buffer, realLen);
                        msg.Byte = buffer;
                        UpdateLogDB(e.MessageData);
                        _mqSender.SendMessage(msg);                       
                    }
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("LoadResendLog MQ DispatchMsg_DispatchCompleted异常！");
                sb.AppendLine();
                sb.AppendFormat("Exception.Message:{0}.", ex.Message);
                //CommonHelper.AddEventLog(EventSourceName, sb.ToString());
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
                    //handler = new EncryptCommunicationHandler();
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
            if (ex != null)
            {
                //CommonHelper.AddEventLog(EventSourceName, ex.Message);
                //RecordDB(ex.Message, respData);
                return;
            }
            if (respData == null)
            {
                //CommonHelper.AddEventLog(EventSourceName, "返回数据为空！");
                return;
            }

            byte[] rebytes = respData.GetRespMessage();

            // 发送个MQ(MessageID, buffer)
            MQMessage msg = GetMessage(respData.BizMsgID, respData.MessageID, respData.TragetPlatform, respData.IsMultiPackage);
            int realLen = rebytes.Length;
            if (respData.TragetPlatform != PlatformType.Encrypt)
            {
                byte end = 0;
                realLen = Array.IndexOf(rebytes, end);
            }

            byte[] buffer = new byte[realLen];
            Array.Copy(rebytes, buffer, realLen);
            msg.Byte = buffer;
            _mqSender.SendMessage(msg);
        }
        #endregion

        private void UpdateLogDB( MessageData msg)
        {
            TTRD_AIDSYS_MSG_LOG logrow = new TTRD_AIDSYS_MSG_LOG();
            logrow.M_ID = msg.MessageID.ToString();
            logrow.M_STATE = "0";
            
            TTRD_AIDSYS_MSG_LOG_Manager manager = new TTRD_AIDSYS_MSG_LOG_Manager();
            manager.LogUpdate(logrow);
            return;
        }

        private MQMessage GetMessage(String msgId, Guid id, PlatformType targetSys, bool isSinglePkg)
        {
            MQMessage msg = new MQMessage();
            msg.HeaderUser.UserServiceId.Value = msgId;
            msg.HeaderMcd.McdType = MQMessage.MQHeaderMcd.Request;
            msg.HeaderUser.UserServiceStatus.Value = "0";
            msg.HeaderUser.UserServiceGuage.Value = "0";
            msg.HeaderUser.UserServiceGuageInfo.Value = "";
            msg.HeaderUser.UserTaskCode.Value = "";
            //msg.HeaderUser.UserUserId.Value = USER_CODE;

            msg.HeaderUser.UserDefined.Add(new MQParameter<string>("TARGETSYS", targetSys.ToString()));
            msg.HeaderUser.UserDefined.Add(new MQParameter<string>("PKGTYPE", isSinglePkg.ToString()));
            msg.HeaderUser.UserDefined.Add(new MQParameter<string>("GUID", id.ToString()));

            return msg;
        }
        #endregion
    }
}
