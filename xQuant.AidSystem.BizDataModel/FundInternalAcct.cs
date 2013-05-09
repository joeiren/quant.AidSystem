using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    public class FundInternalAcct
    {
        #region Property
        /// <summary>
        /// 账号,20
        /// </summary>
        public String AccountNO
        {
            get;
            set;
        }
        /// <summary>
        /// 归属机构号,6
        /// </summary>
        public String OrgnaztionNO
        {
            get;
            set;
        }
        /// <summary>
        /// 币种,3
        /// </summary>
        public String Currcency
        {
            get;
            set;
        }
        /// <summary>
        /// 科目号,8
        /// </summary>
        public String SubjectNO
        {
            get;
            set;
        }
        /// <summary>
        /// 顺序号,4
        /// </summary>
        public String SequenceNO
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
        /// 上一交易日,8
        /// </summary>
        public String PreviousTradeDate
        {
            get;
            set;
        }
        /// <summary>
        /// 昨日余额方向,1
        /// </summary>
        public String PreviousBalanceDirection
        {
            get;
            set;
        }
        /// <summary>
        /// 昨日余额,17
        /// </summary>
        public String PreviousBalance
        {
            get;
            set;
        }
        /// <summary>
        /// 余额方向,1
        /// </summary>
        public String BalanceDirection
        {
            get;
            set;
        }
        /// <summary>
        ///  当前余额,17
        /// </summary>
        public String CurrentBalance
        {
            get;
            set;
        }
        /// <summary>
        ///透支限额,17
        /// </summary>
        public String OverdraftLimitation
        {
            get;
            set;
        }
        /// <summary>
        /// 利率,10
        /// </summary>
        public String Rate
        {
            get;
            set;
        }
        /// <summary>
        /// 透支利率,10
        /// </summary>
        public String OverdraftRate
        {
            get;
            set;
        }
        /// <summary>
        /// 利息积数,19
        /// </summary>
        public String InterestCumulation
        {
            get;
            set;
        }
        /// <summary>
        /// 透支积数,19
        /// </summary>
        public String OverdraftCumulation
        {
            get;
            set;
        }
        /// <summary>
        /// 使用范围,1
        /// </summary>
        public String UsageRange
        {
            get;
            set;
        }
        /// <summary>
        ///是否采用销账管理,1
        /// </summary>
        public String IsEliminate
        {
            get;
            set;
        }
        /// <summary>
        ///是否收付现,1
        /// </summary>
        public String IsExchangePay
        {
            get;
            set;
        }
        /// <summary>
        ///结息方式,1
        /// </summary>
        public String SettlementType
        {
            get;
            set;
        }
        /// <summary>
        ///结息是否入账,1
        /// </summary>
        public String IsSettlementWithAccount
        {
            get;
            set;
        }
        /// <summary>
        ///收息账号,20
        /// </summary>
        public String IncomeAccount
        {
            get;
            set;
        }
        /// <summary>
        ///付息账号,20
        /// </summary>
        public String PaymentAccount
        {
            get;
            set;
        }
        /// <summary>
        ///账户状态,1
        /// </summary>
        public String AccountStatus
        {
            get;
            set;
        }
        /// <summary>
        ///开户机构,6
        /// </summary>
        public String AccountOrgan
        {
            get;
            set;
        }
        /// <summary>
        ///归属总账机构,6
        /// </summary>
        public String HeadOrgan
        {
            get;
            set;
        }
        /// <summary>
        ///通兑级别,1
        /// </summary>
        public String CirculateExchangeLevel
        {
            get;
            set;
        }
        /// <summary>
        ///开户日期,8
        /// </summary>
        public String AccountDate
        {
            get;
            set;
        }
        /// <summary>
        ///开户柜员,7
        /// </summary>
        public String AccountTeller
        {
            get;
            set;
        }

        #endregion
    }
}
