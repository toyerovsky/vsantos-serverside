/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Core.Interfaces;
using VRP.Core.Services.Model;
using VRP.Core.Tools;

namespace VRP.Core.Services
{
    public class UsersWatcherService : IUsersWatcherService
    {
        private readonly ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        private Task WatcherTask { get; set; }

        public event EventHandler<ActionData> AccountLoggedOut;
        public event EventHandler<ActionData> AccountLoggedIn;

        public event EventHandler ConnectionEstablished;

        public UsersWatcherService(IConfiguration configuration, ILogger logger)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public UsersWatcherService(IConfiguration configuration, ILogger logger,
            EventHandler<ActionData> logInAction, EventHandler<ActionData> logOutAction)
        {
            _logger = logger;
            _configuration = configuration;

            AccountLoggedIn += logInAction;
            AccountLoggedOut += logOutAction;
            Watch();
        }

        public void Watch()
        {
            WatcherTask = Task.Run(() =>
            {
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Loopback,
                    _configuration.GetValue<int>("UserWatchPort"));
                Socket socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    socketListener.Bind(localEndPoint);
                    socketListener.Listen(100);

                    while (true)
                    {
                        _manualResetEvent.Reset();
                        socketListener.BeginAccept(AcceptCallback, socketListener);
                        _manualResetEvent.WaitOne();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });
        }

        private void AcceptCallback(IAsyncResult asyncResult)
        {
            _manualResetEvent.Set();
            Socket listener = (Socket)asyncResult.AsyncState;
            State state = new State { WorkSocket = listener.EndAccept(asyncResult) };
            //Calling Read
            state.WorkSocket.BeginReceive(state.Buffer, 0, 1024, 0, ReadCallback, state);
        }

        private void ReadCallback(IAsyncResult asyncResult)
        {
            State state = (State)asyncResult.AsyncState;

            int bytesRead = state.WorkSocket.EndReceive(asyncResult);

            if (bytesRead > 0)
            {
                state.RecievedData.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));

                string content = state.RecievedData.ToString();

                ActionData data = JsonConvert.DeserializeObject<ActionData>(content);

                if (data.ActionType == BroadcasterActionType.LogOut)
                {
                    // handle user log out 
                    AccountLoggedOut?.Invoke(this, data);
                }
                else if (data.ActionType == BroadcasterActionType.LogIn)
                {
                    // handle user log in
                    AccountLoggedIn?.Invoke(this, data);
                }
                else if (data.ActionType == BroadcasterActionType.Ready)
                {
                    ConnectionEstablished?.Invoke(this, new EventArgs());
                }

            }
        }

        public void Dispose()
        {
            _manualResetEvent?.Dispose();
            WatcherTask?.Dispose();
        }
    }
}