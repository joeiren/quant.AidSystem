using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    /// <summary>
    /// 核心查询记账接口 返回数据
    /// </summary>
    public class CoreAcctDetail
    {
        public AcctDetail_BGO308000 DB_BGO30800
        {
            get;
            set;
        }

        public List<AcctDetail_BGO30801> DB_BGO30801_List
        {
            get;
            set;
        }

        public AcctDetail_BGO30802 DB_BGO30802
        {
            get;
            set;
        }

        public List<AcctDetail_BGO30803> DB_BGO30803_List
        {
            get;
            set;
        }

        public CoreAcctDetail()
        {
            DB_BGO30800 = new AcctDetail_BGO308000();
            DB_BGO30801_List = new List<AcctDetail_BGO30801>();
            DB_BGO30802 = new AcctDetail_BGO30802();
            DB_BGO30803_List = new List<AcctDetail_BGO30803>();
        }
    }

    /// <summary>
    /// 记账查询 返回结构BGO30800
    /// </summary>
    public class AcctDetail_BGO308000
    {
        /// <summary>
        /// 核心交易流水号,12
        /// </summary>
        public String CoreTradeSN
        {
            get;
            set;
        }
        /// <summary>
        /// 发起方系统代码,2
        /// </summary>
        public String SrcSystemCode
        {
            get;
            set;
        }
        /// <summary>
        /// 有会计流水标志,1
        /// </summary>
        public String AcctSNFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 外系统跟踪号,12
        /// </summary>
        public String OutsideSN
        {
            get;
            set;
        }
        /// <summary>
        /// 被抹账标志,1
        /// </summary>
        public String ErasedFlag
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 记账查询 返回结构BGO30801
    /// </summary>
    public class AcctDetail_BGO30801
    {
        /// <summary>
        /// 核心交易流水号,12
        /// </summary>
        public String CoreTradeSN
        {
            get;
            set;
        }        
        /// <summary>
        /// 交易账号,20
        /// </summary>
        public String TradeAccount
        {
            get;
            set;
        }
        /// <summary>
        /// 户名,80
        /// </summary>
        public String AccountName
        {
            get;
            set;
        }
        /// <summary>
        /// 现转标志,1
        /// </summary>
        public String CashFalg
        {
            get;
            set;
        }
        /// <summary>
        /// 借贷标志,1
        /// </summary>
        public String LoanFlag
        {
            get;
            set;
        }
        //private Decimal _amount = new Decimal(0);
        /// <summary>
        /// 金额,17
        /// </summary>
        public String Amount
        {
            get;
            //{
            //    return Decimal.Round(_amount, 2);
            //}
            set;
            //{
            //    _amount = value;
            //}
        }
        /// <summary>
        /// 凭证号码,20
        /// </summary>
        public String VoucherNO
        {
            get;
            set;
        }        
        /// <summary>
        /// 发起方系统代码,2
        /// </summary>
        public String SrcSystemCode
        {
            get;
            set;
        }        
        /// <summary>
        /// 入账配钞标志,1
        /// </summary>
        public String PostingCashFlag
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 记账查询 返回结构BGO30802
    /// </summary>
    public class AcctDetail_BGO30802
    {
        /// <summary>
        /// 是否配钞 ,1
        /// </summary>
        public String IsQuotaMoney
        {
            get;
            set;
        }
        /// <summary>
        /// 配钞笔数,2
        /// </summary>
        public UInt16 QuotaMoneyCount
        {
            get;
            set;
        }
    }
   
    /// <summary>
    /// 记账查询 返回结构BGO30803
    /// </summary>
    public class AcctDetail_BGO30803
    {
        /// <summary>
        /// 币种,3
        /// </summary>
        public String Currency
        {
            get;
            set;
        }

        //private Decimal _quotaAmount = new Decimal(0);
        /// <summary>
        /// 配钞金额,17
        /// </summary>
        public String QuotaAmount
        {
            get;
            //{
            //    return Decimal.Round(_quotaAmount, 2);
            //}
            set;
            //{
            //    _quotaAmount = value;
            //}
        }
    }
}
