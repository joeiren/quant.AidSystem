using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using xQuant.AidSystem.DBAction;
using System.Data;

namespace xQuant.AidSystem.CoreMessageData
{
    public class MsgTransfer
    {
        private MsgTransfer()
        {
        }
        #region Common Fields
        private const String MESSAGE_ENCODE_EXCEPTION = "消息编码异常";
        private const String MESSAGE_DECODE_EXCEPTION = "消息解码异常";

        private static object _locker = new object();

        private static Dictionary<Guid, object> _msgSentList;
        public static Dictionary<Guid, object> MsgSentList
        {
            get
            {
                if (_msgSentList == null)
                {
                    lock (_locker)
                    {
                        _msgSentList = new Dictionary<Guid, object>();
                    }
                    return _msgSentList;
                }
                else
                {
                    return _msgSentList;
                }
            }
        }
        private static void InsertMsgList(Guid msgid, object dataref)
        {
            lock (_locker)
            {
                MsgSentList.Add(msgid, dataref);
            }
        }

        private static bool RemoveMsgList(Guid msgid)
        {
            lock (_locker)
            {
                return MsgSentList.Remove(msgid);
            }
        }
        #endregion

        #region Unified Process
        public static BizMsgDataBase DecodeMsg(Guid msgid, byte[] recbytes)
        {
            // 由msgid从对应表（内存List或者数据库表）获取对应业务类型
            object dataRef = null;
            if (MsgSentList.TryGetValue(msgid, out dataRef))
            {
                MsgSentList.Remove(msgid);
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
                return DecodeBinaryToMsg(recbytes, (IMessageRespHandler)dataRef);
            }
        
            return null;
        }

        private static BizMsgDataBase DecodeBinaryToMsg(byte[] recbytes, IMessageRespHandler respdata)
        {
            try
            {
                if (respdata != null)
                {
                    if (respdata is EncryptTellerAuth)
                    {
                        EncryptTellerAuth tellerData = respdata as EncryptTellerAuth;
                        if (tellerData != null)
                        {
                            if (tellerData.AfterEncrypt)
                            {
                                tellerData.FromBytes(recbytes);
                                return tellerData;
                            }
                            else
                            {
                                tellerData.TellerEncrypt.FromBytes(recbytes);
                                return tellerData;
                            }
                        }
                        else
                        {
                            throw new Exception(MESSAGE_DECODE_EXCEPTION);
                        }
                    }
                    else
                    {
                        BizMsgDataBase bizData = respdata.FromBytes(recbytes) as BizMsgDataBase;
                        //return respdata.FromBytes(recbytes) as BizMsgDataBase;
                        return bizData;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Error, string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
                throw new Exception(MESSAGE_DECODE_EXCEPTION, ex);
            }
        }

        /// <summary>
        /// 核心编码组织
        /// </summary>
        /// <param name="data"></param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid CodeMsgToBinary(IMessageReqHandler data, ref byte[] codemsg)
        {
            try
            {
                ((BizMsgDataBase)data).OnArgumentsValidation();
                if (data is EncryptTellerAuth)
                {
                    EncryptTellerAuth authdata = data as EncryptTellerAuth;
                    if (authdata != null)
                    {
                        codemsg = authdata.AfterEncrypt ? data.ToBytes() : (data as EncryptTellerAuth).TellerEncrypt.ToBytes();
                    }
                    else
                    {
                        throw new Exception(MESSAGE_ENCODE_EXCEPTION);
                    }
                }
                else
                {
                    codemsg = data.ToBytes();
                }
                Guid msgID = Guid.NewGuid();
                InsertMsgList(msgID, data);
                return msgID;
            }
            catch (BizArgumentsException bizex)
            {
                throw bizex;
            }
            catch (Exception ex)
            {
                xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Error, string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        /// <summary>
        /// 多包上传组织方式
        /// </summary>
        /// <param name="data"></param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid CodeMsgToMultiBinary(CoreBizMsgMultiReqDataBase data, ref byte[] codemsg)
        {
            try
            {
                data.OnArgumentsValidation();
                codemsg = data.ToBytes();
                Guid msgID = Guid.NewGuid();
                InsertMsgList(msgID, data);
                return msgID;
            }
            catch (BizArgumentsException bizex)
            {
                throw bizex;
            }
            catch (Exception ex)
            {
                xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Error, string.Format("{0}\r\n{1}",ex.Message, ex.StackTrace));
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        /// <summary>
        /// 编码成多数组（未使用）
        /// </summary>
        /// <param name="data"></param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid CodeMsgToBinaryCollection(IMessageMultiReqHandler data, ref List<byte[]> codemsg)
        {
            try
            {
                ((BizMsgDataBase)data).OnArgumentsValidation();
                codemsg = data.ToMultiBytes();
               
                Guid msgID = Guid.NewGuid();
                InsertMsgList(msgID, data);
                return msgID;
            }
            catch (BizArgumentsException bizex)
            {
                throw bizex;
            }
            catch (Exception ex)
            {
                xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Error, string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
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
