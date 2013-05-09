using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 县级行社上存清算资金查询
    /// </summary>
    public class DepositClearingFundRQDTL : IMessageReqHandler
    {
        public const UInt16 TOTAL_WIDTH = 12;
        #region Property
        /// <summary>
        /// 机构号,6
        /// </summary>
        public String OrgNO
        {
            get
            {
                return CommonDataHelper.SpaceString(20);
            }
            set { }
        }
        /// <summary>
        /// 地区号,2
        /// </summary>
        public String DistrictNO
        {
            get;
            set;
        }
        /// <summary>
        /// 币种,3
        /// </summary>
        public String Currency
        {
            get;
            set;
        }
        /// <summary>
        /// 选项列表,1;1-全部，2-负金额
        /// </summary>
        public String Option
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
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(OrgNO, 6));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(DistrictNO, 2));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(Currency, 3));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(Option, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            return bytes;
        }

        #endregion
    }
}
