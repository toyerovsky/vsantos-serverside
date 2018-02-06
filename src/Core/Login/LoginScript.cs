/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Serverside.Admin.Enums;
using Serverside.Constant;
using Serverside.Core.Database;
using Serverside.Core.Database.Models;
using Serverside.Core.Enums;
using Serverside.Core.Extensions;
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
            Tools.ConsoleOutput($"[{nameof(LoginScript)}] {ConstantMessages.ResourceStartMessage}", ConsoleColor.DarkMagenta);
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

        public static void LoginToAccount(Client sender, string email, string password)
        {
            Tuple<long, string, short, string> userData = ForumDatabaseHelper.CheckPasswordMatch(email, password);
            if (userData.Item1 == -1)
            {
                //args[0] wiadomosc
                //args[1] czas wyswietlania w ms
                sender.TriggerEvent("ShowNotification", "Podane login lub hasło są nieprawidłowe, bądź takie konto nie istnieje", 3000);
            }
            else
            {
                AccountModel accountModel = new AccountModel
                {
                    UserId = userData.Item1,
                    Name = userData.Item2,
                    ForumGroup = userData.Item3,
                    OtherForumGroups = userData.Item4,
                    Email = email,
                    SocialClub = sender.SocialClubName,
                    Ip = sender.Address,
                    Serial = sender.Serial
                };

                //Sprawdzenie czy konto z danym userid istnieje jak nie dodanie konta do bazy danych i załadowanie go do core.
                //Dodanie grupy serwerowej do konta //toyer

                //FixMe
                //if (!AccountEntity.DoesAccountExist(userData.Item1))
                //{
                //    if (Enum.GetNames(typeof(ServerRank)).Any(e => e == ((ForumGroup)userData.Item3).ToString()))
                //        accountModel.ServerRank = (ServerRank)Enum.Parse(typeof(ServerRank), ((ForumGroup)userData.Item3).ToString());
                //    else
                //        accountModel.ServerRank = ServerRank.Uzytkownik;

                //    AccountEntity.RegisterAccount(sender, accountModel);
                //}
                //else
                //{
                //    //Sprawdzenie czy ktoś już jest zalogowany z tego konta.
                //    AccountEntity account = EntityManager.GetAccount(userData.Item1);
                //    if (account != null)
                //    {
                //        if (account.DbModel.Online)
                //        {
                //            NAPI.Player.KickPlayer(account.Client);
                //            ChatScript.SendMessageToPlayer(sender,
                //                $"Osoba o IP: {account.DbModel.Ip} znajduje się obecnie na twoim koncie. Została ona wyrzucona z serwera. Rozważ zmianę hasła.", ChatMessageType.ServerInfo);
                //        }
                //    }
                //    AccountEntity.LoadAccount(sender, userData.Item1);
                //}
            }
        }

        public static void LoginMenu(Client player)
        {
            //Nie używać tego wymiaru, jest zajęty na logowanie
            player.Dimension = (uint)Dimension.Login;
            player.Position = new Vector3(-1666f, -1020f, 12f);
            player.TriggerEvent("ShowLoginMenu");
        }

        public static void LogOut(AccountEntity account)
        {
            account.DbModel.Online = false;
            if (account.CharacterEntity != null)
                account.CharacterEntity.DbModel.Online = false;
            account.Save();
            account.Client.ResetData("RP_ACCOUNT");
            EntityManager.RemoveAccount(account.AccountId);
        }

        public static void LogOut(Client player)
        {
            AccountEntity account = player.GetAccountEntity();
            account.DbModel.Online = false;
            if (account.CharacterEntity != null)
                account.CharacterEntity.DbModel.Online = false;
            account.Save();
            player.ResetData("RP_ACCOUNT");
            EntityManager.RemoveAccount(account.AccountId);
        }
    }
}
