using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    class CoreMessageHeader : IMessageRespHandler, IMessageReqHandler
    {
        #region Const Field
        public const UInt16 MESSAGE_LENGTH_WIDTH = 8;
        public const UInt16 FMT_TYPE_WIDTH = 3;
        public const UInt16 LAST_FLAG_WIDTH = 1;
        public const UInt16 RESERVED_WIDTH = 8;
        public const UInt16 TOTAL_WIDTH = MESSAGE_LENGTH_WIDTH + FMT_TYPE_WIDTH + LAST_FLAG_WIDTH + RESERVED_WIDTH;
        #endregion

        #region Property
        //信息长度,8位长度
        private UInt32 _messageLength;
        public UInt32 MH_MESSAGE_LENGTH
        {
            get
            {
                return _messageLength;
            }
            set
            {
                _messageLength = value;
            }
        }
        //信息结构类型,3位长度；'DB1' - 数据块结构
        public String MH_FMT_TYPE
        {
            get
            {
                return "DB1";
            }
            set
            { }
        }
        // 1位长度；'0' - 非最后信息;'1' - 最后信息
        private Boolean _lastFlag = true;
        public Boolean MH_LAST_FLAG
        {
            get
            {
                return _lastFlag;
            }
            set
            {
                _lastFlag = value;
            }
        }
        //预留,8
        public String MH_RESERVED
        {
            get
            {
                return CommonDataHelper.SpaceString(8);
            }
            set
            { }
        }
        #endregion

        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(MH_MESSAGE_LENGTH.ToString().PadLeft(MESSAGE_LENGTH_WIDTH));
            sb.Append(MH_FMT_TYPE);
            sb.Append(MH_LAST_FLAG ? "1" : "0");
            sb.Append(MH_RESERVED);

            byte[] bytes = new byte[TOTAL_WIDTH * 2];
            int len = EBCDICEncoder.WideCharToEBCDIC(EBCDICEncoder.CCSID_IBM_1388, sb.ToString(), sb.Length, bytes, bytes.Length);
            if (len != bytes.Length)
            {
                return CommonDataHelper.SubBytes(bytes, 0, len);
            }
            return bytes;
        }

        #endregion

        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= TOTAL_WIDTH)
            {
                String result = CommonDataHelper.GetValueFromBytes(ref messagebytes, TOTAL_WIDTH);

                UInt32.TryParse(result.Substring(0,8), out _messageLength);
                MH_FMT_TYPE = result.Substring(8,3);
                if (result.Substring(11, 1).CompareTo("0") == 0)
                {
                    _lastFlag = false;
                }
                else
                {
                    _lastFlag = true;
                }
                MH_RESERVED = result.Substring(12,8);
               
            }
            return this;
        }

        #endregion
    }
}
