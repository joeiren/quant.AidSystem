using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xQuant.AidSystem.Communication;
using xQuant.MQ;
using xQuant.BizCore;
using System.Net;
using xQuant.AidSystem.DBAction;
using System.Data;
using xIR.Framework.Transactions;

namespace xQuant.AidSystem.ClientSyncWrapper
{
    public class MQMsgCommonHelper
    {
        public static MessageData FromMQMessage(MQMessage mqmsg, out string error)
        {
            error = "";
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
            data.RespPackageList.Enqueue(new PackageData(1, mqmsg.Byte));

            error = FindDefinedParameterValue(mqmsg.HeaderUser.UserDefined, "ERROR");
            //if (!String.IsNullOrEmpty(error))
            //{
            //    throw new Exception(string.Format("解析MQ消息，GUID={0};ERROR={1};", data.MessageID, error));
            //}

            return data;
        }

        public static string FindDefinedParameterValue(List<MQParameter<string>> mqParameterList, string parameterName)
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

        public static MQMessage ToMQMessage(String msgId, Guid id, PlatformType targetSys, bool isSinglePkg, String usercode, String definedError)
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
            msg.HeaderUser.UserUserId.Value = usercode;

            msg.HeaderUser.UserDefined.Add(new MQParameter<string>("TARGETSYS", targetSys.ToString()));
            msg.HeaderUser.UserDefined.Add(new MQParameter<string>("PKGTYPE", isSinglePkg.ToString()));
            msg.HeaderUser.UserDefined.Add(new MQParameter<string>("GUID", id.ToString()));
            msg.HeaderUser.UserDefined.Add(new MQParameter<string>("ERROR", definedError));
            msg.HeaderUser.UserDefined.Add(new MQParameter<string>("IP", GetHostIP()));
            

            return msg;
        }

        public static String GetHostIP()
        {
            string hostIP = "";
            
            IPHostEntry entry = Dns.GetHostByName(Dns.GetHostName());
            if (entry.AddressList.Length > 1)
            {
                IPAddress addr = new System.Net.IPAddress(entry.AddressList[1].Address);
                hostIP = addr.ToString();
            }
            else
            {
                IPAddress addr = new System.Net.IPAddress(entry.AddressList[0].Address);
                hostIP = addr.ToString();
            }
            return hostIP;
        }

        #region MQ消息发送接收日志
        public static int ExistedByFlowNO(string flowNO)
        {
            string outmsg = "";
            if (!string.IsNullOrEmpty(flowNO))
            {
                DataTable result = QueryMQLog(string.Format("FLOW_NO='{0}'", flowNO), out outmsg);
                if (result != null)
                {
                    return result.Rows.Count;
                }
            }
            return 0;
            
        }

        public static int AddMQLog(MessageData msgdata, byte[] senddata, MsgLogState state, out string exception)
        {
            exception = "";
            try
            {
                TTRD_SET_MSG_LOG log = new TTRD_SET_MSG_LOG();
                log.GUID = msgdata.MessageID.ToString();
                log.FLOW_NO = msgdata.BizMsgID??"";
                log.USER_CODE = msgdata.TellerNO;
                log.INS_ID = msgdata.OrgNO;
                log.IS_MUL_PKG = msgdata.IsMultiPackage ? "1" : "0";
                log.MSGTYPE = msgdata.MsgBizType;
                log.PLATFORMTYPE = (int)msgdata.TragetPlatform;
                log.SEND_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                log.SEND_CONTENT = senddata;
                log.STATE = (int)state;
                int count = 0;
                using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    TTRD_SET_MSG_LOG_Manager manager = new TTRD_SET_MSG_LOG_Manager();
                    count = manager.LogInsert(log);
                    trans.Complete();
                }
                return count;
            }
            catch(Exception ex)
            {
                exception = ex.Message;
                xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Error, string.Format("<MQMsgCommonHelper-AddMQLog> Exception:{0}; StatkTrace:{1}.", ex.Message, ex.StackTrace));
                return 0;
            }
        }

        public static int UpdateMQLogByFlowNO(MessageData msgdata, string hostFlowNO, string error, MsgLogState state, out string exception)
        {
            exception = "";
            try
            {
                TTRD_SET_MSG_LOG log = new TTRD_SET_MSG_LOG();
                log.GUID = msgdata.MessageID.ToString();
                log.FLOW_NO = msgdata.BizMsgID;
                log.USER_CODE = msgdata.TellerNO;
                log.INS_ID = msgdata.OrgNO;
                //log.IS_MUL_PKG = msgdata.IsMultiPackage ? "1" : "0";
                log.HOSTFLOW_NO = hostFlowNO;
                log.MSGTYPE = msgdata.MsgBizType;
                log.PLATFORMTYPE = (int)msgdata.TragetPlatform;
                log.RESP_TIME = " ";// DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                log.SEND_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                log.SEND_CONTENT = msgdata.CurrentReqPackage.PackageMessage;
                //log.RECV_CONTENT = msgdata.CurrentRespPackage.PackageMessage;
                log.STATE = (int)state;
                if (!string.IsNullOrEmpty(error))
                {
                    log.ERRINFO = error;
                }

                int count = 0;
                using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    TTRD_SET_MSG_LOG_Manager manager = new TTRD_SET_MSG_LOG_Manager();
                    count = manager.LogUpdateByFlowNO(log);
                    trans.Complete();
                }
                return count;
            }
            catch (Exception ex)
            {
                exception = ex.Message;
                xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Error, string.Format("<MQMsgCommonHelper-UpdateMQLog> Exception:{0}; StatkTrace:{1}.", ex.Message, ex.StackTrace));
                return 0;
                //throw ex;
            }
        }

        public static int UpdateMQLog(MessageData msgdata, string hostflow, string error, MsgLogState state, out string exception)
        {
            exception = "";
            try
            {
                TTRD_SET_MSG_LOG log = new TTRD_SET_MSG_LOG();
                log.GUID = msgdata.MessageID.ToString();
                log.FLOW_NO = msgdata.BizMsgID;
                log.USER_CODE = msgdata.TellerNO;
                log.INS_ID = msgdata.OrgNO;
                //log.IS_MUL_PKG = msgdata.IsMultiPackage ? "1" : "0";
                log.HOSTFLOW_NO = hostflow;
                log.MSGTYPE = msgdata.MsgBizType;
                log.PLATFORMTYPE = (int)msgdata.TragetPlatform;
                log.RESP_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                log.RECV_CONTENT = msgdata.CurrentRespPackage.PackageMessage;
                log.STATE = (int)state;
                if (!string.IsNullOrEmpty(error))
                {
                    log.ERRINFO = error;
                }

                int count = 0;
                using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    TTRD_SET_MSG_LOG_Manager manager = new TTRD_SET_MSG_LOG_Manager();
                    count = manager.LogUpdate(log);
                    trans.Complete();
                }
                return count;
            }
            catch(Exception ex)
            {
                exception = ex.Message;
                xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Error, string.Format("<MQMsgCommonHelper-UpdateMQLog> Exception:{0}; StatkTrace:{1}.", ex.Message, ex.StackTrace));
                return 0;
                //throw ex;
            }
        }
        /// <summary>
        /// 把支付交易序号更新为资金流水号，只适用来账销账
        /// </summary>
        /// <param name="msgdata"></param>
        /// <param name="paysn"></param>
        /// <param name="hostflow"></param>
        /// <param name="error"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static int UpdateMQLog(MessageData msgdata, string paysn, string hostflow, string error, MsgLogState state, out string exception)
        {
            exception = "";
            try
            {
                TTRD_SET_MSG_LOG log = new TTRD_SET_MSG_LOG();
                log.GUID = msgdata.MessageID.ToString();
                log.FLOW_NO = paysn;
                log.USER_CODE = msgdata.TellerNO;
                log.INS_ID = msgdata.OrgNO;
                //log.IS_MUL_PKG = msgdata.IsMultiPackage ? "1" : "0";
                log.HOSTFLOW_NO = hostflow;
                log.MSGTYPE = msgdata.MsgBizType;
                log.PLATFORMTYPE = (int)msgdata.TragetPlatform;
                log.RESP_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                log.RECV_CONTENT = msgdata.CurrentRespPackage.PackageMessage;
                log.STATE = (int)state;
                if (!string.IsNullOrEmpty(error))
                {
                    log.ERRINFO = error;
                }
 
                int count = 0;
                using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    TTRD_SET_MSG_LOG_Manager manager = new TTRD_SET_MSG_LOG_Manager();
                    count = manager.LogUpdate(log);
                    trans.Complete();
                }
                return count;
            }
            catch (Exception ex)
            {
                exception = ex.Message;
                xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Error, string.Format("<MQMsgCommonHelper-UpdateMQLog> Exception:{0}; StatkTrace:{1}.", ex.Message, ex.StackTrace));
                return 0;
                //throw ex;
            }
        }
        // 只适合更新日志状态
        public static int UpdateMQLog(Guid guid,  string error, MsgLogState state, out string exception)
        {
            exception = "";
            try
            {
                TTRD_SET_MSG_LOG log = new TTRD_SET_MSG_LOG();
                log.GUID = guid.ToString();              
                log.STATE = (int)state;
                if (!string.IsNullOrEmpty(error))
                {
                    log.ERRINFO = error;
                }
                int count = 0;
                using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    TTRD_SET_MSG_LOG_Manager manager = new TTRD_SET_MSG_LOG_Manager();
                    count = manager.LogUpdate(log);
                    trans.Complete();
                }
                return count;
            }
            catch (Exception ex)
            {
                exception = ex.Message;
                xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Error, string.Format("<MQMsgCommonHelper-UpdateMQLog> Exception:{0}; StatkTrace:{1}.", ex.Message, ex.StackTrace));
                return 0;
                //throw ex;
            }
        }

        public static DataTable QueryMQLog(string condition, out string exception)
        {
            exception = "";
            try
            {
                TTRD_SET_MSG_LOG_Manager manager = new TTRD_SET_MSG_LOG_Manager();
                return manager.LogQuery(condition);
            }
            catch (Exception ex)
            {
                exception = ex.Message;
                xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Error, string.Format("<MQMsgCommonHelper-QueryMQLog> Exception:{0}; StatkTrace:{1}.", ex.Message, ex.StackTrace));
                return null;
            }
        }
        #endregion
    }
}
