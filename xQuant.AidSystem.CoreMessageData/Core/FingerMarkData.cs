using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class FingerMarkData : BizMsgDataBase, IMessageRespHandler, IMessageReqHandler
    {
        public FingerMarkReq ReqData
        {
            get;
            set;
        }

        public FingerMarkResp RespData
        {
            get;
            set;
        }

        public FingerMarkData()
        {
            ReqData = new FingerMarkReq();
            RespData = new FingerMarkResp();
        }

        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            return ReqData.ToBytes();
        }

        #endregion

        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            RespData = RespData.FromBytes(messagebytes) as FingerMarkResp;
            return this;
        }

        #endregion

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

        
    }
}
