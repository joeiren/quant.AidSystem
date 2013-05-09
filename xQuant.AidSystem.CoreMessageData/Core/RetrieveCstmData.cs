using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace xQuant.AidSystem.CoreMessageData
{
    public class RetrieveCstmData : CoreBizMsgDataBase
    {
        #region Property
        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                // DBH + RQHDR + DBH + RQDTL
                return CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + RetrieveCstmRQDTL.TOTAL_WIDTH;
            }
        }

        public RetrieveCstmODATA OData
        {
            get;
            set;
        }
        public RetrieveCstmRQDTL RQDTL
        {
            get;
            set;
        }

        public RetrieveCstmData()
            : base()
        {
            OData = new RetrieveCstmODATA();
            RQDTL = new RetrieveCstmRQDTL();
        }
        #endregion

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            OData = (RetrieveCstmODATA)OData.FromBytes(buffer);
        }

        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, RetrieveCstmRQDTL.TOTAL_WIDTH);
            return dest;
        }

        protected override ushort GetRQDTLLen()
        {
            return RetrieveCstmRQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            return RetrieveCstmODATA.TOTAL_WIDTH;
        }
    }
}
