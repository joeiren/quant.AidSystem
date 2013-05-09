using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Timers;

namespace xQuant.AidSystem.Communication
{
    internal class SocketClientAsync : IDisposable
    {
        #region Field
        private const String SourceName = "xQuant.AidSystem.Communication";
        /// <summary>
        /// Time out
        /// </summary>
        private const Int32 TimeOutSecond = 5 * 60 * 1000;

        private const Int16 LastFlagOffset = 12;
        /// <summary>
        /// the max length of each received bytes
        /// </summary>
        internal const Int32 ReceivedByteMaxLength = 32 * 1024;

        /// <summary>
        /// Const variables: socket operations.
        /// </summary>
        //private const Int16 ReceiveOperation = 1, SendOperation = 0;

        private Socket _clientSocket;

        /// <summary>
        /// Flag for connected socket.
        /// </summary>
        private Boolean _isConnected = false;

        /// <summary>
        /// Listener server endpoint.
        /// </summary>
        private IPEndPoint _hostEndPoint;

        /// <summary>
        /// Signals for a connection.
        /// </summary>        
        private AutoResetEvent _autoConnectionEvent = new AutoResetEvent(false);

        /// <summary>
        /// Signals for the send operation.
        /// </summary>
        private AutoResetEvent _autoSendEvent = new AutoResetEvent(false);

        /// <summary>
        /// Signals for the receive operation.
        /// </summary>
        private AutoResetEvent _autoReceiveEvent = new AutoResetEvent(false);


        internal Queue<PackageData> SendBuffers
        {
            get;
            set;
        }
        internal Queue<PackageData> RecvBuffers
        {
            get;
            set;
        }
        SocketCompletedEventHandler _completeDelegate;
        #endregion

        #region Property
        public String HostServerName
        {
            get;
            set;
        }

        public Int32 Port
        {
            get;
            set;
        }

        public Boolean SupportIPv6
        {
            get;
            set;
        }
        #endregion

        internal SocketClientAsync(String hostname, Int32 port)
        {
            try
            {
                IPHostEntry host = Dns.GetHostEntry(hostname);
                IPAddress[] addressArray = host.AddressList;
                if (addressArray.Length > 0)
                {
                    _hostEndPoint = new IPEndPoint(addressArray[addressArray.Length - 1], port);
                    _clientSocket = new Socket(_hostEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    //_clientSocket.SendTimeout = TimeOutSecond;
                }
            }
            catch (SocketException sockexc)
            {
                StringBuilder sb = new StringBuilder("SocketClientAsync初始化异常！");
                sb.AppendLine();
                sb.AppendFormat("SocketException.Message:{0}.SocketException.ErrorCode:{1}", sockexc.Message, sockexc.ErrorCode);
                //CommonHelper.AddEventLog(SourceName, sb.ToString());
                throw sockexc;
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("SocketClientAsync初始化异常！");
                sb.AppendLine();
                sb.AppendFormat("Exception.Message:{0}.", ex.Message);
                //CommonHelper.AddEventLog(SourceName, sb.ToString());
                throw ex;
            }
        }

        internal SocketAsyncEventArgs Init(Queue<PackageData> sendBuffers)
        {
            if (_clientSocket != null && _hostEndPoint != null)
            {
                SocketAsyncEventArgs eventArgs = new SocketAsyncEventArgs();
                eventArgs.UserToken = _clientSocket;
                eventArgs.RemoteEndPoint = _hostEndPoint;
                eventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);
                RecvBuffers = new Queue<PackageData>();
                SendBuffers = sendBuffers;
                return eventArgs;
            }
            return null;
        }
        
        #region Socket Operation

        internal void BeginTransmit(SocketAsyncEventArgs eventArgs, SocketCompletedEventHandler callback)
        {
            _completeDelegate = callback;
            if (_clientSocket != null && eventArgs != null)
            {
                bool willRaiseEvent = _clientSocket.ConnectAsync(eventArgs);
                if (!willRaiseEvent)
                {
                    _clientSocket.ConnectAsync(eventArgs);
                }
            }
            
            //_autoConnectionEvent.WaitOne();            
        }

        internal void ProcessConnecting(SocketAsyncEventArgs connectArgs)
        {
            SocketError errorCode = connectArgs.SocketError;
            if (errorCode != SocketError.Success)
            {
                SocketException sockex = new SocketException((Int32)errorCode);
                StringBuilder sb = new StringBuilder("ProcessConnecting异常！");
                sb.AppendLine();
                sb.AppendFormat("ErrorCode Name:{0}.Exception:{1}", Enum.GetName(typeof(SocketError), errorCode), sockex.Message);
                //CommonHelper.AddEventLog(SourceName, sb.ToString());
                ProcessError();
                _completeDelegate(RecvBuffers, sockex);
                
            }
            else
            {
                if (SendBuffers.Count > 0)
                {
                    PackageData package = SendBuffers.Dequeue();
                    connectArgs.SetBuffer(package.PackageMessage, 0, package.PackageMessage.Length);
                    connectArgs.UserToken = package.PackageID;
                    _clientSocket.SendAsync(connectArgs);
                }
                else
                {
 
                }
            }
        }

        internal void ProcessSending(SocketAsyncEventArgs sendArgs)
        {
            SocketError errorCode = sendArgs.SocketError;
            if (errorCode != SocketError.Success)
            {
                StringBuilder sb = new StringBuilder("ProcessSending异常！");
                sb.AppendLine();
                sb.AppendFormat("ErrorCode Name:{0}.Exception:{1}", Enum.GetName(typeof(SocketError), errorCode), new SocketException((Int32)errorCode).Message);
                //CommonHelper.AddEventLog(SourceName, sb.ToString());
                //throw new SocketException((Int32)errorCode);
                ProcessError();
                _completeDelegate(RecvBuffers, new SocketException((Int32)errorCode));
            }
            else
            {             
                short packageid = Int16.Parse(sendArgs.UserToken.ToString());
                PackageData package = new PackageData(packageid, new byte[10 * 1024]);
                sendArgs.SetBuffer(package.PackageMessage, 0, package.PackageMessage.Length);
                sendArgs.UserToken = package.PackageID;
                _clientSocket.ReceiveAsync(sendArgs);
                
            }
        }

        internal void ProcessReceiving(SocketAsyncEventArgs receiveArgs)
        {
            SocketError errorCode = receiveArgs.SocketError;
            if (receiveArgs.BytesTransferred > 0 && receiveArgs.SocketError == SocketError.Success)
            {
                RecvBuffers.Enqueue(new PackageData(Int16.Parse(receiveArgs.UserToken.ToString()), receiveArgs.Buffer));
                if (SendBuffers.Count > 0)
                {
                    PackageData package = SendBuffers.Dequeue();
                    receiveArgs.SetBuffer(package.PackageMessage, 0, package.PackageMessage.Length);
                    receiveArgs.UserToken = package.PackageID;
                    _clientSocket.SendAsync(receiveArgs);
                }
                else
                {
                    CloseSocket();
                    _completeDelegate(RecvBuffers, null);
                }
            }
            else
            {
                StringBuilder sb = new StringBuilder("ProcessReceiving异常！");
                sb.AppendLine();
                sb.AppendFormat("ErrorCode Name:{0}.Exception:{1}", Enum.GetName(typeof(SocketError), errorCode), new SocketException((Int32)errorCode).Message);
                //CommonHelper.AddEventLog(SourceName, sb.ToString());
                ProcessError();
                _completeDelegate(RecvBuffers, new SocketException((Int32)errorCode));
            }

        }

        internal void DisConnect()
        {
            _clientSocket.Disconnect(false);
        }

        internal void CloseSocket()
        {
            if (_clientSocket.Connected)
            {
                try
                {
                    _clientSocket.Shutdown(SocketShutdown.Both);
                }
                catch
                { }
                finally
                {
                    if (_clientSocket.Connected)
                    {
                        _clientSocket.Close();
                    }
                }
            }
        }

        internal void ProcessError()
        {
            //Socket sock = e.UserToken as Socket;
            if (_clientSocket != null && _clientSocket.Connected)
            {
                try
                {
                    _clientSocket.Shutdown(SocketShutdown.Both);
                }
                catch
                {
                }
                finally
                {
                    if (_clientSocket.Connected)
                    {
                        _clientSocket.Close();
                    }
                }

            }
            else
            {
                //StringBuilder sb = new StringBuilder("ProcessError！");
                //sb.AppendLine();
                //sb.AppendFormat("ErrorCode:{0}.Name:{1}.", (Int32)SocketError.NotConnected, Enum.GetName(typeof(SocketError), SocketError.NotConnected));
                //CommonHelper.AddEventLog(SourceName, sb.ToString());
                //throw new SocketException((Int32)SocketError.NotConnected);            
            }
        }
        #endregion

        #region Async callback function
        internal void OnIOCompleted(object sender, SocketAsyncEventArgs e)
        {
            // determine which type of operation just completed and call the associated handler
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    ProcessConnecting(e);
                    break;
                case SocketAsyncOperation.Receive:
                    ProcessReceiving(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSending(e);
                    break;
                default:
                    //throw new ArgumentException("The last operation completed on the socket was not a receive or send");
                    break;
            }  
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _autoConnectionEvent.Close();
            _autoReceiveEvent.Close();
            _autoSendEvent.Close();
            CloseSocket();
        }

        #endregion

        
    }

    internal class AsyncReceiveState
    {
        internal byte[] Buffer = new byte[SocketClientAsync.ReceivedByteMaxLength]; // 8K buffer 
        internal StringBuilder Content = new StringBuilder(); // Place to store the content as it's received 
        internal SocketAsyncEventArgs ReadEventArgs = new SocketAsyncEventArgs(); 
        internal Socket ClientSocket; 
    }
}
