/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VRP.Core.Database;
using VRP.Core.Enums;
using VRP.Core.Repositories;
using VRP.vAPI.Model;
using VRP.vAPI.Services.Model;

namespace VRP.vAPI.Services
{
    public class UsersWatcher : IUsersWatcher, IDisposable
    {
        /// <summary>
        /// Key is user token
        /// </summary>
        private readonly Dictionary<Guid, AppUser> _onlineUsers = new Dictionary<Guid, AppUser>();
        private readonly ManualResetEvent _manualResetEvent = new ManualResetEvent(false);

        private Task WatcherTask { get; }

        public UsersWatcher()
        {
            WatcherTask = Task.Run(() => Watch());
        }

        private void Watch()
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Loopback,
                Startup.Configuration.GetValue<int>("UserListenPort"));
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
                // Check for end-of-file tag. If it is not there, read more data. 
                if (content.IndexOf("<EOF>", StringComparison.Ordinal) > -1)
                {
                    UserData data = JsonConvert.DeserializeObject<UserData>(content);

                    if (data.ActionType == BroadcasterActionType.SignIn)
                    {
                        // Add user to dictionary
                        using (RoleplayContext ctx = RolePlayContextFactory.NewContext())
                        using (AccountsRepository accountsRepository = new AccountsRepository(ctx))
                        using (CharactersRepository charactersRepository = new CharactersRepository(ctx))
                        {
                            AppUser appUser = data.CharacterId != -1 ?
                                new AppUser(accountsRepository.Get(data.AccountId), charactersRepository.Get(data.CharacterId)) :
                                new AppUser(accountsRepository.Get(data.AccountId));

                            _onlineUsers.Add(Guid.Parse(data.Token), appUser);
                        }
                    }
                    else
                    {
                        // Remove user from dictionary
                        _onlineUsers.Remove(Guid.Parse(data.Token));
                    }
                }
                else
                {
                    state.WorkSocket.BeginReceive(state.Buffer, 0, 1024, 0, ReadCallback, state);
                }
            }
        }

        public bool IsUserOnline(Guid token) => _onlineUsers.ContainsKey(token);

        public bool TryGetUser(Guid token, out AppUser appUser) =>
            _onlineUsers.TryGetValue(token, out appUser);

        public void Dispose()
        {
            _manualResetEvent?.Dispose();
            WatcherTask?.Dispose();
        }
    }
}