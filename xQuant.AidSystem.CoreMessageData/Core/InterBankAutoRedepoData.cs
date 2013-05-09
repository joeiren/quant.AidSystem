/*
* 版权所有：杭州衡泰软件
* 作    者：林尚都(linshangdou@yahoo.com.cn)
* 创建时间：2011-6-27 18:07:58
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

namespace xQuant.AidSystem.CoreMessageData
{
    //自动转存
    public class InterBankAutoRedepoData : CoreBizMsgDataBase
    {
        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                // DBH + RQHDR + DBH + RQDTL
                return CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + InterBankAutoRedepoRQDTL.TOTAL_WIDTH;
            }
        }

        public InterBankAutoRedepoRQDTL RQDTL { get; set; }

        public InterBankAutoRedepoData()
        {
            RQDTL=new InterBankAutoRedepoRQDTL();
        }

        #region [ 实现基类方法 ]

        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, InterBankAutoRedepoRQDTL.TOTAL_WIDTH);
            return dest;
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        protected override ushort GetRQDTLLen()
        {
            return InterBankAutoRedepoRQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
