/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ForumDatabaseHelper _forumDatabaseHelper = new ForumDatabaseHelper();

        [ServerEvent(Event.PlayerConnected)]
        public void OnPlayerConnected(Client client)
        {
            client.Dimension = (uint)Dimension.Login;
        }

        private void LoginToAccount(Client sender, string email, string password)
        {
            if (_forumDatabaseHelper.CheckPasswordMatch(email, password, out ForumLoginData forumLoginData))
            {
                using (RoleplayContext ctx = RolePlayContextFactory.NewContext())
                using (AccountsRepository repository = new AccountsRepository(ctx))
                {
                    AccountModel accountModel = new AccountModel
                    {
                        ForumUserId = forumLoginData.Id,
                        Name = forumLoginData.UserName,
                        ForumGroup = forumLoginData.GroupId,
                        OtherForumGroups = forumLoginData.OtherGroups,
                        Email = email,
                        SocialClub = sender.SocialClubName,
                        Ip = sender.Address,
                        Serial = sender.Serial,
                        LastLogin = DateTime.Now,
                        Online = true,
                        Characters = new List<CharacterModel>(),
                        Penalties = new List<PenaltyModel>(),
                    };

                    if (!repository.Contains(accountModel))
                    {
                        if (Enum.TryParse(typeof(ServerRank), ((ForumGroup)forumLoginData.GroupId).ToString(),
                            out object rank))
                            accountModel.ServerRank = (ServerRank)rank;
                        else
                            accountModel.ServerRank = ServerRank.Uzytkownik;

                        repository.Insert(accountModel);
                        repository.Save();
                    }

                    // We do this to see account Id
                    accountModel = repository.GetByUserId(accountModel.ForumUserId);

                    // Check if someone is logged on this account
                    AccountEntity account = EntityHelper.GetById(accountModel.Id);
                    if (account != null)
                    {
                        sender.TriggerEvent(RemoteEvents.PlayerNotifyRequested,
                            $"Osoba o IP: {account.DbModel.Ip} znajduje się obecnie na twoim koncie. Została ona wyrzucona z serwera. Rozważ zmianę hasła.",
                            NotificationType.Warning);
                        account.Kick(null, "Próba zalogowania na zalogowane konto.");
                    }

                    // Seed the data
                    if (accountModel.Characters.Count == 0)
                    {
                        using (CharactersRepository charactersRepository = new CharactersRepository(ctx))
                        {
                            string[] name = accountModel.Email.Split('@');
                            CharacterModel model = new CharacterModel()
                            {
                                Name = name[0],
                                Surname = name[1],
                                Model = PedHash.FreemodeMale01.ToString(),
                                Freemode = true,
                                IsAlive = true,
                                CreateTime = DateTime.Now,
                                Account = accountModel,
                            };

                            accountModel.Characters.Add(model);
                            charactersRepository.Insert(model);
                            charactersRepository.Save();
                        }
                    }

                    account = new AccountEntity(accountModel, sender);
                    account.Login();
                }
            }
            else
            {
                sender.TriggerEvent(RemoteEvents.PlayerNotifyRequested, "Podane login lub hasło są nieprawidłowe, bądź takie konto nie istnieje.", NotificationType.Info);
            }
        }

        [RemoteEvent(RemoteEvents.CharacterSelectRequested)]
        public void SelectCharacter(Client sender, params object[] args)
        {
            int characterIndex = Convert.ToInt32(args[0]);

            AccountEntity account = sender.GetAccountEntity();
            if (account == null)
            {
                sender.TriggerEvent(RemoteEvents.PlayerNotifyRequested, "Nie udało się załadować Twojego konta... Skontaktuj się z Administratorem!", NotificationType.Warning);
                return;
            }

            if (account.DbModel.Characters.Count == 0)
            {
                sender.TriggerEvent(RemoteEvents.PlayerNotifyRequested, "Twoje konto nie posiada żadnych postaci!", NotificationType.Info);

            }
            else
            {
                int characterId = account.DbModel.Characters.ToList()[characterIndex].Id;
                using (CharactersRepository repository = new CharactersRepository())
                {
                    CharacterModel characterModel = repository.Get(characterId);
                    CharacterEntity characterEntity = new CharacterEntity(account, characterModel);
                    characterEntity.LoginCharacter(account);
                    Singletons.UserBroadcaster.Broadcast(account.DbModel.Id, characterEntity.DbModel.Id, account.WebApiToken.ToString(), BroadcasterActionType.SignIn);
                }
            }
        }

        #region RemoteEvents

        [RemoteEvent(RemoteEvents.PlayerLoginRequested)]
        public void OnLoginRequested(Client sender, params object[] args)
        {
            LoginData loginData = JsonConvert.DeserializeObject<LoginData>(args[0].ToString());
            LoginToAccount(sender, loginData.Email, loginData.Password);
        }

        #endregion
    }
}
