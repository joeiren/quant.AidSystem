using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class TellerAuthData : CoreBizMsgDataBase
    {
        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                // DBH + RQHDR + DBH + RQDTL
                return CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + TellerAuthRQDTL.TOTAL_WIDTH;
            }
        }
        public TellerAuthODATA OData
        {
            get;
            set;
        }
        public TellerAuthRQDTL RQDTL
        {
            get;
            set;
        }

        public TellerAuthData()
            : base()
        {
            OData = new TellerAuthODATA();
            RQDTL = new TellerAuthRQDTL();
        }

        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, TellerAuthRQDTL.TOTAL_WIDTH);
            return dest;
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            OData = (TellerAuthODATA)OData.FromBytes(buffer);
        }

        protected override ushort GetRQDTLLen()
        {
            return TellerAuthRQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            return TellerAuthODATA.TOTAL_WIDTH;
        }
    }
}
