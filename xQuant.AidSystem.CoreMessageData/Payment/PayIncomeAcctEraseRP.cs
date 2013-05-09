using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class PayIncomeAcctEraseRP : IMessageRespHandler
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
        /// <summary>
        /// 抹账主机流水号,x12
        /// </summary>
        //public String RetHostFlowNO
        //{
        //    get;
        //    set;
        //}

        #endregion
        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= TOTAL_WIDTH - PaymentBizMsgDataBase.HEADER_WIDTH)
            {
                RetCode = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 2);
                RetMsg = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 80);
                //RetHostFlowNO = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 12);
            }
            return this;
        }

        #endregion

        
    }
}
