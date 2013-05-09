using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xQuant.AidSystem.Communication;
using xQuant.AidSystem.CoreMessageData;
using System.Text;

namespace xQuant.AidSystem.ClientSyncWrapper
{
    internal class MsgHandlerEntry
    {
        internal const String Type_Convert_Error = "类型转换错误！";
        internal const String Encrypt_Pin_Format_Incorrect = "安全平台加密返回信息格式不正确";
        internal const String Encrypt_Pin_Failed = "安全平台加密失败，返回错误：";
        internal const String Info_Return_Core = "核心平台返回信息：";
        internal const String Info_Return_Pay = "支付平台返回信息：";
        internal const String System_Organ_ID = "000000";
        internal const String System_Teller_ID = "0000000";

        internal const String MQ_Exception_Title = "";//"MQ队列异常消息！";

        #region Common Methods
        private static ICommunicationHandler _coreHandler;
        private static ICommunicationHandler _paymentHandler;
        private static ICommunicationHandler _encryptHandler;
        private static ICommunicationHandler _nullableHandler;
        private static ICommunicationHandler _fingerMarksHandler;


        public static MessageData CreateMessageData(Guid msgid, PlatformType platform, string tellerno, string orgno, int msgbiztype, byte[] codemsg)
        {
            MessageData msgdata = new MessageData { MessageID = msgid, FirstTime = DateTime.Now, TragetPlatform = platform, TellerNO = tellerno, OrgNO=orgno, MsgBizType=msgbiztype, ReSentTime = 0 };
            msgdata.ReqPackageList.Enqueue(new PackageData(1, codemsg));
            msgdata.IsMultiPackage = false;
            return msgdata;
        }

        public static MessageData CreateMessageData(Guid msgid, PlatformType platform, string tellerno, string orgno, int msgbiztype, byte[] codemsg, string flowno)
        {
            MessageData msgdata = CreateMessageData(msgid, platform, tellerno, orgno, msgbiztype, codemsg);
            msgdata.BizMsgID = flowno;
            return msgdata;
        }

        public static MessageData CreateMessageData(Guid msgid, PlatformType platform, string tellerno, string orgno, int msgbiztype, List<byte[]> codemsg)
        {
            MessageData msgdata = new MessageData { MessageID = msgid, FirstTime = DateTime.Now, TragetPlatform = platform, TellerNO = tellerno, OrgNO = orgno, MsgBizType = msgbiztype, ReSentTime = 0 };
            short index = 0;
            foreach (var item in codemsg)
            {
                msgdata.ReqPackageList.Enqueue(new PackageData(index++, item));
            }
            msgdata.IsMultiPackage = true;
            return msgdata;
        }

        /// <summary>
        /// Devlivering to handler which has owned a corresponding Socket
        /// </summary>
        /// <param name="msgdata"></param>
        /// <returns></returns>
        public static MessageData DeliverMessage(MessageData msgdata)
        {
            if (msgdata == null)
            {
                return null;
            }
            switch (msgdata.TragetPlatform)
            {
                case PlatformType.Encrypt:
                    if (_encryptHandler == null)
                    {
                        _encryptHandler = new EncryptCommunicationHandler();
                    }
                    return _encryptHandler.MessageHandler(msgdata);

                case PlatformType.Core:
                    if (_coreHandler == null)
                    {
                        _coreHandler = new CoreCommunicationHandler();
                    }
                    return _coreHandler.MessageHandler(msgdata);
                case PlatformType.Payment:
                case PlatformType.PaymentDownload:
                    if (_paymentHandler == null)
                    {
                        _paymentHandler = new PayCommunicationHandler();
                    }
                    return _paymentHandler.MessageHandler(msgdata);

                case PlatformType.FingerMarks:
                    if (_fingerMarksHandler == null)
                    {
                        _fingerMarksHandler = new MarkComminicationHandler();
                    }
                    return _fingerMarksHandler.MessageHandler(msgdata);

                default:
                    if (_nullableHandler == null)
                    {
                        _nullableHandler = new NullableHandler();
                    }
                    return _nullableHandler.MessageHandler(msgdata);
            }
        }

        public static String ExtractOMsg(CoreBizMsgDataBase coremsg)
        {
            if (coremsg == null || coremsg.OmsgHandler == null)
            {
                return String.Empty;
            }
            StringBuilder outmsg = new StringBuilder();
            foreach (OMSG_Item_Handler item in coremsg.OmsgHandler.OMSGItemList)
            {
                outmsg.Append(item.MSG_TEXT);
                outmsg.AppendLine();
            }
            return outmsg.ToString();
        }
        #endregion
    }
}
