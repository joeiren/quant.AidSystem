using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class UpdateCstmRQDTL: IMessageReqHandler
    {

        public const UInt16 TOTAL_WIDTH = 343;
        #region Property
        /// <summary>
        /// 客户内码,11位
        /// </summary>
        public String CUS_CDE
        {
            get;
            set;
        }
        /// <summary>
        /// 客户名称,长度80
        /// </summary>
        public String CUS_NAM
        {
            get;
            set;
        }
        /// <summary>
        /// 其他名称，长度30
        /// </summary>
        public String CUS_ONAM
        {
            get;
            set;
        }
        /// <summary>
        /// 英文名称，长度80
        /// </summary>
        public String CUS_ENAM
        {
            get;
            set;
        }
        /// <summary>
        /// 国籍，长度3(国家代码，默认156)
        /// </summary>
        public String NATION
        {
            get
            {
                return "156";
            }
            set
            { }
        }
        /// <summary>
        /// 贵宾类型，1位
        /// </summary>
        public String VIP_TYP
        {
            get
            {
                return "4";
            }
            set
            { }
        }
        /// <summary>
        /// 客户状态,1位
        /// </summary>
        public String CUS_STS
        {
            get
            {
                if (IsDelete)
                {
                    return "3";
                }
                else
                {
                    return "1";
                }
            }
            set
            { }
        }
        /// <summary>
        /// 地址序号,2位
        /// </summary>
        public String ADD_SN
        {
            get
            {
                return "01";
            }
        }
        /// <summary>
        /// 地址,长度80
        /// </summary>
        public String ADDR
        {
            get;
            set;
        }
        /// <summary>
        /// 电话序号,2位
        /// </summary>
        public String TEL_SN
        {
            get
            {
                return "01";
            }
            set
            { }
        }
        /// <summary>
        /// 电话号码，长度20
        /// </summary>
        public String TEL_NO
        {
            get;
            set;
        }
        /// <summary>
        /// 手机号码，长度20
        /// </summary>
        public String MBL_NO
        {
            get;
            set;
        }
        /// <summary>
        /// 邮编，长度10
        /// </summary>
        public String ZIP
        {
            get;
            set;
        }
        /// <summary>
        /// 区域类型，长度3
        /// </summary>
        public String CMB_QYLX
        {
            get
            {
                return "222";
            }
            set
            { }
        }

        private Boolean _isDelete = false;
        /// <summary>
        /// 删除客户基本信息标志
        /// </summary>
        public Boolean IsDelete
        {
            get
            {
                return _isDelete;
            }
            set
            {
                _isDelete = value;
            }
        }
        #endregion
        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH];

            StringBuilder sb = new StringBuilder();
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CUS_CDE, 11));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CUS_NAM, 80));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CUS_ONAM, 30));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CUS_ENAM, 80));
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
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(ADD_SN, 2));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(ADDR, 80));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(TEL_SN, 2));
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
