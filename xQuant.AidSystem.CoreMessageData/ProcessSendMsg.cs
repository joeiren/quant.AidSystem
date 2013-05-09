﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xQuant.AidSystem.BizDataModel;

namespace xQuant.AidSystem.CoreMessageData
{
    public sealed class ProcessSendMsg
    {
        private ProcessSendMsg()
        { }

        #region Common Fields
        public const String MESSAGE_ENCODE_EXCEPTION = "消息编码异常";
        public const String MESSAGE_DECODE_EXCEPTION = "消息解码异常";

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

        public static void InsertMsgList(Guid msgid, object dataref)
        {
            lock(_locker)
            {
                MsgSentList.Add(msgid, dataref);
            }
        }

        public static bool RemoveMsgList(Guid msgid)
        {
            lock (_locker)
            {
                return MsgSentList.Remove(msgid);
            }
        }
        #endregion

        #region 客户
        /// <summary>
        /// 查询客户基本信息
        /// </summary>
        /// <param name="tellerNO">操作柜员号</param>
        /// <param name="organNO">操作柜员所属机构号</param>
        /// <param name="tradeDate">业务发生时间</param>
        /// <param name="docType">证件类型</param>
        /// <param name="docNO">证件号码</param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid RetrieveCstmBaseInfo(String tellerNO, String organNO, DateTime tradeDate, String docType, String docNO, ref byte[] codemsg)
        {
            RetrieveCstmBaseInfoData retrieveData = new RetrieveCstmBaseInfoData();
            retrieveData.RQDTL.DOC_TYPE = docType;
            retrieveData.RQDTL.DOC_NO = docNO;
            retrieveData.RQhdrHandler.SYS_TXID = "300011"; // 查询,主机启动原交易码
            retrieveData.RQhdrHandler.TX_OUNO = organNO;
            retrieveData.RQhdrHandler.TEL_ID = tellerNO;
            retrieveData.RQhdrHandler.TX_MODE = "0";
            retrieveData.RQhdrHandler.TX_DTE = tradeDate.ToString("yyyy-MM-dd");
            retrieveData.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            retrieveData.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            retrieveData.RQhdrHandler.HOST_JNO = CommonDataHelper.SpaceString(11);
            try
            {
                return PreProcessReqMsg(retrieveData, ref codemsg);
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        /// <summary>
        /// 查询客户接口
        /// </summary>
        /// <param name="tellerNO">操作柜员号</param>
        /// <param name="organNO">操作柜员所属机构号</param>
        /// <param name="tradeDate">业务发生时间</param>
        /// <param name="internalID">核心客户内码</param>        
        public static Guid RetrieveCstm(String tellerNO, String organNO, DateTime tradeDate, String internalID, ref byte[] codemsg)
        {
            RetrieveCstmData retrieveData = new RetrieveCstmData();
            retrieveData.RQDTL.CUS_CDE = CommonDataHelper.FillSpecifyWidthString(internalID, RetrieveCstmRQDTL.TOTAL_WIDTH);
            retrieveData.RQhdrHandler.SYS_TXID = "300012"; // 查询,主机启动原交易码
            retrieveData.RQhdrHandler.TX_OUNO = organNO;
            retrieveData.RQhdrHandler.TEL_ID = tellerNO;
            retrieveData.RQhdrHandler.TX_MODE = "0";
            retrieveData.RQhdrHandler.TX_DTE = tradeDate.ToString("yyyy-MM-dd");
            retrieveData.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            retrieveData.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            retrieveData.RQhdrHandler.HOST_JNO = CommonDataHelper.SpaceString(11);
            try
            {
                return PreProcessReqMsg(retrieveData, ref codemsg);
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        /// <summary>
        /// 新增客户接口
        /// </summary>
        /// <param name="tellerNO">操作柜员号</param>
        /// <param name="organNO">操作柜员所属机构号</param>
        /// <param name="tradeDate">业务发生时间</param>
        /// <param name="customer">客户新增的数据对象</param>
        public static Guid AddCustomer(String tellerNO, String organNO, DateTime tradeDate, ref byte[] codemsg, CoreCustomerBrief customer)
        {
            AddCstmData addData = new AddCstmData();

            addData.RQhdrHandler.SYS_TXID = "300013"; // 新增,主机启动原交易码
            addData.RQhdrHandler.TX_OUNO = organNO;
            addData.RQhdrHandler.TEL_ID = tellerNO;
            addData.RQhdrHandler.TX_MODE = "0";
            addData.RQhdrHandler.TX_DTE = tradeDate.ToString("yyyy-MM-dd");
            addData.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            addData.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            addData.RQhdrHandler.HOST_JNO = CommonDataHelper.SpaceString(11);

            addData.RQDTL.CUS_NO = customer.CstmNO; 
            addData.RQDTL.CUS_NAM = customer.CName;
            addData.RQDTL.CUS_ONAM = customer.OName;
            addData.RQDTL.CUS_ENAM = customer.EName;
            addData.RQDTL.ADDR = customer.Address;
            addData.RQDTL.TEL_NO = customer.TeleNO;
            addData.RQDTL.MBL_NO = customer.MobileNO;
            addData.RQDTL.ZIP = customer.ZIP;

            try
            {
                return PreProcessReqMsg(addData, ref codemsg);
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        /// <summary>
        /// 更新客户接口
        /// </summary>
        /// <param name="tellerNO">操作柜员号</param>
        /// <param name="organNO">操作柜员所属机构号</param>
        /// <param name="tradeDate">业务发生时间</param>
        /// <param name="customer">客户更新的数据对象</param>
        public static Guid UpdateCustomer(String tellerNO, String organNO, DateTime tradeDate, String interCode, ref byte[] codemsg, CoreCustomerBrief customer)
        {
            UpdateCstmData updateData = new UpdateCstmData();
            updateData.RQDTL.CUS_CDE = interCode;
            updateData.RQhdrHandler.SYS_TXID = "300014"; // 查询,主机启动原交易码
            updateData.RQhdrHandler.TX_OUNO = organNO;
            updateData.RQhdrHandler.TEL_ID = tellerNO;
            updateData.RQhdrHandler.TX_MODE = "0";
            updateData.RQhdrHandler.TX_DTE = tradeDate.ToString("yyyy-MM-dd");
            updateData.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            updateData.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            updateData.RQhdrHandler.HOST_JNO = CommonDataHelper.SpaceString(11);

            updateData.RQDTL.CUS_NAM = customer.CName;
            updateData.RQDTL.CUS_ONAM = customer.OName;
            updateData.RQDTL.CUS_ENAM = customer.EName;
            updateData.RQDTL.ADDR = customer.Address;
            updateData.RQDTL.TEL_NO = customer.TeleNO;
            updateData.RQDTL.MBL_NO = customer.MobileNO;
            updateData.RQDTL.IsDelete = false;
            try
            {
                return PreProcessReqMsg(updateData, ref codemsg);
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        /// <summary>
        /// 删除客户接口
        /// </summary>
        /// <param name="tellerNO">操作柜员号</param>
        /// <param name="organNO">操作柜员所属机构号</param>
        /// <param name="tradeDate">业务发生时间</param>
        /// <param name="tradeDate">客户内码</param>
        /// <param name="tradeDate">编码缓冲</param>        
        public static Guid DeleteCustomer(String tellerNO, String organNO, DateTime tradeDate, String interCode, ref byte[] codemsg)
        {
            UpdateCstmData updateData = new UpdateCstmData();
            updateData.RQDTL.CUS_CDE = interCode;
            updateData.RQhdrHandler.SYS_TXID = "300014"; // 查询,主机启动原交易码
            updateData.RQhdrHandler.TX_OUNO = organNO;
            updateData.RQhdrHandler.TEL_ID = tellerNO;
            updateData.RQhdrHandler.TX_MODE = "0";
            updateData.RQhdrHandler.TX_DTE = tradeDate.ToString("yyyy-MM-dd");
            updateData.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            updateData.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            updateData.RQhdrHandler.HOST_JNO = CommonDataHelper.SpaceString(11);

            //updateData.RQDTL.CUS_NAM = customer.CName;
            //updateData.RQDTL.CUS_ONAM = customer.OName;
            //updateData.RQDTL.CUS_ENAM = customer.EName;
            //updateData.RQDTL.ADDR = customer.Address;
            //updateData.RQDTL.TEL_NO = customer.TeleNO;
            //updateData.RQDTL.MBL_NO = customer.MobileNO;
            updateData.RQDTL.IsDelete = true;
            try
            {
                return PreProcessReqMsg(updateData, ref codemsg);
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }
        #endregion

        #region 柜员
        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="tellerLogin"></param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid EncryptPassword(String orgNO, String tellerNO, String password, ref byte[] codemsg)
        {
            EncryptTellerAuth tellerLogin = new EncryptTellerAuth(tellerNO, orgNO, DateTime.Now, password);
            tellerLogin.AfterEncrypt = false;
            try
            {
                return PreProcessEncryptMsg(tellerLogin, ref codemsg);
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        public static Guid EncryptPassword(EncryptTellerAuth encryptTeller, ref byte[] codemsg)
        {
            return PreProcessEncryptMsg(encryptTeller, ref codemsg);
        }

        /// <summary>
        /// 机构柜员认证
        /// </summary>
        /// <param name="tellerLogin"></param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid AuthTeller(EncryptTellerAuth tellerLogin,  ref byte[] codemsg)
        {         
            tellerLogin.TellerData.RQhdrHandler.SYS_TXID = "106330"; // 认证,主机启动原交易码
            tellerLogin.TellerData.RQhdrHandler.TX_MODE = "0";

            tellerLogin.TellerData.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            tellerLogin.TellerData.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            tellerLogin.TellerData.RQhdrHandler.HOST_JNO = CommonDataHelper.SpaceString(11);

            tellerLogin.TellerData.RQDTL.PIN_BLK = tellerLogin.TellerEncrypt.RespPin;
            tellerLogin.TellerData.RQDTL.SignType = "1"; //密码方式
            tellerLogin.AfterEncrypt = true;
            return PreProcessReqMsg(tellerLogin, ref codemsg);
        }

        /// <summary>
        /// 柜员认证
        /// </summary>
        /// <param name="orgNO">机构号</param>
        /// <param name="tellerNO">柜员号</param>
        /// <param name="password">密码</param>
        /// <param name="encryptPwd">加密后密码</param>
        /// <param name="codemsg">编码串</param>
        /// <returns></returns>
        public static Guid AuthTeller(String orgNO, String tellerNO, string password, byte[] encryptPwd, ref byte[] codemsg)
        {
            EncryptTellerAuth tellerLogin = new EncryptTellerAuth(tellerNO, orgNO, DateTime.Now, password);
            tellerLogin.TellerData.RQhdrHandler.SYS_TXID = "106330"; // 认证,主机启动原交易码
            tellerLogin.TellerData.RQhdrHandler.TX_MODE = "0";

            tellerLogin.TellerData.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            tellerLogin.TellerData.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            tellerLogin.TellerData.RQhdrHandler.HOST_JNO = CommonDataHelper.SpaceString(11);

            tellerLogin.TellerEncrypt.RespPin = encryptPwd;
            tellerLogin.TellerData.RQDTL.SignType = "1";
            tellerLogin.TellerData.RQDTL.PIN_BLK = tellerLogin.TellerEncrypt.RespPin;
            tellerLogin.AfterEncrypt = true;
            try
            {
                return PreProcessReqMsg(tellerLogin, ref codemsg);
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }

        }
        #endregion

        #region 会计分录，记账，抹账
        /// <summary>
        /// 记账接口
        /// </summary>
        /// <param name="tellerNO">复核入账柜员号</param>
        /// <param name="organNO">复核入账柜员所属机构</param>
        /// <param name="tradeDate">业务发生时间</param>
        /// <param name="flowNO">资金业务系统流水号</param>
        /// <param name="list">会计分录列表数据对象</param>
        public static Guid AccountingRecord(String tellerNO, String organNO, DateTime tradeDate, String flowNO, List<CoreBillRecord> list, ref byte[] codemsg)
        {
            AcctRecordData recordData = new AcctRecordData();
            
            recordData.RQhdrHandler.SYS_TXID = "203300"; // 会计记账,主机启动原交易码
            recordData.RQhdrHandler.TX_OUNO = organNO;
            recordData.RQhdrHandler.TEL_ID = tellerNO;
            recordData.RQhdrHandler.TX_MODE = "0";
            recordData.RQhdrHandler.TX_DTE = tradeDate.ToString("yyyy-MM-dd");
            recordData.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            recordData.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            recordData.RQhdrHandler.HOST_JNO = CommonDataHelper.SpaceString(11);

            recordData.RQDTL.BGR33CHPG1 = (UInt16)list.Count;
            int i =1;
            List<AccountingEliminate> eliminateList = new List<AccountingEliminate>();
            foreach (CoreBillRecord bill in list)
            {
                AccountingEntry entry = new AccountingEntry();
                //套内序号
                entry.BGR33SN021 = i.ToString();
                //交易账号
                //entry.BGR33AC201 = bill.TradeAcct;
                //机构号
                entry.BGR33BRNO1 = bill.OrgNO;
                //币种
                entry.BGR33CCYC1 = bill.Currency;
                //科目
                entry.BGR33ACID1 = bill.Subject;
                //内部账顺序号
                entry.BGR33SN041 = bill.InterAcctSN;
                //生成交易账号
                if (string.IsNullOrEmpty(entry.BGR33AC201))
                {
                    string tradeacct = "";
                    string currency = "01";
                    if (bill.Currency == "CNY")
                    {
                        currency = "01";
                    }
                    if (BizDataHelper.GenerateInnerAcctNO(bill.OrgNO, currency, bill.Subject, bill.InterAcctSN, out tradeacct))
                    {
                        entry.BGR33AC201 = tradeacct;
                    }
                }
                // 红蓝字
                entry.BGR33RDBL1 = bill.RedBlueFlag;
                //借贷标志
                entry.BGR33CDFG1 = bill.Opt;
                //发生额
                entry.BGR33AMT1 = CommonDataHelper.FillSpecifyWidthString(bill.TradeMoney, 17);//CommonDataHelper.ConvertDecimal(bill.TradeMoney, 2);
                //对方账号
                entry.BGR33AC321 = bill.OptAcct;
                //挂销账标志
                entry.BGR33WRFG1 = bill.IsEliminateFlag;
                if (bill.IsEliminateFlag == "1" || bill.IsEliminateFlag == "2")
                {
                    AccountingEliminate eliminate = new AccountingEliminate();
                    eliminate.BGR33SN022 = i.ToString();
                    //挂账序号
                    eliminate.BGR33SQNO1 = bill.PendingAcctSN;
                    //挂账对方账号
                    eliminate.BGR33AC322 = entry.BGR33AC321;
                    eliminate.BGR33FLNM3 = CommonDataHelper.SpaceString(80);
                    eliminate.BGR33AMT2 = CommonDataHelper.FillSpecifyWidthString(bill.TradeMoney, 17);
                    recordData.RQDTL.EliminateList.Add(eliminate);
                }

                recordData.RQDTL.EntryList.Add(entry);
                i++;
            }
            recordData.RQDTL.BGR33NO181 = flowNO;//CommonDataHelper.PadLeft4BizFlowNO(flowNO, '0', 18);
            recordData.RQDTL.BGR33CHPG2 = (UInt16)recordData.RQDTL.EliminateList.Count;
            try
            {
                return PreProcessReqMsg(recordData, ref codemsg);
            }
            catch (BizArgumentsException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }
        
        /// <summary>
        /// 内部账记账交易接口 ，原交易码203400（多借一贷,支持客户账）
        /// </summary>
        /// <param name="tellerNO">复核入账柜员号</param>
        /// <param name="organNO">复核入账柜员所属机构</param>
        /// <param name="tradeDate">业务发生时间</param>
        /// <param name="flowNO">资金业务系统流水号</param>
        /// <param name="list">会计分录列表数据对象</param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid AcctRecordMultiLendOneLoan(String tellerNO, String organNO, DateTime tradeDate, String flowNO, List<CoreBillRecord> list, ref byte[] codemsg)
        {
            AcctRecordMulti2OneData recordData = new AcctRecordMulti2OneData();

            recordData.RQhdrHandler.SYS_TXID = "203400"; // 会计记账,主机启动原交易码
            recordData.RQhdrHandler.TX_OUNO = organNO;
            recordData.RQhdrHandler.TEL_ID = tellerNO;
            recordData.RQhdrHandler.TX_MODE = "0";
            recordData.RQhdrHandler.TX_DTE = tradeDate.ToString("yyyy-MM-dd");
            recordData.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            recordData.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            recordData.RQhdrHandler.HOST_JNO = CommonDataHelper.SpaceString(11);

            recordData.RQDTL.BGR33CHPG1 = (UInt16)list.Count;
            int i = 1;

            string loanAcct = "";
            List<AccountingEliminateMulti2One> eliminateList = new List<AccountingEliminateMulti2One>();
            foreach (CoreBillRecord bill in list)
            {
                AccountingEntryMulti2One entry = new AccountingEntryMulti2One();                
                //套内序号
                entry.BGR33SN021 = i.ToString();
                //交易账号（客户账号）
                entry.BGR33AC201 = bill.TradeAcctNO;
                //机构号
                entry.BGR33BRNO1 = bill.OrgNO;
                //币种
                entry.BGR33CCYC1 = bill.Currency;
                //科目
                entry.BGR33ACID1 = bill.Subject;
                //内部账顺序号
                entry.BGR33SN041 = bill.InterAcctSN;
                //钞汇属性
                entry.BGR34CHSX1 = bill.IsNote;
                
                //生成交易账号
                if (string.IsNullOrEmpty(entry.BGR33AC201))
                {
                    string tradeacct = "";
                    string currency = "01";
                    if (bill.Currency == "CNY")
                    {
                        currency = "01";
                    }
                    if (BizDataHelper.GenerateInnerAcctNO(bill.OrgNO, currency, bill.Subject, bill.InterAcctSN, out tradeacct))
                    {
                        entry.BGR33AC201 = tradeacct;
                    }
                }
                //借贷标志
                entry.BGR33CDFG1 = bill.Opt;
                if (bill.Opt == "2") // 如果是贷
                {
                    loanAcct = entry.BGR33AC201;
                }
                // 红蓝字“1”正常
                entry.BGR33RDBL1 = "1";
                //发生额
                entry.BGR33AMT1 = CommonDataHelper.FillSpecifyWidthString(bill.TradeMoney, 17);//CommonDataHelper.ConvertDecimal(bill.TradeMoney, 2);
                //对方账号
                entry.BGR33AC321 = bill.OptAcct;
                //挂销账标志
                entry.BGR33WRFG1 = bill.IsEliminateFlag;
                if (bill.IsEliminateFlag == "1" || bill.IsEliminateFlag == "2")
                {
                    AccountingEliminateMulti2One eliminate = new AccountingEliminateMulti2One();
                    eliminate.BGR33SN022 = i.ToString();
                    //挂账序号
                    eliminate.BGR33SQNO1 = bill.PendingAcctSN;
                    //挂账对方账号
                    eliminate.BGR33AC322 = entry.BGR33AC321;
                    eliminate.BGR33FLNM3 = CommonDataHelper.SpaceString(80);
                    eliminate.BGR33AMT2 = CommonDataHelper.FillSpecifyWidthString(bill.TradeMoney, 17);
                    recordData.RQDTL.EliminateList.Add(eliminate);
                }

                recordData.RQDTL.EntryList.Add(entry);
                i++;
            }

            foreach (AccountingEntryMulti2One entry in recordData.RQDTL.EntryList)
            {
                if (entry.BGR33CDFG1 == "1")
                {
                    entry.BGR33AC321 = loanAcct;
                }
            }
            recordData.RQDTL.BGR33NO181 = flowNO;//CommonDataHelper.PadLeft4BizFlowNO(flowNO, '0', 18);
            recordData.RQDTL.BGR33CHPG2 = (UInt16)recordData.RQDTL.EliminateList.Count;
            try
            {
                return PreProcessReqMsg(recordData, ref codemsg);
            }
            catch (BizArgumentsException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        /// <summary>
        /// 内部账记账交易接口 ，原交易码203500（多贷一借，支持客户账）
        /// </summary>
        /// <param name="tellerNO">复核入账柜员号</param>
        /// <param name="organNO">复核入账柜员所属机构</param>
        /// <param name="tradeDate">业务发生时间</param>
        /// <param name="flowNO">资金业务系统流水号</param>
        /// <param name="list">会计分录列表数据对象</param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid AcctRecordMultiLoanOneLend(String tellerNO, String organNO, DateTime tradeDate, String flowNO, List<CoreBillRecord> list, ref byte[] codemsg)
        {
            AcctRecordMulti2OneData recordData = new AcctRecordMulti2OneData();

            recordData.RQhdrHandler.SYS_TXID = "203500"; // 会计记账,主机启动原交易码
            recordData.RQhdrHandler.TX_OUNO = organNO;
            recordData.RQhdrHandler.TEL_ID = tellerNO;
            recordData.RQhdrHandler.TX_MODE = "0";
            recordData.RQhdrHandler.TX_DTE = tradeDate.ToString("yyyy-MM-dd");
            recordData.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            recordData.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            recordData.RQhdrHandler.HOST_JNO = CommonDataHelper.SpaceString(11);

            recordData.RQDTL.BGR33CHPG1 = (UInt16)list.Count;
            int i = 1;
            string lendAcct = "";
            List<AccountingEliminateMulti2One> eliminateList = new List<AccountingEliminateMulti2One>();
            foreach (CoreBillRecord bill in list)
            {
                AccountingEntryMulti2One entry = new AccountingEntryMulti2One();
                //套内序号
                entry.BGR33SN021 = i.ToString();
                //交易账号
                entry.BGR33AC201 = bill.TradeAcctNO;
                //机构号
                entry.BGR33BRNO1 = bill.OrgNO;
                //币种
                entry.BGR33CCYC1 = bill.Currency;
                //科目
                entry.BGR33ACID1 = bill.Subject;
                //内部账顺序号
                entry.BGR33SN041 = bill.InterAcctSN;
                //生成交易账号
                if (string.IsNullOrEmpty(entry.BGR33AC201))
                {
                    string tradeacct = "";
                    string currency = "01";
                    if (bill.Currency == "CNY")
                    {
                        currency = "01";
                    }
                    if (BizDataHelper.GenerateInnerAcctNO(bill.OrgNO, currency, bill.Subject, bill.InterAcctSN, out tradeacct))
                    {
                        entry.BGR33AC201 = tradeacct;
                    }
                }
                // 红蓝字“1”正常
                entry.BGR33RDBL1 = "1";
                //借贷标志
                entry.BGR33CDFG1 = bill.Opt;
                if (bill.Opt == "1") // 如果是借
                {
                    lendAcct = entry.BGR33AC201;
                    //entry.BGR33RDBL1 = "2";  //红字
                }
                //发生额
                entry.BGR33AMT1 = CommonDataHelper.FillSpecifyWidthString(bill.TradeMoney, 17);//CommonDataHelper.ConvertDecimal(bill.TradeMoney, 2);
                //对方账号
                entry.BGR33AC321 = bill.OptAcct;
                //挂销账标志
                entry.BGR33WRFG1 = bill.IsEliminateFlag;
                if (bill.IsEliminateFlag == "1" || bill.IsEliminateFlag == "2")
                {
                    AccountingEliminateMulti2One eliminate = new AccountingEliminateMulti2One();
                    eliminate.BGR33SN022 = i.ToString();
                    //挂账序号
                    eliminate.BGR33SQNO1 = bill.PendingAcctSN;
                    //挂账对方账号
                    eliminate.BGR33AC322 = entry.BGR33AC321;
                    eliminate.BGR33FLNM3 = CommonDataHelper.SpaceString(80);
                    eliminate.BGR33AMT2 = CommonDataHelper.FillSpecifyWidthString(bill.TradeMoney, 17);
                    recordData.RQDTL.EliminateList.Add(eliminate);
                }

                recordData.RQDTL.EntryList.Add(entry);
                i++;
            }
            foreach (AccountingEntryMulti2One entry in recordData.RQDTL.EntryList)
            {
                if (entry.BGR33CDFG1.Trim() == "2") // 所有贷方的对方账号
                {
                    entry.BGR33AC321 = lendAcct;
                }
            }
            recordData.RQDTL.BGR33NO181 = flowNO;//CommonDataHelper.PadLeft4BizFlowNO(flowNO, '0', 18);
            recordData.RQDTL.BGR33CHPG2 = (UInt16)recordData.RQDTL.EliminateList.Count;

            try
            {
                return PreProcessReqMsg(recordData, ref codemsg);
            }
            catch (BizArgumentsException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        /// <summary>
        /// 记账查询
        /// </summary>
        /// <param name="tellerNO"></param>
        /// <param name="organNO"></param>
        /// <param name="tradeDate"></param>
        /// <param name="coreTradeSN">核心交易流水号</param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid RetrieveAccounting(String tellerNO, String organNO, DateTime tradeDate, String coreTradeSN, ref byte[] codemsg)
        {
            AcctRetrieveData retrieveData = new AcctRetrieveData();

            retrieveData.RQhdrHandler.SYS_TXID = "203080"; // 会计记账,主机启动原交易码
            retrieveData.RQhdrHandler.TX_OUNO = organNO;
            retrieveData.RQhdrHandler.TEL_ID = tellerNO;
            retrieveData.RQhdrHandler.TX_MODE = "0";
            retrieveData.RQhdrHandler.TX_DTE = tradeDate.ToString("yyyy-MM-dd");
            retrieveData.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            retrieveData.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            retrieveData.RQhdrHandler.HOST_JNO = CommonDataHelper.SpaceString(11);

            retrieveData.RQDTL.CoreTradeSN = coreTradeSN;
            try
            {
                return PreProcessReqMsg(retrieveData, ref codemsg);
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        /// <summary>
        /// 抹账
        /// </summary>
        /// <param name="tellerNO"></param>
        /// <param name="organNO"></param>
        /// <param name="tradeDate"></param>
        /// <param name="coreTradeSN">需要抹账的核心交易流水号</param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid EraseAccounting(String tellerNO, String organNO, DateTime tradeDate, String coreTradeSN, ref byte[] codemsg)
        {
            AcctEraseData eraseData = new AcctEraseData();

            eraseData.RQhdrHandler.SYS_TXID = "990009"; // 会计记账,主机启动原交易码
            eraseData.RQhdrHandler.TX_OUNO = organNO;
            eraseData.RQhdrHandler.TEL_ID = tellerNO;
            eraseData.RQhdrHandler.TX_MODE = "2";     //2 - 正常抹账交易 (反交易)
            eraseData.RQhdrHandler.TX_DTE = tradeDate.ToString("yyyy-MM-dd");
            eraseData.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            eraseData.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            eraseData.RQhdrHandler.HOST_JNO = coreTradeSN;
            try
            {
                return PreProcessReqMsg(eraseData, ref codemsg);
            }
            catch (BizArgumentsException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        /// <summary>
        /// 对账信息查询
        /// </summary>
        /// <param name="tellerNO">柜员号</param>
        /// <param name="orgNO">机构号</param>
        /// <param name="tradeDate">发起交易日期</param>
        /// <param name="queryDate">查询日期</param>
        /// <param name="flowNO">查询的资金流水号</param>
        /// <param name="queryOrgNO">查询机构号</param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid AccountingCheck(String tellerNO, string orgNO, DateTime tradeDate, DateTime queryDate, String flowNO, String queryOrgNO, ref byte[] codemsg)
        {
            AcctCheckData checkData = new AcctCheckData();

            checkData.RQhdrHandler.SYS_TXID = "206070"; //
            checkData.RQhdrHandler.TX_OUNO = orgNO;
            checkData.RQhdrHandler.TEL_ID = tellerNO;
            checkData.RQhdrHandler.TX_MODE = "0";
            checkData.RQhdrHandler.TX_DTE = tradeDate.ToString("yyyy-MM-dd");
            checkData.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            checkData.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            checkData.RQhdrHandler.HOST_JNO = CommonDataHelper.SpaceString(11);

            checkData.RQDTL.BizFlowNO = flowNO;
            //if (string.IsNullOrEmpty(queryOrgNO))
            //{
            //    queryOrgNO = orgNO;
            //}
            checkData.RQDTL.OrgNO = queryOrgNO;
            checkData.RQDTL.TradeDate = queryDate.ToString("yyyyMMdd");

            try
            {
                return PreProcessReqMsg(checkData, ref codemsg);
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }
        #endregion

        #region 实时资金头寸查询
        /// <summary>
        /// 内部账查询
        /// </summary>
        /// <param name="tellerNO">操作柜员号</param>
        /// <param name="organNO">交易机构号</param>
        /// <param name="tradeDate">业务交易日</param>
        /// <param name="condition">查询条件数据对象</param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid RetrieveInternalAcct(String tellerNO, String organNO, DateTime tradeDate, InternalAcctCondtion condition, ref byte[] codemsg)
        {
            InterAcctRetrieveData interdata = new InterAcctRetrieveData();

            interdata.RQhdrHandler.SYS_TXID = "230110"; // 会计记账,主机启动原交易码
            interdata.RQhdrHandler.TX_OUNO = organNO;
            interdata.RQhdrHandler.TEL_ID = tellerNO;
            interdata.RQhdrHandler.TX_MODE = "0";
            interdata.RQhdrHandler.TX_DTE = tradeDate.ToString("yyyy-MM-dd");
            interdata.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            interdata.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            interdata.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            interdata.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            interdata.RQhdrHandler.HOST_JNO = CommonDataHelper.SpaceString(11);

            interdata.RQDTL.AccountNO = condition.AccountNO;
            interdata.RQDTL.Currcency = condition.Currcency;
            interdata.RQDTL.OrgnaztionNO = condition.OrgnaztionNO;
            interdata.RQDTL.SequenceNO = condition.SequenceNO;
            interdata.RQDTL.BalanceQueryType = condition.BalanceQueryType;
            interdata.RQDTL.CheckCode = condition.CheckCode;
            try
            {
                return PreProcessReqMsg(interdata, ref codemsg);
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        /// <summary>
        /// 存放省内上级机构活期款项查询
        /// </summary>
        /// <param name="tellerNO"></param>
        /// <param name="organNO"></param>
        /// <param name="tradeDate"></param>
        /// <param name="currcency"></param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid RetrieveSuperiorCurrentAcct(String tellerNO, String organNO, DateTime tradeDate, String currcency, ref byte[] codemsg)
        {
            SuperiorCurrentAcctData acctdata = new SuperiorCurrentAcctData();

            acctdata.RQhdrHandler.SYS_TXID = "205060"; // 会计记账,主机启动原交易码
            acctdata.RQhdrHandler.TX_OUNO = organNO;
            acctdata.RQhdrHandler.TEL_ID = tellerNO;
            acctdata.RQhdrHandler.TX_MODE = "0";
            acctdata.RQhdrHandler.TX_DTE = tradeDate.ToString("yyyy-MM-dd");
            acctdata.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            acctdata.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            acctdata.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            acctdata.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            acctdata.RQhdrHandler.HOST_JNO = CommonDataHelper.SpaceString(11);

            acctdata.RQDTL.Currcency = currcency;
           
            try
            {
                return PreProcessReqMsg(acctdata, ref codemsg);
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        /// <summary>
        /// 县级行社上存清算资金查询
        /// </summary>
        /// <param name="tellerNO"></param>
        /// <param name="organNO"></param>
        /// <param name="tradeDate"></param>
        /// <param name="currcency"></param>
        /// <param name="option"></param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid RetrieveDepositClearingFund(String tellerNO, String organNO, DateTime tradeDate, String currcency, String option, ref byte[] codemsg)
        {
            DepositClearingFundData acctdata = new DepositClearingFundData();

            acctdata.RQhdrHandler.SYS_TXID = "205050"; // 会计记账,主机启动原交易码
            acctdata.RQhdrHandler.TX_OUNO = organNO;
            acctdata.RQhdrHandler.TEL_ID = tellerNO;
            acctdata.RQhdrHandler.TX_MODE = "0";
            acctdata.RQhdrHandler.TX_DTE = tradeDate.ToString("yyyy-MM-dd");
            acctdata.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            acctdata.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            acctdata.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            acctdata.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            acctdata.RQhdrHandler.HOST_JNO = CommonDataHelper.SpaceString(11);

            acctdata.RQDTL.Currency = currcency;
            acctdata.RQDTL.DistrictNO = "";
            acctdata.RQDTL.OrgNO = "";
            acctdata.RQDTL.Option = option;

            try
            {
                return PreProcessReqMsg(acctdata, ref codemsg);
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        /// <summary>
        /// 资金业务账号余额查询
        /// </summary>
        /// <param name="tellerNO"></param>
        /// <param name="organNO"></param>
        /// <param name="tradeDate"></param>
        /// <param name="inputlist"></param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid RetrieveAcctCrntBalance(String tellerNO, String organNO, DateTime tradeDate, List<CoreAcctCrntBalance> inputlist, ref byte[] codemsg)
        {
            RetrieveAcctCrntBalanceData acctdata = new RetrieveAcctCrntBalanceData();

            acctdata.RQhdrHandler.SYS_TXID = "206060"; // 会计记账,主机启动原交易码
            acctdata.RQhdrHandler.TX_OUNO = organNO;
            acctdata.RQhdrHandler.TEL_ID = tellerNO;
            acctdata.RQhdrHandler.TX_MODE = "0";
            acctdata.RQhdrHandler.TX_DTE = tradeDate.ToString("yyyy-MM-dd");
            acctdata.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            acctdata.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            acctdata.RQhdrHandler.SRV_JNO = CommonDataHelper.SpaceString(12);
            acctdata.RQhdrHandler.SRV_REV_JNO = CommonDataHelper.SpaceString(12);
            acctdata.RQhdrHandler.HOST_JNO = CommonDataHelper.SpaceString(11);

            if (inputlist == null)
            {
                acctdata.RQDTL.AcctList = new List<AcctCrntBalanceRQDTLItem>();
                acctdata.RQDTL.AcctCount = 0;
            }
            else
            {
                inputlist = inputlist.Take(30).ToList();
                foreach (var item in inputlist)
                {
                    AcctCrntBalanceRQDTLItem rqdtlitem = new AcctCrntBalanceRQDTLItem();
                    rqdtlitem.AcctNO = item.AcctNO;
                    rqdtlitem.AcctProperty = item.AcctProperty;
                    rqdtlitem.Currency = item.Currency;
                    acctdata.RQDTL.AcctList.Add(rqdtlitem);
                }
                acctdata.RQDTL.AcctCount = inputlist.Count;
            }
            try
            {
                return PreProcessReqMsg(acctdata, ref codemsg);
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }
        #endregion

        #region 支付平台接口
        /// <summary>
        /// 登记簿查询
        /// </summary>
        /// <param name="tellerNO">柜员号</param>
        /// <param name="organNO">交易机构号</param>
        /// /// <param name="queryOrgNO">查询机构号</param>
        /// <param name="flowNO">资金流水号</param>
        /// <param name="orignalDate">原委托日期</param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid PayRegisterRetrieved(String tellerNO, String orgNO, String flowNO, DateTime orignalDate, ref byte[] codemsg)
        {
            PayRegisterRetrievedData retrieveData = new PayRegisterRetrievedData();
            retrieveData.RQData.PayBank = orgNO;
            retrieveData.RQData.TradTeller = tellerNO;
            retrieveData.RQData.OriDelegateDate = orignalDate.ToString("yyyyMMdd");
            retrieveData.RQData.TransferFlowNo = flowNO;//CommonDataHelper.PadLeft4BizFlowNO(flowNO, '0', 18);
            try
            {
               return PreProcessPaymentMsg(retrieveData, ref codemsg);
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        /// <summary>
        /// 资金划拨
        /// </summary>
        /// <param name="tellerNO">柜员号</param>
        /// <param name="orgNO">机构号</param>
        /// <param name="tradeDate">交易日期</param>
        /// <param name="inputData">资金划拨输入数据对象</param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid PayFundTransfer(String tellerNO, String orgNO, DateTime tradeDate, PayFundTransfer inputData, ref byte[] codemsg)
        {
            PayFundTransferData data = new PayFundTransferData();
            data.RQData.Teller = tellerNO;
            data.RQData.TransferFlowNo = inputData.TransferFlowNo;// CommonDataHelper.PadLeft4BizFlowNO(inputData.TransferFlowNo, '0', 18);
            data.RQData.PayBank = orgNO;
            data.RQData.TranDate = tradeDate.ToString("yyyyMMdd");
            data.RQData.PayAccount = inputData.PayAccount;
            data.RQData.PayAccountName = inputData.PayAccountName;
            if (string.IsNullOrEmpty(inputData.RecAccount))
            {
                inputData.RecAccount = "0";
            }
            data.RQData.RecAccount = inputData.RecAccount;
            data.RQData.RecAccountName = inputData.RecAccountName;
            data.RQData.RecAccountBanks = inputData.RecAccountBanks;
            data.RQData.PackageChannelType = inputData.PackageChannelType;
            data.RQData.CurrencyType = inputData.CurrencyType;
            data.RQData.PayAmount = inputData.PayAmount;
            data.RQData.BizType = inputData.BizType;
            data.RQData.Fee = inputData.Fee;
            data.RQData.Remark = inputData.Remark;            
            data.RQData.ChannelId = inputData.ChannelId;
            data.RQData.PendingSN = inputData.PendingSN;
          
            try
            {
                return PreProcessPaymentMsg(data, ref codemsg);
            }
            catch (BizArgumentsException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        /// <summary>
        /// 往帐抹账交易
        /// </summary>
        /// <param name="tellerNO">柜员号</param>
        /// <param name="orgNO">机构号</param>
        /// <param name="flowNO">资金业务流水号</param>
        /// <param name="orignalDate">原委托日期</param>
        /// <param name="hostFlowNO">主机流水号（同业拆借或资金划拨返回的核心交易流水号）</param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid PayEraseAccounting(String tellerNO, String orgNO, String flowNO, DateTime orignalDate, String hostFlowNO, ref byte[] codemsg)
        {
            PayOutcomeAcctEraseData data = new PayOutcomeAcctEraseData();
            data.RQData.PayBank = orgNO;
            data.RQData.Operator = tellerNO;
            data.RQData.OriDelegateDate = orignalDate.ToString("yyyyMMdd");
            data.RQData.TransferFlowNo = flowNO;// CommonDataHelper.PadLeft4BizFlowNO(flowNO, '0', 18);
            data.RQData.HostTranFlowNo = hostFlowNO;
          
            try
            {
                return PreProcessPaymentMsg(data, ref codemsg);
            }
            catch (BizArgumentsException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }            
        }
        
        /// <summary>
        /// 来账的抹账交易
        /// </summary>
        /// <param name="tellerNO"></param>
        /// <param name="orgNO"></param>
        /// <param name="paySN">支付交易序号</param>
        /// <param name="srcBankNO">发起行行号</param>
        /// <param name="orignalDate"></param>
        /// <param name="hostFlowNO">主机流水号</param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid PayIncomeEraseAccounting(String tellerNO, String orgNO, String paySN, String srcBankNO, DateTime orignalDate, String channelType, String hostFlowNO, ref byte[] codemsg)
        {
            PayIncomeAcctEraseData data = new PayIncomeAcctEraseData();
            data.RQData.SrcBankNO = srcBankNO;
            data.RQData.PayTransSN = paySN;
            data.RQData.OriDelegateDate = orignalDate.ToString("yyyyMMdd");
            data.RQData.HostTranFlowNo = hostFlowNO;
            data.RQData.ChangelType = channelType;
            
            try
            {
                return PreProcessPaymentMsg(data, ref codemsg);
            }
            catch (BizArgumentsException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }
        /// <summary>
        /// 来账的销账
        /// </summary>
        /// <param name="tellerNO">柜员号</param>
        /// <param name="orgNO">机构号</param>
        /// <param name="delegateDate">委托日期</param>
        /// <param name="inputData">来账销账的输入数据对象</param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid PayVostroAccountEliminate(String tellerNO, String orgNO, string delegatedate, PayVostroAcctEliminate inputData, ref byte[] codemsg)
        {
            PayVostroAcctEliminateData data = new PayVostroAcctEliminateData();
            data.RQData.PayBank = orgNO;
            data.RQData.Operator = tellerNO;
            data.RQData.PackageChannelType = inputData.PackageChannelType;
            data.RQData.BizType = inputData.BizType;
            data.RQData.TransSeq = inputData.TransSeq;
            data.RQData.AccountBanks = inputData.AccountBanks;
            data.RQData.DelegateDate = delegatedate;//delegateDate.ToString("yyyyMMdd");
            data.RQData.FundDest = inputData.FundDest;
            data.RQData.Amount = inputData.Amount;
            data.RQData.PostAcount = inputData.PostAcount;
            data.RQData.PostAccountName = inputData.PostAccountName;
            data.RQData.PostBank = inputData.PostBank;
            data.RQData.PostBankName = inputData.PostBankName;
            
            try
            {
                return PreProcessPaymentMsg(data, ref codemsg);
            }
            catch (BizArgumentsException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        /// <summary>
        /// 同业拆借
        /// </summary>
        /// <param name="tellerNO">柜员号</param>
        /// <param name="orgNO">机构号</param>
        /// <param name="tradeDate">交易日期</param>
        /// <param name="inputData">同业拆借的输入数据对象</param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid PayInterBankBiz(String tellerNO, String orgNO, DateTime tradeDate, PayInterBank inputData, ref byte[] codemsg)
        {
            PayInterBankData data = new PayInterBankData();
            data.RQData.PayBank = orgNO;
            data.RQData.Teller = tellerNO;
            data.RQData.TransferFlowNo = inputData.TransferFlowNo;//CommonDataHelper.PadLeft4BizFlowNO(inputData.TransferFlowNo, '0', 18);
            data.RQData.TranDate = tradeDate.ToString("yyyyMMdd");
            data.RQData.PayAccount = inputData.PayAccount;
            data.RQData.PayAccountName = inputData.PayAccountName;
            data.RQData.OnAccountSN = inputData.OnAccountSN;
            data.RQData.RecAccount = inputData.RecAccount;
            data.RQData.RecAccountName = inputData.RecAccountName;
            data.RQData.RecAccountBanks = inputData.RecAccountBanks;
            data.RQData.PackageChannelType = inputData.PackageChannelType;
            data.RQData.CurrencyType = inputData.CurrencyType;
            data.RQData.PayAmount = inputData.PayAmount;
            data.RQData.Rate = inputData.Rate;
            data.RQData.TimeLimit = inputData.TimeLimit;
            data.RQData.BizType = inputData.BizType;
            data.RQData.Fee = inputData.Fee;
            data.RQData.Remark = inputData.Remark;
            data.RQData.AuthTeller = inputData.AuthTeller;
            data.RQData.ChannelId = inputData.ChannelId;
            try
            {
                return PreProcessPaymentMsg(data, ref codemsg);
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        /// <summary>
        /// 支付对账查询
        /// </summary>
        /// <param name="queryDate">记账日期</param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid PayCheckAccount(DateTime queryDate, ref byte[] codemsg)
        {
            PayAcctCheckData data = new PayAcctCheckData();
            data.RQData.RequestDate = queryDate.ToString("yyyyMMdd");
         
            try
            {
                return PreProcessPaymentMsg(data, ref codemsg);
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        public static Guid PayRetrieveBanksInfo(ref byte[] codemsg)
        {
            PayBankInfoData data = new PayBankInfoData();
            try
            {
                return PreProcessPaymentMsg(data, ref codemsg);
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }

        #endregion

        #region 指纹处理
        public static Guid FingerMarksValidation(String tellerNO, String orgNO, DateTime orignalDate, String markType, String markInfo, ref byte[] codemsg)
        {
            FingerMarkData reqdata = new FingerMarkData();
            reqdata.ReqData.TradeDate = orignalDate.ToString("yyyy-MM-dd");
            reqdata.ReqData.TradeTime = orignalDate.ToString("HH:mm:ss");
            reqdata.ReqData.UnionNO = orgNO.Substring(0, 3);
            reqdata.ReqData.TellerNO = tellerNO;
            reqdata.ReqData.RespCount = "0";
            reqdata.ReqData.FileCount = "0";
            reqdata.ReqData.AuthFlag = "0";

            FingerMarkReqSubMsg submsg = new FingerMarkReqSubMsg();
            submsg.OperateType = "1";
            submsg.DeviceType = markType;
            submsg.InstNO = orgNO.Substring(0, 3);
            submsg.TellerNO = tellerNO;
            submsg.MarkInfo = markInfo;
            reqdata.ReqData.MarkData = submsg.ToString();

            try
            {
                return PreProcessFingerMarksMsg(reqdata, ref codemsg);
            }
            catch (Exception ex)
            {
                throw new Exception(MESSAGE_ENCODE_EXCEPTION, ex);
            }
        }
        #endregion

        #region Unified Process
        /// <summary>
        /// 指纹消息处理
        /// </summary>
        /// <param name="data"></param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid PreProcessFingerMarksMsg(IMessageReqHandler data, ref byte[] codemsg)
        {
            // 编码
            codemsg = MarksMsgDataToBytes(data);
            Guid msgID = Guid.NewGuid();

            // 写数据库
            // ....

            // 插入已发送列表
            InsertMsgList(msgID, data);
            return msgID;
        }

        /// <summary>
        /// 支付平台消息处理
        /// </summary>
        /// <param name="data"></param>
        /// <param name="codemsg"></param>
        /// <returns></returns>
        public static Guid PreProcessPaymentMsg(IMessageReqHandler data, ref byte[] codemsg)
        {
            // 编码
            ((BizMsgDataBase)data).OnArgumentsValidation();
            codemsg = PaymentMsgDataToBytes(data);
            Guid msgID = Guid.NewGuid();

            // 写数据库
            // ....

            // 插入已发送列表
            InsertMsgList(msgID, data);
            return msgID;
        }

        public static Guid PreProcessEncryptMsg(EncryptTellerAuth data, ref byte[] codemsg)
        {
            data.AfterEncrypt = false;
            codemsg = data.TellerEncrypt.ToBytes();
            Guid msgID = Guid.NewGuid();
            // 写数据库
            // ....

            // 插入已发送列表
            InsertMsgList(msgID, data);
            return msgID;
        }

        public static Guid PreProcessReqMsg(IMessageReqHandler data, ref byte[] codemsg)
        {
            // 编码
            ((BizMsgDataBase)data).OnArgumentsValidation();
            codemsg = CoreMsgDataToBytes(data);
            Guid msgID = Guid.NewGuid();

            // 写数据库
            // ....

            // 插入已发送列表
            InsertMsgList(msgID, data);
            return msgID;
        }

        public static byte[] CoreMsgDataToBytes(IMessageReqHandler data)
        {
            if (data != null)
            {
                //统一编码消息头 MH

                CoreMessageHeader msgHeader = new CoreMessageHeader();
                msgHeader.MH_MESSAGE_LENGTH = CoreMessageHeader.TOTAL_WIDTH + ((CoreBizMsgDataBase)data).RQ_TOTAL_WIDTH;
                byte[] buffer = new byte[msgHeader.MH_MESSAGE_LENGTH];
                Array.Copy(msgHeader.ToBytes(), buffer, CoreMessageHeader.TOTAL_WIDTH);

                Array.Copy(data.ToBytes(), 0, buffer, CoreMessageHeader.TOTAL_WIDTH, ((CoreBizMsgDataBase)data).RQ_TOTAL_WIDTH);
                return buffer;
            }
            return null;
        }

        public static byte[] PaymentMsgDataToBytes(IMessageReqHandler data)
        {
            return data.ToBytes();
        }

        public static byte[] MarksMsgDataToBytes(IMessageReqHandler data)
        {
            return data.ToBytes();
        }
        #endregion

       
    } 
    
}
