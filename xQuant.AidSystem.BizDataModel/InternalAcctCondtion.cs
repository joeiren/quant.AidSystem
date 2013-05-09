using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    public class InternalAcctCondtion
    {
        #region Property
        /// <summary>
        /// 账号,20 (为空)
        /// </summary>
        public String AccountNO
        {
            get;
            set;
        }
        /// <summary>
        /// 归属机构,6
        /// </summary>
        public String OrgnaztionNO
        {
            get;
            set;
        }
        /// <summary>
        /// 核算码，8
        /// </summary>
        public String CheckCode
        {
            get;
            set;
        }
        /// <summary>
        /// 币种，3
        /// </summary>
        public String Currcency
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
        /// 余额查询方式，1(1- 全部;2- 正余额;3- 负余额)
        /// </summary>
        public String BalanceQueryType
        {
            get;
            set;
        }

        #endregion
    }
}
