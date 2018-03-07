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
using VRP.Serverside.Constant.RemoteEvents;
using VRP.Serverside.Core.Login.RemoteData;
using VRP.Serverside.Core.Scripts;
using VRP.Serverside.Entities;
using VRP.Serverside.Entities.Core;
using ChatMessageType = VRP.Core.Enums.ChatMessageType;

namespace VRP.Serverside.Core.Login
{
    public class LoginScript : Script
    {
        private static readonly ForumDatabaseHelper ForumDatabaseHelper = new ForumDatabaseHelper();

        public LoginScript()
        {
            AccountEntity.AccountLoggedIn += RPLogin_OnPlayerLogin;
        }

        private void EventOnOnPlayerConnected(Client client)
        {
            client.Dimension = (uint)Dimension.Login;
            client.FreezePosition = true;
        }

        private void Event_OnClientEventTrigger(Client player, string eventName, params object[] args)
        {
            //Przy używaniu tego musimy jako args[0] wysłać indeks na liście postaci
            if (eventName == "OnPlayerSelectedCharacter")
            {
                int characterId = Convert.ToInt32(args[0]);
                //FixMe
                //CharacterEntity.SelectCharacter(player, characterId);
            }
        }

        private void RPLogin_OnPlayerLogin(Client sender, AccountEntity account)
        {
            var characters = account.DbModel.Characters
                .Where(c => c.IsAlive)
                .Select(x => new
                {
                    name = x.Name,
                    surname = x.Surname,
                    money = x.Money,
                    playedTime = x.PlayedTime
                });

            string json = JsonConvert.SerializeObject(characters);
            sender.TriggerEvent(RemoteEvents.PlayerLoginPassed, json);
        }

        private void LoginToAccount(Client sender, string email, string password)
        {
            if (ForumDatabaseHelper.CheckPasswordMatch(email, password, out ForumLoginData forumLoginData))
            {
                using (RoleplayContext ctx = RolePlayContextFactory.NewContext())
                using (AccountsRepository repository = new AccountsRepository(ctx))
                {
                    AccountModel accountModel = new AccountModel
                    {
                        UserId = forumLoginData.Id,
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

                    //We do this to see account Id
                    accountModel = repository.GetByUserId(accountModel.UserId);

                    //Check if someone is logged on this account
                    AccountEntity account = EntityHelper.Get(accountModel.Id);
                    if (account != null)
                    {
                        //FixMe dać wiadomość jako warning
                        ChatScript.SendMessageToPlayer(sender,
                            $"Osoba o IP: {account.DbModel.Ip} znajduje się obecnie na twoim koncie. Została ona wyrzucona z serwera. Rozważ zmianę hasła.", ChatMessageType.ServerInfo);

                        account.Kick(null, "Próba zalogowania na zalogowane konto.");
                    }

                    //Seed the data
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
                sender.SendNotification("Podane login lub hasło są nieprawidłowe, bądź takie konto nie istnieje.");
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
