/* Copyright (C) Przemys�aw Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemys�aw Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Timers;
using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Core.Repositories;
using VRP.Core.Tools;
using VRP.Serverside.Core.CharacterCreator;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Economy.Money;
using VRP.Serverside.Economy.Offers;
using VRP.Serverside.Entities.Base;
using VRP.Serverside.Entities.Core.Building;
using VRP.Serverside.Entities.Core.Group;
using VRP.Serverside.Entities.Core.Item;
using VRP.Serverside.Entities.Interfaces;
using VRP.Serverside.Entities.Misc.Description;

namespace VRP.Serverside.Entities.Core
{
    public class CharacterEntity : GameEntity, IDbEntity<CharacterModel>
    {
        public CharacterModel DbModel { get; set; }

        public AccountEntity AccountEntity { get; private set; }
        public GroupEntity OnDutyGroup { get; set; }
        public Description Description { get; set; }
        public CharacterCreator CharacterCreator { get; set; }
        public BuildingEntity CurrentBuilding { get; set; }

        internal List<ItemEntity> ItemsInUse { get; set; } = new List<Item.ItemEntity>();
        internal Cellphone CurrentCellphone => ItemsInUse.Single(x => x is Cellphone) as Cellphone;

        public string FormatName => $"{DbModel.Name} {DbModel.Surname}";

        public event DimensionChangeEventHandler OnPlayerDimensionChanged;
        public static event CharacterSelectEventHandler CharacterSelected;

        public IInteractive CurrentInteractive { get; set; }
        public Offer PendingOffer { get; set; }

        public bool CanSendPrivateMessage { get; set; }
        public bool CanCommand { get; set; }
        public bool CanTalk { get; set; }
        public bool CanNarrate { get; set; }
        public bool CanPay { get; set; }

        private Timer PositionSyncTimer { get; }

        public int Health
        {
            get => DbModel.Health;
            set
            {
                AccountEntity.Client.Health = value;
                DbModel.Health = value;
                Save();
            }
        }

        public Vector3 Position
        {
            get => new Vector3(DbModel.LastPositionX, DbModel.LastPositionY, DbModel.LastPositionZ);
            set
            {
                AccountEntity.Client.Position = value;
                DbModel.LastPositionX = value.X;
                DbModel.LastPositionY = value.Y;
                DbModel.LastPositionZ = value.Z;
                Save();
            }
        }

        public Vector3 Rotation
        {
            get => new Vector3(DbModel.LastPositionRotX, DbModel.LastPositionRotY, DbModel.LastPositionRotZ);
            set
            {
                AccountEntity.Client.Rotation = value;
                DbModel.LastPositionRotX = value.X;
                DbModel.LastPositionRotY = value.Y;
                DbModel.LastPositionRotZ = value.Z;
                Save();
            }
        }

        public uint Dimension
        {
            get => DbModel.CurrentDimension;
            set
            {
                NAPI.Entity.SetEntityDimension(AccountEntity.Client, value);

                OnPlayerDimensionChanged?.Invoke(this,
                    new DimensionChangeEventArgs(this, AccountEntity.Client.Dimension, value));
                DbModel.CurrentDimension = value;
                Save();
            }
        }

        public CharacterEntity(AccountEntity accountEntity, CharacterModel dbModel)
        {
            DbModel = dbModel;
            AccountEntity = accountEntity;
            AccountEntity.CharacterEntity = this;
            DbModel.Account = accountEntity.DbModel;
            DbModel.LastLoginTime = DateTime.Now;
            DbModel.Online = true;

            using (CharactersRepository repository = new CharactersRepository())
            {
                repository.Update(dbModel);
                repository.Save();
            }

            if (DbModel.Freemode)
                CharacterCreator = new CharacterCreator(this);
            Description = new Description(AccountEntity);
            PositionSyncTimer = new Timer(10000);
        }

        public void Save()
        {
            using (CharactersRepository repository = new CharactersRepository())
            {
                repository.Update(DbModel);
                repository.Save();
            }
        }

        public void LoginCharacter(AccountEntity accountEntity)
        {
            AccountEntity = accountEntity;
            accountEntity.CharacterEntity = this;
            Spawn();
        }

        public override void Spawn()
        {
            AccountEntity.Client.Nametag = $"({AccountEntity.ServerId}) {AccountEntity.CharacterEntity.FormatName}";
            AccountEntity.Client.Name = AccountEntity.CharacterEntity.FormatName;

            // FixMe obs�uga kreatora postaci
            AccountEntity.Client.SetSkin(NAPI.Util.PedNameToModel(DbModel.Model));

            // FixMe spawn w domu
            AccountEntity.Client.Position = new Vector3(DbModel.LastPositionX, DbModel.LastPositionY, DbModel.LastPositionZ);
            AccountEntity.Client.Dimension = 0;

            if (DbModel.MinutesToRespawn > 0)
                NAPI.Player.SetPlayerHealth(AccountEntity.Client, -1);
            else
                NAPI.Player.SetPlayerHealth(AccountEntity.Client, DbModel.Health);

            CanTalk = true;
            CanNarrate = true;
            CanSendPrivateMessage = true;
            CanCommand = true;
            CanPay = true;

            AccountEntity.Client.TriggerEvent(Constant.RemoteEvents.RemoteEvents.CharacterMoneyChangeRequested, DbModel.Money.ToString(CultureInfo.InvariantCulture));
            AccountEntity.Client.Notify(
                $"Twoja posta� {FormatName} zosta�a pomy�lnie za�adowana �yczymy mi�ej gry!", NotificationType.Info);

            CharacterSelected?.Invoke(AccountEntity.Client, this);

            // every 10 seconds synchronize player position and rotation
            PositionSyncTimer.Elapsed += (o, e) =>
            {
                DbModel.LastPositionX = AccountEntity.Client.Position.X;
                DbModel.LastPositionY = AccountEntity.Client.Position.Y;
                DbModel.LastPositionZ = AccountEntity.Client.Position.Z;
                DbModel.LastPositionRotX = AccountEntity.Client.Rotation.X;
                DbModel.LastPositionRotY = AccountEntity.Client.Rotation.Y;
                DbModel.LastPositionRotZ = AccountEntity.Client.Rotation.Z;
                Save();
            };
            PositionSyncTimer.Start();
        }

        public override void Dispose()
        {
            Description?.Dispose();
            PositionSyncTimer.Dispose();
        }

        public bool HasMoney(decimal count, bool bank = false)
        {
            return MoneyManager.HasMoney(this, count, bank);
        }

        public void AddMoney(decimal count, bool bank = false)
        {
            MoneyManager.AddMoney(this, count, bank);
        }

        public void RemoveMoney(decimal count, bool bank = false)
        {
            MoneyManager.RemoveMoney(this, count, bank);
        }

        public void Notify(string message, NotificationType notificationType)
        {
            AccountEntity.Client.Notify(message, notificationType);
        }

        public void SetSharedData(string key, object value)
        {
            AccountEntity.Client.SetSharedData(key, value);
        }

        public void ResetSharedData(string key)
        {
            AccountEntity.Client.ResetSharedData(key);
        }

        public bool HasSharedData(string key)
        {
            return AccountEntity.Client.HasSharedData(key);
        }
    }
}