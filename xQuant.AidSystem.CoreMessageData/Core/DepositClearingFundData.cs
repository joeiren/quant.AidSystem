using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 县级行社上存清算资金查询
    /// </summary>
    public class DepositClearingFundData : CoreBizMsgDataBase
    {
        #region Property
        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                // DBH + RQHDR + DBH + RQDTL
                return CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + DepositClearingFundRQDTL.TOTAL_WIDTH;
            }
        }

        public DepositClearingFundODATA OData
        {
            get;
            set;
        }
        public DepositClearingFundRQDTL RQDTL
        {
            get;
            set;
        }

        public DepositClearingFundData()
            : base()
        {
            OData = new DepositClearingFundODATA();
            RQDTL = new DepositClearingFundRQDTL();
        }
        #endregion

        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, DepositClearingFundRQDTL.TOTAL_WIDTH);
            return dest;
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            OData = (DepositClearingFundODATA)OData.FromBytes(buffer);
        }

        protected override ushort GetRQDTLLen()
        {
            return DepositClearingFundRQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            return (UInt16)OData.TOTAL_WIDTH;
        }
    }
}
