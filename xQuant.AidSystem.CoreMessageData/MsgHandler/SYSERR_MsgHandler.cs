using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace xQuant.AidSystem.CoreMessageData
{
    public class SYSERR_MsgHandler : IMessageRespHandler
    {

        public const UInt16 TOTAL_WIDTH = 80;

        public String Message
        {
            get;
            set;
        }
        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes != null && messagebytes.Length > 0)
            {
                byte[] buffer = new byte[TOTAL_WIDTH];
                Array.Copy(messagebytes, buffer, TOTAL_WIDTH);
                //ms.Read(buffer, 0, TOTAL_WIDTH);
                Message = CommonDataHelper.GetValueFromBytes(ref buffer, 80).TrimEnd();
                
            }
            return this;
        }

        #endregion
    }
}
