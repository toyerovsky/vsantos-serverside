/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Linq;
using GTANetworkAPI;
using VRP.Core.Database.Forum;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Core.Repositories;
using VRP.Serverside.Constant.RemoteEvents;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Core.Login
{
    public class LoginScript : Script
    {
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
            if (Guid.TryParse(args[0].ToString(), out Guid userGuid)
                && int.TryParse(args[1].ToString(), out int accountId)
                && int.TryParse(args[2].ToString(), out int characterIndex))
            {
                if (EntityHelper.GetAccounts().All(account => account.DbModel.Id != accountId))
                {
                    using (AccountsRepository accountsRepository = new AccountsRepository())
                    using (CharactersRepository charactersRepository = new CharactersRepository())
                    {
                        AccountModel accountModel = accountsRepository.Get(accountId);
                        AccountEntity accountEntity = new AccountEntity(accountModel, sender);
                        accountEntity.Login(userGuid);

                        int characterId = accountEntity.DbModel.Characters.Where(character => character.IsAlive).ToList()[characterIndex].Id;

                        CharacterModel characterModel = charactersRepository.Get(characterId);
                        CharacterEntity characterEntity = new CharacterEntity(accountEntity, characterModel);
                        accountEntity.SelectCharacter(characterEntity);
                    }
                }
            }
            else
            {
                sender.Kick("Nie udało się zalogować. Skontaktuj się z administratorem.");
            }
        }
    }
}
