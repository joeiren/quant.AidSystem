/*
* 版权所有：杭州衡泰软件
* 作    者：林尚都(linshangdou@yahoo.com.cn)
* 创建时间：2011-3-24 9:46:37
* 版 本 号：1.0.0
* 模块说明：
* ----------------------------------
* 修改记录：
* 日    期：
* 版 本 号：
* 修 改 人：
* 修改内容：
* 
*/


using System;
using System.Collections.Generic;

namespace xQuant.AidSystem.BizDataModel
{
    /// <summary>
    /// 销户、部提数据
    /// </summary>
    public class InterBankDeleteAcctInfo
    {
        #region [ Property ]
        /// <summary>
        /// 维护类型 1 (1-新增 2-撤销（必输）)
        /// </summary>
        public AidTypeDefine.INTER_BANK_OPERATION_TYPE OPERATION_TYPE { get; set; }

        /// <summary>
        /// 通知单编号 17 (必输)
        /// </summary>
        public String NOTICE_NO { get; set; }

        /// <summary>
        /// 销户or部提
        /// </summary>
        public AidTypeDefine.INTER_BANK_NOTICE_TYPE NOTICE_TYPE { get; set; }

        /// <summary>
        /// 同业活期1， 同业定期2
        /// </summary>
        public AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE TERM_TYPE { get; set; }

        /// <summary>
        /// 账号 22 (必输)
        /// </summary>
        public String ACCOUNT { get; set; }

        /// <summary>
        /// 收息账号 22
        /// </summary>
        public String INTEREST_ACCOUNT
        {
            get;
            set;
        }

        /// <summary>
        /// 交易金额 17 必输（部提为实际金额，销户为本金）
        /// </summary>
        public double AMOUNT { get; set; }

        /// <summary>
        /// 利息金额 17
        /// </summary>
        public double INTEREST { get; set; }
        
        /// <summary>
        /// 计息明细
        /// </summary>
        public List<InterestAccrualInfo> DETAILS { get; set; }

        /// <summary>
        /// 经办机构
        /// </summary>
        public String OPERATE_ORGANIZATION
        {
            get;
            set;
        }

        #endregion
    }

    /// <summary>
    /// 计息结构
    /// </summary>
    public class InterestAccrualInfo
    {
        /// <summary>
        /// 起息日期 8 
        /// </summary>
        public DateTime VALUE_DATE { get; set; }

        /// <summary>
        /// 到期日期 8
        /// </summary>
        public DateTime MATURITY_DATE { get; set; }

        /// <summary>
        /// 利率 11 (6位小数（+-xxx.xxxxxx）)
        /// </summary>
        public double RATE { get; set; }

        /// <summary>
        /// 积数 19
        /// </summary>
        public double CHARGE_NUMBER { get; set; }

        /// <summary>
        /// 利息 17
        /// </summary>
        public double INTEREST { get; set; }
    }
}
