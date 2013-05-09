using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class AcctCheckData : CoreBizMsgMultiRespDataBase
    {
        public AcctCheckODATA OData
        {
            get;
            set;
        }
        public AcctCheckRQDTL RQDTL
        {
            get;
            set;
        }

        private List<AcctCheckOBDataItem> _obDataList = null;
        public List<AcctCheckOBDataItem> OBDataList
        {
            get
            {
                return _obDataList;
            }
            set
            {
                _obDataList = value;
            }
        }

        public AcctCheckData():base()
        {
            OData = new AcctCheckODATA();
            RQDTL = new AcctCheckRQDTL();
            _obDataList = new List<AcctCheckOBDataItem>();
        }

        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                return CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + AcctCheckRQDTL.TOTAL_WIDTH;
            }
        }
        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, AcctCheckRQDTL.TOTAL_WIDTH);
            return dest;
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            OData = (AcctCheckODATA)OData.FromBytes(buffer);
        }

        protected override ushort GetRQDTLLen()
        {
            return AcctCheckRQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            return (UInt16)OData.TOTAL_WIDTH;
        }

        protected override void OBDATA_FromBytes(byte[] buffer)
        {
            //base.OBDATA_FromBytes(buffer);
            //...
            AcctCheckOBData obdata = new AcctCheckOBData();
            obdata = (AcctCheckOBData)obdata.FromBytes(buffer);

            _obDataList.AddRange(obdata._obDataItemList.ToList());

        }

    }
}
