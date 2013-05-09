using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 资金业务账号余额查询
    /// </summary>
    public class RetrieveAcctCrntBalanceData : CoreBizMsgDataBase
    {
        #region Property
        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                // DBH + RQHDR + DBH + RQDTL
                return (UInt32)(CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + RQDTL.TOTAL_WIDTH);
            }
        }

        public RetrieveAcctCrntBalanceODATA OData
        {
            get;
            set;
        }
        public RetrieveAcctCrntBalanceRQDTL RQDTL
        {
            get;
            set;
        }
        #endregion

        public RetrieveAcctCrntBalanceData()
            : base()
        {
            OData = new RetrieveAcctCrntBalanceODATA();
            RQDTL = new RetrieveAcctCrntBalanceRQDTL();
        }
        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, RQDTL.TOTAL_WIDTH);
            return dest;
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            OData = (RetrieveAcctCrntBalanceODATA)OData.FromBytes(buffer);
        }

        protected override ushort GetRQDTLLen()
        {
            return (UInt16)RQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            return (UInt16)OData.TOTAL_WIDTH;
        }
    }
}
