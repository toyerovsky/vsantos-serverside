/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using GTANetworkInternals;
using Serverside.Core;
using Serverside.Entities.Core;
using Serverside.Entities.Game;

namespace Serverside.Entities
{
    public static class EntityManager
    {
        private static readonly Dictionary<long, AccountEntity> Accounts = new Dictionary<long, AccountEntity>();
        private static readonly List<VehicleEntity> Vehicles = new List<VehicleEntity>();
        private static readonly List<GroupEntity> Groups = new List<GroupEntity>();
        private static readonly List<BuildingEntity> Buildings = new List<BuildingEntity>();

        public static void LoadEntities(EventClass events)
        {
            GroupEntity.LoadGroups();
            BuildingEntity.LoadBuildings(events);
        }

        #region ACCOUNT METHODS

        public static void Add(AccountEntity accountEntity)
        {
            if (Accounts.ContainsKey(accountEntity.AccountId))
            {
                Tools.ConsoleOutput("[Error] Nastąpiła interferencja zalogowanych użytkowników.", ConsoleColor.Red);
                return;
            }   

            Accounts.Add(accountEntity.AccountId, accountEntity);
        }

        public static void Remove(AccountEntity accountEntity) => Accounts.Remove(accountEntity.AccountId);

        public static AccountEntity Get(long accountId) => accountId > -1 ? Accounts[accountId] : null;

        public static AccountEntity GetAccountByServerId(int id) => id > -1 ? Accounts.Values.ElementAtOrDefault(id) : null;

        public static AccountEntity GetAccountByCharacterId(long characterId)
        {
            return characterId > -1 ? Accounts.Values.Single(ch => ch.CharacterEntity.DbModel.Id == characterId) : null;
        }

        public static Dictionary<long, AccountEntity> GetAccounts() => Accounts;

        public static int CalculateServerId(AccountEntity account)
        {
            if (!Accounts.ContainsValue(account))
            {
                Tools.ConsoleOutput("[Error] Próbowano uzyskać ID dla gracza który nie jest zalogowany.", ConsoleColor.Red);
                return -1;
            }
            return Accounts.Values.ToList().IndexOf(account);
        }

        #endregion

        #region VEHICLE METHODS
        public static void Add(VehicleEntity vehicle) => Vehicles.Add(vehicle);

        public static void Remove(VehicleEntity vehicle) => Vehicles.Remove(vehicle);

        public static VehicleEntity GetVehicle(Vehicle vehicle) => Vehicles.Find(x => x.GameVehicle == vehicle);

        public static VehicleEntity GetVehicle(NetHandle vehicle) => Vehicles.Find(x => x.GameVehicle.Handle == vehicle);

        public static VehicleEntity GetVehicle(long id)
        {
            return id > -1 ? Vehicles.Find(x => x.DbModel.Id == id) : null;
        }

        public static List<VehicleEntity> GetVehicles() => Vehicles;
        #endregion

        #region GROUP METHODS
        public static void Add(GroupEntity group) => Groups.Add(group);

        public static void Remove(GroupEntity group) => Groups.Remove(group);

        public static GroupEntity GetGroup(long groupId)
        {
            return groupId > -1 ? Groups.Find(x => x.Id == groupId) : null;
        }

        public static List<GroupEntity> GetPlayerGroups(AccountEntity accountEntity)
        {
            if (Groups.Any(g => g.DbModel.Workers.Any(x => x.Character.Id == accountEntity.CharacterEntity.DbModel.Id)))
            {
                return Groups.Where(
                    g => g.DbModel.Workers.Any(x => x.Character?.Id == accountEntity.CharacterEntity.DbModel.Id))
                .ToList();
            }
            return null;
        }

        public static List<GroupEntity> GetGroups() => Groups;

        #endregion

        #region BUILDING METHODS

        public static void Add(BuildingEntity building) => Buildings.Add(building);

        public static void Remove(BuildingEntity building) => Buildings.Remove(building);

        public static BuildingEntity GetBuilding(long buildingId)
        {
            return buildingId > -1 ? Buildings.Find(x => x.DbModel.Id == buildingId) : null;
        }

        public static BuildingEntity GetBuilding(string buildingName)
        {
            return Buildings.Single(x => x.DbModel.Name.StartsWith(buildingName.ToLower()));
        }

        public static List<BuildingEntity> GetPlayerBuildings(AccountEntity accountEntity)
        {
            return Buildings.Where(b => b.DbModel.Character.Id == accountEntity.CharacterEntity.DbModel.Id).ToList();
        }

        public static List<BuildingEntity> GetBuildings() => Buildings;

        #endregion
    }
}
