using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace xQuant.AidSystem.Communication
{
    internal class SocketClient : IDisposable
    {
        /// <summary>
        /// Listener server endpoint.
        /// </summary>
        private IPEndPoint _hostEndPoint;

        private Socket _clientSocket;

        //private const String SourceName = "xQuant.AidSystem.Communication";

        /// <summary>
        /// Flag for connected socket.
        /// </summary>
        //private Boolean _isConnected = false;
        public static readonly UInt16 ONE_MINUTE = 1000 * 60;

        private int _receiveTimeout = ONE_MINUTE;
        public int ReceiveTimeout
        {
            get
            {
                return _receiveTimeout;
            }
            set 
            {
                _receiveTimeout = value;
            }
        }
        internal SocketClient(String hostname, Int32 port)
        {
            try
            {
                IPAddress hostIPAddress;
                if (!IPAddress.TryParse(hostname, out hostIPAddress))
                {
                    IPHostEntry host = Dns.GetHostEntry(hostname);
                    IPAddress[] addressArray = host.AddressList;
                    if (addressArray.Length > 0)
                    {
                        hostIPAddress = addressArray[addressArray.Length - 1];
                    }
                }
                _hostEndPoint = new IPEndPoint(hostIPAddress, port);
                _clientSocket = new Socket(_hostEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);               
            }
            catch (SocketException sockexc)
            {
                //StringBuilder sb = new StringBuilder("SocketClient初始化异常！");
                //sb.AppendLine();
                //sb.AppendFormat("SocketException.Message:{0}.SocketException.ErrorCode:{1}", sockexc.Message, sockexc.ErrorCode);
                //CommonHelper.AddEventLog(SourceName, sb.ToString());
                throw sockexc;
            }
            catch (Exception ex)
            {
                //StringBuilder sb = new StringBuilder("SocketClient初始化异常！");
                //sb.AppendLine();
                //sb.AppendFormat("Exception.Message:{0}.", ex.Message);
                //CommonHelper.AddEventLog(SourceName, sb.ToString());
                throw ex;
            }
        }
        #region IDisposable Members

        public void Dispose()
        {
            CloseSocket();
        }

        #endregion

        internal void Connect()
        {
            try
            {
                if (_clientSocket != null && _hostEndPoint != null)
                {
                    _clientSocket.Connect(_hostEndPoint);
                }
            }
            catch (SocketException sex)
            {

                //StringBuilder sb = new StringBuilder("Connect异常！");
                //sb.AppendLine();
                //sb.AppendFormat("Exception.Message:{0}.", sex.Message);
                //CommonHelper.AddEventLog(SourceName, sb.ToString());
                throw sex;
            }
            catch (Exception ex)
            {
                //StringBuilder sb = new StringBuilder("Connect异常！");
                //sb.AppendLine();
                //sb.AppendFormat("Exception.Message:{0}.", ex.Message);
                //CommonHelper.AddEventLog(SourceName, sb.ToString());
                throw ex;
            }
        }

        internal byte[] SingleSendReceive(byte[] buffer, Int32 maxLength)
        {
            try
            {
                if (_clientSocket.Connected)
                {
                    int len = _clientSocket.Send(buffer);
                    buffer = new byte[maxLength];
                    _clientSocket.ReceiveTimeout = ReceiveTimeout;
                    _clientSocket.Receive(buffer);
                    return buffer;
                }
                else
                {
                    throw new SocketException((int)SocketError.NotConnected);
                }
            }
            catch (SocketException sex)
            {
                throw sex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal byte[] SendReceive(byte[] buffer, Int32 maxLength)
        {
            try
            {
                if (_clientSocket.Connected)
                {
                    int len = _clientSocket.Send(buffer);
                    
                    byte[] temp = new byte[maxLength];
                    _clientSocket.ReceiveTimeout = _receiveTimeout;
                    len = 0;
                    len =_clientSocket.Receive(temp);
                    List<byte[]> buftemp = new List<byte[]>();
                    while (len > 0)
                    {
                        byte[] swap = new byte[len];
                        Array.Copy(temp, swap, len);
                        buftemp.Add(swap);
                        Array.Clear(temp, 0, maxLength);
                        if (_clientSocket.Connected)
                        {
                            len = _clientSocket.Receive(temp);
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (buftemp.Count > 0)
                    {
                        buffer = new byte[maxLength * buftemp.Count];
                        int offset = 0;
                        foreach (byte[] item in buftemp)
                        {
                            Array.Copy(item, 0, buffer, offset, item.Length);
                            offset += item.Length;
                        }
                    }
                    else
                    {
                        AidLogHelper.Write(xQuant.Log4.LogLevel.Debug, "SocketClient->SendReceive():socket接收到空数据！");
                        buffer = new byte[1]{0};
                    }
                    return buffer;
                }
                else
                {
                    throw new SocketException((int)SocketError.NotConnected);
                }
            }
            catch (SocketException sex)
            {
                throw sex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // 多包上传，只发不收
        internal void Send(byte[] buffer)
        {
            try
            {
                if (_clientSocket.Connected)
                {
                    int len = _clientSocket.Send(buffer);
                }
                else
                {
                    throw new SocketException((int)SocketError.NotConnected);
                }
            }
            catch (SocketException sex)
            {
                throw sex;
            }
            catch (Exception ex)
            {
                throw ex;
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
                catch(Exception ex)
                {
                    //StringBuilder sb = new StringBuilder("Shutdown异常！");
                    //sb.AppendLine();
                    //sb.AppendFormat("Exception.Message:{0}.", ex.Message);
                    //CommonHelper.AddEventLog(SourceName, sb.ToString());
                    throw ex;
                }
                finally
                {
                    if (_clientSocket.Connected)
                    {
                        _clientSocket.Close();
                    }
                }
            }
        }
    }
}
