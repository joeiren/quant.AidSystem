using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 登记簿查询
    /// </summary>
    public class PayRegisterRetrievedData : PaymentBizMsgDataBase
    {
        public PayRegisterRetrievedRP RPData
        {
            get;
            set;
        }

        public PayRegisterRetrievedRQ RQData
        {
            get;
            set;
        }


        public PayRegisterRetrievedData()
            : base()
        {
            RPData = new PayRegisterRetrievedRP();
            RQData = new PayRegisterRetrievedRQ();
            TradeCode = "ZJ0020";
        }
        public override uint RP_TOTAL_WIDTH
        {
            get
            {
                return PayRegisterRetrievedRP.TOTAL_WIDTH;
            }
            set
            {
            }
        }

        public override uint RQ_TOTAL_WIDTH
        {
            get
            {
                return PayRegisterRetrievedRQ.TOTAL_WIDTH;
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
