using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 登记簿查询应答协议报文
    /// </summary>
    public class PayRegisterRetrievedRP : IMessageRespHandler
    {
        public const UInt32 TOTAL_WIDTH = 86 + PaymentBizMsgDataBase.HEADER_WIDTH;
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
        /// <summary>
        /// 状态,X2
        /// </summary>
        public String AccountStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 渠道标志,X2
        /// </summary>
        public String PackageChannelType
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
                AccountStatus = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 2);
                PackageChannelType = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 2);
            }
            return this;
        }

        #endregion
    }
}
