using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class PayAcctCheckData : PaymentBizMsgDataBase
    {
        public PayAcctCheckRP RPData
        {
            get;
            set;
        }

        public PayAcctCheckRQ RQData
        {
            get;
            set;
        }
        public PayAcctCheckData()
            : base()
        {
            RPData = new PayAcctCheckRP();
            RQData = new PayAcctCheckRQ();
            TradeCode = "ZJ0040";
        }
        public override uint RP_TOTAL_WIDTH
        {
            get
            {
                return (uint)RPData.TOTAL_WIDTH;
            }
            set
            {
            }
        }

        public override uint RQ_TOTAL_WIDTH
        {
            get
            {
                return PayAcctCheckRQ.TOTAL_WIDTH;
            }
            set
            {
            }
        }
        public override byte[] ReqToBytes()
        {
            return RQData.ToBytes();
        }

        public override void RespFromBytes(byte[] bytes)
        {
            RPData.FromBytes(bytes);
        }
    }
}
