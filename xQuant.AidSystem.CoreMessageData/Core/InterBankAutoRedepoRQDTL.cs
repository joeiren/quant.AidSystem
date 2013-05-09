/*
* 版权所有：杭州衡泰软件
* 作    者：林尚都(linshangdou@yahoo.com.cn)
* 创建时间：2011-6-27 18:13:07
* 版 本 号：1.0.0
* 模块说明：
* ----------------------------------
* 修改记录：
* 日    期：
* 版 本 号：
* 修 改 人：
* 修改内容：
* 
*/


using System;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class InterBankAutoRedepoRQDTL : IMessageReqHandler
    {
        #region [ Property ]

        public const UInt16 TOTAL_WIDTH = 75;

        /// <summary>
        /// 账号 15
        /// </summary>
        public string ACCOUNT { get; set; }

        /// <summary>
        /// 币种 3 
        /// </summary>
        public string CURRENCY { get { return "CNY"; } }

        /// <summary>
        /// 钞汇属性 1 
        /// </summary>
        public string CASH_PROPERTY { get { return "1"; } }

        /// <summary>
        /// 新起息日期 8
        /// </summary>
        public string START_DATE { get; set; }

        /// <summary>
        /// 新到期日期 8
        /// </summary>
        public string MATURITY_DATE { get; set; }

        /// <summary>
        /// 备用 40
        /// </summary>
        public string RESERVE { get; set; }

        #endregion

        #region [ 实现接口 ]

        public byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH];

            StringBuilder sb = new StringBuilder();
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(ACCOUNT, 15));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CURRENCY, 3));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CASH_PROPERTY, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(START_DATE, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(MATURITY_DATE, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(RESERVE, 40));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);

            return bytes;
        }

        #endregion
    }
}
