using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 存放省内上级机构活期款项查询
    /// </summary>
    public class SuperiorCurrentAcctData : CoreBizMsgDataBase
    {
        #region Property
        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                // DBH + RQHDR + DBH + RQDTL
                return CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + SuperiorCurrentAcctRQDTL.TOTAL_WIDTH;
            }
        }

        public SuperiorCurrentAcctODATA OData
        {
            get;
            set;
        }
        public SuperiorCurrentAcctRQDTL RQDTL
        {
            get;
            set;
        }

        public SuperiorCurrentAcctData()
            : base()
        {
            OData = new SuperiorCurrentAcctODATA();
            RQDTL = new SuperiorCurrentAcctRQDTL();
        }
        #endregion

        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, SuperiorCurrentAcctRQDTL.TOTAL_WIDTH);
            return dest;
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            OData = (SuperiorCurrentAcctODATA)OData.FromBytes(buffer);
        }

        protected override ushort GetRQDTLLen()
        {
            return SuperiorCurrentAcctRQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            return (UInt16)OData.TOTAL_WIDTH;
        }
    }
}
