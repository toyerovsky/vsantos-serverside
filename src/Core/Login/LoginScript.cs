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
using Serverside.Admin.Enums;
using Serverside.Constant.RemoteEvents;
using Serverside.Core.Database.Forum;
using Serverside.Core.Database.Models;
using Serverside.Core.Enums;
using Serverside.Core.Login.RemoteData;
using Serverside.Core.Repositories;
using Serverside.Core.Scripts;
using Serverside.Entities;
using Serverside.Entities.Core;

namespace Serverside.Core.Login
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
                    x.Name,
                    x.Surname,
                    x.Money,
                    x.PlayedTime
                }).ToList();

            string json = JsonConvert.SerializeObject(characters);
            sender.TriggerEvent(RemoteEvents.PlayerLoginPassed, json);
        }

        private void LoginToAccount(Client sender, string email, string password)
        {
            if (ForumDatabaseHelper.CheckPasswordMatch(email, password, out var forumLoginData))
            {
                using (AccountsRepository repository = new AccountsRepository())
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
                            out var rank))
                            accountModel.ServerRank = (ServerRank)rank;
                        else
                            accountModel.ServerRank = ServerRank.Uzytkownik;

                        repository.Insert(accountModel);
                        repository.Save();
                    }

                    //Robimy tak, aby poznać Id konta
                    accountModel = repository.GetByUserId(accountModel.UserId);

                    //Sprawdzenie czy ktoś już jest zalogowany z tego konta.
                    AccountEntity account = EntityHelper.Get(accountModel.Id);
                    if (account != null)
                    {
                        //FixMe dać wiadomość jako warning
                        ChatScript.SendMessageToPlayer(sender,
                            $"Osoba o IP: {account.DbModel.Ip} znajduje się obecnie na twoim koncie. Została ona wyrzucona z serwera. Rozważ zmianę hasła.", ChatMessageType.ServerInfo);

                        account.Kick(null, "Próba zalogowania na zalogowane konto.");
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
