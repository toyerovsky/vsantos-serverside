/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Linq;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Serverside.Core.Database.Models;
using Serverside.Core.Extensions;
using Serverside.Core.Repositories;
using Serverside.Entities.EventArgs;
using Serverside.Entities.Interfaces;
using Serverside.Items;

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

            client.SetData("RP_ACCOUNT", this);

            DbModel.LastLogin = DateTime.Now;
            DbModel.Online = true;

            //tutaj dajemy inne rzeczy które mają być inicjowane po zalogowaniu się na konto, np: wybór postaci.

            string[] ip = DbModel.Ip.Split('.');
            string safeIp = $"{ip[0]}.{ip[1]}.***.***";
            client.Notify($"Witaj, ~g~~h~{DbModel.Name} zostałeś pomyślnie zalogowany. ~n~Ostatnie logowanie: {DbModel.LastLogin} z adresu IP: {safeIp}");

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
            
            EntityManager.AddAccount(AccountId, this);
            AccountLoggedIn?.Invoke(client, this);
        }

        public static AccountEntity GetAccountControllerFromName(string formatname)
        {
            Client client = EntityManager.GetAccounts().First(x => x.Value.CharacterEntity.FormatName.ToLower().Contains(formatname.ToLower())).Value.Client;
            return client?.GetAccountEntity();
        }

        public static void LoadAccount(Client sender, long userId)
        {
            using (AccountsRepository repository = new AccountsRepository())
                new AccountEntity(repository.GetAll().FirstOrDefault(x => x.UserId == userId), sender);
        }

        public static void RegisterAccount(Client sender, AccountModel accountModel)
        {
            using (AccountsRepository repository = new AccountsRepository())
            {
                repository.Insert(accountModel);
                repository.Save();

                new AccountEntity(accountModel, sender);
            }
        }

        public void Save()
        {
            //tutaj wywołać metody synchronizacji danych z innych controllerów np character.
            //CharacterEntity?.Save(resourceStop);

            //ContextFactory.Instance.Accounts.Attach(DbModel);
            //ContextFactory.Instance.Entry(DbModel).State = EntityState.Modified;
            //ContextFactory.Instance.SaveChanges();
        }

        public void Dispose()
        {
            CharacterEntity?.Dispose();
        }
    }
}