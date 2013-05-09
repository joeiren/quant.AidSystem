using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 资金划拨报文数据
    /// </summary>
    public class PayFundTransferData : PaymentBizMsgDataBase
    {
        public PayFundTransferRP RPData
        {
            get;
            set;
        }

        public PayFundTransferRQ RQData
        {
            get;
            set;
        }


        public PayFundTransferData() : base()
        {
            RPData = new PayFundTransferRP();
            RQData = new PayFundTransferRQ();
            TradeCode = "ZJ0011";
        }
        public override uint RP_TOTAL_WIDTH
        {
            get
            {
                return PayFundTransferRP.TOTAL_WIDTH;
            }
            set
            {
            }
        }

        public override uint RQ_TOTAL_WIDTH
        {
            get
            {
                return PayFundTransferRQ.TOTAL_WIDTH;
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
            if (string.IsNullOrEmpty(RQData.TransferFlowNo))
            {
                msg.Append("资金流水号不能为空！");
            }
            if (string.IsNullOrEmpty(RQData.PayAccount))
            {
                msg.Append("付款账号不能为空！");
            }
            if (string.IsNullOrEmpty(RQData.PayAccountName))
            {
                msg.Append("付款人名称不能为空！");
            }
            if (string.IsNullOrEmpty(RQData.PendingSN))
            {
                msg.Append("挂账序号不能为空！");
            }
            if (string.IsNullOrEmpty(RQData.RecAccount))
            {
                msg.Append("收款账号不能为空！");
            }
            if (string.IsNullOrEmpty(RQData.RecAccountName))
            {
                msg.Append("收款人名称不能为空！");
            }
            if (string.IsNullOrEmpty(RQData.RecAccountBanks))
            {
                msg.Append("收款行行号不能为空！");
            }
            if (string.IsNullOrEmpty(RQData.PayAmount))
            {
                msg.Append("交易金额不能为空！");
            }
            if (msg.Length > 0)
            {
                throw new BizArgumentsException(msg.ToString());
            }
            return true;
        }
    }
}
