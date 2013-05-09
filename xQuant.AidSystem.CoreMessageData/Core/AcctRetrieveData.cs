using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 记账查询的报文数据
    /// </summary>
    public class AcctRetrieveData : CoreBizMsgDataBase
    {
        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                // DBH + RQHDR + DBH + RQDTL
                return CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + AcctRetrieveRQDTL.TOTAL_WIDTH;
            }
        }

        public AcctRetrieveODATA OData
        {
            get;
            set;
        }
        public AcctRetrieveRQDTL RQDTL
        {
            get;
            set;
        }

        public AcctRetrieveData()
            : base()
        {
            OData = new AcctRetrieveODATA();
            RQDTL = new AcctRetrieveRQDTL();
        }

        #region Implemented Methods from Base class

        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, AcctRetrieveRQDTL.TOTAL_WIDTH);
            return dest;
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            OData = (AcctRetrieveODATA)OData.FromBytes(buffer);
        }

        protected override ushort GetRQDTLLen()
        {
            return AcctRetrieveRQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            return (UInt16)OData.TOTAL_WIDTH;
        }
        #endregion
    }
}
