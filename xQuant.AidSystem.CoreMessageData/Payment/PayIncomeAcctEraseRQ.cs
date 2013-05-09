using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 来帐销账后的抹帐交易
    /// </summary>
    public class PayIncomeAcctEraseRQ : IMessageReqHandler
    {
        public const UInt32 TOTAL_WIDTH = 41 + PaymentBizMsgDataBase.HEADER_WIDTH;

        #region Property

        ///支付交易序号,X8
        public String PayTransSN
        {
            get;
            set;
        }
        //发起行行号,12
        public String SrcBankNO
        {
            get;
            set;
        }
        //原委托日期,X8
        public String OriDelegateDate
        {
            get;
            set;
        }
        /// <summary>
        /// 渠道标志，0 大额 1 农信银
        /// </summary>
        public String ChangelType
        {
            get;
            set;
        }
        //主机流水号,X12
        public String HostTranFlowNo
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
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(PayTransSN, 8));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(SrcBankNO, 12));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(OriDelegateDate, 8));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(ChangelType, 1));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(HostTranFlowNo, 12));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
         
            return bytes;
        }

        #endregion
    }
}
