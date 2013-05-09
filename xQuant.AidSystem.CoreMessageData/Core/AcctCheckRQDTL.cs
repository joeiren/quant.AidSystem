using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class AcctCheckRQDTL : IMessageReqHandler
    {
        public const UInt16 TOTAL_WIDTH = 32;
        #region Property
        /// <summary>
        /// 查询交易日期，8
        /// </summary>
        public String TradeDate
        {
            get;
            set;
        }
        /// <summary>
        /// 资金业务流水号,18
        /// </summary>
        public String BizFlowNO
        {
            get;
            set;
        }
        /// <summary>
        /// 机构号，6
        /// </summary>
        public String OrgNO
        {
            get;
            set;
        }
        #endregion

        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH];

            StringBuilder sb = new StringBuilder();
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(TradeDate, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BizFlowNO, 18));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(OrgNO, 6));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);

            return bytes;
        }

        #endregion
    }
}
