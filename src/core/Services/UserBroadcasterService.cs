/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VRP.Core.Enums;
using VRP.Core.Interfaces;

namespace VRP.Core.Tools
{
    public class UsersBroadcasterService : IUserBroadcasterService, IDisposable
    {
        private readonly Socket _workingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public UsersBroadcasterService(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void Prepare()
        {
            IPAddress ip = IPAddress.Loopback;
            int port = Convert.ToInt32(_configuration.GetSection("UserBroadcastPort").Value);
            IPEndPoint endPoint = new IPEndPoint(ip, port);
            _workingSocket.Connect(endPoint);
            _logger.LogInfo($"[{nameof(UsersBroadcasterService)}] Prepared socket: {ip}:{port}.");
        }

        /// <summary>
        /// Method used to send information about player sign in/out to WebAPI
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="characterId"></param>
        /// <param name="token"></param>
        /// <param name="actionType"></param>
        public void Broadcast(int accountId, int characterId, Guid token, BroadcasterActionType actionType)
        {
            // Convert the string data to byte array using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(new
            {
                AccountId = accountId,
                Token = token,
                BroadcasterActionType = actionType
            }).ToCharArray());

            _logger.LogInfo($"[{nameof(UsersBroadcasterService)}][{DateTime.Now.ToShortTimeString()}] Sending data. " +
                            $"{{ token: {token} accountId: {accountId}, characterId: {characterId}, actionType: {actionType} }}");

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

                _logger.LogInfo($"[{nameof(UsersBroadcasterService)}][{DateTime.Now.ToShortTimeString()}] Data send successfully completed.");

            }
            catch (Exception e)
            {
                _logger.LogError($"[{nameof(UsersBroadcasterService)}][{DateTime.Now.ToShortTimeString()}] Data send unsuccessfully completed.");
                Console.WriteLine(e.ToString());
            }
        }

        public void Dispose()
        {
            _workingSocket?.Dispose();
        }
    }
}