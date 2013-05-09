/*
* 版权所有：杭州衡泰软件
* 作    者：林尚都(linshangdou@yahoo.com.cn)
* 创建时间：2011-5-12 16:50:18
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

namespace xQuant.AidSystem.BizDataModel
{
    public class InterBankOpenAcctInfo
    {
        #region [ Property ]
        /// <summary>
        /// 是否是新增  （维护类型 1）
        /// </summary>
        public AidTypeDefine.INTER_BANK_OPERATION_TYPE OPERATION_TYPE { get; set; }

        /// <summary>
        /// 通知单编号 20 (必输)
        /// </summary>
        public String NOTICE_NO { get; set; }

        /// <summary>
        /// 是否是同业活期
        /// </summary>
        public AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE BIZ_TERM_TYPE { get; set; }

        /// <summary>
        /// 存款种类 2 (工，农，中，建，其他)
        /// </summary>
        public AidTypeDefine.INTER_BANK_DEPOSITING_CATEGORY DEPOSIT_TYPE { get; set; }

        /// <summary>
        /// 资金来源活期账号 22 (定期开户时必输（活期账户）)
        /// </summary>
        public String CURRENT_ACCOUNT { get; set; }

        /// <summary>
        /// 收息账号 22 （定期开户时必输，活期开户可为空）
        /// </summary>
        public String INTEREST_ACCOUNT { get; set; }

        /// <summary>
        /// 定期自动转存标志 1 (1-自动转存；0-不自动转存)
        /// </summary>
        public bool AUTO_REDEPO { get; set; }

        /// <summary>
        /// 起息日期 8 
        /// </summary>
        public DateTime VALUE_DATE { get; set; }

        /// <summary>
        /// 到期日期 8
        /// </summary>
        public DateTime MATURITY_DATE { get; set; }

        /// <summary>
        /// 交易金额 17 (定期开户时必输（定期存款）)
        /// </summary>
        public double AMOUNT { get; set; }

        /// <summary>
        /// 利率 11 (6位小数（+-xxx.xxxxxx）)
        /// </summary>
        public double RATE { get; set; }

        /// <summary>
        /// 计息方式 1 (1-，2,3，)
        /// </summary>
        public AidTypeDefine.INTER_BANK_COUPON_TYPE INTEREST_BEARING_MANNER { get; set; }

        /// <summary>
        /// 客户内码 11 (必输)
        /// </summary>
        public String CUSTOMER_CODE { get; set; }

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
}
