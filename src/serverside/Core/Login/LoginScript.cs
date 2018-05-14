/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using GTANetworkAPI;
using Newtonsoft.Json;
using VRP.Core.Database;
using VRP.Core.Database.Forum;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Core.Repositories;
using VRP.Core.Tools;
using VRP.Serverside.Constant.RemoteEvents;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Core.Login.RemoteData;
using VRP.Serverside.Entities;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Core.Login
{
    public class LoginScript : Script
    {
        private readonly UserBroadcaster _userBroadcaster = new UserBroadcaster();

        public LoginScript()
        {
            AccountEntity.AccountLoggedOut += (sender, account) =>
            {
                _userBroadcaster.Broadcast(-1, -1, account.WebApiToken.ToString(), BroadcasterActionType.SignOut);
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
                sender.SendWarning("Nie udało się załadować Twojego konta... Skontaktuj się z Administratorem!");
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
            int userId = (int)args[0];

            using (AccountsRepository repository = new AccountsRepository())
            {
                AccountModel accountModel = repository.Get(userId);
                AccountEntity account = new AccountEntity(accountModel, sender);
                account.Login();
            }
        }

        #endregion
    }
}
