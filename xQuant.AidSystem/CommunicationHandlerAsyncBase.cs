using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Collections.Specialized;

namespace xQuant.AidSystem.Communication
{
    public delegate void SocketCompletedEventHandler(Queue<PackageData> respData, Exception ex);
    public delegate void MessageHandlerCompleteAsync(MessageData respMsg, Exception ex);
    public abstract class CommunicationHandlerAsyncBase
    {
        public MessageHandlerCompleteAsync _callbackHandler;
        public MessageData _respMsg;
        public abstract void MessageAsyncHandler(MessageData reqMsg, MessageHandlerCompleteAsync callbackHandler);
        
    }
}
