using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using VRP.Core.Enums;

namespace VRP.Core.Tools
{
    public class UserBroadcaster
    {
        private readonly Socket _workingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        /// <summary>
        /// Prepare the socket
        /// </summary>
        private void Connect()
        {
            IPAddress ip = IPAddress.Loopback;
            int port = Convert.ToInt32(Singletons.Configuration.GetSection("UserBroadcastPort").Value);
            _workingSocket.Connect(new IPEndPoint(ip, port));
            Colorful.Console.WriteLine($"[Info][{nameof(UserBroadcaster)}] Prepared socket: {ip}:{port}.", Color.CornflowerBlue);
        }

        /// <summary>
        /// Method used to send information about player sign in/out to WebAPI
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="characterId"></param>
        /// <param name="token"></param>
        /// <param name="actionType"></param>
        public void Broadcast(int accountId, int characterId, string token, BroadcasterActionType actionType)
        {
            // Convert the string data to byte array using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(new
            {
                AccountId = accountId,
                CharacterId = characterId,
                Token = token,
            }).ToCharArray());

            if (!_workingSocket.Connected)
                Connect();

            Colorful.Console.WriteLine($"[Info][{nameof(UserBroadcaster)}][{DateTime.Now.ToShortTimeString()}] Sending data to WebAPI. " +
                                       $"{{token: {token} accountId: {accountId}, characterId: {characterId}, actionType: {actionType}", Color.CornflowerBlue);

            // Begin sending the data WebAPI  
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

                Colorful.Console.WriteLine($"[Info][{nameof(UserBroadcaster)}][{DateTime.Now.ToShortTimeString()}] Data send successfully completed.", Color.DarkGreen);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Colorful.Console.WriteLine($"[Info][{nameof(UserBroadcaster)}][{DateTime.Now.ToShortTimeString()}] Data send unsuccessfully completed.", Color.DarkRed);
                Console.WriteLine(e.ToString());
            }
        }
    }
}