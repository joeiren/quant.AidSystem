using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xQuant.AidSystem.BizDataModel;
using xQuant.AidSystem.Communication;
using System.Data;
using xIR.Framework.Transactions;
using xQuant.AidSystem.DBAction;

namespace xQuant.AidSystem.ClientSyncWrapper
{
    public class AidSysClientSyncWrapper
    {
        #region 柜员认证
        /// <summary>
        /// 柜员身份认证
        /// </summary>
        /// <param name="orgno">机构号</param>
        /// <param name="tellerno">柜员号</param>
        /// <param name="password">密码</param>
        /// <param name="duedate">发生日期</param>
        /// <param name="outmsg">出错返回信息</param>
        /// <returns>身份认证通过与否</returns>
        public static bool TellerAuth(String orgno, String tellerno, String password, out String duedate, out String outmsg)
        {
            duedate = "";
            byte[] encryptpwd = null;
            if (ClientSyncHelper.OnEncryptPassword(tellerno, orgno, password, out encryptpwd, out outmsg))
            {
                if (encryptpwd == null)
                {
                    return false;
                }
                if (ClientSyncHelper.OnTellerAuth(tellerno, orgno, password, encryptpwd, out duedate, out outmsg))
                {
                    return true;
                }
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            
            return false;
        }

        /// <summary>
        /// 指纹验证
        /// </summary>
        /// <param name="tellerno">柜员号</param>
        /// <param name="orgno">机构号</param>
        /// <param name="tradedate">交易日期</param>
        /// <param name="deviceType">指纹仪类型</param>
        /// <param name="markInfo">指纹数据</param>
        /// <param name="outmsg">返回错误信息</param>
        /// <returns>是否验证通过</returns>
        public static bool FingerMarkAuth(String tellerno, String orgno, DateTime tradedate, String deviceType, String markInfo, out String outmsg)
        {
            if (ClientSyncHelper.OnFingerMarkValidation(tellerno, orgno, tradedate, deviceType, markInfo, out outmsg))
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            return false;
        }


        /// <summary>
        /// 柜员密码修改
        /// </summary>
        /// <param name="tellerNO">柜员号</param>
        /// <param name="organNO">机构号</param>
        /// <param name="tradeDate">操作时间</param>
        /// <param name="oldPwd">旧密码</param>
        /// <param name="newPwd">新密码</param>
        /// <param name="outmsg">调用接口结果信息</param>
        /// <returns></returns>
        public static bool TellerChangePwd(String tellerNO, String organNO, DateTime tradeDate, String oldPwd, String newPwd, out string outmsg)
        {
            if (oldPwd.Length != 6 || newPwd.Length != 6)
            {
                outmsg = "密码必须为6位！";
                return false;
            }

            if (oldPwd.Equals(newPwd))
            {
                outmsg = "新密码与旧密码不能相同！";
                return false;
            }
            //byte[] encryptpwd = null;

            if (ClientSyncHelper.OnTellerChangePwd(tellerNO, organNO, tradeDate, oldPwd, newPwd, out outmsg))
            {
                return true;
            }
            outmsg = string.Format("{0},{1}",MsgHandlerEntry.Info_Return_Core,outmsg);
            return false;
        }
        #endregion

        #region 客户
        /// <summary>
        /// 客户创建
        /// </summary>
        /// <param name="orgno">机构号</param>
        /// <param name="tellerno">柜员号</param>
        /// <param name="tradedate">发生日期</param>
        /// <param name="customer">需创建的客户信息</param>
        /// <param name="internalcode">成功后的返回客户内码</param>
        /// <param name="outmsg">出错返回信息</param>
        /// <returns>客户创建成功与否</returns>
        public static bool CreateCustomer(String orgno, String tellerno, DateTime tradedate, CoreCustomerBrief customer, out String internalcode, out String outmsg)
        {
            if (ClientSyncHelper.OnCreateCstm(tellerno, orgno, tradedate, customer, out internalcode, out outmsg))
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            return false;
        }

        /// <summary>
        /// 查询客户
        /// </summary>
        /// <param name="orgno">机构号</param>
        /// <param name="tellerno">柜员号</param>
        /// <param name="tradedate">发生日期</param>
        /// <param name="internalcode">客户内码</param>
        /// <param name="customer">返回客户对象</param>
        /// <param name="outmsg">出错返回信息</param>
        /// <returns></returns>
        public static bool RetrieveCustomer(String orgno, String tellerno, DateTime tradedate, String internalcode, out CoreCustomer customer, out String outmsg)
        {
            customer = ClientSyncHelper.OnRetrieveCstm(tellerno, orgno, tradedate, internalcode, out outmsg);
            if (customer != null)
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            return false;
        }

        /// <summary>
        /// 更新客户
        /// </summary>
        /// <param name="orgno">机构号</param>
        /// <param name="tellerno">柜员号</param>
        /// <param name="tradedate">发生日期</param>
        /// <param name="internalcode">客户内码</param>
        /// <param name="customer">需更新的客户信息</param>
        /// <param name="outmsg">出错返回信息</param>
        /// <returns>更新成功与否</returns>
        public static bool UpdateCustomer(String orgno, String tellerno, DateTime tradedate, String internalcode, CoreCustomerBrief customer, out String outmsg)
        {
            if (ClientSyncHelper.OnUpdateCstm(tellerno, orgno, tradedate, internalcode, customer, out outmsg))
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            return false;
        }

        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="orgno">机构号</param>
        /// <param name="tellerno">柜员号</param>
        /// <param name="tradedate">发生日期</param>
        /// <param name="internalcode">客户内码</param>
        /// <param name="outmsg">出错返回信息</param>
        /// <returns>删除成功与否</returns>
        public static bool DeleteCustomer(String orgno, String tellerno, DateTime tradedate, String internalcode, out String outmsg)
        {
            if (ClientSyncHelper.OnDeleteCstm(tellerno, orgno, tradedate, internalcode, out outmsg))
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            return false;
        }

        /// <summary>
        /// 核心客户
        /// </summary>
        /// <param name="orgno"></param>
        /// <param name="tellerno"></param>
        /// <param name="tradedate"></param>
        /// <param name="doctype"></param>
        /// <param name="docno"></param>
        /// <param name="outdata"></param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        public static bool RetrieveCustomerBaseInfo(String orgno, String tellerno, DateTime tradedate, String doctype, String docno, out List<CoreCustomBaseInfo> outdatalist, out String outmsg)
        {
            if (ClientSyncHelper.OnRetrieveCstmBaseInfo(tellerno, orgno, tradedate, doctype, docno, out outdatalist, out outmsg))
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            return false;
        }
        #endregion

        #region 记账
        /// <summary>
        /// 核心记账
        /// </summary>
        /// <param name="orgno">机构号</param>
        /// <param name="tellerno">柜员号</param>
        /// <param name="tradedate">发生日期</param>
        /// <param name="followno">资金业务系统流水号</param>
        /// <param name="billlist">记账分录</param>
        /// <param name="resultlist">返回记账成功后的会计分录</param>
        /// <param name="outmsg">记账失败时的信息</param>
        /// <returns>记账成功与否</returns>
        public static bool CoreAcctRecord(String orgno, String tellerno, DateTime tradedate, String followno, List<CoreBillRecord> billlist, out CoreAcctResult result, out String outmsg)
        {
            result = ClientSyncHelper.OnRecordAcct(tellerno, orgno, tradedate, followno, billlist, out outmsg);
            if (result != null)
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            return false;
        }

        /// <summary>
        /// 核心记账(多贷一借，多借一贷)
        /// </summary>
        /// <param name="orgno">机构号</param>
        /// <param name="tellerno">柜员号</param>
        /// <param name="tradedate">发生日期</param>
        /// <param name="followno">资金业务系统流水号</param>
        /// <param name="multiLend">是否多借一贷</param>
        /// <param name="billlist">记账分录</param>
        /// <param name="result">返回记账成功后的会计分录</param>
        /// <param name="outmsg">记账失败时的信息</param>
        /// <returns></returns>
        public static bool CoreAcctRecordMulti2One(String orgno, String tellerno, DateTime tradedate, String followno, bool multiLend, List<CoreBillRecord> billlist, out CoreAcctResult result, out String outmsg)
        {
            result = ClientSyncHelper.OnRecordAcctMulti2One(tellerno, orgno, tradedate, followno, multiLend, billlist, out outmsg);
            if (result != null)
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            return false;
        }

        /// <summary>
        /// 记账查询
        /// </summary>
        /// <param name="orgno">机构号</param>
        /// <param name="tellerno">柜员号</param>
        /// <param name="tradedate">发生日期</param>
        /// <param name="coreSN">核心交易流水号</param>
        /// <param name="result">查询结构</param>
        /// <param name="outmsg">记账失败时的信息</param>
        /// <returns>查询成功与否</returns>
        public static bool CoreAcctRetrieve(String orgno, String tellerno, DateTime tradedate, String coreSN, out CoreAcctDetail result, out String outmsg)
        {
            result = ClientSyncHelper.OnRetrieveAccounting(tellerno, orgno, tradedate, coreSN, out outmsg);
            if (result != null)
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            return false;
        }

        /// <summary>
        /// 抹账
        /// </summary>
        /// <param name="orgno">机构号</param>
        /// <param name="tellerno">柜员号</param>
        /// <param name="tradedate">发生日期</param>
        /// <param name="coreSN">核心交易流水号</param>
        /// <param name="result">结果数据</param>
        /// <param name="outmsg">记账失败时的信息</param>
        /// <returns>抹账成功与否</returns>
        public static bool CoreAcctErase(String orgno, String tellerno, DateTime tradedate, String coreSN, out CoreAcctErase result, out String outmsg)
        {
            result = ClientSyncHelper.OnEraseAccounting(tellerno, orgno, tradedate, coreSN, out outmsg);
            if (result != null)
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            return false;
        }

        public static bool CoreAcctChecking(String tellerno, String orgno, DateTime tradedate, DateTime querydate, String flowNO, String queryOrgNO, out List<CoreCheckAcctInfo> result, out String outmsg)
        {
            result = ClientSyncHelper.OnRetrieveCheckingAcct(tellerno, orgno, tradedate, querydate, flowNO, queryOrgNO, out outmsg);
            if (result != null)
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            return false;
        }

        #endregion

        #region 资金头寸查询
        /// <summary>
        /// 内部账查询
        /// </summary>
        /// <param name="tellerno">机构号</param>
        /// <param name="orgno">柜员号</param>
        /// <param name="tradedate">发生日期</param>
        /// <param name="condition">查询数据条件</param>
        /// <param name="result">结果数据</param>
        /// <param name="outmsg">失败时的信息</param>
        /// <returns>查询成功与否</returns>
        public static bool RetrieveInternalAcct(String tellerno, String orgno, DateTime tradedate, InternalAcctCondtion condition, out FundInternalAcct result, out String outmsg)
        {
            result = ClientSyncHelper.OnRetrieveInternalAcct(tellerno, orgno, tradedate, condition, out outmsg);
            if (result != null)
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            return false;
        }

        /// <summary>
        /// 存放省内上级机构活期款项查询
        /// </summary>
        /// <param name="tellerno">机构号</param>
        /// <param name="orgno">柜员号</param>
        /// <param name="tradedate">发生日期</param>
        /// <param name="currcency">币种</param>
        /// <param name="result">结果数据</param>
        /// <param name="outmsg">失败时的信息</param>
        /// <returns>查询成功与否</returns>
        public static bool RetrieveSuperiorCurrentAcct(String tellerno, String orgno, DateTime tradedate, String currcency, out FundAcctInfo result, out String outmsg)
        {
            List<FundAcctInfo> resultlist = ClientSyncHelper.OnRetrieveSuperiorCurrentAcct(tellerno, orgno, tradedate, currcency, out outmsg);
            result = null;
            if (resultlist != null)
            {
                if (resultlist.Count > 0)
                {
                    result = resultlist[0];
                }
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            return false;
        }

        /// <summary>
        /// 县级行社上存清算资金查询
        /// </summary>
        /// <param name="tellerno"></param>
        /// <param name="orgno"></param>
        /// <param name="tradedate"></param>
        /// <param name="currcency">币种,CNY</param>
        /// <param name="option">1-全部，2-负金额</param>
        /// <param name="result"></param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        public static bool RetrieveDepositClearingFund(String tellerno, String orgno, DateTime tradedate, String currcency, String option, out List<FundAcctInfo> result, out String outmsg)
        {
            result = ClientSyncHelper.OnRetrieveDepositClearingFund(tellerno, orgno, tradedate, currcency, option, out outmsg);
            if (result != null)
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            return false;
        }

        /// <summary>
        /// 资金业务账号余额查询
        /// </summary>
        /// <param name="tellerno"></param>
        /// <param name="orgno"></param>
        /// <param name="tradedate"></param>
        /// <param name="inputlist">查询账号列表</param>
        /// <param name="result"></param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        public static bool RetrieveAcctCrntBalance(String tellerno, String orgno, DateTime tradedate, List<CoreAcctCrntBalance> inputlist, out List<FundCrntAcctBalance> result, out String outmsg)
        {
            result = ClientSyncHelper.OnRetrieveAcctCrntBalance(tellerno, orgno, tradedate, inputlist, out outmsg);
            if (result != null)
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            return false;
        }
        #endregion

        #region 支付平台接口
        /// <summary>
        /// 登记簿查询
        /// </summary>
        /// <param name="tellerNO">交易机构号</param>
        /// <param name="orgNO">柜员号</param>
        /// <param name="flowNO">资金业务系统流水号</param>
        /// <param name="orignalDate">原委托日期</param>
        /// <param name="result">返回结果的数据</param>
        /// <param name="outmsg">失败时的信息</param>
        /// <returns>查询成功与否</returns>
        public static bool RetrievePayRegister(String tellerNO, String orgNO, String flowNO, DateTime orignalDate, out PaymentResult result, out String outmsg)
        {
            result = ClientSyncHelper.OnPayRegisterRetrieved(tellerNO, orgNO, flowNO, orignalDate, out outmsg);
            if (result != null && result.Succeed)
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Pay, outmsg);
            return false;
        }

        /// <summary>
        /// 资金划拨
        /// </summary>
        /// <param name="tellerNO">机构号</param>
        /// <param name="orgNO">柜员号</param>
        /// <param name="tradeDate">交易日期</param>
        /// <param name="inputData">输入信息的数据</param>
        /// <param name="result">返回结果的数据</param>
        /// <param name="outmsg">失败时的信息</param>
        /// <returns>成功与否</returns>
        public static bool PayFundTransfer(String tellerNO, String orgNO, DateTime tradeDate, PayFundTransfer inputData, out PaymentResult result, out String outmsg)
        {
            result = ClientSyncHelper.OnPayFundTransfer(tellerNO, orgNO, tradeDate, inputData, out outmsg);
            if (result != null && result.Succeed)
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Pay, outmsg);
            return false;
        }

        /// <summary>
        /// 抹账交易
        /// </summary>
        /// <param name="tellerNO">机构号</param>
        /// <param name="orgNO">柜员号</param>
        /// <param name="flowNO">资金业务系统流水号</param>
        /// <param name="orignalDate">原委托日期</param>
        /// <param name="hostFlowNO">主机流水号</param>
        /// <param name="result">返回结果的数据</param>
        /// <param name="outmsg">失败时的信息</param>
        /// <returns>成功与否</returns>
        public static bool PayEraseAccounting(String tellerNO, String orgNO, String flowNO, DateTime orignalDate, String hostFlowNO, out PaymentResult result, out String outmsg)
        {
            result = ClientSyncHelper.OnPayEraseAccounting(tellerNO, orgNO, flowNO, orignalDate, hostFlowNO, out outmsg);
            if (result != null && result.Succeed)
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Pay, outmsg);
            return false;
        }

        /// <summary>
        /// 来帐销账后的抹帐交易
        /// </summary>
        /// <returns></returns>
        public static bool PayIncomeEraseAccounting(String tellerNO, String orgNO, String srcBankNO, String paySN, DateTime orignalDate, String channelType, String hostFlowNO, out PaymentResult result, out String outmsg)
        {
            result = ClientSyncHelper.OnPayIncomeEraseAccounting(tellerNO, orgNO, paySN, srcBankNO, orignalDate, channelType, hostFlowNO, out outmsg);
            if (result != null && result.Succeed)
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Pay, outmsg);
            return false;
        }

        /// <summary>
        /// 来账的销账
        /// </summary>
        /// <param name="tellerNO">机构号</param>
        /// <param name="orgNO">柜员号</param>
        /// <param name="delegateDate">委托日期</param>
        /// <param name="inputData">销账输入数据</param>
        /// <param name="result">返回结果的数据</param>
        /// <param name="outmsg">失败时的信息</param>
        /// <returns>成功与否</returns>
        public static bool PayVostroAccountEliminate(String tellerNO, String orgNO, DateTime delegateDate, PayVostroAcctEliminate inputData, String exFlowNO, out PaymentResult result, out String outmsg)
        {
            result = ClientSyncHelper.OnPayVostroAccountEliminate(tellerNO, orgNO, delegateDate, inputData, exFlowNO, out outmsg);
            if (result != null && result.Succeed)
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Pay, outmsg);
            return false;
        }
        /// <summary>
        /// 同业拆借业务
        /// </summary>
        /// <param name="tellerNO">机构号</param>
        /// <param name="orgNO">柜员号</param>
        /// <param name="tradeDate">交易日期</param>
        /// <param name="inputData">输入数据</param>
        /// <param name="result">返回结果的数据</param>
        /// <param name="outmsg">失败时的信息</param>
        /// <returns>成功与否</returns>
        public static bool PayInterBankBiz(String tellerNO, String orgNO, DateTime tradeDate, PayInterBank inputData, out PaymentResult result, out String outmsg)
        {
            result = ClientSyncHelper.OnPayInterBankBiz(tellerNO, orgNO, tradeDate, inputData, out outmsg);
            if (result != null && result.Succeed)
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Pay, outmsg);
            return false;
        }

        /// <summary>
        /// 支付平台对账
        /// </summary>
        /// <param name="queryDate"></param>
        /// <param name="outData"></param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        public static bool PayCheckAccount(DateTime queryDate, out List<PayCheckAcct> outData, out String outmsg)
        {
            PaymentResult result = ClientSyncHelper.OnPayCheckAcct(queryDate, out outData, out outmsg);
            if (result != null && result.Succeed)
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Pay, outmsg);
            return false;
        }
        /// <summary>
        /// 支付行号下载
        /// </summary>
        /// <param name="outData"></param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        public static bool PayBanksInfoDownload(out List<PayBanksInfo> outData, out String outmsg)
        {
            PaymentResult result = ClientSyncHelper.OnPayDownloadBanksInfo(out outData, out outmsg);
            if (result != null && result.Succeed)
            {
                return true;
            }
            outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Pay, outmsg);
            return false;
        }
        #endregion

        #region 钆账
        /// <summary>
        /// 支付平台钆账
        /// </summary>
        /// <param name="queryDate"></param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        public static bool PayRollingAcct(DateTime queryDate, out string outmsg)
        {
            List<PayCheckAcct> outData;
            if (!PayCheckAccount(queryDate, out outData, out outmsg))
            {
                //outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
                return false;
            }
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    TTRD_PAY_Controller.Delete(queryDate.ToString("yyyyMMdd"));
                    if (outData != null)
                    {
                        TTRD_PAY_Controller.Insert(outData);
                    }
                    trans.Complete();
                }
                return true;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 核心钆账
        /// </summary>
        /// <param name="tellerno"></param>
        /// <param name="orgno"></param>
        /// <param name="tradedate"></param>
        /// <param name="querydate"></param>
        /// <param name="flowNO"></param>
        /// <param name="queryOrgNO"></param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        public static bool CoreRollingAcct(String tellerno, String orgno, DateTime tradedate, DateTime querydate, String flowNO, String queryOrgNO, out String outmsg)
        {
            List<CoreCheckAcctInfo> outData;
            if (!CoreAcctChecking(tellerno, orgno, tradedate, querydate, flowNO, queryOrgNO, out outData, out outmsg))
            {
                //outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
                return false;
            }
            try
            {
                if (string.IsNullOrEmpty(queryOrgNO))
                {
                    queryOrgNO = orgno;
                }
                
                using (TransactionScope trans = new TransactionScope())
                {
                    TTRD_ZJAA_Controller.Delete(queryOrgNO, querydate.ToString("yyyyMMdd"));
                    //TTRD_ZJAA_Controller.Clear();
                    if (outData != null)
                    {
                        
                        TTRD_ZJAA_Controller.Insert(outData);
                    }                    
                    trans.Complete();
                }
                return true;
            }
            catch (Exception ex)
            {
                outmsg = ex.Message;
                return false;
            }
        }
        #endregion

        #region 日志维护
        public static int AddMQMsgLog(MessageData msgdata, byte[] senddata, MsgLogState state, out string dbexception)
        { 
            return MQMsgCommonHelper.AddMQLog(msgdata, senddata, state, out dbexception);
        }

        public static int UpdateMQMsgLog(MessageData msgdata, string hostflow, string error, MsgLogState state, out string dbexception)
        {
            return MQMsgCommonHelper.UpdateMQLog(msgdata, hostflow, error, state, out dbexception);
        }

        public static DataTable QueryMQMsgLog(string condition, out string dbexception)
        {
            return MQMsgCommonHelper.QueryMQLog(condition, out dbexception);
        }

        #endregion

        #region 同业存放
        /// <summary>
        /// 同业存放计息
        /// </summary>
        /// <param name="tellerno"></param>
        /// <param name="orgno"></param>
        /// <param name="tradedate"></param>
        /// <param name="info"></param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        public static RegularResult InterBankSetInterest(String tellerno, String orgno, DateTime tradedate, InterBankInterestInfo info)
        {
            string outmsg = "";
            bool result = ClientSyncHelper.OnSettleInterBank(tellerno, orgno, tradedate, info, out outmsg);
            if (!result)
            {
                outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            }
            return new RegularResult(result,outmsg);
        }

        /// <summary>
        /// 同业存放计提
        /// </summary>
        /// <param name="tellerno"></param>
        /// <param name="orgno"></param>
        /// <param name="tradedate"></param>
        /// <param name="info"></param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        public static RegularResult InterBankAssetsPrepared(String tellerno, String orgno, DateTime tradedate, List<InterBankPreparedInfo> info)
        {
            string outmsg = "";
            bool result = ClientSyncHelper.OnPreparedInterBank(tellerno, orgno, tradedate, info, out outmsg);
            if (!result)
            {
                outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            }
            return new RegularResult(result, outmsg);
        } 

        /// <summary>
        /// 活期定期开户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="isCurrent">活期or定期</param>
        /// <param name="isCancel">是否撤销</param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        public static RegularResult InterBankOpenAccount(string orgID, string tellerNO, DateTime tradeDate, InterBankOpenAcctInfo info)
        {
            string outmsg = "";
            bool isOk = ClientSyncHelper.OnOpenAccount(orgID, tellerNO, tradeDate, info, out outmsg);
            if (!isOk)
            {
                outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            }

            return new RegularResult(isOk, outmsg);
        }

        /// <summary>
        /// 活期定期销户、部提
        /// </summary>
        /// <param name="info"></param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        public static RegularResult InterBankDeleteAccountOrExtract(string orgID, string tellerNO, DateTime tradeDate, InterBankDeleteAcctInfo info)
        {
            string outmsg = "";
            bool isOk = ClientSyncHelper.OnDeleteAccount(orgID, tellerNO, tradeDate, info, out outmsg);
            if (!isOk)
            {
                outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            }

            return new RegularResult(isOk, outmsg);
        }

        /// <summary>
        /// 通知单查询
        /// </summary>
        /// <param name="tradeDate">业务交易日</param>
        /// <param name="tradeOrg">交易机构号</param>
        /// <param name="tellerNo">柜员号</param>
        /// <param name="accountDate">会计日期</param>
        /// <param name="noticeNo">通知单编号</param>
        /// <param name="noticeType">通知单类型</param>
        /// <param name="isCurrent">是否是活期</param>
        /// <param name="result">查询结果</param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        public static TupleResult<RegularResult, InterBankNoticeLetterInfo> InterBankRetrieveNotice(string tradeOrg, string tellerNo, DateTime tradeDate, InterBankNoticeQueryInfo info)
        {
            InterBankNoticeLetterInfo result;
            string outmsg = "";
            result = ClientSyncHelper.OnRetrieveNoticeLetter(tradeDate, tradeOrg, tellerNo, info, out outmsg);
            if (result == null)
            {
                outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
                
                return new TupleResult<RegularResult,InterBankNoticeLetterInfo>(new RegularResult(false, outmsg), result);
            }

            return new TupleResult<RegularResult,InterBankNoticeLetterInfo>(new RegularResult(true, outmsg), result);
        }

        /// <summary>
        /// 定期同业存放自动转存
        /// </summary>
        /// <param name="tradeDate">业务交易日 10</param>
        /// <param name="tradeOrg">交易机构号 6</param>
        /// <param name="tellerNo">柜员号 7</param>
        /// <param name="account">账号</param>
        /// <param name="startDate">新起息日期</param>
        /// <param name="maturityDate">新到期日期</param>
        /// <param name="bizFlow">资金业务指令ID</param>
        /// <returns></returns>
        public static RegularResult InterBankAutoRedepo(string tradeOrg, string tellerNo, DateTime tradeDate, string account, DateTime startDate, DateTime maturityDate, string bizFlow)
        {
            string outmsg = "";
            bool isOk = ClientSyncHelper.OnInterBankAutoRedepo(tradeDate, tradeOrg, tellerNo, account, startDate, maturityDate, "", out outmsg);
            if (!isOk)
            {
                outmsg = string.Format("{0}{1}", MsgHandlerEntry.Info_Return_Core, outmsg);
            }

            return new RegularResult(isOk, outmsg);
        }

        /// <summary>
        /// 活期同业存放余额查询
        /// </summary>
        /// <param name="tradeOrg"></param>
        /// <param name="tellerNO"></param>
        /// <param name="tradeDate"></param>
        /// <param name="accountNO"></param>
        /// <returns></returns>
        public static TupleResult<RegularResult, double> InterBankRetrieveAccount(string tradeOrg, string tellerNO, DateTime tradeDate, string accountNO)
        {
            string outmsg = "";
            double balance = ClientSyncHelper.OnInterBankRetrieveBalance(tradeOrg, tellerNO, tradeDate, accountNO, out outmsg);
            if (string.IsNullOrEmpty(outmsg))
            {
                RegularResult regular = new RegularResult(true, string.Empty);
                if (double.IsNaN(balance) || double.IsInfinity(balance))
                {
                    return new TupleResult<RegularResult, double>(regular, 0.0);
                }
                else
                {
                    return new TupleResult<RegularResult, double>(regular, balance);
                }
            }
            else
            {
                RegularResult regular = new RegularResult(false, outmsg);
                return new TupleResult<RegularResult, double>(regular, 0.0);
            }
            
        }
        #endregion      
    }
}
