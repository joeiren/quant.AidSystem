using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class AddCstmData : CoreBizMsgDataBase
    {
        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                // DBH + RQHDR + DBH + RQDTL
                return CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + AddCstmRQDTL.TOTAL_WIDTH;
            }
        }

        public AddCstmODATA OData
        {
            get;
            set;
        }
        public AddCstmRQDTL RQDTL
        {
            get;
            set;
        }

        public AddCstmData()
            : base()
        {
            OData = new AddCstmODATA();
            RQDTL = new AddCstmRQDTL();
        }


        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, AddCstmRQDTL.TOTAL_WIDTH);
            return dest;
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            OData = (AddCstmODATA)OData.FromBytes(buffer);
        }

        protected override ushort GetRQDTLLen()
        {
            return AddCstmRQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            return AddCstmODATA.TOTAL_WIDTH;
        }
    }
}
