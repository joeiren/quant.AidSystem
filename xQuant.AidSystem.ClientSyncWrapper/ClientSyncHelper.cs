#define MQSYNC
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xQuant.AidSystem.CoreMessageData;
using xQuant.AidSystem.Communication;
using xQuant.AidSystem.BizDataModel;
using xQuant.AidSystem.DBAction;

namespace xQuant.AidSystem.ClientSyncWrapper
{
    internal class ClientSyncHelper
    {
        #region Teller Authentication
        internal static bool OnEncryptPassword(String tellerno, String orgno, String password, out byte[] encryptpwd, out String outmsg)
        {
            outmsg = "";
            byte[] codepwd = null;
            encryptpwd = null;
            bool sent = false;
            MessageData reqdata = null;
            MessageData recmsg = null;
            string dbexception = "";
            Guid msgid = Guid.Empty;

            try
            {
                msgid = MsgTransferUtility.EncryptPassword(orgno, tellerno, password, ref codepwd);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Encrypt, tellerno, orgno, (int)MessageBizType.EncryptPwd, codepwd);
#if MQSYNC
                recmsg = MQMsgHandlerEntry.DeliverMessage(reqdata, codepwd, out sent);
#else
                recmsg = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Encrypt, codepwd));
#endif
                EncryptTellerAuth tellauth = MsgTransfer.DecodeMsg(recmsg.MessageID, recmsg.CurrentRespPackage.PackageMessage) as EncryptTellerAuth;
                if (tellauth != null)
                {
                    if (tellauth.TellerEncrypt.RespCode == "A")
                    {
                        encryptpwd = tellauth.TellerEncrypt.RespPin;
                        //result.ResultCode = 1;
                        //MQMsgCommonHelper.UpdateMQLog(recmsg, null, outmsg, MsgLogState.RecvSucceed, out dbexception);
                        return true;
                    }
                    else if (tellauth.TellerEncrypt.RespCode == "E")
                    {
                        outmsg = MsgHandlerEntry.Encrypt_Pin_Failed + Encoding.ASCII.GetString(tellauth.TellerEncrypt.RespPin);
                    }
                    else
                    {
                        outmsg = MsgHandlerEntry.Encrypt_Pin_Format_Incorrect;
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                OnUpdateDBLog(recmsg.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);

                return false;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recmsg, msgid, ex, outmsg);
                return false;
            }
        }

        internal static bool OnTellerAuth(String tellerno, String orgno, String password, byte[] encryptpwd, out String duedate, out String outmsg)
        {
            outmsg = "";
            duedate = "";
            byte[] codemsg = null;
            MessageData reqdata = null;
            MessageData recdata = null;
            string dbexception = "";
            bool sent = false;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.AuthTeller(orgno, tellerno, password, encryptpwd, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerno, orgno, (int)MessageBizType.CoreAuthTeller, codemsg);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif                
                EncryptTellerAuth tellauth = (EncryptTellerAuth)MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage);

                if (tellauth != null)
                {
                    if (!String.IsNullOrEmpty(tellauth.TellerData.OData.DUE_DATE))
                    {
                        duedate = tellauth.TellerData.OData.DUE_DATE;
                        //MQMsgCommonHelper.UpdateMQLog(recdata, null, outmsg, MsgLogState.RecvSucceed, out dbexception);
                        return true;
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(tellauth.TellerData.SyserrHandler.Message))
                        {
                            outmsg = tellauth.TellerData.SyserrHandler.Message;
                        }
                        else
                        {
                            outmsg = MsgHandlerEntry.ExtractOMsg(tellauth.TellerData);
                        }
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return false;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return false;
            }
        }

        internal static bool OnFingerMarkValidation(String tellerno, String orgno, DateTime tradedate, String deviceType, String markInfo, out String outmsg)
        {
            outmsg = "";

            byte[] codemsg = null;
            MessageData reqdata = null;
            MessageData recdata = null;
            string dbexception = "";
            bool sent = false;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.FingerMarksValidation(tellerno, orgno, tradedate, deviceType, markInfo, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.FingerMarks, tellerno, orgno, (int)MessageBizType.FingerMarkValidation, codemsg);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif
                FingerMarkData markdata = (FingerMarkData)MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage);
                if (markdata != null)
                {
                    if (CommonDataHelper.StrTrimer(markdata.RespData.TradeState, null) == "0")
                    {
                       //MQMsgCommonHelper.UpdateMQLog(recdata, null, outmsg, MsgLogState.RecvSucceed, out dbexception);
                       return true;
                    }
                    else
                    {
                        outmsg = CommonDataHelper.StrTrimer(markdata.RespData.RespInof, null);
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return false;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return false;
            }
        }

        /// <summary>
        /// 柜员修改密码
        /// </summary>
        /// <param name="tellerNO">柜员号</param>
        /// <param name="organNO">机构号</param>
        /// <param name="tradeDate">操作时间</param>
        /// <param name="oldPwd">原密码【加密后的】</param>
        /// <param name="newPwd">新密码【加密后的】</param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        internal static bool OnTellerChangePwd(String tellerNO, String organNO, DateTime tradeDate, String oldPwd, string newPwd, out string outmsg)
        {
            outmsg = "";
            //internalcode = "";
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;

            try
            {
                byte[] encryptOldPwd = null;
                //加密原密码【安全平台接口】
                if (OnEncryptPassword(tellerNO, organNO, oldPwd, out encryptOldPwd, out outmsg))
                {
                    if (encryptOldPwd == null)
                        return false;
                }
                else
                    return false;
                //加密新密码【安全平台接口】
                byte[] encryptNewPwd = null;
                if(OnEncryptPassword(tellerNO,organNO,newPwd,out encryptNewPwd,out outmsg))
                {
                    if(encryptNewPwd == null)
                        return false;
                }
                else
                    return false;
                //以下开始处理密码修改【核心接口】
                msgid = MsgTransferUtility.ChangePswd(tellerNO, organNO, tradeDate, encryptOldPwd, encryptNewPwd, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid,PlatformType.Core,tellerNO,organNO,(int)MessageBizType.CoreTellerChangPwd, codemsg);
                #if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif                
                TellerChangePwdData chgPwdData = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as TellerChangePwdData;

                if (chgPwdData != null)
                {

                    if (!String.IsNullOrEmpty(chgPwdData.SyserrHandler.Message))
                    {
                        outmsg = chgPwdData.SyserrHandler.Message;
                    }
                    else
                    {
                        outmsg = MsgHandlerEntry.ExtractOMsg(chgPwdData);
                    }

                    if (string.IsNullOrEmpty(outmsg) || outmsg.Trim() == "")
                        return true;
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }

                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return false;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return false;
            }
        }
        #endregion

        #region CRUD Customer
        internal static bool OnCreateCstm(String tellerno, String orgno, DateTime tradedate, CoreCustomerBrief customer, out String internalcode, out String outmsg)
        {
            outmsg = "";
            internalcode = "";
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                internalcode = String.Empty;
                msgid = MsgTransferUtility.AddCustomer(tellerno, orgno, tradedate, ref codemsg, customer);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerno, orgno, (int)MessageBizType.CoreCstmCreate, codemsg);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif

                AddCstmData cstmdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as AddCstmData;
                if (cstmdata != null)
                {
                    if (!String.IsNullOrEmpty(cstmdata.OData.CUS_CDE))
                    {
                        internalcode = cstmdata.OData.CUS_CDE.TrimEnd();
                        //MQMsgCommonHelper.UpdateMQLog(recdata, null, outmsg, MsgLogState.RecvSucceed, out dbexception);
                        return true;
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(cstmdata.SyserrHandler.Message))
                        {
                            outmsg = cstmdata.SyserrHandler.Message;
                        }
                        else
                        {
                            outmsg = MsgHandlerEntry.ExtractOMsg(cstmdata);
                        }
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;                    
                }

                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return false;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return false;
            } 
        }

        internal static CoreCustomer OnRetrieveCstm(String tellerno, String orgno, DateTime tradedate, String internalcode, out String outmsg)
        {
            outmsg = "";
            CoreCustomer customer = null;
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.RetrieveCstm(tellerno, orgno, tradedate, internalcode, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerno, orgno, (int)MessageBizType.CoreCstmRetrieve, codemsg);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif
                RetrieveCstmData cstmdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as RetrieveCstmData;
                if (cstmdata != null)
                {
                    if (!String.IsNullOrEmpty(cstmdata.OData.CUS_CDE))
                    {
                        if (customer == null)
                        {
                            customer = new CoreCustomer();
                        }
                        customer.InternalCode = CommonDataHelper.StrTrimer(cstmdata.OData.CUS_CDE, null);
                        customer.CustomerNO = CommonDataHelper.StrTrimer(cstmdata.OData.CUS_NO, null);
                        customer.CustomerType = CommonDataHelper.StrTrimer(cstmdata.OData.CUS_TYP, null);
                        customer.CustomerName = CommonDataHelper.StrTrimer(cstmdata.OData.CUS_NAM, null);
                        customer.CustomerOName = CommonDataHelper.StrTrimer(cstmdata.OData.CUS_ONAM, null);
                        customer.CustomerEName = CommonDataHelper.StrTrimer(cstmdata.OData.CUS_ENAM, null);
                        customer.Nation = CommonDataHelper.StrTrimer(cstmdata.OData.NATION, null);
                        customer.VIPType = CommonDataHelper.StrTrimer(cstmdata.OData.VIP_TYP, null);
                        customer.CustomerStatus = CommonDataHelper.StrTrimer(cstmdata.OData.CUS_STS, null);
                        customer.AddressSN = CommonDataHelper.StrTrimer(cstmdata.OData.ADD_SN, null);
                        customer.Address = CommonDataHelper.StrTrimer(cstmdata.OData.ADDR, null);
                        customer.TeleSN = CommonDataHelper.StrTrimer(cstmdata.OData.TEL_SN, null);
                        customer.TeleNO = CommonDataHelper.StrTrimer(cstmdata.OData.TEL_NO, null);
                        customer.MobileNO = CommonDataHelper.StrTrimer(cstmdata.OData.MBL_NO, null);
                        customer.DistrictType = CommonDataHelper.StrTrimer(cstmdata.OData.CMB_QYLX,null);
                        customer.CreatedDept = CommonDataHelper.StrTrimer(cstmdata.OData.CRT_PDT,null);
                        customer.CreatedOperater = CommonDataHelper.StrTrimer(cstmdata.OData.CRT_OPR, null);
                        customer.CreatedDate = CommonDataHelper.StrTrimer(cstmdata.OData.CRT_DTE, null);
                        customer.UpdatedDept = CommonDataHelper.StrTrimer(cstmdata.OData.UP_PDT, null);
                        customer.UpdatedOperator = CommonDataHelper.StrTrimer(cstmdata.OData.UP_OPR, null);
                        customer.UpdatedDate = CommonDataHelper.StrTrimer(cstmdata.OData.UP_DTE, null);
                        //MQMsgCommonHelper.UpdateMQLog(recdata, null, outmsg, MsgLogState.RecvSucceed, out dbexception);
                        return customer;
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(cstmdata.SyserrHandler.Message))
                        {
                            outmsg = cstmdata.SyserrHandler.Message;
                        }
                        else
                        {
                            outmsg = MsgHandlerEntry.ExtractOMsg(cstmdata);
                        }                        
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;                    
                }
                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return customer;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return customer;
            }
        }

        internal static bool OnUpdateCstm(String tellerno, String orgno, DateTime tradedate, String internalcode, CoreCustomerBrief customer, out String outmsg)
        {
            outmsg = "";
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.UpdateCustomer(tellerno, orgno, tradedate,internalcode, ref codemsg, customer);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerno, orgno, (int)MessageBizType.CoreCstmUpdate, codemsg);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif
                UpdateCstmData cstmdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as UpdateCstmData;
                if (cstmdata != null)
                {
                    if (!String.IsNullOrEmpty(cstmdata.OData.CUS_CDE))
                    {
                        //MQMsgCommonHelper.UpdateMQLog(recdata, null, outmsg, MsgLogState.RecvSucceed, out dbexception);
                        return true;
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(cstmdata.SyserrHandler.Message))
                        {
                            outmsg = cstmdata.SyserrHandler.Message;
                        }
                        else if (cstmdata.OmsgHandler.NUM_ENT > 0)
                        {
                            outmsg = MsgHandlerEntry.ExtractOMsg(cstmdata);
                        }
                        
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return false;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return false;
            } 
        }

        internal static bool OnDeleteCstm(String tellerno, String orgno, DateTime tradedate, String internalcode, out String outmsg)
        {
            outmsg = "";
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                internalcode = String.Empty;
                msgid = MsgTransferUtility.DeleteCustomer(tellerno, orgno, tradedate, internalcode, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerno, orgno, (int)MessageBizType.CoreCstmDelete, codemsg);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif
                UpdateCstmData cstmdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as UpdateCstmData;
                if (cstmdata != null)
                {
                    if (!String.IsNullOrEmpty(cstmdata.OData.CUS_CDE))
                    {
                        //MQMsgCommonHelper.UpdateMQLog(recdata, null, outmsg, MsgLogState.RecvSucceed, out dbexception);
                        return true;
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(cstmdata.SyserrHandler.Message))
                        {
                            outmsg = cstmdata.SyserrHandler.Message;
                        }
                        else if (cstmdata.OmsgHandler.NUM_ENT > 0)
                        {
                            outmsg = MsgHandlerEntry.ExtractOMsg(cstmdata);
                        }
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                    
                }
                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return false;                
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return false;
            }
        }

        internal static bool OnRetrieveCstmBaseInfo(String tellerno, String orgno, DateTime tradedate, String doctype, String docno, out List<CoreCustomBaseInfo> outdatalist, out String outmsg)
        {
            outmsg = "";
            outdatalist = null;
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.RetrieveCstmBaseInfo(tellerno, orgno, tradedate, doctype, docno, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerno, orgno, (int)MessageBizType.CoreCstmBaseInfo, codemsg);
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);

                RetrieveCstmBaseInfoData cstmdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as RetrieveCstmBaseInfoData;
                if (cstmdata != null)
                {
                    if (cstmdata.OData.CstmBaseInfoList.Count>0)
                    {
                        outdatalist = new List<CoreCustomBaseInfo>();
                        foreach (var item in cstmdata.OData.CstmBaseInfoList)
                        {
                            CoreCustomBaseInfo outdata = new CoreCustomBaseInfo();

                            outdata.CustomerInnerCode = CommonDataHelper.StrTrimer(item.CUS_CDE, null);
                            outdata.CustomerName = CommonDataHelper.StrTrimer(item.CUS_NAM, null);
                            outdata.Address = CommonDataHelper.StrTrimer(item.ADDR, null);
                            outdata.AddressSN = CommonDataHelper.StrTrimer(item.ADSNPTSN, null);
                            outdata.TelephoneNumber = CommonDataHelper.StrTrimer(item.TEL_NO, null);
                            outdata.TelephoneSN = CommonDataHelper.StrTrimer(item.TEL_SN, null);
                            outdatalist.Add(outdata);
                        }
                        

                        //MQMsgCommonHelper.UpdateMQLog(recdata, null, outmsg, MsgLogState.RecvSucceed, out dbexception);
                        return true;
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(cstmdata.SyserrHandler.Message))
                        {
                            outmsg = cstmdata.SyserrHandler.Message;
                        }
                        else if (cstmdata.OmsgHandler.NUM_ENT > 0)
                        {
                            outmsg = MsgHandlerEntry.ExtractOMsg(cstmdata);
                        }
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return false;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return false;
            }
        }

        #endregion

        #region Core Recording Accounting
        internal static CoreAcctResult OnRecordAcct(String tellerno, String orgno, DateTime tradedate, String followcode, List<CoreBillRecord> bills, out String outmsg)
        {
            outmsg = "";
            CoreAcctResult result = null;
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.AccountingRecord(tellerno, orgno, tradedate, followcode, bills, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerno, orgno, (int)MessageBizType.CoreAcctRecord, codemsg, followcode);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif
                AcctRecordData recddata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as AcctRecordData;
                if (recddata != null)
                {
                    if (!String.IsNullOrEmpty(recddata.SyserrHandler.Message))
                    {
                        outmsg = recddata.SyserrHandler.Message;
                    }
                    else if (recddata.OmsgHandler.NUM_ENT > 0)
                    {
                        outmsg = MsgHandlerEntry.ExtractOMsg(recddata);
                    }
                    else
                    {
                        result = new CoreAcctResult();
                        if (recddata.OData.OdataItemList != null && recddata.OData.OdataItemList.Count > 0)
                        {
                            foreach (AcctRecordODATA_Item itemdata in recddata.OData.OdataItemList)
                            {
                                CoreAcctEntry entry = new CoreAcctEntry();
                                entry.AccountNO = CommonDataHelper.StrTrimer(itemdata.ACCT,null);
                                entry.CheckCode = CommonDataHelper.StrTrimer(itemdata.GL_NO,null);
                                entry.Amount = CommonDataHelper.StrTrimer(itemdata.AMT,null);
                                entry.Currency = CommonDataHelper.StrTrimer(itemdata.CCY,null);
                                entry.Option = CommonDataHelper.StrTrimer(itemdata.CD_IND,null);
                                entry.InnerSN = CommonDataHelper.StrTrimer(itemdata.GL_SEQ, null);
                                result.AcctEntryList.Add(entry);
                            }
                        }

                        if (recddata.OData.OdataPendingList != null && recddata.OData.OdataPendingList.Count > 0)
                        {
                            foreach (AcctRecordODATA_PendingItem itemdata in recddata.OData.OdataPendingList)
                            {
                                CorePendingAcctEntry entry = new CorePendingAcctEntry();
                                entry.FlowNO = CommonDataHelper.StrTrimer(itemdata.FlowNO,null);
                                entry.InnerSN = CommonDataHelper.StrTrimer(itemdata.InnerSN,null);
                                entry.PendingAccount = CommonDataHelper.StrTrimer(itemdata.PendingAccount,null);
                                entry.PendingSN = CommonDataHelper.StrTrimer(itemdata.PendingSN,null);
                                entry.PendingAmount = CommonDataHelper.StrTrimer(itemdata.PendingAmount, null);
                                result.PendingAcctList.Add(entry);
                            }
                        }
                        string coreSN = recddata.RPhdrHandler.SEQ_NO.TrimStart().TrimEnd();
                        OnUpdateDBLog(recdata, coreSN, outmsg, MsgLogState.RecvSucceed, out dbexception);
                        //MQMsgCommonHelper.UpdateMQLog(recdata, coreSN, outmsg, MsgLogState.RecvSucceed, out dbexception);
                        return result;
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return result;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return result;
            }
        }

        internal static CoreAcctResult OnRecordAcctMulti2One(String tellerno, String orgno, DateTime tradedate, String followcode, bool isMultiLend, List<CoreBillRecord> bills,  out String outmsg)
        {
            outmsg = "";
            CoreAcctResult result = null;
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = Guid.Empty;
                if (isMultiLend)
                {
                    msgid = MsgTransferUtility.AcctRecordMultiLendOneLoan(tellerno, orgno, tradedate, followcode, bills, ref codemsg);
                }
                else
                {
                    msgid = MsgTransferUtility.AcctRecordMultiLoanOneLend(tellerno, orgno, tradedate, followcode, bills, ref codemsg);
                }
                if (msgid == Guid.Empty && !string.IsNullOrEmpty(outmsg))
                {
                    return result;
                }
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerno, orgno, isMultiLend ? (int)MessageBizType.CoreAcctMultiLend : (int)MessageBizType.CoreAcctMultiLoan, codemsg, followcode);
                
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif
                AcctRecordMulti2OneData recddata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as AcctRecordMulti2OneData;
                if (recddata != null)
                {
                    if (!String.IsNullOrEmpty(recddata.SyserrHandler.Message))
                    {
                        outmsg = recddata.SyserrHandler.Message;
                    }
                    else if (recddata.OmsgHandler.NUM_ENT > 0)
                    {
                        outmsg = MsgHandlerEntry.ExtractOMsg(recddata);
                    }
                    else
                    {
                        result = new CoreAcctResult();
                        if (recddata.OData.OdataItemList != null && recddata.OData.OdataItemList.Count > 0)
                        {
                            foreach (AcctRecordMulti2OneODATA_Item itemdata in recddata.OData.OdataItemList)
                            {
                                CoreAcctEntry entry = new CoreAcctEntry();
                                entry.AccountNO = CommonDataHelper.StrTrimer(itemdata.ACCT, null);
                                entry.CheckCode = CommonDataHelper.StrTrimer(itemdata.GL_NO, null);
                                entry.Amount = CommonDataHelper.StrTrimer(itemdata.AMT, null);
                                entry.Currency = CommonDataHelper.StrTrimer(itemdata.CCY, null);
                                entry.Option = CommonDataHelper.StrTrimer(itemdata.CD_IND, null);
                                entry.InnerSN = CommonDataHelper.StrTrimer(itemdata.GL_SEQ, null);
                                result.AcctEntryList.Add(entry);
                            }
                        }

                        if (recddata.OData.OdataPendingList != null && recddata.OData.OdataPendingList.Count > 0)
                        {
                            foreach (AcctRecordMulti2OneODATA_PendingItem itemdata in recddata.OData.OdataPendingList)
                            {
                                CorePendingAcctEntry entry = new CorePendingAcctEntry();
                                entry.InnerSN = CommonDataHelper.StrTrimer(itemdata.FlowNO, null);
                                entry.InnerSN = CommonDataHelper.StrTrimer(itemdata.InnerSN, null);
                                entry.PendingAccount = CommonDataHelper.StrTrimer(itemdata.PendingAccount, null);
                                entry.PendingSN = CommonDataHelper.StrTrimer(itemdata.PendingSN, null);
                                entry.PendingAmount = CommonDataHelper.StrTrimer(itemdata.PendingAmount, null);
                                result.PendingAcctList.Add(entry);
                            }
                        }
                        string coreSN = recddata.RPhdrHandler.SEQ_NO.TrimStart().TrimEnd();
                        OnUpdateDBLog(recdata, coreSN, outmsg, MsgLogState.RecvSucceed, out dbexception);
                        //MQMsgCommonHelper.UpdateMQLog(recdata, coreSN, outmsg, MsgLogState.RecvSucceed, out dbexception);
                        return result;
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return result;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return result;
            }
        }

        internal static CoreAcctDetail OnRetrieveAccounting(String tellerno, String orgno, DateTime tradedate, String coreSN, out String outmsg)
        {
            outmsg = "";
            CoreAcctDetail resultdetail = null;
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.RetrieveAccounting(tellerno, orgno, tradedate, coreSN, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerno, orgno, (int)MessageBizType.CoreAcctRetrieve, codemsg);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif
                AcctRetrieveData retdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as AcctRetrieveData;
                if (retdata != null)
                {
                    if (!String.IsNullOrEmpty(retdata.SyserrHandler.Message))
                    {
                        outmsg = retdata.SyserrHandler.Message;
                    }
                    else if (retdata.OmsgHandler.NUM_ENT > 0)
                    {
                        outmsg = MsgHandlerEntry.ExtractOMsg(retdata);
                    }
                    else if (retdata.OData != null)
                    {
                        resultdetail = new CoreAcctDetail();
                        if (retdata.OData.DB_BGO30800 != null && !String.IsNullOrEmpty(retdata.OData.DB_BGO30800.CoreTradeSN))
                        {
                            resultdetail.DB_BGO30800.CoreTradeSN = CommonDataHelper.StrTrimer(retdata.OData.DB_BGO30800.CoreTradeSN, null);
                            resultdetail.DB_BGO30800.SrcSystemCode = CommonDataHelper.StrTrimer(retdata.OData.DB_BGO30800.SrcSystemCode, null);
                            resultdetail.DB_BGO30800.AcctSNFlag = CommonDataHelper.StrTrimer(retdata.OData.DB_BGO30800.AcctSNFlag, null);
                            resultdetail.DB_BGO30800.OutsideSN = CommonDataHelper.StrTrimer(retdata.OData.DB_BGO30800.OutsideSN, null);
                            resultdetail.DB_BGO30800.ErasedFlag = CommonDataHelper.StrTrimer(retdata.OData.DB_BGO30800.ErasedFlag, null);
                        }

                        foreach (AcctRetrieveODATA_DB2 item in retdata.OData.DB_BGO30801_List)
                        {
                            AcctDetail_BGO30801 detail01item = new AcctDetail_BGO30801();
                            detail01item.CoreTradeSN = CommonDataHelper.StrTrimer(item.CoreTradeSN, null);
                            detail01item.TradeAccount = CommonDataHelper.StrTrimer(item.TradeAccount, null);
                            detail01item.AccountName = CommonDataHelper.StrTrimer(item.AccountName, null);
                            detail01item.CashFalg = CommonDataHelper.StrTrimer(item.CashFalg, null);
                            detail01item.LoanFlag = CommonDataHelper.StrTrimer(item.LoanFlag, null);
                            detail01item.Amount = CommonDataHelper.StrTrimer(item.Amount, null);
                            detail01item.VoucherNO = CommonDataHelper.StrTrimer(item.VoucherNO, null);
                            detail01item.SrcSystemCode = CommonDataHelper.StrTrimer(item.SrcSystemCode, null);
                            detail01item.PostingCashFlag = CommonDataHelper.StrTrimer(item.PostingCashFlag, null);
                            resultdetail.DB_BGO30801_List.Add(detail01item);
                        }

                        if (retdata.OData.DB_BGO30802 != null)
                        {
                            resultdetail.DB_BGO30802.IsQuotaMoney = CommonDataHelper.StrTrimer(retdata.OData.DB_BGO30802.IsQuotaMoney, null);
                            resultdetail.DB_BGO30802.QuotaMoneyCount = retdata.OData.DB_BGO30802.QuotaMoneyCount;
                        }

                        foreach (AcctRetrieveODATA_DB4 item in retdata.OData.DB_BGO30803_List)
                        {
                            AcctDetail_BGO30803 detail03item = new AcctDetail_BGO30803();
                            detail03item.Currency = CommonDataHelper.StrTrimer(item.Currency, null);
                            detail03item.QuotaAmount = CommonDataHelper.StrTrimer(item.QuotaAmount, null);
                            resultdetail.DB_BGO30803_List.Add(detail03item);
                        }
                        //MQMsgCommonHelper.UpdateMQLog(recdata, null, outmsg, MsgLogState.RecvSucceed, out dbexception);
                        return resultdetail;
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return resultdetail;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return resultdetail;
            }
        }

        internal static CoreAcctErase OnEraseAccounting(String tellerno, String orgno, DateTime tradedate, String coreSN, out String outmsg)
        {
            outmsg = "";
            CoreAcctErase resultdetail = null;
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData recdata = null;
            MessageData reqdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.EraseAccounting(tellerno, orgno, tradedate, coreSN, ref codemsg);
                if (msgid == Guid.Empty && !string.IsNullOrEmpty(outmsg))
                {
                    return resultdetail;
                }
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerno, orgno, (int)MessageBizType.CoreAcctErase, codemsg);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif
                AcctEraseData retdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as AcctEraseData;
                if (retdata != null)
                {
                    if (!String.IsNullOrEmpty(retdata.SyserrHandler.Message))
                    {
                        outmsg = retdata.SyserrHandler.Message;
                    }
                    else if (retdata.OmsgHandler.NUM_ENT > 0)
                    {
                        outmsg = MsgHandlerEntry.ExtractOMsg(retdata);
                    }
                    else
                    {
                        resultdetail = new CoreAcctErase();
                        if (retdata.OData.DB_BY999000 != null)
                        {
                            resultdetail.DB_BY999000.TIME = CommonDataHelper.StrTrimer(retdata.OData.DB_BY999000.TIME, null);
                            resultdetail.DB_BY999000.JNO = CommonDataHelper.StrTrimer(retdata.OData.DB_BY999000.JNO, null);
                            resultdetail.DB_BY999000.Date = CommonDataHelper.StrTrimer(retdata.OData.DB_BY999000.Date, null);
                            resultdetail.DB_BY999000.TX_TIME = CommonDataHelper.StrTrimer(retdata.OData.DB_BY999000.TX_TIME, null);
                            resultdetail.DB_BY999000.TEL_NO = CommonDataHelper.StrTrimer(retdata.OData.DB_BY999000.TEL_NO, null);
                            resultdetail.DB_BY999000.TX_ID = CommonDataHelper.StrTrimer(retdata.OData.DB_BY999000.TX_ID, null);
                            resultdetail.DB_BY999000.TX_JNO = CommonDataHelper.StrTrimer(retdata.OData.DB_BY999000.TX_JNO, null);
                        }

                        foreach (AcctEraseODATA_DB2 item in retdata.OData.DB_BY999001_List)
                        {
                            AcctErase_BY999001 erase01item = new AcctErase_BY999001();
                            erase01item.GL_SEQ = CommonDataHelper.StrTrimer(item.GL_SEQ, null);
                            erase01item.ACC_NO = CommonDataHelper.StrTrimer(item.ACC_NO, null);
                            erase01item.ACC_NAME = CommonDataHelper.StrTrimer(item.ACC_NAME, null);
                            erase01item.DRCR_IND = CommonDataHelper.StrTrimer(item.DRCR_IND, null);
                            erase01item.TX_AMT = CommonDataHelper.StrTrimer(item.TX_AMT, null);
                            erase01item.TX_CCY = CommonDataHelper.StrTrimer(item.TX_CCY, null);
                            resultdetail.DB_BY999001_List.Add(erase01item);
                        }
                        //MQMsgCommonHelper.UpdateMQLog(recdata, null, outmsg, MsgLogState.RecvSucceed, out dbexception);
                        return resultdetail;
                    }    
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                    
                }
                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return resultdetail;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return resultdetail;
            }
        }

        internal static List<CoreCheckAcctInfo> OnRetrieveCheckingAcct(String tellerno, String orgno, DateTime tradedate, DateTime querydate, String flowNO, String queryOrgNO, out String outmsg)
        {
            outmsg = "";
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData recdata = null;
            MessageData reqdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.AccountingCheck(tellerno, orgno, tradedate, querydate, flowNO, queryOrgNO, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerno, orgno, (int)MessageBizType.CoreAcctChecking, codemsg);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif
                AcctCheckData retdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as AcctCheckData;
                if (retdata != null)
                {
                    if (!String.IsNullOrEmpty(retdata.SyserrHandler.Message))
                    {
                        outmsg = retdata.SyserrHandler.Message;
                        // 如果核心改日无数据，则返回正确
                        if (outmsg != null && outmsg.IndexOf("BGM3052") >= 0)
                        {
                            return new List<CoreCheckAcctInfo>();
                        }
                    }
                    else if (retdata.OmsgHandler.NUM_ENT > 0)
                    {
                        outmsg = MsgHandlerEntry.ExtractOMsg(retdata);
                        // 如果核心改日无数据，则返回正确
                        if (outmsg != null && outmsg.IndexOf("BGM3052") >= 0)
                        {
                            return new List<CoreCheckAcctInfo>();
                        }
                    }
                    else
                    {
                        List<CoreCheckAcctInfo> listdata = new List<CoreCheckAcctInfo>();
                        foreach (var item in retdata.OBDataList)
                        {
                            CoreCheckAcctInfo info = new CoreCheckAcctInfo();
                            info.Amount = item.Amount;
                            info.BizFlowNO = item.BizFlowNO;
                            info.CheckCode = item.CheckCode;
                            info.Currency = item.Currency;
                            info.DCFlag = item.DCFlag;
                            info.OrgNO = item.OrgNO;
                            info.OrgNOWithinAcct = item.OrgNOWithinAcct;
                            info.RedBlueFlag = item.RedBlueFlag;
                            info.Status = item.Status;
                            info.TellerFlowNO = item.TellerFlowNO;
                            info.TellerNO = item.TellerNO;
                            info.TradeAcctNO = item.TradeAcctNO;
                            info.TradeDate = item.TradeDate;
                            listdata.Add(info);
                        }

                        //MQMsgCommonHelper.UpdateMQLog(recdata, null, outmsg, MsgLogState.RecvSucceed, out dbexception);
                        return listdata;
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;

                }
                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return null;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return null;
            }
        }
        #endregion

        #region Fund Retrieve
        internal static FundInternalAcct OnRetrieveInternalAcct(String tellerno, String orgno, DateTime tradedate, InternalAcctCondtion condition, out String outmsg)
        {
            outmsg = "";
            FundInternalAcct resultdetail = null;
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData recdata = null;
            MessageData reqdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.RetrieveInternalAcct(tellerno, orgno, tradedate, condition, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerno, orgno, (int)MessageBizType.CoreInnerAcctRetrieve, codemsg);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif
                InterAcctRetrieveData retdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as InterAcctRetrieveData;
                if (retdata != null)
                {
                    if (!String.IsNullOrEmpty(retdata.SyserrHandler.Message))
                    {
                        outmsg = retdata.SyserrHandler.Message;
                    }
                    else if (retdata.OmsgHandler.NUM_ENT > 0)
                    {
                        outmsg = MsgHandlerEntry.ExtractOMsg(retdata);
                    }
                    else
                    {
                        if (retdata.OData != null)
                        {
                            resultdetail = new FundInternalAcct();
                            resultdetail.AccountNO = CommonDataHelper.StrTrimer(retdata.OData.AccountNO, null);
                            resultdetail.OrgnaztionNO = CommonDataHelper.StrTrimer(retdata.OData.OrgnaztionNO, null);
                            resultdetail.Currcency = CommonDataHelper.StrTrimer(retdata.OData.Currcency, null);
                            resultdetail.SubjectNO = CommonDataHelper.StrTrimer(retdata.OData.SubjectNO, null);
                            resultdetail.SequenceNO = CommonDataHelper.StrTrimer(retdata.OData.SequenceNO, null);
                            resultdetail.AccountName = CommonDataHelper.StrTrimer(retdata.OData.AccountName, null);
                            resultdetail.PreviousTradeDate = CommonDataHelper.StrTrimer(retdata.OData.PreviousTradeDate, null);
                            resultdetail.PreviousBalanceDirection = CommonDataHelper.StrTrimer(retdata.OData.PreviousBalanceDirection, null);
                            resultdetail.PreviousBalance = CommonDataHelper.StrTrimer(retdata.OData.PreviousBalance, null);
                            resultdetail.BalanceDirection = CommonDataHelper.StrTrimer(retdata.OData.BalanceDirection, null);
                            resultdetail.CurrentBalance = CommonDataHelper.StrTrimer(retdata.OData.CurrentBalance, null);
                            resultdetail.OverdraftLimitation = CommonDataHelper.StrTrimer(retdata.OData.OverdraftLimitation, null);
                            resultdetail.Rate = CommonDataHelper.StrTrimer(retdata.OData.Rate, null);
                            resultdetail.OverdraftRate = CommonDataHelper.StrTrimer(retdata.OData.OverdraftRate, null);
                            resultdetail.InterestCumulation = CommonDataHelper.StrTrimer(retdata.OData.InterestCumulation, null);
                            resultdetail.OverdraftCumulation = CommonDataHelper.StrTrimer(retdata.OData.OverdraftCumulation, null);
                            resultdetail.UsageRange = CommonDataHelper.StrTrimer(retdata.OData.UsageRange, null);
                            resultdetail.IsEliminate = CommonDataHelper.StrTrimer(retdata.OData.IsEliminate, null);
                            resultdetail.IsExchangePay = CommonDataHelper.StrTrimer(retdata.OData.IsExchangePay, null);
                            resultdetail.SettlementType = CommonDataHelper.StrTrimer(retdata.OData.SettlementType, null);
                            resultdetail.IsSettlementWithAccount = CommonDataHelper.StrTrimer(retdata.OData.IsSettlementWithAccount, null);
                            resultdetail.IncomeAccount = CommonDataHelper.StrTrimer(retdata.OData.IncomeAccount, null);
                            resultdetail.PaymentAccount = CommonDataHelper.StrTrimer(retdata.OData.PaymentAccount, null);
                            resultdetail.AccountStatus = CommonDataHelper.StrTrimer(retdata.OData.AccountStatus, null);
                            resultdetail.AccountOrgan = CommonDataHelper.StrTrimer(retdata.OData.AccountOrgan, null);
                            resultdetail.HeadOrgan = CommonDataHelper.StrTrimer(retdata.OData.HeadOrgan, null);
                            resultdetail.CirculateExchangeLevel = CommonDataHelper.StrTrimer(retdata.OData.CirculateExchangeLevel, null);
                            resultdetail.AccountDate = CommonDataHelper.StrTrimer(retdata.OData.AccountDate, null);
                            resultdetail.AccountTeller = CommonDataHelper.StrTrimer(retdata.OData.AccountTeller, null);
                        }
                        //MQMsgCommonHelper.UpdateMQLog(recdata, null, outmsg, MsgLogState.RecvSucceed, out dbexception);
                        return resultdetail;
                    }                    
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return resultdetail;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                throw ex;
                //return resultdetail;
            }
        }

        internal static List<FundAcctInfo> OnRetrieveSuperiorCurrentAcct(String tellerno, String orgno, DateTime tradedate, String currcency, out String outmsg)
        {
            outmsg = "";
            List<FundAcctInfo> resultdetail = null;
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData recdata = null;
            MessageData reqdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.RetrieveSuperiorCurrentAcct(tellerno, orgno, tradedate, currcency, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerno, orgno, (int)MessageBizType.CoreSuperiorCrntAcctRetrieve, codemsg);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif
                SuperiorCurrentAcctData retdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as SuperiorCurrentAcctData;
                if (retdata != null)
                {
                    if (!String.IsNullOrEmpty(retdata.SyserrHandler.Message))
                    {
                        outmsg = retdata.SyserrHandler.Message;
                    }
                    else if (retdata.OmsgHandler.NUM_ENT > 0)
                    {
                        outmsg = MsgHandlerEntry.ExtractOMsg(retdata);
                    }
                    else
                    {
                        if (retdata.OData != null)
                        {
                            resultdetail = new List<FundAcctInfo>();
                            foreach (var item in retdata.OData.CrntAcctList)
                            {
                                FundAcctInfo acct = new FundAcctInfo();
                                acct.OrganNO = CommonDataHelper.StrTrimer(item.OrgNO, null);
                                acct.Currcency = CommonDataHelper.StrTrimer(item.Currency, null);
                                acct.Subject = CommonDataHelper.StrTrimer(item.Subject, null);
                                acct.PerviousBalance = CommonDataHelper.StrTrimer(item.PerviousBalance, null);
                                acct.DebitAmount = CommonDataHelper.StrTrimer(item.DebitAmount, null);
                                acct.CreditAmount = CommonDataHelper.StrTrimer(item.CreditAmount, null);
                                acct.CurrentBalance = CommonDataHelper.StrTrimer(item.CurrentBalance, null);
                                acct.FloorAmount = CommonDataHelper.StrTrimer(item.FloorAmount, null);
                                acct.OffsetBalance = CommonDataHelper.StrTrimer(item.OffsetBalance, null);
                                resultdetail.Add(acct);
                            }
                           
                        }
                        //MQMsgCommonHelper.UpdateMQLog(recdata, null, outmsg, MsgLogState.RecvSucceed, out dbexception);
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return resultdetail;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                throw ex;
                //return resultdetail;
            }
        }

        // 县级行社上存清算资金查询  
        internal static List<FundAcctInfo> OnRetrieveDepositClearingFund(String tellerno, String orgno, DateTime tradedate, String currcency, String option, out String outmsg)
        {
            outmsg = "";
            List<FundAcctInfo> resultdetail = null;
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData recdata = null;
            MessageData reqdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.RetrieveDepositClearingFund(tellerno, orgno, tradedate, currcency, option, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerno, orgno, (int)MessageBizType.CoreDepositClearingFundRetrieve, codemsg);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif
                DepositClearingFundData retdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as DepositClearingFundData;
                if (retdata != null)
                {
                    if (!String.IsNullOrEmpty(retdata.SyserrHandler.Message))
                    {
                        outmsg = retdata.SyserrHandler.Message;
                    }
                    else if (retdata.OmsgHandler.NUM_ENT > 0)
                    {
                        outmsg = MsgHandlerEntry.ExtractOMsg(retdata);
                    }
                    else
                    {
                        if (retdata.OData != null)
                        {
                            //resultdetail = new FundSuperiorCurrentAcct();
                            foreach (var item in retdata.OData.BalanceInfoList)
                            {
                                FundAcctInfo acct = new FundAcctInfo();
                                acct.OrganNO = CommonDataHelper.StrTrimer(item.OrgNO, null);
                                acct.Currcency = CommonDataHelper.StrTrimer(item.Currency, null);
                                acct.Subject = CommonDataHelper.StrTrimer(item.Subject, null);
                                acct.PerviousBalance = CommonDataHelper.StrTrimer(item.PerviousBalance, null);
                                acct.DebitAmount = CommonDataHelper.StrTrimer(item.DebitAmount, null);
                                acct.CreditAmount = CommonDataHelper.StrTrimer(item.CreditAmount, null);
                                acct.CurrentBalance = CommonDataHelper.StrTrimer(item.CurrentBalance, null);
                                acct.FloorAmount = CommonDataHelper.StrTrimer(item.FloorAmount, null);
                                acct.OffsetBalance = CommonDataHelper.StrTrimer(item.OffsetBalance, null);
                                resultdetail.Add(acct);
                            }

                        }
                        //MQMsgCommonHelper.UpdateMQLog(recdata, null, outmsg, MsgLogState.RecvSucceed, out dbexception);
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return resultdetail;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                throw ex;
                //return resultdetail;
            }
        }
        //资金业务账号余额查询
        internal static List<FundCrntAcctBalance> OnRetrieveAcctCrntBalance(String tellerno, String orgno, DateTime tradedate, List<CoreAcctCrntBalance> inputlist,out String outmsg)
        {
            outmsg = "";
            List<FundCrntAcctBalance> resultdetail = null;
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData recdata = null;
            MessageData reqdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.RetrieveAcctCrntBalance(tellerno, orgno, tradedate, inputlist, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerno, orgno, (int)MessageBizType.CoreAcctCrntBalanceRetrieve, codemsg);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif
                RetrieveAcctCrntBalanceData retdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as RetrieveAcctCrntBalanceData;
                if (retdata != null)
                {
                    if (!String.IsNullOrEmpty(retdata.SyserrHandler.Message))
                    {
                        outmsg = retdata.SyserrHandler.Message;
                    }
                    else if (retdata.OmsgHandler.NUM_ENT > 0)
                    {
                        outmsg = MsgHandlerEntry.ExtractOMsg(retdata);
                    }
                    else
                    {
                        if (retdata.OData != null)
                        {
                            resultdetail = new List<FundCrntAcctBalance>();
                            foreach (var item in retdata.OData.BalanceList)
                            {
                                FundCrntAcctBalance acct = new FundCrntAcctBalance();
                                acct.AcctNO = CommonDataHelper.StrTrimer(item.AcctNO, null);
                                acct.AcctProperty = CommonDataHelper.StrTrimer(item.AcctProperty, null);
                                acct.Balance = CommonDataHelper.StrTrimer(item.Balance, null);
                                acct.BalanceDirection = CommonDataHelper.StrTrimer(item.BalanceDirection, null);
                                acct.ResultFlag = CommonDataHelper.StrTrimer(item.ResultFlag, null);
                                resultdetail.Add(acct);
                            }

                        }
                        //MQMsgCommonHelper.UpdateMQLog(recdata, null, outmsg, MsgLogState.RecvSucceed, out dbexception);
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return resultdetail;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                throw ex;
                //return resultdetail;
            }
        }
        #endregion

        #region Inter-Bank
        internal static bool OnSettleInterBank(String tellerNO, String orgNO, DateTime tradeDate, InterBankInterestInfo info, out String outmsg)
        {
            outmsg = "";
            
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.InterBankInterestSettle(tellerNO, orgNO, tradeDate, info, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerNO, orgNO, (int)MessageBizType.CoreInterBankSettle, codemsg, orgNO);
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);

                InterBankInterestData recddata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as InterBankInterestData;
                if (recddata != null)
                {
                    if (!String.IsNullOrEmpty(recddata.SyserrHandler.Message))
                    {
                        outmsg = recddata.SyserrHandler.Message;
                    }
                    else if (recddata.OmsgHandler.NUM_ENT > 0)
                    {
                        outmsg = MsgHandlerEntry.ExtractOMsg(recddata);
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return false;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                if (ex is BizArgumentsException)
                {
                    return false;
                }
                else
                {
                    throw ex;
                }
                //return false;
            }
        }

        internal static bool OnPreparedInterBank(String tellerNO, String orgNO, DateTime tradeDate, List<InterBankPreparedInfo> info, out String outmsg)
        {
            outmsg = "";

           byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.InterBankAssetsPrepared(tellerNO, orgNO, tradeDate, info, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerNO, orgNO, (int)MessageBizType.CoreInterBankPrepared, codemsg, orgNO);
                
                //recdata = MQMsgHandlerEntry.DeliverMultiMessage(reqdata, codemsg, out sent);
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
                InterBankPreparedData recddata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as InterBankPreparedData;
                if (recddata != null)
                {
                    if (!String.IsNullOrEmpty(recddata.SyserrHandler.Message))
                    {
                        outmsg = recddata.SyserrHandler.Message;
                    }
                    else if (recddata.OmsgHandler.NUM_ENT > 0)
                    {
                        outmsg = MsgHandlerEntry.ExtractOMsg(recddata);
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return false;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                if (ex is BizArgumentsException)
                {
                    return false;
                }
                else
                {
                    throw ex;
                }
                //return false;
            }
        }
        #endregion

        #region Payment
        internal static PaymentResult OnPayRegisterRetrieved(String tellerNO, String orgNO, String flowNO, DateTime orignalDate, out String outmsg)
        {
            outmsg = "";
            PaymentResult resultdetail = null;
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.PayRegisterRetrieved(tellerNO, orgNO,flowNO,orignalDate, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Payment, tellerNO, orgNO, (int)MessageBizType.PayRegisterRetrieve, codemsg, flowNO);
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);

                PayRegisterRetrievedData retdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as PayRegisterRetrievedData;
                MsgLogState statecode = MsgLogState.RecvFailed;
                if (retdata != null && retdata.RPData != null)
                {
                    resultdetail = new PaymentResult();
                    resultdetail.Succeed = (retdata.RPData.RetCode == "00");
                    resultdetail.HostReturnMessage = CommonDataHelper.StrTrimer(retdata.RPData.RetMsg,null);
                    resultdetail.AccountStatus = CommonDataHelper.StrTrimer(retdata.RPData.AccountStatus,null);
                    resultdetail.PackageChannelType = CommonDataHelper.StrTrimer(retdata.RPData.PackageChannelType,null);
                    outmsg = CommonDataHelper.StrTrimer(retdata.RPData.RetMsg, null);
                                       
                    if (retdata.RPData.RetCode == "00")
                    {
                        statecode = MsgLogState.RecvSucceed;
                    }
                    else if (retdata.RPData.RetCode == "02")
                    {
                        statecode = MsgLogState.PayTimeOut;
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }

                if (statecode != MsgLogState.RecvSucceed)
                {
                    OnUpdateDBLog(recdata.MessageID, outmsg, statecode, out dbexception);
                }
                
                return resultdetail;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                
                return resultdetail;
            }
        }

        internal static PaymentResult OnPayFundTransfer(String tellerNO, String orgNO, DateTime tradeDate, PayFundTransfer inputData, out String outmsg)
        {
            outmsg = "";
            PaymentResult resultdetail = null;
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData recdata = null;
            MessageData reqdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.PayFundTransfer(tellerNO, orgNO, tradeDate, inputData, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Payment, tellerNO, orgNO, (int)MessageBizType.PayFundTrans, codemsg, inputData.TransferFlowNo);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Payment, codemsg));
#endif
                PayFundTransferData retdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as PayFundTransferData;
                MsgLogState statecode = MsgLogState.RecvFailed;
                if (retdata != null && retdata.RPData != null)
                {
                    resultdetail = new PaymentResult();
                    resultdetail.Succeed = (retdata.RPData.RetCode == "00");
                    resultdetail.HostReturnMessage = CommonDataHelper.StrTrimer(retdata.RPData.HostReturnMessage,null);
                    resultdetail.HostReturnCode = CommonDataHelper.StrTrimer(retdata.RPData.HostReturnCode,null);
                    resultdetail.HostTranFlowNo = CommonDataHelper.StrTrimer(retdata.RPData.HostTranFlowNo,null);
                    resultdetail.TransSeq = CommonDataHelper.StrTrimer(retdata.RPData.TransSeq, null);
                    outmsg = CommonDataHelper.StrTrimer(retdata.RPData.HostReturnMessage,null);
                    
                    if (retdata.RPData.RetCode == "00")
                    {
                        statecode = MsgLogState.RecvSucceed;
                    }
                    else if (retdata.RPData.RetCode == "02")
                    {
                        statecode = MsgLogState.PayTimeOut;
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                //MQMsgCommonHelper.UpdateMQLog(recdata, resultdetail.HostTranFlowNo, outmsg, statecode, out dbexception);
                OnUpdateDBLog(recdata, resultdetail.HostTranFlowNo, outmsg, statecode, out dbexception);
                return resultdetail;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return resultdetail;
            }
        }

        internal static PaymentResult OnPayEraseAccounting(String tellerNO, String orgNO, String flowNO, DateTime orignalDate, String hostFlowNO, out String outmsg)
        {
            outmsg = "";
            PaymentResult resultdetail = null;
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.PayEraseAccounting(tellerNO, orgNO, flowNO, orignalDate, hostFlowNO, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Payment, tellerNO, orgNO, (int)MessageBizType.PayAcctErase, codemsg);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Payment, codemsg));
#endif
                PayOutcomeAcctEraseData retdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as PayOutcomeAcctEraseData;
                MsgLogState statecode = MsgLogState.RecvFailed;
                if (retdata != null && retdata.RPData != null)
                {
                    resultdetail = new PaymentResult();
                    resultdetail.Succeed = (retdata.RPData.RetCode == "00");
                    resultdetail.HostReturnMessage = CommonDataHelper.StrTrimer(retdata.RPData.RetMsg, null);
                    //resultdetail.HostTranFlowNo = CommonDataHelper.StrTrimer(retdata.RPData.RetHostFlowNO, null);
                    outmsg = CommonDataHelper.StrTrimer(retdata.RPData.RetMsg, null);
                    
                    if (retdata.RPData.RetCode == "00")
                    {
                        statecode = MsgLogState.RecvSucceed;
                    }
                    else if (retdata.RPData.RetCode == "02")
                    {
                        statecode = MsgLogState.PayTimeOut;
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                //MQMsgCommonHelper.UpdateMQLog(recdata, resultdetail.HostTranFlowNo, outmsg, statecode, out dbexception);
                OnUpdateDBLog(recdata, resultdetail.HostTranFlowNo, outmsg, statecode, out dbexception);
                return resultdetail;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return resultdetail;
            }
        }

        internal static PaymentResult OnPayIncomeEraseAccounting(String tellerNO, String orgNO, String paySN, String srcBankNO, DateTime orignalDate, String channelType, String hostFlowNO, out String outmsg)
        {
            outmsg = "";
            PaymentResult resultdetail = null;
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.PayIncomeEraseAccounting(tellerNO, orgNO, paySN, srcBankNO, orignalDate, channelType, hostFlowNO, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Payment, tellerNO, orgNO, (int)MessageBizType.PayAcctErase, codemsg);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Payment, codemsg));
#endif
                PayIncomeAcctEraseData retdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as PayIncomeAcctEraseData;
                MsgLogState statecode = MsgLogState.RecvFailed;
                if (retdata != null && retdata.RPData != null)
                {
                    resultdetail = new PaymentResult();
                    resultdetail.Succeed = (retdata.RPData.RetCode == "00");
                    resultdetail.HostReturnMessage = CommonDataHelper.StrTrimer(retdata.RPData.RetMsg, null);
                    //resultdetail.HostTranFlowNo = CommonDataHelper.StrTrimer(retdata.RPData.RetHostFlowNO, null);
                    outmsg = CommonDataHelper.StrTrimer(retdata.RPData.RetMsg, null);
                    
                    if (retdata.RPData.RetCode == "00")
                    {
                        statecode = MsgLogState.RecvSucceed;
                    }
                    else if (retdata.RPData.RetCode == "02")
                    {
                        statecode = MsgLogState.PayTimeOut;
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                //MQMsgCommonHelper.UpdateMQLog(recdata, resultdetail.HostTranFlowNo, outmsg, statecode, out dbexception);
                OnUpdateDBLog(recdata, resultdetail.HostTranFlowNo, outmsg, statecode, out dbexception);
                return resultdetail;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return resultdetail;
            }
        }

        internal static PaymentResult OnPayVostroAccountEliminate(String tellerNO, String orgNO, DateTime delegateDate, PayVostroAcctEliminate inputData, String exFlowNO, out String outmsg)
        {
            outmsg = "";
            PaymentResult resultdetail = null;
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.PayVostroAccountEliminate(tellerNO, orgNO, delegateDate.ToString("yyyyMMdd"), inputData, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Payment, tellerNO, orgNO, (int)MessageBizType.PayVostroAcctEliminate, codemsg);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Payment, codemsg));
#endif
                PayVostroAcctEliminateData retdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as PayVostroAcctEliminateData;
                MsgLogState statecode = MsgLogState.RecvFailed;
                if (retdata != null && retdata.RPData != null)
                {
                    resultdetail = new PaymentResult();
                    resultdetail.Succeed = (retdata.RPData.RetCode == "00");
                    resultdetail.HostReturnMessage = CommonDataHelper.StrTrimer(retdata.RPData.HostReturnMessage, null);
                    resultdetail.HostReturnCode = CommonDataHelper.StrTrimer(retdata.RPData.HostReturnCode, null);
                    resultdetail.HostTranFlowNo = CommonDataHelper.StrTrimer(retdata.RPData.HostTranFlowNo, null);
                    resultdetail.TransSeq = CommonDataHelper.StrTrimer(retdata.RPData.TransSeq, null);
                    resultdetail.PendingSN = CommonDataHelper.StrTrimer(retdata.RPData.PendingSN, null);
                    outmsg = CommonDataHelper.StrTrimer(retdata.RPData.HostReturnMessage, null);
                    
                    if (retdata.RPData.RetCode == "00")
                    {
                        statecode = MsgLogState.RecvSucceed;
                    }
                    else if (retdata.RPData.RetCode == "02")
                    {
                        statecode = MsgLogState.PayTimeOut;
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                //转换保存此次来账销账的资金流水号
                if (!string.IsNullOrEmpty(exFlowNO))
                {
                    recdata.BizMsgID = exFlowNO;
                    //MQMsgCommonHelper.UpdateMQLog(recdata,  resultdetail.HostTranFlowNo, outmsg, statecode, out dbexception);
                    OnUpdateDBLog(recdata, resultdetail.HostTranFlowNo, outmsg, statecode, out dbexception);
                }
                return resultdetail;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return resultdetail;
            }
        }

        internal static PaymentResult OnPayInterBankBiz(String tellerNO, String orgNO, DateTime tradeDate, PayInterBank inputData, out String outmsg)
        {
            outmsg = "";
            PaymentResult resultdetail = null;
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData recdata = null;
            MessageData reqdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.PayInterBankBiz(tellerNO, orgNO, tradeDate, inputData, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Payment, tellerNO, orgNO, (int)MessageBizType.PayInterBank, codemsg, inputData.TransferFlowNo);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Payment, codemsg));
#endif
                PayInterBankData retdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as PayInterBankData;
                MsgLogState statecode = MsgLogState.RecvFailed;
                if (retdata != null && retdata.RPData != null)
                {
                    resultdetail = new PaymentResult();
                    resultdetail.Succeed = (retdata.RPData.RetCode == "00");
                    resultdetail.HostReturnMessage = CommonDataHelper.StrTrimer(retdata.RPData.HostReturnMessage, null);
                    resultdetail.HostReturnCode = CommonDataHelper.StrTrimer(retdata.RPData.HostReturnCode, null);
                    resultdetail.HostTranFlowNo = CommonDataHelper.StrTrimer(retdata.RPData.HostTranFlowNo, null);
                    resultdetail.TransSeq = CommonDataHelper.StrTrimer(retdata.RPData.TransSeq, null);
                    outmsg = CommonDataHelper.StrTrimer(retdata.RPData.HostReturnMessage, null);
                    
                    if (retdata.RPData.RetCode == "00")
                    {
                        statecode = MsgLogState.RecvSucceed;
                    }
                    else if (retdata.RPData.RetCode == "02")
                    {
                        statecode = MsgLogState.PayTimeOut;
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                //MQMsgCommonHelper.UpdateMQLog(recdata, resultdetail.HostTranFlowNo, outmsg, statecode, out dbexception);
                OnUpdateDBLog(recdata, resultdetail.HostTranFlowNo, outmsg, statecode, out dbexception);
                return resultdetail;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return resultdetail;
            }
        }

        internal static PaymentResult OnPayCheckAcct(DateTime queryDate, out List<PayCheckAcct> outData, out String outmsg)
        {
            outmsg = "";
            outData = null;
            PaymentResult resultdetail = null;
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData recdata = null;
            MessageData reqdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.PayCheckAccount(queryDate, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.PaymentDownload, MsgHandlerEntry.System_Organ_ID, MsgHandlerEntry.System_Teller_ID, (int)MessageBizType.PayCheckAccout, codemsg, "");
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Payment, codemsg));
#endif
                PayAcctCheckData retdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as PayAcctCheckData;
                MsgLogState statecode = MsgLogState.RecvFailed;
                if (retdata != null && retdata.RPData != null)
                {
                    resultdetail = new PaymentResult();
                    resultdetail.Succeed = (CommonDataHelper.StrTrimer(retdata.RPData.RetCode, null) == "0");
                    resultdetail.HostReturnMessage = CommonDataHelper.StrTrimer(retdata.RPData.RetMsg, null);
                    outmsg = CommonDataHelper.StrTrimer(retdata.RPData.RetMsg, null);
                    
                    if (CommonDataHelper.StrTrimer(retdata.RPData.RetCode, null) == "0")
                    {
                        statecode = MsgLogState.RecvSucceed;
                        if (retdata.RPData.AccoutCount > 0)
                        {
                            outData = new List<PayCheckAcct>();
                            foreach (PayAcctCheckItemRP item in retdata.RPData.AccountList)
                            {
                                PayCheckAcct check = new PayCheckAcct();
                                check.AccountFlag = CommonDataHelper.StrTrimer(item.AccountFlag, null);
                                check.Amount = CommonDataHelper.StrTrimer(item.Amount, null);
                                check.BizType = CommonDataHelper.StrTrimer(item.BizType, null);
                                check.ChanelFlag = CommonDataHelper.StrTrimer(item.ChanelFlag, null);
                                check.Currency = CommonDataHelper.StrTrimer(item.Currency, null);
                                check.Fee = CommonDataHelper.StrTrimer(item.Fee, null);
                                check.HostFlowNO = CommonDataHelper.StrTrimer(item.HostFlowNO, null);
                                check.HostRespCode = CommonDataHelper.StrTrimer(item.HostRespCode, null);
                                check.HostRespMsg = CommonDataHelper.StrTrimer(item.HostRespMsg, null);
                                check.HostTradeDate = CommonDataHelper.StrTrimer(item.HostTradeDate, null);
                                check.IBOLimit = CommonDataHelper.StrTrimer(item.IBOLimit, null);
                                check.IBORate = CommonDataHelper.StrTrimer(item.IBORate, null);
                                check.MsgNO = CommonDataHelper.StrTrimer(item.MsgNO, null);
                                check.Note = CommonDataHelper.StrTrimer(item.Note, null);
                                check.Operator = CommonDataHelper.StrTrimer(item.Operator, null);
                                check.OrganNO = CommonDataHelper.StrTrimer(item.OrganNO, null);
                                check.PayAccount = CommonDataHelper.StrTrimer(item.PayAccount, null);
                                check.PayBankName = CommonDataHelper.StrTrimer(item.PayBankName, null);
                                check.PayBankNO = CommonDataHelper.StrTrimer(item.PayBankNO, null);
                                check.PayName = CommonDataHelper.StrTrimer(item.PayName, null);
                                check.PlatDate = CommonDataHelper.StrTrimer(item.PlatDate, null);
                                check.RecvAccount = CommonDataHelper.StrTrimer(item.RecvAccount, null);
                                check.RecvBankName = CommonDataHelper.StrTrimer(item.RecvBankName, null);
                                check.RecvBankNO = CommonDataHelper.StrTrimer(item.RecvBankNO, null);
                                check.RecvName = CommonDataHelper.StrTrimer(item.RecvName, null);
                                check.SeqNO = CommonDataHelper.StrTrimer(item.SeqNO, null);
                                check.SysSeqNO = CommonDataHelper.StrTrimer(item.SysSeqNO, null);
                                check.TradeDate = CommonDataHelper.StrTrimer(item.TradeDate, null);
                                check.TradeTime = CommonDataHelper.StrTrimer(item.TradeTime, null);
                                check.UploadChannel = CommonDataHelper.StrTrimer(item.UploadChannel, null);
                                outData.Add(check);
                            }
                        }

                    }
                    else
                    {
                        statecode = MsgLogState.RecvFailed;
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                if (statecode != MsgLogState.RecvSucceed)
                {
                    //MQMsgCommonHelper.UpdateMQLog(recdata.MessageID, outmsg, statecode, out dbexception);
                    OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                }
                return resultdetail;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return resultdetail;
            }
        }

        internal static PaymentResult OnPayDownloadBanksInfo(out List<PayBanksInfo> outData, out String outmsg)
        {
            outmsg = "";
            outData = null;
            PaymentResult resultdetail = null;
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData recdata = null;
            MessageData reqdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.PayRetrieveBanksInfo(ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.PaymentDownload, MsgHandlerEntry.System_Organ_ID, MsgHandlerEntry.System_Teller_ID, (int)MessageBizType.PayBanksInfoDownload, codemsg, "");
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Payment, codemsg));
#endif
                PayBankInfoData retdata = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as PayBankInfoData;
                MsgLogState statecode = MsgLogState.RecvFailed;
                if (retdata != null && retdata.RPData != null)
                {
                    resultdetail = new PaymentResult();
                    resultdetail.Succeed = (CommonDataHelper.StrTrimer(retdata.RPData.RetCode, null) == "0");
                    resultdetail.HostReturnMessage = CommonDataHelper.StrTrimer(retdata.RPData.RetMsg, null);
                    outmsg = CommonDataHelper.StrTrimer(retdata.RPData.RetMsg, null);
                    
                    if (CommonDataHelper.StrTrimer(retdata.RPData.RetCode, null) == "0")
                    {
                        statecode = MsgLogState.RecvSucceed;
                        if (retdata.RPData.BankCount > 0)
                        {
                            outData = new List<PayBanksInfo>();
                            foreach (PayBankInfoItemRP item in retdata.RPData.BankList)
                            {
                                PayBanksInfo check = new PayBanksInfo();
                                check.Address = CommonDataHelper.StrTrimer(item.Address, null);
                                check.BankName = CommonDataHelper.StrTrimer(item.BankName, null);
                                check.BankNO = CommonDataHelper.StrTrimer(item.BankNO, null);
                                check.CityCode = CommonDataHelper.StrTrimer(item.CityCode, null);
                                check.DirectParticipator = CommonDataHelper.StrTrimer(item.DirectParticipator, null);
                                check.NodeCode = CommonDataHelper.StrTrimer(item.NodeCode, null);
                                check.TelephoneNO = CommonDataHelper.StrTrimer(item.TelephoneNO, null);
                               
                                outData.Add(check);
                            }
                        }
                    }
                    else
                    {
                        statecode = MsgLogState.RecvFailed;
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }
                if (statecode != MsgLogState.RecvSucceed)
                {
                    //MQMsgCommonHelper.UpdateMQLog(recdata.MessageID, outmsg, statecode, out dbexception);
                    OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                }
                return resultdetail;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return resultdetail;
            }
        }
        #endregion 

        #region [ 活期定期开户 ]

        /// <summary>
        /// 活期定期开户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        internal static bool OnOpenAccount(string orgID, string tellerNO, DateTime tradeDate, InterBankOpenAcctInfo info, out string outmsg)
        {
            outmsg = "";
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.OpenAccount(orgID, tellerNO, tradeDate, info, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerNO, orgID, (int)MessageBizType.CoreInterBankOpenAccount, codemsg, info.NOTICE_NO);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif

                InterBankOpenAcctData data = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as InterBankOpenAcctData;
                if (data != null)
                {
                    if (!String.IsNullOrEmpty(data.SyserrHandler.Message))
                    {
                        outmsg = data.SyserrHandler.Message;
                    }
                    else
                    {
                        outmsg = MsgHandlerEntry.ExtractOMsg(data);
                    }

                    if (string.IsNullOrEmpty(outmsg) || outmsg.Trim() == "")
                        return true;
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }

                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return false;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return false;
            }

        }

        #endregion

        #region [ 活期定期销户、部提 ]

        /// <summary>
        /// 活期定期开户撤销
        /// </summary>
        /// <param name="info"></param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        internal static bool OnDeleteAccount(string orgID, string tellerNO, DateTime tradeDate, InterBankDeleteAcctInfo info, out string outmsg)
        {
            outmsg = "";
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.DeleteAccount(orgID, tellerNO, tradeDate, info, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerNO, orgID, (int)MessageBizType.CoreInterBankDeleteAccount, codemsg, info.NOTICE_NO);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif

                InterBankDeleteAcctData data = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as InterBankDeleteAcctData;
                if (data != null)
                {
                    if (!String.IsNullOrEmpty(data.SyserrHandler.Message))
                    {
                        outmsg = data.SyserrHandler.Message;
                    }
                    else
                    {
                        outmsg = MsgHandlerEntry.ExtractOMsg(data);
                    }

                    if (string.IsNullOrEmpty(outmsg) || outmsg.Trim() == "")
                        return true;
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }

                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return false;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return false;
            }
        } 

        #endregion

        #region [ 通知单查询 ]

        internal static InterBankNoticeLetterInfo OnRetrieveNoticeLetter(DateTime tradeDate, string tradeOrg, string tellerNo, InterBankNoticeQueryInfo info, out string outmsg)
        {
            outmsg = "";
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;
            InterBankNoticeLetterInfo resultdetail = null;
            try
            {
                msgid = MsgTransferUtility.RetrieveNoticeLetter(tradeDate,tradeOrg, tellerNo, info, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerNo, tradeOrg, (int)MessageBizType.CoreInterBankNoticeLetter, codemsg, info.NoticeID);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif

                InterBankNoticeLetterData data = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as InterBankNoticeLetterData;
                if (data != null)
                {
                    if (!String.IsNullOrEmpty(data.SyserrHandler.Message))
                    {
                        outmsg = data.SyserrHandler.Message;
                    }
                    else if (data.OmsgHandler.NUM_ENT > 0)
                    {
                        outmsg = MsgHandlerEntry.ExtractOMsg(data);
                    }
                    else
                    {
                        if (data.OData != null)
                        {
                            resultdetail = new InterBankNoticeLetterInfo();
                            resultdetail.ACCOUNT_DATE = CommonDataHelper.StrTrimer(data.OData.ACCOUNT_DATE, null);
                            resultdetail.NOTICE_NO = CommonDataHelper.StrTrimer(data.OData.NOTICE_NO, null);
                            resultdetail.NOTICE_TYPE = string.IsNullOrEmpty(data.OData.NOTICE_TYPE) ? 
                                AidTypeDefine.INTER_BANK_NOTICE_TYPE.OpenAccount : (AidTypeDefine.INTER_BANK_NOTICE_TYPE)Enum.Parse(typeof(AidTypeDefine.INTER_BANK_NOTICE_TYPE), CommonDataHelper.StrTrimer(data.OData.NOTICE_TYPE, null));
                            resultdetail.BUSINESS_TYPE = string.IsNullOrEmpty(data.OData.BUSINESS_TYPE) ?
                                AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE.Current : (AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE)Enum.Parse(typeof(AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE), CommonDataHelper.StrTrimer(data.OData.BUSINESS_TYPE, null));
                            resultdetail.ACCOUNT = CommonDataHelper.StrTrimer(data.OData.ACCOUNT_NO, null);
                            resultdetail.CURRENCY = CommonDataHelper.StrTrimer(data.OData.CURRENCY, null);
                            resultdetail.CUSTOMER_TYPE = CommonDataHelper.StrTrimer(data.OData.CUSTOMER_TYPE, null);
                            resultdetail.CASH_PROPERTY = CommonDataHelper.StrTrimer(data.OData.CASH_PROPERTY, null);
                            resultdetail.PRODUCT_TYPE = CommonDataHelper.StrTrimer(data.OData.PRODUCT_TYPE, null);
                            resultdetail.PRODUCT_CODE = CommonDataHelper.StrTrimer(data.OData.PRODUCT_CODE, null);
                            resultdetail.DEPOSIT_TYPE = string.IsNullOrEmpty(data.OData.DEPOSIT_TYPE) ? 
                                AidTypeDefine.INTER_BANK_DEPOSITING_CATEGORY.Other_Banks : (AidTypeDefine.INTER_BANK_DEPOSITING_CATEGORY)Enum.Parse(typeof(AidTypeDefine.INTER_BANK_DEPOSITING_CATEGORY), CommonDataHelper.StrTrimer(data.OData.DEPOSIT_TYPE, null));
                            resultdetail.ACCOUNT_PROPERTY = CommonDataHelper.StrTrimer(data.OData.ACCOUNT_PROPERTY, null);
                            resultdetail.ACCOUNT_TYPE = CommonDataHelper.StrTrimer(data.OData.ACCOUNT_TYPE, null);
                            resultdetail.CURRENT_ACCOUNT = CommonDataHelper.StrTrimer(data.OData.CURRENT_ACCOUNT, null);
                            resultdetail.INTEREST_ACCOUNT = CommonDataHelper.StrTrimer(data.OData.INTEREST_ACCOUNT, null);
                            resultdetail.FIXED_NEW_ACCOUNT = CommonDataHelper.StrTrimer(data.OData.FIXED_NEW_ACCOUNT, null);
                            resultdetail.VOUCHER_NO = CommonDataHelper.StrTrimer(data.OData.VOUCHER_NO, null);
                            resultdetail.VOUCHER_NEW_NO = CommonDataHelper.StrTrimer(data.OData.VOUCHER_NEW_NO, null);
                            resultdetail.AUTO_REDEPO = CommonDataHelper.StrTrimer(data.OData.AUTO_REDEPO, null) == "1";
                            resultdetail.DEPOSIT_TERM = CommonDataHelper.StrTrimer(data.OData.DEPOSIT_TERM, null);
                            resultdetail.VALUE_DATE = CommonDataHelper.StrTrimer(data.OData.VALUE_DATE, null);
                            resultdetail.MATURITY_DATE = CommonDataHelper.StrTrimer(data.OData.MATURITY_DATE, null);
                            resultdetail.AMOUNT = CommonDataHelper.ConvertDecimal(CommonDataHelper.StrTrimer(data.OData.AMOUNT, null), 6);
                            resultdetail.RATE = CommonDataHelper.ConvertDecimal(CommonDataHelper.StrTrimer(data.OData.RATE, null), 6);
                            resultdetail.INTEREST_BEARING_MANNER = string.IsNullOrEmpty(data.OData.INTEREST_BEARING_MANNER) ? 
                                AidTypeDefine.INTER_BANK_COUPON_TYPE.InterestWithPrincipal : (AidTypeDefine.INTER_BANK_COUPON_TYPE)Enum.Parse(typeof(AidTypeDefine.INTER_BANK_COUPON_TYPE),  CommonDataHelper.StrTrimer(data.OData.INTEREST_BEARING_MANNER, null));
                            resultdetail.INTEREST = CommonDataHelper.ConvertDecimal(CommonDataHelper.StrTrimer(data.OData.INTEREST, null), 6);
                            resultdetail.CUSTOMER_CODE = CommonDataHelper.StrTrimer(data.OData.CUSTOMER_CODE, null);
                            resultdetail.BANK_FLAG = CommonDataHelper.StrTrimer(data.OData.BANK_FLAG, null);
                            resultdetail.HANDLE_ORGNAZTION = CommonDataHelper.StrTrimer(data.OData.HANDLE_ORGNAZTION, null);
                            resultdetail.APPROVE_ORGNAZTION = CommonDataHelper.StrTrimer(data.OData.APPROVE_ORGNAZTION, null);
                            resultdetail.HANDLE_TELLER = CommonDataHelper.StrTrimer(data.OData.HANDLE_TELLER, null);
                            resultdetail.APPROVE_TELLER = CommonDataHelper.StrTrimer(data.OData.APPROVE_TELLER, null);
                            resultdetail.HANDLE_FLAG = CommonDataHelper.StrTrimer(data.OData.HANDLE_FLAG, null);
                            resultdetail.TRADE_FLOW_NO = CommonDataHelper.StrTrimer(data.OData.TRADE_FLOW_NO, null);
                            resultdetail.CHILD_TRADE_FLOW_NO = CommonDataHelper.StrTrimer(data.OData.CHILD_TRADE_FLOW_NO, null);
                            resultdetail.TELLER_FLOW_NO = CommonDataHelper.StrTrimer(data.OData.TELLER_FLOW_NO, null);
                            resultdetail.RECORD_FLAG = CommonDataHelper.StrTrimer(data.OData.RECORD_FLAG, null);
                            resultdetail.BOOK_TYPE = CommonDataHelper.StrTrimer(data.OData.BOOK_TYPE, null);
                            resultdetail.ACCOUNT_NAME = CommonDataHelper.StrTrimer(data.OData.ACCOUNT_NAME, null);
                            resultdetail.BALANCE = CommonDataHelper.ConvertDecimal(CommonDataHelper.StrTrimer(data.OData.BALANCE, null),6);
                            resultdetail.SUM_AMOUNT = CommonDataHelper.ConvertDecimal(CommonDataHelper.StrTrimer(data.OData.SUM_AMOUNT, null),6);
                            resultdetail.RESERVE = CommonDataHelper.StrTrimer(data.OData.RESERVE, null);                            
                        }                        
                        return resultdetail;
                    }
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }

                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return resultdetail;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return resultdetail;
            }
        }


        #endregion

        #region [ 定期同业存放自动转存 ]

        /// <summary>
        /// 定期同业存放自动转存
        /// </summary>
        /// <param name="tradeDate">业务交易日 10</param>
        /// <param name="tradeOrg">交易机构号 6</param>
        /// <param name="tellerNo">柜员号 7</param>
        /// <param name="account">账号</param>
        /// <param name="startDate">新起息日期</param>
        /// <param name="maturityDate">新到期日期</param>
        /// <param name="bizFlow">资金业务指令号</param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        internal static bool OnInterBankAutoRedepo(DateTime tradeDate, string tradeOrg, string tellerNo, string account, DateTime startDate, DateTime maturityDate, string bizFlow, out string outmsg)
        {
            outmsg = "";
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.InterBankAutoRedepo(tradeOrg, tellerNo, tradeDate, account, startDate, maturityDate, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerNo, tradeOrg, (int)MessageBizType.CoreInterBankAutoRedepo, codemsg, bizFlow);
#if MQSYNC
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);
#else
                recdata = MsgHandlerEntry.DeliverMessage(MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, codemsg));
#endif

                InterBankAutoRedepoData data = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as InterBankAutoRedepoData;
                if (data != null)
                {
                    if (!String.IsNullOrEmpty(data.SyserrHandler.Message))
                    {
                        outmsg = data.SyserrHandler.Message;
                    }
                    else
                    {
                        outmsg = MsgHandlerEntry.ExtractOMsg(data);
                    }

                    if (string.IsNullOrEmpty(outmsg) || outmsg.Trim() == "")
                        return true;
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }

                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return false;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return false;
            }
        }

        #endregion

        #region 同业存放活期账户余额查询
        internal static double OnInterBankRetrieveBalance(string tradeOrg, string tellerNO, DateTime tradeDate, string accountNO, out string outmsg)
        {
            outmsg = "";
            double balance = 0.0d;
            byte[] codemsg = null;
            bool sent = false;
            string dbexception = "";
            MessageData reqdata = null;
            MessageData recdata = null;
            Guid msgid = Guid.Empty;
            try
            {
                msgid = MsgTransferUtility.InterBankRetrieveAcctInfo(tradeOrg, tellerNO, tradeDate, accountNO, ref codemsg);
                reqdata = MsgHandlerEntry.CreateMessageData(msgid, PlatformType.Core, tellerNO, tradeOrg, (int)MessageBizType.CoreInterBankRetrieveAcctInfo, codemsg);
                recdata = MQMsgHandlerEntry.DeliverMessage(reqdata, codemsg, out sent);

                InterBankAcctInfoData data = MsgTransfer.DecodeMsg(recdata.MessageID, recdata.CurrentRespPackage.PackageMessage) as InterBankAcctInfoData;
                if (data != null)
                {
                    if (!String.IsNullOrEmpty(data.SyserrHandler.Message))
                    {
                        outmsg = data.SyserrHandler.Message;
                    }
                    else
                    {
                        outmsg = MsgHandlerEntry.ExtractOMsg(data);
                    }

                    if (string.IsNullOrEmpty(outmsg) || outmsg.Trim() == "")
                    {
                        if (data.OData != null)
                        {
                            double.TryParse(data.OData.CurrentBalance, out balance);
                            return balance;
                        }
                    }
                    
                }
                else
                {
                    outmsg = MsgHandlerEntry.Type_Convert_Error;
                }

                OnUpdateDBLog(recdata.MessageID, outmsg, MsgLogState.RecvFailed, out dbexception);
                return balance;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                OnException(sent, reqdata, recdata, msgid, ex, outmsg);
                return balance;
            }
        }
        #endregion

        private static void OnException(bool sent, MessageData reqdata, MessageData recdata, Guid msgid, Exception ex, String outmsg)
        {
            String dbexception="";
            if (ex is AidException)
            {
                // 不做任何数据库更新
            }
            else
            {
                if (sent)
                {
                    if (recdata != null)
                    {
                        MQMsgCommonHelper.UpdateMQLog(recdata, null, outmsg, MsgLogState.RecvFailed, out dbexception);
                    }
                    else
                    {
                        MQMsgCommonHelper.UpdateMQLog(reqdata, null, outmsg, MsgLogState.RecvFailed, out dbexception);
                    }
                }
            }

            xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Error, string.Format("<ClientSyncHelper> Exception:{0}; \r\nStatkTrace:{1}.", ex.Message, ex.StackTrace));
        }

        private static void OnUpdateDBLog(Guid guid, string msg, MsgLogState state, out string dbexception)
        {
            dbexception = "";
            MQMsgCommonHelper.UpdateMQLog(guid, msg, state, out dbexception);
        }

        private static void OnUpdateDBLog(MessageData msgdata, string flowno, string msg, MsgLogState state, out string dbexception)
        {
            dbexception = "";
            MQMsgCommonHelper.UpdateMQLog(msgdata, flowno, msg, state, out dbexception);
        }
    }
}
