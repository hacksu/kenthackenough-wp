using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Net;

namespace Kent_Hack_Enough
{
    class SocketClient
    {
        Socket _socket = null;

        static ManualResetEvent _clientDone = new ManualResetEvent(false);

        // Timeout for async call
        // Value is in milliseconds
        const int TIMEOUT = 5000;

        // Max buffer size of the async socket call
        const int MAX_BUFFER_SIZE = 2048;


        public string Connect(String host, int port)
        {
            string result = string.Empty;

            DnsEndPoint hostEntry = new DnsEndPoint(host, port);

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
            socketEventArg.RemoteEndPoint = hostEntry;

            socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
            {
                // Get the result of request
                result = e.SocketError.ToString();

                // Signal that the request is completed
                _clientDone.Set();
            });

            _clientDone.Reset();

            _socket.ConnectAsync(socketEventArg);

            _clientDone.WaitOne(TIMEOUT);

            return result;

        }



        public string Send(string data)
        {
            string response = "Operation Timeout";

            if (_socket != null)
            {
                // Create SocketAsyncEventArgs context object
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();

                // Set properties on context object
                socketEventArg.RemoteEndPoint = _socket.RemoteEndPoint;
                socketEventArg.UserToken = null;

                socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
                {
                    response = e.SocketError.ToString();

                    // Unblock the UI thread
                    _clientDone.Set();
                });

                // Add the data to be sent into the buffer
                byte[] payload = Encoding.UTF8.GetBytes(data);
                socketEventArg.SetBuffer(payload, 0, payload.Length);

                // Sets the state of the event to nonsignaled, causing threads to block
                _clientDone.Reset();

                // Make an async send request over the socket
                _socket.SendAsync(socketEventArg);

                // Block the UI thread for a max of TIMEOUT
                // If no response comes back within this time then proceed
                _clientDone.WaitOne(TIMEOUT);
            }
            else
            {
                response = "Socket is not initialized";
            }

            return response;
        }


        public string Receive()
        {
            string response = "Operation Timeout";

            if (_socket != null)
            {
                // Create SocketAsyncEventArgs object
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
                socketEventArg.RemoteEndPoint = _socket.RemoteEndPoint;

                // Setup the buffer to receive the data
                socketEventArg.SetBuffer(new Byte[MAX_BUFFER_SIZE], 0, MAX_BUFFER_SIZE);

                socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
                {
                    if (e.SocketError == SocketError.Success)
                    {
                        // Retrieve the data from the buffer
                        response = Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);
                        response = response.Trim('\0');
                    }
                    else
                    {
                        response = e.SocketError.ToString();
                    }

                    _clientDone.Set();
                });


                // Sets the state of the event to nonsignaled, causing threads to block
                _clientDone.Reset();

                // Make an async receive request over the socket
                _socket.ReceiveAsync(socketEventArg);

                // Block the UI thread for a max of TIMEOUT
                // If no response comes back within this time then proceed
                _clientDone.WaitOne(TIMEOUT);
            }
            else
            {
                response = "Socket is not initialized";
            }

            return response;
        }

        public void Close()
        {
            if (_socket != null)
            {
                _socket.Close();
            }
        }
    }
}
