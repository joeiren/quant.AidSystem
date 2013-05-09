using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 支付平台抹账交易应答协议报文
    /// </summary>
    public class PayOutcomeAcctEraseRP : IMessageRespHandler
    {
        public const UInt32 TOTAL_WIDTH = 82 + PaymentBizMsgDataBase.HEADER_WIDTH;
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
        /// 交易结果描述,X80
        /// </summary>
        public String RetMsg
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
                RetMsg = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 80);
            }
            return this;
        }

        #endregion
    }
}
