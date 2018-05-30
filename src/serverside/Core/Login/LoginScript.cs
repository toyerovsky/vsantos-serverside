/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Core.Repositories;
using VRP.Serverside.Constant.RemoteEvents;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Core.Login
{
    public class LoginScript : Script
    {
        private readonly IDictionary<Guid, int> _usersInLogin = new Dictionary<Guid, int>();
        
        public LoginScript()
        {
            Singletons.LogInWatcher.AccountLoggedIn += (sender, data) =>
            {
                _usersInLogin.Add(data.TempUserToken, data.AccountId);
            };
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnOnPlayerDisconnected(Client client, DisconnectionType type, string reason)
        {
            AccountEntity account = client.GetAccountEntity();
            account?.Dispose();
        }

        [ServerEvent(Event.PlayerConnected)]
        public void OnPlayerConnected(Client client)
        {
            client.Dimension = (uint)Dimension.Login;
        }

        [RemoteEvent(RemoteEvents.CharacterSelectRequested)]
        public void SelectCharacter(Client sender, params object[] args)
        {
            int characterIndex = Convert.ToInt32(args[0]);

            AccountEntity account = sender.GetAccountEntity();
            if (account == null)
            {
                sender.SendError("Nie udało się załadować Twojego konta... Skontaktuj się z Administratorem!");
                return;
            }

            int characterId = account.DbModel.Characters.ToList()[characterIndex].Id;
            using (CharactersRepository repository = new CharactersRepository())
            {
                CharacterModel characterModel = repository.Get(characterId);
                CharacterEntity characterEntity = new CharacterEntity(characterModel);
                characterEntity.SelectCharacter(account);
            }
        }

        #region RemoteEvents

        [RemoteEvent(RemoteEvents.PlayerLoginRequested)]
        public void OnLoginRequested(Client sender, params object[] args)
        {
            Guid userGuid = Guid.Parse(args[0].ToString());
            if (_usersInLogin.ContainsKey(userGuid))
            {
                using (AccountsRepository repository = new AccountsRepository())
                {
                    AccountModel accountModel = repository.Get(_usersInLogin[userGuid]);
                    AccountEntity account = new AccountEntity(accountModel, sender);
                    account.Login(userGuid);
                    _usersInLogin.Remove(userGuid);
                }
            }
            else
            {
                sender.Kick("Nie udało się zalogować. Skontaktuj się z administratorem.");
            }
        }

        #endregion
    }
}
