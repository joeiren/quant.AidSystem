using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 来账的销账
    /// </summary>
    public class PayVostroAcctEliminateData : PaymentBizMsgDataBase
    {
        public PayVostroAcctEliminateRP RPData
        {
            get;
            set;
        }

        public PayVostroAcctEliminateRQ RQData
        {
            get;
            set;
        }


        public PayVostroAcctEliminateData()
            : base()
        {
            RPData = new PayVostroAcctEliminateRP();
            RQData = new PayVostroAcctEliminateRQ();
            TradeCode = "ZJ0030";
        }
        public override uint RP_TOTAL_WIDTH
        {
            get
            {
                return PayVostroAcctEliminateRP.TOTAL_WIDTH;
            }
            set
            {
            }
        }

        public override uint RQ_TOTAL_WIDTH
        {
            get
            {
                return PayVostroAcctEliminateRQ.TOTAL_WIDTH;
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
            if (string.IsNullOrEmpty(RQData.TransSeq))
            {
                msg.Append("支付交易序号不能为空！");
            }
            if (string.IsNullOrEmpty(RQData.AccountBanks))
            {
                msg.Append("发起行行号不能为空！");
            }
            if (string.IsNullOrEmpty(RQData.DelegateDate))
            {
                msg.Append("委托日期不能为空！");
            }
            if (string.IsNullOrEmpty(RQData.Amount))
            {
                msg.Append("金额不能为空！");
            }
            if (msg.Length > 0)
            {
                throw new BizArgumentsException(msg.ToString());
            }
            return true;
        }
    }
}
