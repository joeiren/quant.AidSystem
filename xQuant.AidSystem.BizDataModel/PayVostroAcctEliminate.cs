using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    /// <summary>
    /// 来账的销账
    /// </summary>
    public class PayVostroAcctEliminate
    {
        #region Property
        /// <summary>
        /// 交易机构号,X6
        /// </summary>
        public String PayBank
        {
            get;
            set;
        }
        /// <summary>
        /// 操作员,X7
        /// </summary>
        public String Operator
        {
            get;
            set;
        }
        /// <summary>
        /// 渠道标志,X2(0：大额1：农信银)
        /// </summary>
        public String PackageChannelType
        {
            get;
            set;
        }
        /// <summary>
        /// 业务类型,X2
        /// </summary>
        private String _bizType;
        public String BizType
        {

            get
            {
                return "11";
            }
        }
        /// <summary>
        /// 支付交易序号,X8
        /// </summary>
        public String TransSeq
        {
            get;
            set;
        }
        /// <summary>
        /// 发起行行号,X12
        /// </summary>
        public String AccountBanks
        {
            get;
            set;
        }
        /// <summary>
        /// 委托日期,X8
        /// </summary>
        public String DelegateDate
        {
            get;
            set;
        }
        /// <summary>
        /// 资金去向,X1
        /// </summary>
        public String FundDest
        {
            get
            {
                return "5";
            }
            
        }
        /// <summary>
        /// 金额 ,S15.2
        /// </summary>
        public String Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 入账账号,X32
        /// </summary>
        public String PostAcount
        {
            get;
            set;
        }
        /// <summary>
        /// 入账户名,X60
        /// </summary>
        public String PostAccountName
        {
            get;
            set;
        }
        /// <summary>
        /// 入账机构号,X6
        /// </summary>
        public String PostBank
        {
            get;
            set;
        }
        /// <summary>
        /// 入账机构名称,X60
        /// </summary>
        public String PostBankName
        {
            get;
            set;
        }
        #endregion
    }
}
