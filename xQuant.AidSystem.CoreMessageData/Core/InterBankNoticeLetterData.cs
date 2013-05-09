/*
* 版权所有：杭州衡泰软件
* 作    者：林尚都(linshangdou@yahoo.com.cn)
* 创建时间：2011-3-24 10:40:07
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
    public class InterBankNoticeLetterData : CoreBizMsgDataBase
    {
        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                // DBH + RQHDR + DBH + RQDTL
                return CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + InterBankNoticeLetterRQDTL.TOTAL_WIDTH;
            }
        }

        public InterBankNoticeLetterRQDTL RQDTL { get; set; }
        public InterBankNoticeLetterODATA OData{ get; set;}

        public InterBankNoticeLetterData()
            : base()
        {
            RQDTL = new InterBankNoticeLetterRQDTL();
            OData = new InterBankNoticeLetterODATA();
        }

        #region Implemented Methods from Base class

        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, InterBankNoticeLetterRQDTL.TOTAL_WIDTH);
            return dest;
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            OData = (InterBankNoticeLetterODATA)OData.FromBytes(buffer);
        }

        protected override ushort GetRQDTLLen()
        {
            return InterBankOpenAcctRQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            return InterBankNoticeLetterODATA.TOTAL_WIDTH;
        }

        public override bool OnArgumentsValidation()
        {
            StringBuilder msg = new StringBuilder();
            if (string.IsNullOrEmpty(RQDTL.NOTICE_NO))
            {
                msg.Append("通知单编号不能为空！");
            }
            if (msg.Length > 0)
            {
                throw new BizArgumentsException(msg.ToString());
            }

            return true;
        }

        #endregion
    }
}
