using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 资金划拨应答协议报文
    /// </summary>
    public class PayFundTransferRP : IMessageRespHandler
    {
        public const UInt32 TOTAL_WIDTH = 112 + PaymentBizMsgDataBase.HEADER_WIDTH;

        #region Property
        /// <summary>
        /// 交易结果,00—成功,其他—失败,X2
        /// </summary>
        public String RetCode
        {
            get;
            set;
        }
        /// <summary>
        /// 返回码,X10
        /// </summary>
        public String HostReturnCode
        {
            get;
            set;
        }
        /// <summary>
        /// 返回信息(交易失败的具体信息),X80
        /// </summary>
        public String HostReturnMessage
        {
            get;
            set;
        }
        /// <summary>
        /// 主机交易流水号,X12
        /// </summary>
        public String HostTranFlowNo
        {
            get;
            set;
        }
        /// <summary>
        /// 支付交易序号,X8
        /// </summary>
        public String TransSeq
        {
            get;
            set;
        }
        #endregion
        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= TOTAL_WIDTH - PaymentBizMsgDataBase.HEADER_WIDTH)
            {
                RetCode = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 2);
                HostReturnCode = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 10);
                HostReturnMessage = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 80);
                HostTranFlowNo = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 12);
                TransSeq = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 8);
            }

            return this;
        }

        #endregion
    }
}
