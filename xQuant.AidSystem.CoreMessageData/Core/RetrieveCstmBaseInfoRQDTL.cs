using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class RetrieveCstmBaseInfoRQDTL : IMessageReqHandler
    {
        public const UInt16 TOTAL_WIDTH = 23;
        /// <summary>
        /// 证件类型,3.202—营业执照; 203—企业法人代码; 305—金融经营许可证 
        /// </summary>
        public String DOC_TYPE
        {
            get;
            set;
        }

        /// <summary>
        /// 证件号码，20
        /// </summary>
        public String DOC_NO
        {
            get;
            set;
        }
        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH];
            StringBuilder sb = new StringBuilder();
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(DOC_TYPE, 3));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(DOC_NO, 20));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            return bytes;
        }

        #endregion
    }
}
