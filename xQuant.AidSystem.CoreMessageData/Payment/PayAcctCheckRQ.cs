using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class PayAcctCheckRQ : IMessageReqHandler
    {
        public const UInt32 TOTAL_WIDTH = 8 + PaymentBizMsgDataBase.HEADER_WIDTH;
        #region Property
        /// <summary>
        /// 请求日期
        /// </summary>
        public String RequestDate
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
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(RequestDate, 8));
            CommonDataHelper.ResetGBKByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            return bytes;
        }

        #endregion
    }
}
