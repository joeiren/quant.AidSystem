/*
* 版权所有：杭州衡泰软件
* 作    者：林尚都(linshangdou@yahoo.com.cn)
* 创建时间：2011-5-12 17:58:08
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
using System.Text;
using System.Collections.Generic;

namespace xQuant.AidSystem.CoreMessageData
{
    public class InterBankDeleteAcctRQDTL : IMessageReqHandler
    {
        #region [ Property ]

        public const UInt16 TOTAL_WIDTH = 1739;
        public readonly UInt16 AIStruct_WIDTH = 20;

        /// <summary>
        /// 维护类型 1 (1-新增 2-撤销（必输）)
        /// </summary>
        public String OPERATE_TYPE { get; set; }

        /// <summary>
        /// 日期 8 (会计日期 （必输）)
        /// </summary>
        public String ACCOUNT_DATE { get; set; }

        /// <summary>
        /// 通知单编号 20 (必输)
        /// </summary>
        public String NOTICE_NO { get; set; }

        /// <summary>
        /// 通知单类型 1 (2-销户 3-部提)
        /// </summary>
        public String NOTICE_TYPE { get; set; }

        /// <summary>
        /// 业务类型 1 (1-同业活期 2-同业定期 （必输）)
        /// </summary>
        public String BUSINESS_TYPE { get; set; }

        /// <summary>
        /// 经办机构 6
        /// </summary>
        public String HANDLE_ORGNAZTION { get; set; }

        /// <summary>
        /// 审批机构 6
        /// </summary>
        public String APPROVE_ORGNAZTION { get; set; }

        /// <summary>
        /// 账号 22 (必输)
        /// </summary>
        public String ACCOUNT { get; set; }

        /// <summary>
        /// 币种 3 (必输（CNY）)
        /// </summary>
        public String CURRENCY { get { return "CNY"; } }

        /// <summary>
        /// 外币客户类别 1 (赋空即可)
        /// </summary>
        public String CUSTOMER_TYPE { get { return CommonDataHelper.SpaceString(1); } }

        /// <summary>
        /// 钞汇属性 1 (必输,1)
        /// </summary>
        public String CASH_PROPERTY { get { return "1"; } }

        /// <summary>
        /// 收息账号,22
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
        /// 银行集团企业标志 1 (‘B’)
        /// </summary>
        public String BANK_FLAG { get { return "B"; } }

        /// <summary>
        /// 操作柜员 7 
        /// </summary>
        public String HANDLE_TELLER { get; set; }

        /// <summary>
        /// 审核柜员 7
        /// </summary>
        public String APPROVE_TELLER { get; set; }

        /// <summary>
        /// 备用1 80 
        /// </summary>
        public String RESERVE_1 { get { return CommonDataHelper.SpaceString(80); } }

        /// <summary>
        /// 备用2 256 
        /// </summary>
        public String RESERVE_2 { get { return CommonDataHelper.SpaceString(256); } }

        /// <summary>
        /// 计息明细个数 2 (20个)
        /// </summary>
        public int DETAIL_NUMBER
        { 
            get; 
            set;
        }

        /// <summary>
        /// 计息明细
        /// </summary>
        public List<DetailInfo> DETAILS { get; set; }

        #endregion

        #region [ IMessageReqHandler Members ]

        public byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH];

            StringBuilder sb = new StringBuilder();
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthFigure(OPERATE_TYPE, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(ACCOUNT_DATE, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(NOTICE_NO, 20));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthFigure(NOTICE_TYPE, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthFigure(BUSINESS_TYPE, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(HANDLE_ORGNAZTION, 6));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(APPROVE_ORGNAZTION, 6));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(ACCOUNT, 22));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CURRENCY, 3));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthFigure(CUSTOMER_TYPE, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthFigure(CASH_PROPERTY, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(INTEREST_ACCOUNT, 22));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthFigure(AMOUNT, 17));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthFigure(INTEREST, 17));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BANK_FLAG, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(HANDLE_TELLER, 7));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(APPROVE_TELLER, 7));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(RESERVE_1, 80));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(RESERVE_2, 256));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthFigure(DETAIL_NUMBER, 2));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);

            if (OPERATE_TYPE == "1")//新增
            {
                int offset = 479;
                foreach (var item in DETAILS)
                {
                    byte[] buffer = item.ToBytes();
                    Array.Copy(buffer, 0, bytes, offset, DetailInfo.TOTAL_WIDTH);
                    offset += DetailInfo.TOTAL_WIDTH;
                }
            }

            return bytes;
        }

        #endregion
    }

    public class DetailInfo : IMessageReqHandler
    {
        public const UInt16 TOTAL_WIDTH = 63;

        /// <summary>
        /// 起息日期 8 
        /// </summary>
        public String VALUE_DATE { get; set; }

        /// <summary>
        /// 到期日期 8
        /// </summary>
        public String MATURITY_DATE { get; set; }

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

        #region [ IMessageReqHandler Members ]

        public byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH];

            StringBuilder sb = new StringBuilder();
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(VALUE_DATE, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(MATURITY_DATE, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthFigure(RATE, 11));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthFigure(CHARGE_NUMBER, 19));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthFigure(INTEREST, 17));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);

            return bytes;
        }

        #endregion
    }
}
