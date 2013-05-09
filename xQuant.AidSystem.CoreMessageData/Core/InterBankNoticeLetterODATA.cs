/*
* 版权所有：杭州衡泰软件
* 作    者：林尚都(linshangdou@yahoo.com.cn)
* 创建时间：2011-3-24 11:00:50
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

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 通知单查询
    /// </summary>
    public class InterBankNoticeLetterODATA : IMessageRespHandler
    {
        #region [ Property ]

        public const UInt16 TOTAL_WIDTH = 615;

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
        public String NOTICE_TYPE { get; set; }

        /// <summary>
        /// 业务类型 1 (1-同业活期 2-同业定期)
        /// </summary>
        public String BUSINESS_TYPE { get; set; }

        /// <summary>
        /// 账号 15
        /// </summary>
        public String ACCOUNT_NO { get; set; }

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
        public String DEPOSIT_TYPE { get; set; }

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
        public String AUTO_REDEPO { get; set; }

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
        /// 交易金额 17 (15P2  定期开户时必输（定期存款）)
        /// </summary>
        public String AMOUNT { get; set; }

        /// <summary>
        /// 利率 11 (6位小数（+-xxx.xxxxxx）)
        /// </summary>
        public String RATE { get; set; }

        /// <summary>
        /// 计息方式 1 (1-，2,3，)
        /// </summary>
        public String INTEREST_BEARING_MANNER { get; set; }

        /// <summary>
        /// 利息金额 17
        /// </summary>
        public String INTEREST { get; set; }

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
        /// 处理标志 1 (‘0’ 未处理  ‘1’开销户处理成功  ‘2’暂定为撤销 )
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
        public String BOOK_TYPE { get; set; }

        /// <summary>
        /// 户名 80
        /// </summary>
        public String ACCOUNT_NAME { get; set; }

        /// <summary>
        /// 余额 17
        /// </summary>
        public String BALANCE { get; set; }

        /// <summary>
        /// 本息合计 17 (余额+利息)
        /// </summary>
        public String SUM_AMOUNT { get; set; }

        /// <summary>
        /// 备用 100
        /// </summary>
        public String RESERVE { get; set; }
        /// <summary>
        /// 开户资金来源账号户名 80
        /// </summary>
        public String ORIGNAL_ACCOUNT_NAME
        {
            get;
            set;
        }
        /// <summary>
        /// 开户资金来源账号机构 6
        /// </summary>
        public String ORIGNAL_ACCOUNT_ORG
        {
            get;
            set;
        }
        /// <summary>
        /// 开户资金来源账号机构 6
        /// </summary>
        public String CANCEL_ACCOUNT_ORG
        {
            get;
            set;
        }

        #endregion

        #region [ 实现接口 ]

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= TOTAL_WIDTH)
            {
                CoreDataBlockHeader dbhdr1 = new CoreDataBlockHeader();
                dbhdr1 = (CoreDataBlockHeader)dbhdr1.FromBytes(messagebytes);
                messagebytes = CommonDataHelper.SubBytes(messagebytes, CoreDataBlockHeader.TOTAL_WIDTH, messagebytes.Length - CoreDataBlockHeader.TOTAL_WIDTH);
                ACCOUNT_DATE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
                NOTICE_NO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 20).TrimEnd();
                NOTICE_TYPE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                BUSINESS_TYPE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                ACCOUNT_NO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 15).TrimEnd();
                CURRENCY = CommonDataHelper.GetValueFromBytes(ref messagebytes, 3).TrimEnd();
                CUSTOMER_TYPE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                CASH_PROPERTY = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                PRODUCT_TYPE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 2).TrimEnd();
                PRODUCT_CODE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 3).TrimEnd();
                DEPOSIT_TYPE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 2).TrimEnd();
                ACCOUNT_PROPERTY = CommonDataHelper.GetValueFromBytes(ref messagebytes, 4).TrimEnd();
                ACCOUNT_TYPE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                CURRENT_ACCOUNT = CommonDataHelper.GetValueFromBytes(ref messagebytes, 22).TrimEnd();
                INTEREST_ACCOUNT = CommonDataHelper.GetValueFromBytes(ref messagebytes, 22).TrimEnd();
                FIXED_NEW_ACCOUNT = CommonDataHelper.GetValueFromBytes(ref messagebytes, 15).TrimEnd();
                VOUCHER_NO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 20).TrimEnd();
                VOUCHER_NEW_NO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 20).TrimEnd();
                AUTO_REDEPO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                DEPOSIT_TERM = CommonDataHelper.GetValueFromBytes(ref messagebytes, 3).TrimEnd();
                VALUE_DATE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
                MATURITY_DATE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
                AMOUNT = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17).TrimEnd();
                RATE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 11).TrimEnd();
                INTEREST_BEARING_MANNER = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                INTEREST = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17).TrimEnd();
                CUSTOMER_CODE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 11).TrimEnd();
                BANK_FLAG = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                HANDLE_ORGNAZTION = CommonDataHelper.GetValueFromBytes(ref messagebytes, 6).TrimEnd();
                APPROVE_ORGNAZTION = CommonDataHelper.GetValueFromBytes(ref messagebytes, 6).TrimEnd();
                HANDLE_TELLER = CommonDataHelper.GetValueFromBytes(ref messagebytes, 7).TrimEnd();
                APPROVE_TELLER = CommonDataHelper.GetValueFromBytes(ref messagebytes, 7).TrimEnd();
                HANDLE_FLAG = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                TRADE_FLOW_NO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
                CHILD_TRADE_FLOW_NO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
                TELLER_FLOW_NO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 11).TrimEnd();
                RECORD_FLAG = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                BOOK_TYPE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                ACCOUNT_NAME = CommonDataHelper.GetValueFromBytes(ref messagebytes, 80).TrimEnd();
                BALANCE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17).TrimEnd();
                SUM_AMOUNT = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17).TrimEnd();
                RESERVE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 100).TrimEnd();
                ORIGNAL_ACCOUNT_NAME = CommonDataHelper.GetValueFromBytes(ref messagebytes, 80).TrimEnd();
                ORIGNAL_ACCOUNT_ORG = CommonDataHelper.GetValueFromBytes(ref messagebytes, 6).TrimEnd();
                CANCEL_ACCOUNT_ORG = CommonDataHelper.GetValueFromBytes(ref messagebytes, 6).TrimEnd();
            }

            return this;
        }

        #endregion
    }
}
