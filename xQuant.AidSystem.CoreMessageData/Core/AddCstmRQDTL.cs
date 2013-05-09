using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class AddCstmRQDTL : IMessageReqHandler
    {
        public const UInt16 TOTAL_WIDTH = 352;

        #region Property
        /// <summary>
        /// 客户号,23
        /// </summary>
        public String CUS_NO
        {
            get;
            set;
        }
        /// <summary>
        /// 客户类型,1
        /// </summary>
        public String CUS_TYP
        {
            get
            {
                return "3";
            }
            set { }
        }
        /// <summary>
        /// 客户名称,80
        /// </summary>
        public String CUS_NAM
        {
            get;
            set;
        }
        /// <summary>
        /// 英文名称,80
        /// </summary>
        public String CUS_ENAM
        {
            get;
            set;
        }
        /// <summary>
        /// 其他名称,30
        /// </summary>
        public String CUS_ONAM
        {
            get;
            set;
        }
        /// <summary>
        /// 国籍,3
        /// </summary>
        public String NATION
        {
            get
            {
                return "156";
            }
            set { }
        }
        /// <summary>
        /// 贵宾类型,1
        /// </summary>
        public String VIP_TYP
        {
            get
            {
                return "4";
            }
            set { }
        }
        /// <summary>
        /// 客户状态,1
        /// </summary>
        public String CUS_STS
        {
            get
            {
                return "1";
            }
            set { }
        }
        /// <summary>
        /// 地址,80
        /// </summary>
        public String ADDR
        {
            get;
            set;
        }
        /// <summary>
        /// 电话号码,20
        /// </summary>
        public String TEL_NO
        {
            get;
            set;
        }
        /// <summary>
        /// 手机号码,20
        /// </summary>
        public String MBL_NO
        {
            get;
            set;
        }
        /// <summary>
        /// 邮编,10
        /// </summary>
        public String ZIP
        {
            get;
            set;
        }
        /// <summary>
        /// 区域类型,3
        /// </summary>
        public String CMB_QYLX
        {
            get
            {
                return "222";
            }
            set { }
        }
        
        #endregion

        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH];

            StringBuilder sb = new StringBuilder();
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CUS_NO, 23));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CUS_TYP, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CUS_NAM, 80));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CUS_ENAM, 80));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CUS_ONAM, 30));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(NATION, 3));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(VIP_TYP, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CUS_STS, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(ADDR, 80));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(TEL_NO, 20));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(MBL_NO, 20));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(ZIP, 10));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CMB_QYLX, 3));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            return bytes;
        }

        #endregion
    }
}
