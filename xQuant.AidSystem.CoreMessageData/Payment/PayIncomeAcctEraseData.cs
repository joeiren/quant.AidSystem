using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class PayIncomeAcctEraseData : PaymentBizMsgDataBase
    {
        public PayIncomeAcctEraseRP RPData
        {
            get;
            set;
        }

        public PayIncomeAcctEraseRQ RQData
        {
            get;
            set;
        }


        public PayIncomeAcctEraseData()
            : base()
        {
            RPData = new PayIncomeAcctEraseRP();
            RQData = new PayIncomeAcctEraseRQ();
            TradeCode = "ZJ0050";
        }
        public override uint RP_TOTAL_WIDTH
        {
            get
            {
                return PayIncomeAcctEraseRP.TOTAL_WIDTH;
            }
            set
            {
            }
        }

        public override uint RQ_TOTAL_WIDTH
        {
            get
            {
                return PayIncomeAcctEraseRQ.TOTAL_WIDTH;
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

        public override bool OnArgumentsValidation()
        {
            StringBuilder msg = new StringBuilder();
            if (string.IsNullOrEmpty(RQData.HostTranFlowNo))
            {
                msg.Append("主机流水号不能为空！");
            }
            if (string.IsNullOrEmpty(RQData.PayTransSN))
            {
                msg.Append("支付交易序号不能为空！");
            }
            if (string.IsNullOrEmpty(RQData.OriDelegateDate))
            {
                msg.Append("原委托日期不能为空！");
            }
            if (string.IsNullOrEmpty(RQData.SrcBankNO))
            {
                msg.Append("发起行行号不能为空！");
            }
            
            if (msg.Length > 0)
            {
                throw new BizArgumentsException(msg.ToString());
            }
            return true;
        }
    }
}
