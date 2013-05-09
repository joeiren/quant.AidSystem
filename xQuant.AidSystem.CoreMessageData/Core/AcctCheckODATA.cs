using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class AcctCheckODATA : IMessageRespHandler
    {

        public UInt32 TOTAL_WIDTH
        {
            get;
            set;
        }

        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            return this;
            //throw new NotImplementedException();
        }

        #endregion
    }

    

}
