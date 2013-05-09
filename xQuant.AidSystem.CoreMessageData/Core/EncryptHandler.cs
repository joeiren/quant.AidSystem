using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace xQuant.AidSystem.CoreMessageData
{
    public abstract class EncryptHandler : IMessageReqHandler, IMessageRespHandler
    {
        //请求报文长度
        public virtual short ReqMsgLength
        {
            get;
            set;
        }
        //应答报文长度
        public virtual short RespMsgLength
        {
            get;
            set;
        }

        //请求码（指令码）
        public virtual String ReqCode
        {
            get;
            set;
        }
        //指令码
        public virtual String RespCode
        {
            get;
            set;
        }

        public abstract byte[] BodyToByte();

        public abstract void BodyFromByte(byte[] bytes);

        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            byte[] codeBytes = Encoding.ASCII.GetBytes(ReqCode);
            byte[] bodyBytes = BodyToByte();
            ReqMsgLength = (short)(codeBytes.Length + bodyBytes.Length);

            byte[] lenBytes = CommonDataHelper.ToNetworkOrder(ReqMsgLength);
            byte[] buffer = new byte[lenBytes.Length + codeBytes.Length + bodyBytes.Length];
            
            Array.Copy(lenBytes, buffer, lenBytes.Length);
            Array.Copy(codeBytes, 0, buffer, lenBytes.Length, codeBytes.Length);
            Array.Copy(bodyBytes, 0, buffer, lenBytes.Length + codeBytes.Length, bodyBytes.Length);
            return buffer;
        }

        #endregion
        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes == null)
            {
                return this;
            }
            int len = messagebytes.Length;
            int offset = 0;
            RespMsgLength = CommonDataHelper.FromNetworkOrder(messagebytes, offset);
            offset += 2;
            if (RespMsgLength > 0)
            {
                RespCode = Encoding.ASCII.GetString(messagebytes, offset, 1);
                offset += 1;
                BodyFromByte(CommonDataHelper.SubBytes(messagebytes, offset, messagebytes.Length - offset));
            }
            return this;
        }

        #endregion
    }
}
