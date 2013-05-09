using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class RetrieveCstmBaseInfoData : CoreBizMsgDataBase
    {
        #region Property
        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                // DBH + RQHDR + DBH + RQDTL
                return CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + RetrieveCstmBaseInfoRQDTL.TOTAL_WIDTH;
            }
        }

        public RetrieveCstmBaseInfoODATA OData
        {
            get;
            set;
        }
        public RetrieveCstmBaseInfoRQDTL RQDTL
        {
            get;
            set;
        }

        public RetrieveCstmBaseInfoData()
            : base()
        {
            OData = new RetrieveCstmBaseInfoODATA();
            RQDTL = new RetrieveCstmBaseInfoRQDTL();
        }
        #endregion
        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, RetrieveCstmBaseInfoRQDTL.TOTAL_WIDTH);
            return dest;
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            OData = (RetrieveCstmBaseInfoODATA)OData.FromBytes(buffer);
        }

        protected override ushort GetRQDTLLen()
        {
            return RetrieveCstmBaseInfoRQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            return RetrieveCstmBaseInfoODATA.TOTAL_WIDTH;
        }
    }
}
