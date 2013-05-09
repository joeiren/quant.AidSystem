using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 实时资金头寸查询接口--内部账查询
    /// </summary>
    public class InterAcctRetrieveRQDTL : IMessageReqHandler
    {
        public const UInt16 TOTAL_WIDTH = 48;
        #region Property
        /// <summary>
        /// 账号,20
        /// </summary>
        public String AccountNO
        {
            get
            {
                return CommonDataHelper.SpaceString(20);
            }
            set { }
        }
        /// <summary>
        /// 归属机构,6
        /// </summary>
        public String OrgnaztionNO
        {
            get;
            set;
        }
        /// <summary>
        /// 核算码，8
        /// </summary>
        public String CheckCode
        {
            get;
            set;
        }
        /// <summary>
        /// 科目号，6
        /// </summary>
        public String SubjectNO
        {
            get;
            set;
        }
        /// <summary>
        /// 币种，3
        /// </summary>
        public String Currcency
        {
            get
            {
                return "CNY";
            }
            set { }
        }
        /// <summary>
        /// 顺序号,4
        /// </summary>
        public String SequenceNO
        {
            get;
            set;
        }
        /// <summary>
        /// 余额查询方式，1(1- 全部;2- 正余额;3- 负余额)
        /// </summary>
        public String BalanceQueryType
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
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(AccountNO, 20));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(OrgnaztionNO, 6));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CheckCode, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(SubjectNO, 6));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(Currcency, 3));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(SequenceNO, 4));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BalanceQueryType, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            return bytes;
        }

        #endregion
    }
}
