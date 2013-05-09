using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 登记簿查询请求协议报文
    /// </summary>
    public class PayRegisterRetrievedRQ : IMessageReqHandler
    {
        public const UInt32 TOTAL_WIDTH = 43 + PaymentBizMsgDataBase.HEADER_WIDTH;

        #region Property

        //交易机构号,X6
        public String PayBank
        {
            get;
            set;
        }
     
        //交易柜员,X7
        public String TradTeller
        {
            get;
            set;
        }
        //原委托日期(查询交易流水的日期),X8
        public String OriDelegateDate
        {
            get;
            set;
        }
        //资金流水号,X22
        public String TransferFlowNo
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
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(TradTeller, 7));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(OriDelegateDate, 8));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(TransferFlowNo, 22));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            return bytes;
        }

        #endregion
    }
}
