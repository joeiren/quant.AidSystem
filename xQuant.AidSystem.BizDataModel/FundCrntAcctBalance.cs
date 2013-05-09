using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    /// <summary>
    /// 资金业务账号余额查询返回数据
    /// </summary>
    public class FundCrntAcctBalance
    {
        #region Property
        /// <summary>
        /// 账号,20
        /// </summary>
        public String AcctNO
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
        /// <summary>
        /// 结果标志,1;1-成功，2-账号不存在或已销户
        /// </summary>
        public String ResultFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 余额,17
        /// </summary>
        public String Balance
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
        #endregion
    }
}
