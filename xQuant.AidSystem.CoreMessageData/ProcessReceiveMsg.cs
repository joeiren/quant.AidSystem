using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using xQuant.AidSystem.CoreMessageData;
using xQuant.AidSystem.DBAction;
using System.Data;

namespace xQuant.AidSystem.CoreMessageData
{
    public class ProcessReceiveMsg
    {

        private  ProcessReceiveMsg()
        {
        }

        #region Unified Process
        public static BizMsgDataBase PreProcessReceiveMsg(Guid msgid, byte[] recbytes)
        {
            // 由msgid从对应表（内存List或者数据库表）获取对应业务类型
            object dataRef = null;
            if (ProcessSendMsg.MsgSentList.TryGetValue(msgid, out dataRef))
            {
                ProcessSendMsg.MsgSentList.Remove(msgid);
            }
            else
            {//不能由内存取到具体对应业务类型的数据引用，就尝试从数据库里读取
                String typename = RetrieveDBTypeFromDB(msgid);
                if (!String.IsNullOrEmpty(typename))
                {
                    Assembly asm = Assembly.GetExecutingAssembly();
                    dataRef = asm.CreateInstance(typename, false);
                }                
            }
            if (dataRef != null)
            {
                if (dataRef is EncryptTellerAuth)
                {
                    EncryptTellerAuth tellerData = dataRef as EncryptTellerAuth;
                    if (!tellerData.AfterEncrypt)
                    {
                        return BytesToEncryptObject(recbytes, (IMessageRespHandler)tellerData);
                    }
                    else
                    {
                        return BytesToCoreMsgData(recbytes, (IMessageRespHandler)tellerData);
                    }
                }
                else if (dataRef is PaymentBizMsgDataBase)
                {
                    return ByteToPaymentMsgData(recbytes, (IMessageRespHandler)dataRef);
                }
                else if (dataRef is CoreBizMsgMultiRespDataBase)
                {
                    return ByteToCoreMultiMsgData(recbytes, (IMessageRespHandler)dataRef);
                }
                else if (dataRef is FingerMarkData)
                {
                    return ByteToFingerMarkData(recbytes, (IMessageRespHandler)dataRef);
                }
                else
                {
                    return BytesToCoreMsgData(recbytes, (IMessageRespHandler)dataRef);
                }
            }
        
            return null;
        }

        public static BizMsgDataBase ByteToFingerMarkData(byte[] recbytes, IMessageRespHandler dataref)
        {
            if (dataref != null)
            {
                dataref.FromBytes(recbytes);
            }
            return dataref as FingerMarkData;
        }

        public static BizMsgDataBase ByteToCoreMultiMsgData(byte[] recbytes, IMessageRespHandler dataref)
        {
            if (recbytes != null && recbytes.Length > 0)
            {
                dataref.FromBytes(recbytes);
                return dataref as CoreBizMsgMultiRespDataBase;
            }
            else
            {
                return null;
            }
        }

        public static BizMsgDataBase BytesToCoreMsgData(byte[] recbytes, IMessageRespHandler dataref)
        {
            if (recbytes != null && recbytes.Length > 0)
            {
                byte[] buffer = new byte[CoreMessageHeader.TOTAL_WIDTH];
                if (recbytes.Length >= buffer.Length)
                {
                    Array.Copy(recbytes, buffer, buffer.Length);
                }
                else
                {
                    return null;
                }
                CoreMessageHeader msgHeader = new CoreMessageHeader();
                msgHeader.FromBytes(buffer);

                UInt32 mbLen = msgHeader.MH_MESSAGE_LENGTH - CoreMessageHeader.TOTAL_WIDTH;
                buffer = new byte[mbLen];

                //Assembly asm = Assembly.GetExecutingAssembly();
                //object dataobj = asm.CreateInstance(type.ToString(), false);
                //IMessageRespHandler bizMsgData = dataref as IMessageRespHandler;
                if (dataref != null)
                {
                    Array.Copy(recbytes, CoreMessageHeader.TOTAL_WIDTH, buffer, 0, mbLen);
                    dataref.FromBytes(buffer);
                }
                return dataref as CoreBizMsgDataBase;
            }
            else
            {
                return null;
            }
        }

        public static BizMsgDataBase BytesToEncryptObject(byte[] recbytes, IMessageRespHandler tellerData)
        {
            //Assembly asm = Assembly.GetExecutingAssembly();
            //object dataobj = asm.CreateInstance(type.ToString(), false);
            EncryptTellerAuth bizMsgData = tellerData as EncryptTellerAuth;
            if (bizMsgData != null)
            {
                bizMsgData.TellerEncrypt.FromBytes(recbytes);
                //tellerData.FromBytes(recbytes);
            }
            return tellerData as CoreBizMsgDataBase;
        }

        public static BizMsgDataBase ByteToPaymentMsgData(byte[] recbytes, IMessageRespHandler payData)
        {
            if (payData != null)
            {
                payData.FromBytes(recbytes);
            }
            return payData as PaymentBizMsgDataBase;
        }
        #endregion

        #region Retrieve Data Type from DB
        public static String RetrieveDBTypeFromDB(Guid msgid)
        {
            try
            {
                if (msgid != Guid.Empty)
                {
                    TTRD_AIDSYS_MSG_LOG_Manager manager = new TTRD_AIDSYS_MSG_LOG_Manager();
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("M_ID={0}", msgid.ToString());
                    DataTable table = manager.LogQuery(sb.ToString());
                    if (table != null && table.Rows.Count == 1)
                    {
                        return table.Rows[0]["MsgDataType"].ToString();
                    }
                }
            }
            catch (Exception)
            {
                return String.Empty;
            }
            return String.Empty;            
        }

        #endregion

    }
}
