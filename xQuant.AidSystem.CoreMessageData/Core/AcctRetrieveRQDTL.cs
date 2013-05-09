using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class AcctRetrieveRQDTL : IMessageReqHandler
    {
        public const UInt16 TOTAL_WIDTH = 12;
        /// <summary>
        /// 核心交易流水号,12
        /// </summary>
        public String CoreTradeSN
        {
            get;
            set;
        }
        

        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            String sn = CommonDataHelper.FillSpecifyWidthString(CoreTradeSN, 12);
            byte[] bytes = new byte[TOTAL_WIDTH * 2];
            int len = EBCDICEncoder.WideCharToEBCDIC(EBCDICEncoder.CCSID_IBM_1388, sn, sn.Length, bytes, bytes.Length);
            if (len != bytes.Length)
            {
                return CommonDataHelper.SubBytes(bytes, 0, len);
            }
            return bytes;
        }

        #endregion
    }
}
