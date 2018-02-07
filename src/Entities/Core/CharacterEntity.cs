/* Copyright (C) Przemys�aw Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemys�aw Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Globalization;
using System.Linq;
using GTANetworkAPI;
using GTANetworkInternals;
using Microsoft.EntityFrameworkCore;
using Serverside.Constant;
using Serverside.Core.Database.Models;
using Serverside.Core.Description;
using Serverside.Core.Extensions;
using Serverside.Core.Repositories;
using Serverside.Entities.Game;
using Serverside.Entities.Interfaces;
using Serverside.Items;

namespace Serverside.Entities.Core
{
    public class CharacterEntity : IDbEntity<CharacterModel>
    {
        public CharacterModel DbModel { get; set; }

        public AccountEntity AccountEntity { get; private set; }
        public GroupEntity OnDutyGroup { get; set; }
        public Description Description { get; set; }
        public CharacterCreator.CharacterCreator CharacterCreator { get; set; }
        public BuildingEntity CurrentBuilding { get; set; }

        internal Cellphone CurrentCellphone { get; set; }

        public string FormatName => $"{DbModel.Name} {DbModel.Surname}";
        
        public event DimensionChangeEventHandler OnPlayerDimensionChanged;
        public static event CharacterLoginEventHandler CharacterLoggedIn;

        public EventClass Events { get; set; }

        public bool CanSendPrivateMessage { get; set; } = false;
        public bool CanCommand { get; set; } = false;
        public bool CanTalk { get; set; } = false;
        public bool CanNarrate { get; set; } = false;
        public bool CanPay { get; set; } = false;

        public CharacterEntity(EventClass events, AccountEntity accountEntity, CharacterModel dbModel)
        {
            Events = events;

            DbModel = dbModel;
            AccountEntity = accountEntity;
            AccountEntity.CharacterEntity = this;
            DbModel.AccountModel = accountEntity.DbModel;
            DbModel.LastLoginTime = DateTime.Now;
            DbModel.Online = true;

            using (CharactersRepository repository = new CharactersRepository())
            {
                repository.Update(dbModel);
                repository.Save();
            }

            if (DbModel.Freemode)
                CharacterCreator = new CharacterCreator.CharacterCreator(this);
            Description = new Description(Events, AccountEntity);
        }

        public CharacterEntity(EventClass events, AccountEntity accountEntity, string name, string surname, PedHash model)
        {
            Events = events;

            Random random = new Random();
            var randomIndex = random.Next(Constant.Items.ServerSpawnPositions.Count);

            DbModel = new CharacterModel();
            accountEntity.CharacterEntity = this;
            DbModel.AccountModel = accountEntity.DbModel;
            DbModel.Name = name;
            DbModel.Surname = surname;
            DbModel.Money = 10000;
            DbModel.BankMoney = 1000000;
            DbModel.CreateTime = DateTime.Now;
            DbModel.Model = model;
            DbModel.HitPoints = 100;
            DbModel.IsAlive = true;
            DbModel.LastPositionX = Constant.Items.ServerSpawnPositions[randomIndex].X;
            DbModel.LastPositionY = Constant.Items.ServerSpawnPositions[randomIndex].Y;
            DbModel.LastPositionZ = Constant.Items.ServerSpawnPositions[randomIndex].Z;
            DbModel.LastPositionRotX = 0f;
            DbModel.LastPositionRotY = 0f;
            DbModel.LastPositionRotZ = 0f;

            using (CharactersRepository repository = new CharactersRepository())
            {
                repository.Insert(DbModel);
                repository.Save();
            }

            if (DbModel.Freemode)
                CharacterCreator = new CharacterCreator.CharacterCreator(this);
            Description = new Description(Events, accountEntity);
        }

        public void Save()
        {
            if (AccountEntity != null)
            {
                DbModel.CurrentDimension = AccountEntity.Client.Dimension;
                DbModel.LastPositionX = AccountEntity.Client.Position.X;
                DbModel.LastPositionY = AccountEntity.Client.Position.Y;
                DbModel.LastPositionZ = AccountEntity.Client.Position.Z;
                DbModel.LastPositionRotX = AccountEntity.Client.Rotation.X;
                DbModel.LastPositionRotY = AccountEntity.Client.Rotation.Y;
                DbModel.LastPositionRotZ = AccountEntity.Client.Rotation.Z;
            }
            using (CharactersRepository repository = new CharactersRepository())
            {
                repository.Update(DbModel);
                repository.Save();
            }
        }

        public static void SelectCharacter(EventClass events, Client player, int selectId)
        {
            AccountEntity account = player.GetAccountEntity();
            if (account == null)
            {
                player.Notify("Nie uda�o si� za�adowa� Twojego konta... Skontaktuj si� z Administratorem!");
                return;
            }

            if (account.DbModel.Characters.Count == 0)
            {
                player.Notify("Twoje konto nie posiada �adnych postaci!");
            }
            else
            {
                long characterId = account.DbModel.Characters.ToList()[selectId].Id;
                using (CharactersRepository repository = new CharactersRepository())
                {
                    CharacterModel characterData = repository.Get(characterId);
                    CharacterEntity characterEntity = new CharacterEntity(events, account, characterData);
                    characterEntity.LoginCharacter(account);
                }
            }
        }

        public void LoginCharacter(AccountEntity accountEntity)
        {
            AccountEntity = accountEntity;
            accountEntity.CharacterEntity = this;

            CharacterModel character = accountEntity.CharacterEntity.DbModel;

            accountEntity.Client.Nametag = $"({EntityManager.CalculateServerId(accountEntity)}) {accountEntity.CharacterEntity.FormatName}";

            accountEntity.Client.Name = accountEntity.CharacterEntity.FormatName;
            accountEntity.Client.SetSkin(character.Model);

            NAPI.Entity.SetEntityPosition(accountEntity.Client, new Vector3(character.LastPositionX, character.LastPositionY, character.LastPositionZ));

            accountEntity.Client.Dimension = 0;

            if (character.MinutesToRespawn > 0)
                NAPI.Player.SetPlayerHealth(accountEntity.Client, -1);
            else
                NAPI.Player.SetPlayerHealth(accountEntity.Client, character.HitPoints);


            accountEntity.CharacterEntity.CanTalk = true;
            accountEntity.CharacterEntity.CanNarrate = true;
            accountEntity.CharacterEntity.CanSendPrivateMessage = true;
            accountEntity.CharacterEntity.CanCommand = true;
            accountEntity.CharacterEntity.CanPay = true;

            accountEntity.Client.TriggerEvent("MoneyChanged", character.Money.ToString(CultureInfo.InvariantCulture));
            accountEntity.Client.TriggerEvent("ToggleHud", true);
            accountEntity.Client.Notify($"Twoja posta� ~g~~h~{accountEntity.CharacterEntity.FormatName} zosta�a pomy�lnie za�adowana �yczymy mi�ej gry!");
            CharacterLoggedIn?.Invoke(AccountEntity.Client, this);
        }

        #region DimensionManager

        public void ChangeDimension(uint dimension)
        {
            OnPlayerDimensionChanged?.Invoke(this,
                new DimensionChangeEventArgs(AccountEntity.Client, AccountEntity.Client.Dimension, dimension));
            NAPI.Entity.SetEntityDimension(AccountEntity.Client, dimension);
        }

        #endregion

        public void Dispose()
        {
            Description?.Dispose();
        }

        public void Spawn()
        {
            throw new NotImplementedException();
        }
    }
}