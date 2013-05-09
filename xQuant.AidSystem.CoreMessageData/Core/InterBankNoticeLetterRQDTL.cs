/*
* 版权所有：杭州衡泰软件
* 作    者：林尚都(linshangdou@yahoo.com.cn)
* 创建时间：2011-3-24 10:42:35
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
    /// <summary>
    /// 通知单查询
    /// </summary>
    public class InterBankNoticeLetterRQDTL : IMessageReqHandler
    {
        #region [ Property ]

        public const UInt16 TOTAL_WIDTH = 287;

        /// <summary>
        /// 查询类型 1
        /// </summary>
        public String QUERY_TYPE { get { return "1"; } }

        /// <summary>
        /// 日期 8 (会计日期)
        /// </summary>
        public String ACCOUNT_DATE { get; set; }

        /// <summary>
        /// 通知单编号 20 (必输)
        /// </summary>
        public String NOTICE_NO { get; set; }

        /// <summary>
        /// 通知单类型 1 (1-开户 2-销户 3-部提)
        /// </summary>
        public String NOTICE_TYPE { get; set; }

        /// <summary>
        /// 业务类型 1 (1-同业活期 2-同业定期)
        /// </summary>
        public String BUSINESS_TYPE { get; set; }

        /// <summary>
        /// 备用 256 
        /// </summary>
        public String RESERVE { get { return CommonDataHelper.SpaceString(256); } }

        #endregion

        #region [ 实现接口 ]

        public byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH];

            StringBuilder sb = new StringBuilder();
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthFigure(QUERY_TYPE, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(ACCOUNT_DATE, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(NOTICE_NO, 20));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthFigure(NOTICE_TYPE, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthFigure(BUSINESS_TYPE, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(RESERVE, 256));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);

            return bytes;
        }

        #endregion
    }
}
