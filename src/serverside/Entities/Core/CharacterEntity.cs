/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Repositories;
using VRP.Serverside.Constant.RemoteEvents;
using VRP.Serverside.Core.CharacterCreator;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Economy.Money;
using VRP.Serverside.Economy.Offers;
using VRP.Serverside.Entities.Base;
using VRP.Serverside.Entities.Core.Building;
using VRP.Serverside.Entities.Core.Group;
using VRP.Serverside.Entities.Core.Item;
using VRP.Serverside.Entities.Misc.Description;
using VRP.Serverside.Interfaces;

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

        internal List<ItemEntity> ItemsInUse { get; set; } = new List<ItemEntity>();
        internal Cellphone CurrentCellphone => ItemsInUse.SingleOrDefault(x => x is Cellphone) as Cellphone;

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

        public bool IsFlying { get; set; }

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

        public void SetBw(int minutes)
        {
            DbModel.MinutesToRespawn = minutes;
            if (minutes == 0)
                Health = 5;
            else
                Health = -1;
            CanTalk = minutes == 0;
        }

        public CharacterEntity(CharacterModel dbModel)
        {
            DbModel = dbModel;
        }

        public void Save()
        {
            if (AccountEntity != null)
            {
                Position = AccountEntity.Client.Position;
                Rotation = AccountEntity.Client.Rotation;
            }

            using (CharactersRepository repository = new CharactersRepository())
            {
                repository.Update(DbModel);
                repository.Save();
            }
        }

        public void LoginCharacter(AccountEntity accountEntity)
        {
            DbModel.LastLoginTime = DateTime.Now;
            DbModel.Online = true;
            Save();

            AccountEntity = accountEntity;
            AccountEntity.CharacterEntity = this;
            Spawn();
        }

        public override void Spawn()
        {
            AccountEntity.Client.Nametag =
                $"({AccountEntity.ServerId}) {AccountEntity.CharacterEntity.FormatName}";
            AccountEntity.Client.Name = AccountEntity.CharacterEntity.FormatName;

            // FixMe obsługa kreatora postaci
            AccountEntity.Client.SetSkin(NAPI.Util.PedNameToModel(DbModel.Model));

            // FixMe spawn w domu
            AccountEntity.Client.Position =
                new Vector3(DbModel.LastPositionX, DbModel.LastPositionY, DbModel.LastPositionZ);
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
            AccountEntity.Client.SendInfo(
                $"Twoja postać {FormatName} została pomyślnie załadowana życzymy miłej gry!");

            CharacterSelected?.Invoke(AccountEntity.Client, this);

            if (DbModel.Freemode)
                CharacterCreator = new CharacterCreator(this);
            Description = new Description(AccountEntity);
            AccountEntity.Client.SetSharedData("Id", DbModel.Id);
        }

        public override void Dispose()
        {
            Description?.Dispose();
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

        public void SendInfo(string message, string title = "")
        {
            AccountEntity.Client.SendInfo(message, title);
        }

        public void SendWarning(string message, string title = "")
        {
            AccountEntity.Client.SendWarning(message, title);
        }

        public void SendError(string message, string title = "")
        {
            AccountEntity.Client.SendError(message, title);
        }
    }
}