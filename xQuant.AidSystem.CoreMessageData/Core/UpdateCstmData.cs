using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class UpdateCstmData : CoreBizMsgDataBase
    {

        #region Property
        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                // DBH + RQHDR + DBH + RQDTL
                return CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + UpdateCstmRQDTL.TOTAL_WIDTH;
            }
        }

        public UpdateCstmODATA OData
        {
            get;
            set;
        }
        public UpdateCstmRQDTL RQDTL
        {
            get;
            set;
        }

        public UpdateCstmData()
            : base()
        {
            OData = new UpdateCstmODATA();
            RQDTL = new UpdateCstmRQDTL();
        }
        #endregion
        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, UpdateCstmRQDTL.TOTAL_WIDTH);
            return dest;
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            OData = (UpdateCstmODATA)OData.FromBytes(buffer);
        }

        protected override ushort GetRQDTLLen()
        {
            return UpdateCstmRQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            return UpdateCstmODATA.TOTAL_WIDTH;
        }
    }
}
