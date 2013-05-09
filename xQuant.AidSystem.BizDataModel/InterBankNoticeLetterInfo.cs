/*
* 版权所有：杭州衡泰软件
* 作    者：林尚都(linshangdou@yahoo.com.cn)
* 创建时间：2011-3-24 14:47:47
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
    public class InterBankNoticeLetterInfo
    {
        #region [ Property ]

        /// <summary>
        /// 日期 8 (会计日期)
        /// </summary>
        public String ACCOUNT_DATE { get; set; }

        /// <summary>
        /// 通知单编号 20 (必输)
        /// </summary>
        public String NOTICE_NO { get; set; }

        /// <summary>
        /// 通知单类型 1 (1-开户 2-销户 3-部提)
        /// </summary>
        public AidTypeDefine.INTER_BANK_NOTICE_TYPE NOTICE_TYPE { get; set; }

        /// <summary>
        /// 业务类型 1 (1-同业活期 2-同业定期)
        /// </summary>
        public AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE BUSINESS_TYPE { get; set; }

        /// <summary>
        /// 账号 15
        /// </summary>
        public String ACCOUNT { get; set; }

        /// <summary>
        /// 币种 3 
        /// </summary>
        public String CURRENCY { get; set; }

        /// <summary>
        /// 外币客户类别 1 
        /// </summary>
        public String CUSTOMER_TYPE { get; set; }

        /// <summary>
        /// 钞汇属性 1 (必输,1)
        /// </summary>
        public String CASH_PROPERTY { get; set; }

        /// <summary>
        /// 产品类别 2 
        /// </summary>
        public String PRODUCT_TYPE { get; set; }

        /// <summary>
        /// 产品代码 3
        /// </summary>
        public String PRODUCT_CODE { get; set; }

        /// <summary>
        /// 存款种类 2 (工，农，中，建，其他)
        /// </summary>
        public AidTypeDefine.INTER_BANK_DEPOSITING_CATEGORY DEPOSIT_TYPE { get; set; }

        /// <summary>
        /// 账户性质 4 
        /// </summary>
        public String ACCOUNT_PROPERTY { get; set; }

        /// <summary>
        /// 帐户类型 1 
        /// </summary>
        public String ACCOUNT_TYPE { get; set; }

        /// <summary>
        /// 资金来源活期账号 22 
        /// </summary>
        public String CURRENT_ACCOUNT { get; set; }

        /// <summary>
        /// 收息账号 22
        /// </summary>
        public String INTEREST_ACCOUNT { get; set; }

        /// <summary>
        /// 定期部提新账号 15
        /// </summary>
        public String FIXED_NEW_ACCOUNT { get; set; }

        /// <summary>
        /// 凭证号码 20
        /// </summary>
        public String VOUCHER_NO { get; set; }

        /// <summary>
        /// 新凭证号码 20
        /// </summary>
        public String VOUCHER_NEW_NO { get; set; }

        /// <summary>
        /// 定期自动转存标志 1 (1-自动转存；0-不自动转存)
        /// </summary>
        public bool AUTO_REDEPO { get; set; }

        /// <summary>
        /// 定期存期 3 (存期月数(通知存款为通知天数)，资金业务空)
        /// </summary>
        public string DEPOSIT_TERM { get; set; }

        /// <summary>
        /// 起息日期 8 
        /// </summary>
        public String VALUE_DATE { get; set; }

        /// <summary>
        /// 到期日期 8
        /// </summary>
        public String MATURITY_DATE { get; set; }

        /// <summary>
        /// 交易金额 17 (定期开户时必输（定期存款）)
        /// </summary>
        public decimal AMOUNT { get; set; }

        /// <summary>
        /// 利率 11 (6位小数（+-xxx.xxxxxx）)
        /// </summary>
        public decimal RATE { get; set; }

        /// <summary>
        /// 计息方式 1 (1-，2,3，)
        /// </summary>
        public AidTypeDefine.INTER_BANK_COUPON_TYPE INTEREST_BEARING_MANNER { get; set; }

        /// <summary>
        /// 利息金额 17
        /// </summary>
        public decimal INTEREST { get; set; }

        /// <summary>
        /// 客户内码 11 (必输)
        /// </summary>
        public String CUSTOMER_CODE { get; set; }

        /// <summary>
        /// 银行集团企业标志 1 (‘B’)
        /// </summary>
        public String BANK_FLAG { get; set; }

        /// <summary>
        /// 经办机构 6
        /// </summary>
        public String HANDLE_ORGNAZTION { get; set; }

        /// <summary>
        /// 审批机构 6
        /// </summary>
        public String APPROVE_ORGNAZTION { get; set; }

        /// <summary>
        /// 操作柜员 7 
        /// </summary>
        public String HANDLE_TELLER { get; set; }

        /// <summary>
        /// 审核柜员 7
        /// </summary>
        public String APPROVE_TELLER { get; set; }

        /// <summary>
        /// 处理标志 1
        /// </summary>
        public String HANDLE_FLAG { get; set; }

        /// <summary>
        /// 原交易流水号 8
        /// </summary>
        public String TRADE_FLOW_NO { get; set; }

        /// <summary>
        /// 子交易流水号 8
        /// </summary>
        public String CHILD_TRADE_FLOW_NO { get; set; }

        /// <summary>
        /// 柜员流水号 11
        /// </summary>
        public String TELLER_FLOW_NO { get; set; }

        /// <summary>
        /// 记录状态 1
        /// </summary>
        public String RECORD_FLAG { get; set; }

        /// <summary>
        /// 存单折类型 1 (2-支票 3-存单)
        /// </summary>
        public string BOOK_TYPE { get; set; }

        /// <summary>
        /// 户名 80
        /// </summary>
        public string ACCOUNT_NAME { get; set; }

        /// <summary>
        /// 余额 17
        /// </summary>
        public decimal BALANCE { get; set; }

        /// <summary>
        /// 本息合计 17 (余额+利息)
        /// </summary>
        public decimal SUM_AMOUNT { get; set; }

        /// <summary>
        /// 备用 100
        /// </summary>
        public String RESERVE { get; set; }

        #endregion
    }

    /// <summary>
    /// 通知单查询结构
    /// </summary>
    public struct InterBankNoticeQueryInfo
    {
        /// <summary>
        /// 通知单编号
        /// </summary>
        public string NoticeID
        {
            get;
            set;
        }
        /// <summary>
        /// 通知单类型;1-开户 2-销户 3-部提
        /// </summary>
        public AidTypeDefine.INTER_BANK_NOTICE_TYPE NoticeType
        {
            get;
            set;
        }
        /// <summary>
        /// 业务类型,"1" :活期; "2":定期
        /// </summary>
        public AidTypeDefine.INTER_BANK_BIZ_TERM_TYPE BizType
        {
            get;
            set;
        }
        /// <summary>
        /// 会计日期
        /// </summary>
        public DateTime AccountDate
        {
            get;
            set;
        }
    }
}
