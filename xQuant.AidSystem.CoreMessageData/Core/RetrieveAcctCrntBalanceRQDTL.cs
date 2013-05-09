using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 资金业务账号余额查询
    /// </summary>
    public class RetrieveAcctCrntBalanceRQDTL : IMessageReqHandler
    {
        public int TOTAL_WIDTH
        {
            get
            {
                return AcctCount * AcctCrntBalanceRQDTLItem.TOTAL_WIDTH + 2;
            }
        }
        public int AcctCount
        {
            get;
            set;
        }

        public List<AcctCrntBalanceRQDTLItem> AcctList
        {
            get;
            set;
        }

        public RetrieveAcctCrntBalanceRQDTL()
        {
            AcctList = new List<AcctCrntBalanceRQDTLItem>();
        }

        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH];

            StringBuilder sb = new StringBuilder();
            sb = sb.AppendFormat(AcctCount.ToString().PadLeft(2));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            int offset = totalLen;
            foreach (AcctCrntBalanceRQDTLItem item in AcctList)
            {
                if (offset < bytes.Length)
                {
                    Array.Copy(item.ToBytes(), 0, bytes, offset, AcctCrntBalanceRQDTLItem.TOTAL_WIDTH);
                    offset += AcctCrntBalanceRQDTLItem.TOTAL_WIDTH;
                }
                else
                {
                    break;
                }
                
            }            
            return bytes;
        }

        #endregion
    }

    public class AcctCrntBalanceRQDTLItem : IMessageReqHandler
    {
        public const int TOTAL_WIDTH = 24;
        /// <summary>
        /// 账号,20
        /// </summary>
        public String AcctNO
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
        /// 账号性质,1;1-表内内部账，2-存款账号
        /// </summary>
        public String AcctProperty
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
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(AcctNO, 20));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(Currency, 3));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(AcctProperty, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
     
            return bytes;
        }

        #endregion
    }
}
