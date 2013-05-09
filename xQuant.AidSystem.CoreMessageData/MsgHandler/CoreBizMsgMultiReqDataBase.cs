using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public abstract class CoreBizMsgMultiReqDataBase : CoreBizMsgDataBase
    {
        protected abstract override byte[] RQDTL_ToBytes(byte[] dest);

        protected abstract override void ODATA_FromBytes(byte[] buffer);

        protected abstract override ushort GetRQDTLLen();

        protected abstract override ushort GetODATALen();

        protected List<IBDATA_MsgHandler> IBDATACollection
        {
            get;
            set;
        }

        public new byte[] ToBytes()
        {

            if (IBDATACollection == null || IBDATACollection.Count == 0)
            {
                base.MessageHeaderLastFlag = true;
                return base.ToBytes();
            }
            else
            {
                base.MessageHeaderLastFlag = false;
                byte[] regularBytes = base.ToBytes();
                int index = 0;
                
                int ibdataLength = (from ibdata in IBDATACollection
                                   where ibdata != null && ibdata.TOTAL_WIDTH > 0
                                    select ibdata.TOTAL_WIDTH).Sum() + IBDATACollection.Count * (CoreDataBlockHeader.TOTAL_WIDTH + CoreMessageHeader.TOTAL_WIDTH);
                byte [] result = new byte[ibdataLength + regularBytes.Length];
                Array.Copy(regularBytes, result, regularBytes.Length);
                int offset = regularBytes.Length;
                foreach (var ibdata in IBDATACollection)
                {
                    CoreMessageHeader msgHeader = new CoreMessageHeader();
                    msgHeader.MH_MESSAGE_LENGTH = (uint)(CoreMessageHeader.TOTAL_WIDTH + CoreDataBlockHeader.TOTAL_WIDTH + ibdata.TOTAL_WIDTH);
                    msgHeader.MH_LAST_FLAG = (++index == IBDATACollection.Count) ? true : false;
                    Array.Copy(msgHeader.ToBytes(), 0, result, offset,  CoreMessageHeader.TOTAL_WIDTH);
                    offset += CoreMessageHeader.TOTAL_WIDTH;
                    
                    CoreDataBlockHeader dbhdr = new CoreDataBlockHeader();
                    dbhdr.DBH_DB_LENGTH = (uint)(CoreDataBlockHeader.TOTAL_WIDTH + ibdata.TOTAL_WIDTH);
                    dbhdr.DBH_DB_ID = "@IBDATA";
                    Array.Copy(dbhdr.ToBytes(), 0, result, offset, CoreDataBlockHeader.TOTAL_WIDTH);
                    offset += CoreDataBlockHeader.TOTAL_WIDTH;
                    Array.Copy(ibdata.ToBytes(), 0, result, offset, ibdata.TOTAL_WIDTH);
                    offset += ibdata.TOTAL_WIDTH;
                }

                return result;                
            }
        }

        public virtual bool OnArgumentsValidation()
        {
            return base.OnArgumentsValidation();
        }

    }
}
