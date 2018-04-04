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
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Core.Repositories;
using VRP.Core.Tools;
using VRP.Serverside.Core.CharacterCreator;

using VRP.Serverside.Core.Extensions;
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
        public static event CharacterLoginEventHandler CharacterLoggedIn;

        public IInteractive CurrentInteractive { get; set; }

        public bool CanSendPrivateMessage { get; set; }
        public bool CanCommand { get; set; }
        public bool CanTalk { get; set; }
        public bool CanNarrate { get; set; }
        public bool CanPay { get; set; }

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

            OnPlayerDimensionChanged += OnOnPlayerDimensionChanged;
        }

        public void Save()
        {
            if (AccountEntity != null)
            {
                DbModel.CurrentDimension = (int)AccountEntity.Client.Dimension;
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

        public void LoginCharacter(AccountEntity accountEntity)
        {
            AccountEntity = accountEntity;
            accountEntity.CharacterEntity = this;
            Spawn();
        }

        #region DimensionManager

        public void ChangeDimension(uint dimension)
        {
            OnPlayerDimensionChanged?.Invoke(this,
                new DimensionChangeEventArgs(AccountEntity.Client, AccountEntity.Client.Dimension, dimension));
            NAPI.Entity.SetEntityDimension(AccountEntity.Client, dimension);
        }

        #endregion

        public override void Spawn()
        {
            AccountEntity.Client.Nametag = $"({AccountEntity.ServerId}) {AccountEntity.CharacterEntity.FormatName}";
            AccountEntity.Client.Name = AccountEntity.CharacterEntity.FormatName;

            // FixMe obs³uga kreatora postaci
            AccountEntity.Client.SetSkin(NAPI.Util.PedNameToModel(DbModel.Model));

            // FixMe spawn w domu
            AccountEntity.Client.Position = new Vector3(DbModel.LastPositionX, DbModel.LastPositionY, DbModel.LastPositionZ);
            AccountEntity.Client.Dimension = 0;

            if (DbModel.MinutesToRespawn > 0)
                NAPI.Player.SetPlayerHealth(AccountEntity.Client, -1);
            else
                NAPI.Player.SetPlayerHealth(AccountEntity.Client, DbModel.HitPoints);

            CanTalk = true;
            CanNarrate = true;
            CanSendPrivateMessage = true;
            CanCommand = true;
            CanPay = true;

            AccountEntity.Client.TriggerEvent(Constant.RemoteEvents.RemoteEvents.CharacterMoneyChangeRequested, DbModel.Money.ToString(CultureInfo.InvariantCulture));
            AccountEntity.Client.Notify(
                $"Twoja postaæ {FormatName} zosta³a pomyœlnie za³adowana ¿yczymy mi³ej gry!", NotificationType.Info);

            CharacterLoggedIn?.Invoke(AccountEntity.Client, this);
        }

        public override void Dispose()
        {
            Description?.Dispose();
        }

        private void OnOnPlayerDimensionChanged(object sender, DimensionChangeEventArgs e)
        {
            AccountEntity account = e.Player.GetAccountEntity();
            account.CharacterEntity.DbModel.CurrentDimension = (int)e.CurrentDimension;
            account.CharacterEntity.Save();
        }
    }
}