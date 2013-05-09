using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    /// <summary>
    /// 支付平台返回结果
    /// </summary>
    public class PaymentResult
    {
        #region Property
        /// <summary>
        /// 交易结果
        /// </summary>
        public bool Succeed
        {
            get;
            set;
        }
        /// <summary>
        /// 返回码,X10
        /// </summary>
        public String HostReturnCode
        {
            get;
            set;
        }
        /// <summary>
        /// 返回信息(交易失败的具体信息),X80
        /// </summary>
        public String HostReturnMessage
        {
            get;
            set;
        }
        /// <summary>
        /// 主机交易流水号,X12
        /// </summary>
        public String HostTranFlowNo
        {
            get;
            set;
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
        /// 登记薄查询的结果：状态,X2
        /// </summary>
        public String AccountStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 登记薄查询的结果：渠道标志,X2
        /// </summary>
        public String PackageChannelType
        {
            get;
            set;
        }
        /// <summary>
        /// 挂账序号
        /// </summary>
        public String PendingSN
        {
            get;
            set;
        }
        #endregion
    }

    
}
