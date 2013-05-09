using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.Communication
{
    public class NullableHandlerAsync : CommunicationHandlerAsyncBase
    {
        public override void MessageAsyncHandler(MessageData reqMsg, MessageHandlerCompleteAsync callbackHandler)
        {
            _callbackHandler = callbackHandler;
            _respMsg = new MessageData { MessageID = reqMsg.MessageID, FirstTime = reqMsg.FirstTime, TragetPlatform = reqMsg.TragetPlatform, ReqPackageList = reqMsg.ReqPackageList };
            _callbackHandler(_respMsg, null);
        }

    }
}
