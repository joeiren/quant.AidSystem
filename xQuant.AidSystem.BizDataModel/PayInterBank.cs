using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    /// <summary>
    /// 同业拆借
    /// </summary>
    public class PayInterBank
    {
        #region Property
        /// <summary>
        /// 资金流水号,X22
        /// </summary>
        public String TransferFlowNo
        {
            get;
            set;
        }
        /// <summary>
        /// 交易机构号,X6
        /// </summary>
        public String PayBank
        {
            get;
            set;
        }
        /// <summary>
        /// 交易日期,X8
        /// </summary>
        public String TranDate
        {
            get;
            set;
        }
        /// <summary>
        /// 付款账号,X32
        /// </summary>
        public String PayAccount
        {
            get;
            set;
        }
        /// <summary>
        /// 付款人名称,X60
        /// </summary>
        public String PayAccountName
        {
            get;
            set;
        }
        private String _onAccountSN;
        /// <summary>
        /// 挂账序号,X14;若无则填“0”
        /// </summary>
        public String OnAccountSN
        {
            get
            {
                if (String.IsNullOrEmpty(_onAccountSN))
                {
                    return "0";
                }
                else
                {
                    return _onAccountSN;
                }

            }
            set
            {
                _onAccountSN = value;
            }
        }
        /// <summary>
        /// 收款账号,X32
        /// </summary>
        public String RecAccount
        {
            get;
            set;
        }
        /// <summary>
        /// 收款人名称,X60
        /// </summary>
        public String RecAccountName
        {
            get;
            set;
        }
        /// <summary>
        /// 收款行行号,X12
        /// </summary>
        public String RecAccountBanks
        {
            get;
            set;
        }
        /// <summary>
        /// 渠道标志,X1；0：大额1：农信银
        /// </summary>
        public String PackageChannelType
        {
            get;
            set;
        }
        /// <summary>
        /// 币种,X3;CNY
        /// </summary>
        public String CurrencyType
        {
            get
            {
                return "CNY";
            }
            set { }
        }
        /// <summary>
        /// 交易金额,S15.2
        /// </summary>
        public String PayAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 拆借利率,X7,字符串类型
        /// </summary>
        public String Rate
        {
            get;
            set;
        }
        /// <summary>
        /// 拆借期限,N5
        /// </summary>
        public String TimeLimit
        {
            get;
            set;
        }
        /// <summary>
        /// 业务种类,X2
        /// </summary>
        public String BizType
        {
            get;
            set;
        }
        /// <summary>
        /// 手续费,S15.2
        /// </summary>
        public String Fee
        {
            get;
            set;
        }
        /// <summary>
        /// 备注,X60
        /// </summary>
        public String Remark
        {
            get;
            set;
        }
        /// <summary>
        /// 录入员,X7
        /// </summary>
        public String Teller
        {
            get;
            set;
        }
        /// <summary>
        /// 授权员,X7
        /// </summary>
        public String AuthTeller
        {
            get;
            set;
        }
        /// <summary>
        /// 上送渠道,X2
        /// </summary>
        public String ChannelId
        {
            get
            {
                return "KP";
            }
        }

        #endregion
    }
}
