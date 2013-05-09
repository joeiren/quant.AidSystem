using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class TellerAuthRQDTL: IMessageReqHandler
    {
        public const UInt16 TOTAL_WIDTH = 66;
        #region Property

        /// <summary>
        /// 方式，1位
        /// </summary>
        public String SignType
        {
            get;
            set;
        }

        /// <summary>
        /// 机构号,6位
        /// </summary>
        public String ORG_NO
        {
            get;
            set;
        }
        /// <summary>
        /// 柜员号,7位
        /// </summary>
        public String STF_NO
        {
            get;
            set;
        }
        /// <summary>
        /// 卡号,20位
        /// </summary>
        public String CARD_NO
        {
            get
            {
                return CommonDataHelper.SpaceString(20);
            }
            set
            { }
        }
        /// <summary>
        /// 柜员密码,24位（经过加密服务器加密后的密文）
        /// </summary>
        public byte[] PIN_BLK
        {
            get;
            set;
        }
        /// <summary>
        /// 系统日期，8
        /// </summary>
        public String SystemDate
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
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(SignType, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(ORG_NO, 6));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(STF_NO, 7));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            
            if (PIN_BLK == null)
            {
                PIN_BLK = new byte[24];
            }
            Array.Copy(PIN_BLK, 0, bytes, totalLen, PIN_BLK.Length);
            totalLen += 24;
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CARD_NO, 20));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(SystemDate, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            return bytes;
        }

        #endregion
    }
}
