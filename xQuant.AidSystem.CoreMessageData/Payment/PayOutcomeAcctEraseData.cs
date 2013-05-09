using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 支付平台抹账交易
    /// </summary>
    public class PayOutcomeAcctEraseData : PaymentBizMsgDataBase
    {
        public PayOutcomeAcctEraseRP RPData
        {
            get;
            set;
        }

        public PayOutcomeAcctEraseRQ RQData
        {
            get;
            set;
        }


        public PayOutcomeAcctEraseData()
            : base()
        {
            RPData = new PayOutcomeAcctEraseRP();
            RQData = new PayOutcomeAcctEraseRQ();
            TradeCode = "ZJ0099";
        }
        public override uint RP_TOTAL_WIDTH
        {
            get
            {
                return PayOutcomeAcctEraseRP.TOTAL_WIDTH;
            }
            set
            {
            }
        }

        public override uint RQ_TOTAL_WIDTH
        {
            get
            {
                return PayOutcomeAcctEraseRQ.TOTAL_WIDTH;
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
                msg.Append("需抹帐的主机流水号不能为空！");
            }
            if (string.IsNullOrEmpty(RQData.OriDelegateDate))
            {
                msg.Append("原委托日期不能为空！");
            }
            if (string.IsNullOrEmpty(RQData.TransferFlowNo))
            {
                msg.Append("需抹帐的资金业务流水号不能为空！");
            }
            if (string.IsNullOrEmpty(RQData.Operator))
            {
                msg.Append("柜员号不能为空！");
            }
            if (string.IsNullOrEmpty(RQData.PayBank))
            {
                msg.Append("机构号不能为空！");
            }
            
            if (msg.Length > 0)
            {
                throw new BizArgumentsException(msg.ToString());
            }
            return true;
        }
    }
}
