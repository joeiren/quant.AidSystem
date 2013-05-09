using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net.Sockets;

namespace xQuant.AidSystem.Communication
{
    public class PayCommunicationHandlerAsync : CommunicationHandlerAsyncBase
    {
        private static readonly String Host = ConfigurationSettings.AppSettings["PaymentHost"];

        private static readonly Int32 Port = Convert.ToInt16(ConfigurationSettings.AppSettings["PaymentPort"]);
        private const Int32 RECEIVE_MAX_LENGTH = 32 * 1024;

        public override void MessageAsyncHandler(MessageData reqMsg, MessageHandlerCompleteAsync callbackHandler)
        {
            _callbackHandler = callbackHandler;
            _respMsg = new MessageData { MessageID = reqMsg.MessageID, FirstTime = reqMsg.FirstTime, TragetPlatform = reqMsg.TragetPlatform, ReqPackageList = reqMsg.ReqPackageList };
            try
            {
                using (SocketClientAsync sca = new SocketClientAsync(Host, Port))
                {
                    SocketAsyncEventArgs eventArgs = sca.Init(reqMsg.ReqPackageList);
                    if (eventArgs != null)
                    {
                        SocketCompletedEventHandler transmitComplete = new SocketCompletedEventHandler(TransmitComplete);
                        sca.BeginTransmit(eventArgs, transmitComplete);
                    }
                }
            }

            catch (Exception ex)
            {
                //throw ex;
                _callbackHandler(_respMsg, ex);
            }
        }

        private void TransmitComplete(Queue<PackageData> respData, Exception ex)
        {
            //byte[] buffer = recv;
            if (_callbackHandler != null && _respMsg != null)
            {
                if (respData.Count == 0 && ex != null)
                {
                    if (ex is SocketException)
                    {
                        SocketException sex = ex as SocketException;
                        if (sex.ErrorCode == (int)SocketError.TimedOut)
                        {
                            StringBuilder sb = new StringBuilder("Socket TimeOut！");
                            sb.AppendFormat("重发！{0}", _respMsg.MessageID);
                            //CommonHelper.AddEventLog("xQuant.AidSystem.Communication.CoreCommunicationHandlerAsync", sb.ToString());
                            MessageAsyncHandler(_respMsg, _callbackHandler);
                            return;
                        }
                    }
                }

                _respMsg.RespPackageList = respData;
                _callbackHandler(_respMsg, ex);
                return;

            }
        }
    }
}
