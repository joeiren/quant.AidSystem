using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class RetrieveCstmRQDTL : IMessageReqHandler
    {
        public const UInt16 TOTAL_WIDTH = 11;

        /// <summary>
        /// 客户内码,11位
        /// </summary>
        public String CUS_CDE
        {
            get;
            set;
        }

        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            String interID = CommonDataHelper.FillSpecifyWidthString(CUS_CDE, 11);
            byte[] bytes = new byte[TOTAL_WIDTH * 2];
            int len = EBCDICEncoder.WideCharToEBCDIC(EBCDICEncoder.CCSID_IBM_1388, interID, interID.Length, bytes, bytes.Length);
            if (len != bytes.Length)
            {
                return CommonDataHelper.SubBytes(bytes, 0, len);
            }
            return bytes;
        }

        #endregion
    }
}
