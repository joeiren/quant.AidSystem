using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 同业拆借请求协议报文
    /// </summary>
    public class PayInterBankRQ : IMessageReqHandler
    {
        public const UInt32 TOTAL_WIDTH = 370 + PaymentBizMsgDataBase.HEADER_WIDTH;

        #region Property
        //资金流水号,X22
        public String TransferFlowNo
        {
            get;
            set;
        }
        //交易机构号,X6
        public String PayBank
        {
            get;
            set;
        }
        //交易日期,X8
        public String TranDate
        {
            get;
            set;
        }
        //付款账号,X32
        public String PayAccount
        {
            get;
            set;
        }
        //付款人名称,X60
        public String PayAccountName
        {
            get;
            set;
        }
        //挂账序号,X14;若无则填“0”
        private String _onAccountSN;
        public String OnAccountSN
        {
            get
            {
                if (String.IsNullOrEmpty(_onAccountSN))
                {
                    return "0";
                }
                else
                {
                    return _onAccountSN;
                }
 
            }
            set
            {
                _onAccountSN = value;
            }
        }
        //收款账号,X32
        public String RecAccount
        {
            get;
            set;
        }
        //收款人名称,X60
        public String RecAccountName
        {
            get;
            set;
        }
        //收款行行号,X12
        public String RecAccountBanks
        {
            get;
            set;
        }
        //渠道标志,X1；0：大额1：农信银
        public String PackageChannelType
        {
            get;
            set;
        }
        //币种,X3;CNY
        public String CurrencyType
        {
            get
            {
                return "CNY";
            }
            set { }
        }
        //交易金额,S15.2
        public String PayAmount
        {
            get;
            set;
        }
        //拆借利率,X7(7位长度，字符串类型)
        public String Rate
        {
            get;
            set;
        }
        //拆借期限,N5
        public String TimeLimit
        {
            get;
            set;
        }
        //业务种类,X2
        public String BizType
        {
            get;
            set;
        }
        //手续费,S15.2
        public String Fee
        {
            get;
            set;
        }
        //备注,X60
        public String Remark
        {
            get;
            set;
        }
        //录入员,X7
        public String Teller
        {
            get;
            set;
        }
        //授权员,X7
        public String AuthTeller
        {
            get
            {
                return String.Empty;
            }
            set { }
        }
        //上送渠道,X2
        public String ChannelId
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
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(TransferFlowNo, 22));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(PayBank, 6));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(TranDate, 8));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(PayAccount, 32));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(PayAccountName, 60));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(OnAccountSN, 14));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(RecAccount, 32));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(RecAccountName, 60));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(RecAccountBanks, 12));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(PackageChannelType, 1));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CurrencyType, 3));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWith0(PayAmount, 15));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(Rate, 7));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(TimeLimit, 5));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BizType, 2));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWith0(Fee, 15));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(Remark, 60));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(Teller, 7));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(AuthTeller, 7));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(ChannelId, 2));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            return bytes;
        }

        #endregion
    }
}
