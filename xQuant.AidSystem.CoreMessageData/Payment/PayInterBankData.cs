using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class PayInterBankData : PaymentBizMsgDataBase
    {
        public PayInterBankRP RPData
        {
            get;
            set;
        }

        public PayInterBankRQ RQData
        {
            get;
            set;
        }


        public PayInterBankData()
            : base()
        {
            RPData = new PayInterBankRP();
            RQData = new PayInterBankRQ();
            TradeCode = "ZJ0010";
        }
        public override uint RP_TOTAL_WIDTH
        {
            get
            {
                return PayInterBankRP.TOTAL_WIDTH;
            }
            set
            {
            }
        }

        public override uint RQ_TOTAL_WIDTH
        {
            get
            {
                return PayInterBankRQ.TOTAL_WIDTH;
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
