using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 实时资金头寸查询接口--内部账查询
    /// </summary>
    public class InterAcctRetrieveData : CoreBizMsgDataBase
    {

        #region Property
        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                // DBH + RQHDR + DBH + RQDTL
                return CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + InterAcctRetrieveRQDTL.TOTAL_WIDTH;
            }
        }

        public InterAcctRetrieveODATA OData
        {
            get;
            set;
        }
        public InterAcctRetrieveRQDTL RQDTL
        {
            get;
            set;
        }

        public InterAcctRetrieveData()
            : base()
        {
            OData = new InterAcctRetrieveODATA();
            RQDTL = new InterAcctRetrieveRQDTL();
        }
        #endregion


        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, InterAcctRetrieveRQDTL.TOTAL_WIDTH);
            return dest;
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            OData = (InterAcctRetrieveODATA)OData.FromBytes(buffer);
        }

        protected override ushort GetRQDTLLen()
        {
            return InterAcctRetrieveRQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            return InterAcctRetrieveODATA.TOTAL_WIDTH;
        }
    }
}
