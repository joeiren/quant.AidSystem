using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 来账的销账请求协议报文
    /// </summary>
    public class PayVostroAcctEliminateRQ : IMessageReqHandler
    {
        public const UInt32 TOTAL_WIDTH = 219 + PaymentBizMsgDataBase.HEADER_WIDTH;

        #region Property

        //交易机构号,X6
        public String PayBank
        {
            get;
            set;
        }
        //操作员,X7
        public String Operator
        {
            get;
            set;
        }
        //渠道标志,X2
        public String PackageChannelType
        {
            get;
            set;
        }
        //业务类型,X2
        public String BizType
        {
            get;
            set;
        }
        //支付交易序号,X8
        public String TransSeq
        {
            get;
            set;
        }
        //发起行行号,X12
        public String AccountBanks
        {
            get;
            set;
        }
        //委托日期,X8
        public String DelegateDate
        {
            get;
            set;
        }
        //资金去向,X1
        public String FundDest
        {
            get
            {
                return "5";
            }
            set
            { }
        }
        //金额 ,S15.2
        public String Amount
        {
            get;
            set;
        }
        //入账账号,X32
        public String PostAcount
        {
            get;
            set;
        }
        //入账户名,X60
        public String PostAccountName
        {
            get;
            set;
        }
        //入账机构号,X6
        public String PostBank
        {
            get;
            set;
        }
        //入账机构名称,X60
        public String PostBankName
        {
            get;
            set;
        }
        #endregion
        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH - PaymentBizMsgDataBase.HEADER_WIDTH];

            StringBuilder sb = new StringBuilder();
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(PayBank, 6));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(Operator, 7));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(PackageChannelType, 2));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BizType, 2));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(TransSeq, 8));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(AccountBanks, 12));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(DelegateDate, 8));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(FundDest, 1));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWith0(Amount, 15));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(PostAcount, 32));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(PostAccountName, 60));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(PostBank, 6));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(PostBankName, 60));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            return bytes;
        }

        #endregion
    }
}
