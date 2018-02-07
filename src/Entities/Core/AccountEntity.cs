﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using GTANetworkAPI;
using Serverside.Core.Database.Models;
using Serverside.Core.Enums;
using Serverside.Core.Extensions;
using Serverside.Core.Repositories;
using Serverside.Entities.EventArgs;
using Serverside.Entities.Interfaces;

namespace Serverside.Entities.Core
{
    public class AccountEntity : IDbEntity<AccountModel>, IDisposable
    {
        public static event AccountLoginEventHandler AccountLoggedIn;
        public static event EventHandler<ServerIdChangeEventArgs> ServerIdChanged;

        public long AccountId => DbModel.UserId;
        public AccountModel DbModel { get; set; }

        public Client Client { get; }
        public CharacterEntity CharacterEntity;

        private int _serverId = -1;
        public int ServerId
        {
            get
            {
                var id = EntityManager.CalculateServerId(this);
                ServerIdChanged?.Invoke(this, new ServerIdChangeEventArgs(_serverId, id));
                return _serverId = id;
            }
        }

        public AccountEntity(AccountModel data, Client client)
        {
            DbModel = data;
            Client = client;
        }

        public void Login()
        {
            Client.SetData("RP_ACCOUNT", this);

            DbModel.LastLogin = DateTime.Now;
            DbModel.Online = true;

            //tutaj dajemy inne rzeczy które mają być inicjowane po zalogowaniu się na konto, np: wybór postaci.

            string[] ip = DbModel.Ip.Split('.');
            string safeIp = $"{ip[0]}.{ip[1]}.***.***";
            Client.Notify($"Witaj, ~g~~h~{DbModel.Name} zostałeś pomyślnie zalogowany. ~n~Ostatnie logowanie: {DbModel.LastLogin} z adresu IP: {safeIp}");

            using (CharactersRepository repository = new CharactersRepository())
            {
                if (DbModel.Characters == null || DbModel.Characters.Count == 0)
                {
                    string[] email = DbModel.Email.Split('@');

                    CharacterModel model = new CharacterModel()
                    {
                        Name = email[0],
                        Surname = email[1],
                        Model = PedHash.FreemodeMale01,
                        Freemode = true
                    };

                    repository.Insert(model);
                }
                repository.Save();
            }

            EntityManager.Add(this);
            AccountLoggedIn?.Invoke(Client, this);
        }

        public void Kick(AccountEntity creator, string reason)
        {
            using (PenaltiesRepository repository = new PenaltiesRepository())
            {
                PenaltyModel model = new PenaltyModel()
                {
                    Creator = creator.DbModel,
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
            EntityManager.Remove(this);

            DbModel.Online = false;
            Save();

            CharacterEntity?.Dispose();
        }
    }
}