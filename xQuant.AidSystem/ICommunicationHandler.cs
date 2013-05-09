using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.Communication
{
    public interface ICommunicationHandler
    {
        MessageData MessageHandler(MessageData reqmsg);
    }
}
