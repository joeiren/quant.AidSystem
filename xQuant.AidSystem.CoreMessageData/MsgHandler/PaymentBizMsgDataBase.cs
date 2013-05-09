using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 支付平台报文
    /// </summary>
    public abstract class PaymentBizMsgDataBase : BizMsgDataBase, IMessageReqHandler, IMessageRespHandler
    {
        //协议报文长度
        public const UInt16 HEADER_WIDTH = 14;
        public override uint RQ_TOTAL_WIDTH
        {
            get;
            set;
        }

        public override uint RP_TOTAL_WIDTH
        {
            get;
            set;
        }
        /// <summary>
        /// 交易码,6
        /// </summary>
        public String TradeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 交易报文长度,8
        /// </summary>
        public UInt32 MessageLength
        {
            get;
            set;
        }


        public abstract byte[] ReqToBytes();

        public abstract void RespFromBytes(byte[] bytes);

        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(CommonDataHelper.FillSpecifyWidthString(TradeCode, 6));
            sb.Append(CommonDataHelper.FillSpecifyWith0(RQ_TOTAL_WIDTH - PaymentBizMsgDataBase.HEADER_WIDTH, 8));
            byte[] buffer = new byte[RQ_TOTAL_WIDTH];
            byte[] headbytes = CommonDataHelper.WideCharToGBK(sb.ToString());
            //int len = EBCDICEncoder.WideCharToEBCDIC(EBCDICEncoder.CCSID_IBM_1388, sb.ToString(), sb.Length, headbytes, headbytes.Length);
            if (HEADER_WIDTH != headbytes.Length)
            {
                headbytes = CommonDataHelper.SubBytes(headbytes, 0, HEADER_WIDTH);
            }
            Array.Copy(headbytes, buffer, headbytes.Length);
            if (RQ_TOTAL_WIDTH - HEADER_WIDTH > 0)
                Array.Copy(ReqToBytes(), 0, buffer, headbytes.Length, RQ_TOTAL_WIDTH - HEADER_WIDTH);
            return buffer;
        }

        #endregion

        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length > HEADER_WIDTH)
            {
                byte[] header = CommonDataHelper.SubBytes(messagebytes, 0, HEADER_WIDTH);
                String result = CommonDataHelper.GetValueFromGBKBytes(ref header, HEADER_WIDTH);
                TradeCode = result.Substring(0, 6);
                UInt32 length = 0;
                UInt32.TryParse(result.Substring(6, 8), out length);
                MessageLength = length;

                if (MessageLength > HEADER_WIDTH)
                {
                    RespFromBytes(CommonDataHelper.SubBytes(messagebytes, HEADER_WIDTH, (int)MessageLength));
                }
                
            }
            return this;
        }

        #endregion
    }
}
