using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    /// <summary>
    /// 存放省内上级机构活期款项查询返回的数据,县级行社上存清算资金查询返回的数据
    /// </summary>
    public class FundAcctInfo
    {
        #region Property
        /// <summary>
        /// 机构号,6
        /// </summary>
        public String OrganNO
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
        /// 科目,8
        /// </summary>
        public String Subject
        {
            get;
            set;
        }
        /// <summary>
        /// 上日余额,17
        /// </summary>
        public String PerviousBalance
        {
            get;
            set;
        }
        /// <summary>
        /// 本日借方发生额,17
        /// </summary>
        public String DebitAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 本日贷方发生额,17
        /// </summary>
        public String CreditAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 当前余额,17
        /// </summary>
        public String CurrentBalance
        {
            get;
            set;
        }
        /// <summary>
        /// 下限金额,17
        /// </summary>
        public String FloorAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 轧差金额,17
        /// </summary>
        public String OffsetBalance
        {
            get;
            set;
        }
        #endregion
    }
}
