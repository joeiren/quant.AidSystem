using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.Communication
{
    public class NullableHandler : ICommunicationHandler
    {
        #region ICommunicationHandler Members

        public MessageData MessageHandler(MessageData reqmsg)
        {
            //reqmsg.RespPackageList.Enqueue(new PackageData(1, new byte[] { 33, 44 }));
            return reqmsg;
        }

        #endregion
    }
}
