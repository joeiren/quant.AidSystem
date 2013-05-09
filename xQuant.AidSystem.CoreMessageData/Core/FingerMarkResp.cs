using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class FingerMarkResp : IMessageRespHandler
    {
        #region Property
        public int MsgLength
        {
            get;
            set;
        }
        public String TradeCode
        {
            get;
            set;
        }
        // 4
        public String ZoneNO
        {
            get;
            set;
        }
        public String TradeDate
        {
            get;
            set;
        }
        public String TradeTime
        {
            get;
            set;
        }
        public String UnionNO
        {
            get;
            set;
        }
        public String TellerNO
        {
            get;
            set;
        }
        public String NetFlowNO
        {
            get;
            set;
        }
        public String HostFlowNO
        {
            get;
            set;
        }
        public String FrontFlowNO
        {
            get;
            set;
        }
        public String RespCode
        {
            get;
            set;
        }
        public String RespMsg
        {
            get;
            set;
        }
        public String TradeState
        {
            get;
            set;
        }
        public String RespCount
        {
            get;
            set;
        }
        public String RespType
        {
            get;
            set;
        }
        public String RespInof
        {
            get;
            set;
        }
        public String FileCount
        {
            get;
            set;
        }
        public String EndFlag
        {
            get;
            set;
        }
        public String AuthFlag
        {
            get;
            set;
        }
        #endregion

        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            string msglen = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 8);
            int len = 0;
            if (int.TryParse(msglen, out len))
            {
                MsgLength = len;
            }
            TradeCode = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 6);
            ZoneNO = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 4);

            string data = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, (UInt16)messagebytes.Length);
            string[] dataArray = data.Split('|');
            if (dataArray == null)
            {
                return this;
            }
            if (dataArray.Length >= 14 && dataArray.Length < 16)
            {
                TradeDate = dataArray[0];
                TradeTime = dataArray[1];
                UnionNO = dataArray[2];
                TellerNO = dataArray[3];
                NetFlowNO = dataArray[4];
                HostFlowNO = dataArray[5];
                FrontFlowNO = dataArray[6];
                RespCode = dataArray[7];
                RespMsg = dataArray[8];
                TradeState = dataArray[9];
                RespCount = dataArray[10];
                //RespType = dataArray[11];
                //RespInof = dataArray[12];
                FileCount = dataArray[11];
                EndFlag = dataArray[12];
                AuthFlag = dataArray[13];
            }
            else if (dataArray.Length >= 16)
            {
                TradeDate = dataArray[0];
                TradeTime = dataArray[1];
                UnionNO = dataArray[2];
                TellerNO = dataArray[3];
                NetFlowNO = dataArray[4];
                HostFlowNO = dataArray[5];
                FrontFlowNO = dataArray[6];
                RespCode = dataArray[7];
                RespMsg = dataArray[8];
                TradeState = dataArray[9];
                RespCount = dataArray[10];
                RespType = dataArray[11];
                RespInof = dataArray[12];
                FileCount = dataArray[13];
                EndFlag = dataArray[14];
                AuthFlag = dataArray[15];
            }
            return this;
        }

        #endregion
    }
}
