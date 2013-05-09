using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    public class CoreAcctResult
    {
        private List<CorePendingAcctEntry> _pendingAcctList;
        public List<CorePendingAcctEntry> PendingAcctList
        {
            get
            {
                return _pendingAcctList;
            }
            set
            {
                _pendingAcctList = value;
            }
        }

        private List<CoreAcctEntry> _acctEntryList;
        public List<CoreAcctEntry> AcctEntryList
        {
            get
            {
                return _acctEntryList;
            }
            set
            {
                _acctEntryList = value;
            }
        }

        public CoreAcctResult()
        {
            _pendingAcctList = new List<CorePendingAcctEntry>();
            _acctEntryList = new List<CoreAcctEntry>();
        }
    }

    public class CorePendingAcctEntry
    {
        /// <summary>
        /// 资金业务系统流水号,18
        /// </summary>
        public String FlowNO
        {
            get;
            set;
        }
        /// <summary>
        /// 套内序号,2
        /// </summary>
        public String InnerSN
        {
            get;
            set;
        }
        /// <summary>
        /// 挂账序号,14
        /// </summary>
        public String PendingSN
        {
            get;
            set;
        }
        /// <summary>
        /// 挂账账号,20
        /// </summary>
        public String PendingAccount
        {
            get;
            set;
        }
        /// <summary>
        /// 挂账金额,17
        /// </summary>
        public String PendingAmount
        {
            get;
            set;
        }
    }
    /// <summary>
    /// 用于记账返回的记账分录
    /// </summary>
    public class CoreAcctEntry
    {
        /// <summary>
        /// 账号
        /// </summary>
        public String AccountNO
        {
            get;
            set;
        }

        /// <summary>
        /// 核算码
        /// </summary>
        public String CheckCode
        {
            get;
            set;
        }
        /// <summary>
        /// 币种
        /// </summary>
        public String Currency
        {
            get;
            set;
        }

        /// <summary>
        /// 借贷标志
        /// </summary>
        public String Option
        {
            get;
            set;
        }

        /// <summary>
        /// 金额
        /// </summary>
        public String Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 分录序号
        /// </summary>
        public String InnerSN
        {
            get;
            set;
        }


    }
}
