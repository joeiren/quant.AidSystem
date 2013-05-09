using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class FingerMarkReq : IMessageReqHandler
    {
        #region Property
        public int MsgLength
        {
            get;
            set;
        }
        public String TradeCode
        {
            get 
            {
                return "C69101";
            }
        }
        // 4
        public String ZoneNO
        {
            get;
            set;
        }
        public String InputTradeCode
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
        public String BizUnitNO
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
        public String MarkData
        {
            get;
            set;
        }
        #endregion


        // 非定长数据
        public String RetrieveMsgData()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}{16}",InputTradeCode??"", TradeDate??"", TradeTime??"", UnionNO??"", TellerNO??"", NetFlowNO??"", HostFlowNO??"", FrontFlowNO??"", BizUnitNO??"", 
                RespCode??"",RespMsg??"", TradeState??"", RespCount??"", FileCount??"", EndFlag??"", AuthFlag??"", MarkData??"");
            return sb.ToString();
        }

        public override String ToString()
        {
            string msgdata = RetrieveMsgData();
            MsgLength = msgdata.Length + 4;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}{1}{2}{3}", CommonDataHelper.FillSpecifyWidthString(MsgLength.ToString(), 8), TradeCode, CommonDataHelper.FillSpecifyWidthString(ZoneNO, 4), msgdata);
            return sb.ToString();
        }
        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {            
            byte[] bytes = CommonDataHelper.WideCharToGBK(ToString());
            return bytes;
        }

        #endregion
    }

    public class FingerMarkReqSubMsg
    {
        public String OperateType
        {
            get;
            set;
        }
        public String DeviceType
        {
            get;
            set;
        }
        public String InstNO
        {
            get;
            set;
        }
        public String TellerNO
        {
            get;
            set;
        }
        public String FingerNO
        {
            get;
            set;
        }
        public String SecurityLevel
        {
            get;
            set;
        }
        public String MarkInfo
        {
            get;
            set;
        }

        public override string ToString()
        {
           StringBuilder sb = new StringBuilder();
           sb.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}|{6}~", OperateType??"", DeviceType??"", InstNO??"", TellerNO??"", FingerNO??"", SecurityLevel??"", MarkInfo??"");
           return sb.ToString();
        }
    }
}
