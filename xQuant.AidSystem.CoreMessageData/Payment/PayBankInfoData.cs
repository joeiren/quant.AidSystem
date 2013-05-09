using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class PayBankInfoData : PaymentBizMsgDataBase
    {
        public PayBankInfoRP RPData
        {
            get;
            set;
        }

        public PayBankInfoRQ RQData
        {
            get;
            set;
        }
        public PayBankInfoData()
        {
            RPData = new PayBankInfoRP();
            RQData = new PayBankInfoRQ();
            TradeCode = "IE0006";
        }
        public override byte[] ReqToBytes()
        {
            return null;
        }

        public override void RespFromBytes(byte[] bytes)
        {
            RPData.FromBytes(bytes);
        }

        public override uint RP_TOTAL_WIDTH
        {
            get
            {
                return RPData.TOTAL_WIDTH;
            }
            set
            {
            }
        }

        public override uint RQ_TOTAL_WIDTH
        {
            get
            {
                return PayBankInfoRQ.TOTAL_WIDTH;
            }
            set
            {
            }
        }
       
    }
}
