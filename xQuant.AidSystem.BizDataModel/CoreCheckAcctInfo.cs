using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    public class CoreCheckAcctInfo
    {
        #region Property
        /// <summary>
        /// 交易日期,8
        /// </summary>
        public String TradeDate
        {
            get;
            set;
        }
        /// <summary>
        /// 资金业务流水号,18
        /// </summary>
        public String BizFlowNO
        {
            get;
            set;
        }
        /// <summary>
        /// 交易机构,6
        /// </summary>
        public String OrgNO
        {
            get;
            set;
        }
        /// <summary>
        /// 交易柜员,7
        /// </summary>
        public String TellerNO
        {
            get;
            set;
        }
        /// <summary>
        /// 柜员流水号,11
        /// </summary>
        public String TellerFlowNO
        {
            get;
            set;
        }
        /// <summary>
        /// 交易账号,6
        /// </summary>
        public String TradeAcctNO
        {
            get;
            set;
        }
        /// <summary>
        /// 账号归属机构,6
        /// </summary>
        public String OrgNOWithinAcct
        {
            get;
            set;
        }
        /// <summary>
        /// 币种,3
        /// </summary>
        public String Currency
        {
            get;
            set;
        }
        /// <summary>
        /// 核算码,8
        /// </summary>
        public String CheckCode
        {
            get;
            set;
        }
        /// <summary>
        /// 借贷标志,1;1-借，2-贷
        /// </summary>
        public String DCFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 红蓝字标志,1;1-正常，2-红字，3-蓝字
        /// </summary>
        public String RedBlueFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 金额，17
        /// </summary>
        public String Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 分录状态,1;  1-正常，2-抹帐
        /// </summary>
        public String Status
        {
            get;
            set;
        }
        #endregion
    }
}
