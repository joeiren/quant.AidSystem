using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class EncryptPin : EncryptHandler
    {
        public EncryptPin()
        { 
        }

        public override string ReqCode
        {
            get
            {
                return "02";
            }
            set
            {
                base.ReqCode = value;
            }
        }

        public short ReqPinLength
        {
            get;
            set;
        }

        public String ReqPin
        {
            get;
            set;
        }

        public byte[] RespPin
        {
            get;
            set;
        }

        public override byte[] BodyToByte()
        {

            byte[] pinBytes = Encoding.ASCII.GetBytes(ReqPin);
            if (pinBytes.Length > 12)
            {
                pinBytes = CommonDataHelper.SubBytes(pinBytes, 0, 12);
            }
            ReqPinLength = (short)pinBytes.Length;

            byte[] lenBytes = CommonDataHelper.ToNetworkOrder(ReqPinLength);

            byte[] buffer = new byte[lenBytes.Length + pinBytes.Length];
            Array.Copy(lenBytes, buffer, lenBytes.Length);
            Array.Copy(pinBytes, 0, buffer, lenBytes.Length, pinBytes.Length);
            return buffer;
        }

        public override void BodyFromByte(byte[] bytes)
        {
            if (bytes == null)
            {
                return;
            }
            if (RespCode == "E" && bytes.Length > 0)
            {
                RespPin = CommonDataHelper.SubBytes(bytes, 0, 1);
            }
            else if (RespCode == "A")
            {
                RespPin = CommonDataHelper.SubBytes(bytes, 0, RespMsgLength -1); 
            }
            
        }
    }
}
