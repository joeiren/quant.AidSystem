using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace xQuant.AidSystem.Communication
{
    class CommonDataType
    {
    }

    public class CommonHelper
    {
        //public static void AddEventLog(String sourceName, String description)
        //{
        //    if (!EventLog.SourceExists(sourceName))
        //    {
        //        EventLog.CreateEventSource(sourceName, "xQuant.AidSystem");
        //    }

        //    EventLog log = new EventLog();
        //    log.Source = sourceName;
        //    log.WriteEntry(description);
        //}
    }

    public class MessageData
    {
        #region Property
        public Guid MessageID
        {
            get;
            set;
        }

        public String BizMsgID
        {
            get;
            set;
        }

        public Queue<PackageData> ReqPackageList
        {
            get;
            set;
        }

        public Queue<PackageData> RespPackageList
        {
            get;
            set;
        }

        public Boolean IsMultiPackage
        {
            get;
            set;
        }

        public PlatformType TragetPlatform
        {
            get;
            set;
        }

        public DateTime FirstTime
        {
            get;
            set;
        }

        public String SourceIP
        {
            get;
            set;
        }

        public String TellerNO
        {
            get;
            set;
        }
        public String OrgNO
        {
            get;
            set;
        }
        public int MsgBizType
        {
            get;
            set;
        }
        #endregion
        public MessageData()
        {
            ReqPackageList = new Queue<PackageData>();
            RespPackageList = new Queue<PackageData>();
        }

        public PackageData CurrentReqPackage
        {
            get
            {
                if (ReqPackageList.Count > 0)
                {
                    return ReqPackageList.Peek();
                }
                else
                {
                    return new PackageData();
                }
            }
        }

        public PackageData CurrentRespPackage
        {
            get
            {
                if (RespPackageList.Count > 0)
                {
                    return RespPackageList.Peek();
                }
                else
                {
                    return new PackageData();
                }
            }
        }

        public byte[] GetReqMessage()
        {
            return CurrentReqPackage.PackageMessage;
        }

        public byte[] GetRespMessage()
        {
            return CurrentRespPackage.PackageMessage;
        }

        public Int16 GetReqPackageID()
        {
            return CurrentReqPackage.PackageID;
        }

        public Int16 GetRespPackageID()
        {
            return CurrentRespPackage.PackageID;
        }

        public int ReSentTime
        {
            get;
            set;
        }

    }

    public struct PackageData
    {
        public Int16 PackageID;
        public byte[] PackageMessage;

        //public PackageData()
        //{
        //    PackageID = 0;
        //    PackageMessage = "";
        //}

        public PackageData(Int16 pageageID, byte[] packageMessage)
        {
            PackageID = pageageID;
            PackageMessage = packageMessage;
        }
    }

    public enum PlatformType
    {
        Core = 1,
        Payment = 2,
        Encrypt = 3,
        PaymentDownload = 4,
        FingerMarks = 5,
    }

    public enum MessageBizType
    {
        CoreCstmCreate = 1,     //核心创建客户
        CoreCstmUpdate = 2,     //核心更新客户
        CoreCstmDelete = 3,     //核心删除客户
        CoreCstmRetrieve = 4,   //核心查询客户
        CoreAuthTeller = 5,     //核心用户认证
        EncryptPwd = 6,         //加密
        CoreCstmBaseInfo = 7,   //核心查询客户基本信息
        FingerMarkValidation = 8, //指纹验证
        CoreTellerChangPwd =9,  //核心柜员密码修改
        CoreAcctRecord = 20,    //核心记账（多借多贷）
        CoreAcctRetrieve = 21,  //核心记账查询
        CoreAcctErase = 22,     //核心抹账
        CoreAcctMultiLend = 23, //核心多借一贷
        CoreAcctMultiLoan = 24, //核心多贷一借
        CoreAcctChecking = 25,  //核心对账查询
        CoreInnerAcctRetrieve = 30, //核心内部账余额查询
        CoreSuperiorCrntAcctRetrieve = 31,  //存放省内上级机构活期款项查询
        CoreDepositClearingFundRetrieve = 32,//县级行社上存清算资金查询
        CoreAcctCrntBalanceRetrieve = 33, //资金业务账号余额查询
        CoreInterBankSettle = 40,
        CoreInterBankPrepared = 41,
        //add by shangdu.lin 20110328
        CoreInterBankOpenAccount = 42,//活期定期开户
        CoreInterBankDeleteAccount=43,//销户
        CoreInterBankNoticeLetter=44,//通知单查询
        CoreInterBankAutoRedepo=45,//自动转存
        CoreInterBankRetrieveAcctInfo = 46, // 活期同业账户余额查询

        PayRegisterRetrieve = 50,   //支付登记薄查询
        PayFundTrans = 51,      //支付资金划拨
        PayAcctErase = 52,      //支付抹账
        PayVostroAcctEliminate = 53,    //支付来账销账
        PayInterBank = 54,  // 支付同业拆借
        PayCheckAccout = 55, //支付对账
        PayBanksInfoDownload = 56, //支付大额行号行名下载

    }

    public enum MsgLogState
    {
        RecvOverdue = -4, // 过期消息
        PayTimeOut = -3,  //   
        RecvFailed = -2,
        SendFailed = -1,
        SendSucceed = 1,
        RecvSucceed = 2,
    }
    
}
