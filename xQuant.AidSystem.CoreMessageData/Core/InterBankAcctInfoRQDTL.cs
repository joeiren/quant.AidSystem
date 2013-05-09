using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 同业存放账户查询请求结构
    /// </summary>
    public class InterBankAcctInfoRQDTL : IMessageReqHandler
    {
        #region [ Property ]

        public const UInt16 TOTAL_WIDTH = 34;

        /// <summary>
        /// 账号 22
        /// </summary>
        public string AccountNO { get; set; }

        /// <summary>
        /// 币种 3 
        /// </summary>
        public string Currency { get { return "CNY"; } }

        /// <summary>
        /// 钞汇属性 1 
        /// </summary>
        public string CashFlag { get { return "1"; } }

        /// <summary>
        /// 册号 3
        /// </summary>
        public string CatalogNO { get; set; }

        /// <summary>
        /// 序号 3
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 股金种类 1
        /// </summary>
        public string StockKind { get; set; }

        /// <summary>
        /// 销户检查标志 1
        /// </summary>
        public string CancelChecker { get; set; }

        #endregion
        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH];

            StringBuilder sb = new StringBuilder();
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(AccountNO, 22));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(Currency, 3));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CashFlag, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CatalogNO, 3));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(SN, 3));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(StockKind, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CancelChecker, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);

            return bytes;
        }

        #endregion
    }
}
