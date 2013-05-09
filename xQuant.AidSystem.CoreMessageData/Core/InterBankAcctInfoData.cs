using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 同业存放账户查询
    /// </summary>
    public class InterBankAcctInfoData : CoreBizMsgDataBase
    {
        #region Property
        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                // DBH + RQHDR + DBH + RQDTL
                return CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + InterBankAcctInfoRQDTL.TOTAL_WIDTH;
            }
        }

        public InterBankAcctInfoODATA OData
        {
            get;
            set;
        }
        public InterBankAcctInfoRQDTL RQDTL
        {
            get;
            set;
        }

        public InterBankAcctInfoData()
            : base()
        {
            OData = new InterBankAcctInfoODATA();
            RQDTL = new InterBankAcctInfoRQDTL();
        }
        #endregion


        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, InterBankAcctInfoRQDTL.TOTAL_WIDTH);
            return dest;
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            OData = (InterBankAcctInfoODATA)OData.FromBytes(buffer);
        }

        protected override ushort GetRQDTLLen()
        {
            return InterBankAcctInfoRQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            return InterBankAcctInfoODATA.TOTAL_WIDTH;
        }
    }
}
