using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 实时资金头寸查询接口--内部账查询
    /// </summary>
    public class InterAcctRetrieveODATA : IMessageRespHandler
    {
        public const UInt16 TOTAL_WIDTH = 314 + CoreDataBlockHeader.TOTAL_WIDTH;
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
        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= TOTAL_WIDTH)
            {
                CoreDataBlockHeader dbhdr1 = new CoreDataBlockHeader();
                dbhdr1 = (CoreDataBlockHeader)dbhdr1.FromBytes(messagebytes);
                messagebytes = CommonDataHelper.SubBytes(messagebytes, CoreDataBlockHeader.TOTAL_WIDTH, messagebytes.Length - CoreDataBlockHeader.TOTAL_WIDTH);
                AccountNO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 20).TrimEnd();
                OrgnaztionNO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 6).TrimEnd();
                Currcency = CommonDataHelper.GetValueFromBytes(ref messagebytes, 3).TrimEnd();
                SubjectNO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
                SequenceNO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 4).TrimEnd();
                AccountName = CommonDataHelper.GetValueFromBytes(ref messagebytes, 80).TrimEnd();
                PreviousTradeDate = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
                PreviousBalanceDirection = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                PreviousBalance = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17).TrimEnd();
                BalanceDirection = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                CurrentBalance = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17).TrimEnd();
                OverdraftLimitation = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17).TrimEnd();
                Rate = CommonDataHelper.GetValueFromBytes(ref messagebytes, 10).TrimEnd();
                OverdraftRate = CommonDataHelper.GetValueFromBytes(ref messagebytes, 10).TrimEnd();
                InterestCumulation = CommonDataHelper.GetValueFromBytes(ref messagebytes, 19).TrimEnd();
                OverdraftCumulation = CommonDataHelper.GetValueFromBytes(ref messagebytes, 19).TrimEnd();
                UsageRange = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                IsEliminate = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                IsExchangePay = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                SettlementType = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                IsSettlementWithAccount = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                IncomeAccount = CommonDataHelper.GetValueFromBytes(ref messagebytes, 20).TrimEnd();
                PaymentAccount = CommonDataHelper.GetValueFromBytes(ref messagebytes, 20).TrimEnd();
                AccountStatus = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                AccountOrgan = CommonDataHelper.GetValueFromBytes(ref messagebytes, 6).TrimEnd();
                HeadOrgan = CommonDataHelper.GetValueFromBytes(ref messagebytes, 6).TrimEnd();
                CirculateExchangeLevel = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                AccountDate = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
                AccountTeller = CommonDataHelper.GetValueFromBytes(ref messagebytes, 7).TrimEnd();
                
            }

            return this;
        }

        #endregion
    }
}
