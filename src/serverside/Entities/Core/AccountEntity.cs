/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using GTANetworkAPI;
using VRP.Core.Database;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Core.Repositories;
using VRP.Core.Tools;
using VRP.Serverside.Constant.RemoteEvents;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Interfaces;

namespace VRP.Serverside.Entities.Core
{
    public class AccountEntity : IDbEntity<AccountModel>, IDisposable
    {
        public delegate void AccountLogOutEventHandler(Client sender, AccountEntity account);

        public static event AccountLoginEventHandler AccountLoggedIn;
        public static event AccountLogOutEventHandler AccountLoggedOut;

        public AccountModel DbModel { get; set; }

        public Client Client { get; }
        public CharacterEntity CharacterEntity;

        public Action<Client> HereHandler { get; set; }

        public int ServerId { get; set; }
        public Guid WebApiToken { get; set; }

        public AccountEntity(AccountModel dbModel, Client client)
        {
            DbModel = dbModel;
            Client = client;
        }

        public void Login()
        {
            Client.SetData("RP_ACCOUNT", this);

            DbModel.LastLogin = DateTime.Now;
            DbModel.Online = true;

            string[] ip = DbModel.Ip.Split('.');
            string safeIp = $"{ip[0]}.{ip[1]}.***.***";

            Client.SendInfo(
                $"Witaj, {DbModel.Name} zostałeś pomyślnie zalogowany. Ostatnie logowanie:" +
                $" {DbModel.LastLogin.ToShortDateString()} {DbModel.LastLogin.ToShortTimeString()} " +
                $"Z adresu IP: {safeIp}");

            EntityHelper.Add(this);

            // setting webapi token
            WebApiToken = Guid.NewGuid();

            // calling static event telling that player logged in
            AccountLoggedIn?.Invoke(Client, this);

            // calling login pass on clientside
            Client.TriggerEvent(RemoteEvents.PlayerLoginPassed, WebApiToken, DbModel.Id);
        }

        public void Kick(AccountEntity creator, string reason)
        {
            using (PenaltiesRepository repository = new PenaltiesRepository())
            {
                PenaltyModel model = new PenaltyModel()
                {
                    Creator = creator?.DbModel,
                    Account = DbModel,
                    Date = DateTime.Now,
                    PenaltyType = PenaltyType.Kick,
                    Reason = reason
                };

                repository.Insert(model);
                repository.Save();
            }

            NAPI.Player.KickPlayer(Client, reason);
            Dispose();
        }

        public void Ban(AccountEntity creator, string reason, DateTime expiryDate)
        {
            using (PenaltiesRepository repository = new PenaltiesRepository())
            {
                PenaltyModel model = new PenaltyModel()
                {
                    Creator = creator.DbModel,
                    Account = DbModel,
                    Date = DateTime.Now,
                    PenaltyType = PenaltyType.Ban,
                    Reason = reason,
                    ExpiryDate = expiryDate
                };

                repository.Insert(model);
                repository.Save();
            }

            NAPI.Player.BanPlayer(Client, reason);
            Dispose();
        }

        public void Save()
        {
            using (AccountsRepository accountsRepository = new AccountsRepository())
            {
                accountsRepository.Update(DbModel);
                accountsRepository.Save();
            }

            //tutaj wywołać metody synchronizacji danych z innych controllerów np character.
            //CharacterEntity?.Save(resourceStop);

            //ContextFactory.Instance.Accounts.Attach(DbModel);
            //ContextFactory.Instance.Entry(DbModel).State = EntityState.Modified;
            //ContextFactory.Instance.SaveChanges();
        }

        public void Dispose()
        {
            Singletons.UserBroadcaster.Broadcast(-1, -1, WebApiToken.ToString(), BroadcasterActionType.SignOut);
            EntityHelper.Remove(this);
            DbModel.Online = false;
            Save();
            CharacterEntity?.Dispose();
            AccountLoggedOut?.Invoke(Client, this);
        }
    }
}