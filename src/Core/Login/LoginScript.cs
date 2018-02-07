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
using Serverside.Constant;
using Serverside.Constant.RemoteEvents;
using Serverside.Core.Database;
using Serverside.Core.Database.Forum;
using Serverside.Core.Database.Models;
using Serverside.Core.Enums;
using Serverside.Core.Extensions;
using Serverside.Core.Repositories;
using Serverside.Core.Scripts;
using Serverside.Entities;
using Serverside.Entities.Core;
using NAPI = GTANetworkAPI.NAPI;

namespace Serverside.Core.Login
{
    public sealed class LoginScript : Script
    {
        private static readonly ForumDatabaseHelper ForumDatabaseHelper = new ForumDatabaseHelper();

        public LoginScript()
        {
            Event.OnResourceStart += API_onResourceStart;
            Event.OnPlayerConnect += Event_OnPlayerConnect;
            AccountEntity.AccountLoggedIn += RPLogin_OnPlayerLogin;
        }


        private void Event_OnPlayerConnect(Client player, CancelEventArgs cancel)
        {
            if (!player.IsCeFenabled)
                cancel.Cancel = true;
        }

        private void API_onResourceStart()
        {
            Tools.ConsoleOutput($"[{nameof(LoginScript)}] {Messages.ResourceStartMessage}", ConsoleColor.DarkMagenta);
        }

        private void Event_OnClientEventTrigger(Client player, string eventName, params object[] args)
        {
            if (eventName == "OnPlayerEnteredLoginData")
            {
                LoginToAccount(player, args[0].ToString(), args[1].ToString());
            }
            //Przy używaniu tego musimy jako args[0] wysłać indeks na liście postaci
            else if (eventName == "OnPlayerSelectedCharacter")
            {
                int characterId = Convert.ToInt32(args[0]);
                //FixMe
                //CharacterEntity.SelectCharacter(player, characterId);
            }
        }

        private void RPLogin_OnPlayerLogin(Client sender, AccountEntity account)
        {
            var chs = account.DbModel.Characters.Where(c => c.IsAlive).Select(x => new { x.Name, x.Surname, x.Money, x.BankMoney }).ToList();
            string json = JsonConvert.SerializeObject(chs);
            sender.TriggerEvent("ShowCharacterSelectMenu", json);
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

                    AccountEntity accountEntity;

                    if (repository.Contains(accountModel))
                    {
                        if (Enum.TryParse(typeof(ServerRank), ((ForumGroup)forumLoginData.GroupId).ToString(),
                            out var rank))
                            accountModel.ServerRank = (ServerRank)rank;
                        else
                            accountModel.ServerRank = ServerRank.Uzytkownik;

                        repository.Insert(accountModel);
                        repository.Save();

                        accountEntity = new AccountEntity(accountModel, sender);
                    }
                    else
                    {
                        //Sprawdzenie czy ktoś już jest zalogowany z tego konta.
                        AccountEntity account = EntityManager.Get(accountModel.Id);
                        if (account != null)
                        {
                            //FixMe dać wiadomość jako warning
                            ChatScript.SendMessageToPlayer(sender,
                                $"Osoba o IP: {account.DbModel.Ip} znajduje się obecnie na twoim koncie. Została ona wyrzucona z serwera. Rozważ zmianę hasła.", ChatMessageType.ServerInfo);
                            
                            account.Kick(null, "Próba zalogowania na zalogowane konto.");
                        }
                        accountEntity = new AccountEntity(repository.Get(accountModel.Id), sender);
                    }
                    accountEntity.Login();
                }
            }
            else
            {
                sender.TriggerEvent("ShowNotification", "Podane login lub hasło są nieprawidłowe, bądź takie konto nie istnieje", 3000);
            }
        }

        public static void LoginMenu(Client player)
        {
            //Nie używać tego wymiaru, jest zajęty na logowanie
            player.Dimension = (uint)Dimension.Login;
            player.Position = new Vector3(-1666f, -1020f, 12f);
            player.TriggerEvent("ShowLoginMenu");
        }

        #region RemoteEvents

        [RemoteEvent(RemoteEvents.PlayerLoginRequested)]
        public void OnLoginRequested(Client sender, params object[] args)
        {

        }

        #endregion
    }
}
