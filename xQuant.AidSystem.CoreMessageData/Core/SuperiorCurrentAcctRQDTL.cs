using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 存放省内上级机构活期款项查询
    /// </summary>
    public class SuperiorCurrentAcctRQDTL : IMessageReqHandler
    {
        public const UInt16 TOTAL_WIDTH = 3;
        #region Property
        /// <summary>
        /// 币种,3
        /// </summary>
        public String Currcency
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
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(Currcency, 3));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            
            return bytes;
        }

        #endregion
    }
}
