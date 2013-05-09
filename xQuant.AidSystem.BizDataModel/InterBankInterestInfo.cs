using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    /// <summary>
    /// 同业存放结息数据
    /// </summary>
    public class InterBankInterestInfo
    {
        public InterBankInterestSummaryInfo SummaryInfo
        {
            get;
            set;
        }

        List<InterBankInterestSettleInfo> _settleCollection = new List<InterBankInterestSettleInfo>();
        public List<InterBankInterestSettleInfo> SettleCollection
        {
            get
            {
                return _settleCollection;
            }
            set 
            {
                _settleCollection = value;
            }
        }
    }

    /// <summary>
    /// 同业存放结息账户信息
    /// </summary>
    public struct InterBankInterestSummaryInfo
    {
        #region Property
        /// <summary>
        /// 批号,15
        /// </summary>
        public string KPSN
        {
            get;
            set;
        }
        /// <summary>
        /// 总金额,17
        /// </summary>
        public double TotalAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 总笔数,8
        /// </summary>
        public int TotalCount
        {
            get;
            set;
        }       
        /// <summary>
        /// 批量名称,80
        /// </summary>
        public string BatchName
        {
            get;
            set;
        }
        #endregion
    }

    /// <summary>
    /// 结息账户
    /// </summary>
    public class InterBankInterestSettleInfo
    {
        #region 属性
        /// <summary>
        /// 活期定期标志;1活 2定
        /// </summary>
        public AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE TermFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 交易起息日,8
        /// </summary>
        public DateTime ValueDate
        {
            get;
            set;
        }
        /// <summary>
        /// 交易记账日期,8
        /// </summary>
        public DateTime RecordDate
        {
            get;
            set;
        }
        /// <summary>
        /// 入账账号,22
        /// </summary>
        public string AccountNO
        {
            get;
            set;
        }
        /// <summary>
        /// 产生利息账号,22
        /// </summary>
        public string InterestAccount
        {
            get;
            set;
        }
        
        /// <summary>
        /// 利息,17
        /// </summary>
        public double Interest
        {
            get;
            set;
        }
        #endregion

        List<InterBankInterestAIInfo> _aiCollection = new List<InterBankInterestAIInfo>();
        public List<InterBankInterestAIInfo> AICollection
        {
            get
            {
                return _aiCollection;
            }
            set
            {
                _aiCollection = value;
            }
        }

    }

    /// <summary>
    /// 计息结构
    /// </summary>
    public struct InterBankInterestAIInfo
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime BeginDate
        {
            get;
            set;
        }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate
        {
            get;
            set;
        }
        /// <summary>
        /// 利率,11
        /// </summary>
        public double Rate
        {
            get;
            set;
        }
        /// <summary>
        /// 积数,19
        /// </summary>
        public double Aggregate
        {
            get;
            set;
        }
        /// <summary>
        /// 利息 17
        /// </summary>
        public double Interest
        {
            get;
            set;
        }
    }
}
