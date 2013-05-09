using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    public struct InterBankPreparedInfo
    {
        /// <summary>
        /// 定活标志;1活 2定
        /// </summary>
        public AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE TermFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 账号,15
        /// </summary>
        public string AccountNO
        {
            get;
            set;
        }
        /// <summary>
        /// 计提日期,8
        /// </summary>
        public DateTime PreparedDate
        {
            get;
            set;
        }
        /// <summary>
        /// 计提利息,17
        /// </summary>
        public double PreparedInterest
        {
            get;
            set;
        }
        /// <summary>
        /// 当前余额,17
        /// </summary>
        public double CurrentBalance
        {
            get;
            set;
        }
    }
}
