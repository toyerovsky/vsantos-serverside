/* Copyright (C) Przemys³aw Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemys³aw Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GTANetworkAPI;
using GTANetworkInternals;
using Serverside.Core;
using Serverside.Core.Database.Models;
using Serverside.Core.Description;
using Serverside.Core.Extensions;
using Serverside.Core.Repositories;
using Serverside.Entities.Base;
using Serverside.Entities.Core.Building;
using Serverside.Entities.Core.Item;
using Serverside.Entities.Interfaces;

namespace Serverside.Entities.Core
{
    public class CharacterEntity : GameEntity, IDbEntity<CharacterModel>
    {
        public CharacterModel DbModel { get; set; }

        public AccountEntity AccountEntity { get; private set; }
        public GroupEntity OnDutyGroup { get; set; }
        public Description Description { get; set; }
        public CharacterCreator.CharacterCreator CharacterCreator { get; set; }
        public BuildingEntity CurrentBuilding { get; set; }

        internal List<Item.Item> ItemsInUse { get; set; } = new List<Item.Item>();
        internal Cellphone CurrentCellphone => ItemsInUse.Single(x => x is Cellphone) as Cellphone;

        public string FormatName => $"{DbModel.Name} {DbModel.Surname}";

        public event DimensionChangeEventHandler OnPlayerDimensionChanged;
        public static event CharacterLoginEventHandler CharacterLoggedIn;

        public IInteractive CurrentInteractive { get; set; }

        public bool CanSendPrivateMessage { get; set; }
        public bool CanCommand { get; set; }
        public bool CanTalk { get; set; }
        public bool CanNarrate { get; set; }
        public bool CanPay { get; set; }

        public CharacterEntity(EventClass events, AccountEntity accountEntity, CharacterModel dbModel) : base(events)
        {
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

            OnPlayerDimensionChanged += OnOnPlayerDimensionChanged;
        }

        public static CharacterEntity Create(EventClass events, AccountEntity accountEntity, string name, string surname, PedHash model)
        {
            var randomIndex = Tools.RandomInt(Constant.Items.ServerSpawnPositions.Count);

            CharacterModel dbModel = new CharacterModel
            {
                AccountModel = accountEntity.DbModel,
                Name = name,
                Surname = surname,
                Money = 10000,
                BankMoney = 1000000,
                CreateTime = DateTime.Now,
                Model = model,
                HitPoints = 100,
                IsAlive = true,
                LastPositionX = Constant.Items.ServerSpawnPositions[randomIndex].X,
                LastPositionY = Constant.Items.ServerSpawnPositions[randomIndex].Y,
                LastPositionZ = Constant.Items.ServerSpawnPositions[randomIndex].Z,
                LastPositionRotX = 0f,
                LastPositionRotY = 0f,
                LastPositionRotZ = 0f
            };

            using (CharactersRepository repository = new CharactersRepository())
            {
                repository.Insert(dbModel);
                repository.Save();
            }

            return new CharacterEntity(events, accountEntity, dbModel);
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
                player.Notify("Nie uda³o siê za³adowaæ Twojego konta... Skontaktuj siê z Administratorem!");
                return;
            }

            if (account.DbModel.Characters.Count == 0)
            {
                player.Notify("Twoje konto nie posiada ¿adnych postaci!");
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
            accountEntity.Client.Notify($"Twoja postaæ ~g~~h~{accountEntity.CharacterEntity.FormatName} zosta³a pomyœlnie za³adowana ¿yczymy mi³ej gry!");
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

        private void OnOnPlayerDimensionChanged(object sender, DimensionChangeEventArgs e)
        {
            AccountEntity account = e.Player.GetAccountEntity();
            account.CharacterEntity.DbModel.CurrentDimension = e.CurrentDimension;
            account.CharacterEntity.Save();
        }

        public override void Dispose()
        {
            Description?.Dispose();
        }

        public override void Spawn()
        {
            throw new NotImplementedException();
        }
    }
}