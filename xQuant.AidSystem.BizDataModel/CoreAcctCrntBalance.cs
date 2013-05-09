using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    /// <summary>
    /// 资金业务账号余额查询输入数据结构
    /// </summary>
    public class CoreAcctCrntBalance
    {
        /// <summary>
        /// 账号,20
        /// </summary>
        public String AcctNO
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
        /// 账号性质,1;1-表内内部账，2-存款账号
        /// </summary>
        public String AcctProperty
        {
            get;
            set;
        }
    }
}
