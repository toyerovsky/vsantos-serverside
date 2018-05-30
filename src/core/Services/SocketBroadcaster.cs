using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using VRP.Core.Interfaces;

namespace VRP.Core.Services
{
    public class SocketBroadcaster : IBroadcaster
    {
        private readonly Socket _workingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private readonly ILogger _logger;
        private readonly IPEndPoint _ipEndPoint;

        public SocketBroadcaster(IPAddress ip, int port, ILogger logger) : this(new IPEndPoint(ip, port), logger) { }

        public SocketBroadcaster(IPEndPoint endPoint, ILogger logger)
        {
            _ipEndPoint = endPoint;
            _logger = logger;
            PrepareSocket();
        }

        public void Broadcast(string json)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(json.ToCharArray());
            _workingSocket.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, _workingSocket);
        }

        /// <summary>
        /// Private Broadcaster callback method used with BeginSend
        /// </summary>
        /// <param name="asyncResult"></param>
        private void SendCallback(IAsyncResult asyncResult)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)asyncResult.AsyncState;
                // Complete sending the data to the remote device.  
                handler.EndSend(asyncResult);

                _logger.LogInfo($"[{nameof(SocketBroadcaster)}][{DateTime.Now.ToShortTimeString()}] Data send successfully completed.");

            }
            catch (Exception e)
            {
                _logger.LogError($"[{nameof(SocketBroadcaster)}][{DateTime.Now.ToShortTimeString()}] Data send unsuccessfully completed.");
                Console.WriteLine(e.ToString());
            }
        }

        private void PrepareSocket()
        {
            _workingSocket.Connect(_ipEndPoint);
            _logger.LogInfo($"[{nameof(SocketBroadcaster)}] Prepared socket: {_ipEndPoint.Address}:{_ipEndPoint.Port}.");
        }

        public void Dispose()
        {
            _workingSocket?.Dispose();
        }
    }
}