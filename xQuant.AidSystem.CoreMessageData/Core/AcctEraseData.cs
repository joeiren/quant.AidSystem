using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 抹账交易的报文数据
    /// </summary>
    public class AcctEraseData : CoreBizMsgDataBase
    {
        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                // DBH + RQHDR  此交易没有RQDTL,所以只有一个DBH
                return CoreDataBlockHeader.TOTAL_WIDTH + RQHDR_MsgHandler.TOTAL_WIDTH;
            }
        }

        public AcctEraseODATA OData
        {
            get;
            set;
        }

        public AcctEraseData() : base()
        {
            OData = new AcctEraseODATA();
        }

        #region Implemented Methods from Base class
        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            return dest;
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            OData = (AcctEraseODATA)OData.FromBytes(buffer);
        }

        protected override ushort GetRQDTLLen()
        {
            return 0;
        }

        protected override ushort GetODATALen()
        {
            return (UInt16)OData.TOTAL_WIDTH;
        }
        #endregion

        public override bool OnArgumentsValidation()
        {
            StringBuilder msg = new StringBuilder();
            if (string.IsNullOrEmpty(RQhdrHandler.HOST_JNO))
            {
                msg.Append("抹帐用主机流水号不能为空！");
            }
            if (msg.Length > 0)
            {
                throw new BizArgumentsException(msg.ToString());
            }
            
            return true;
        }
   
    }
}
