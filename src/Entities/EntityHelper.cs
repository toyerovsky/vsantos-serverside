/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using System.IO;
using System.Linq;
using GTANetworkAPI;
using Serverside.Core.Serialization;
using Serverside.Entities.Common.Atm;
using Serverside.Entities.Common.Atm.Models;
using Serverside.Entities.Common.Booth;
using Serverside.Entities.Common.Booth.Models;
using Serverside.Entities.Common.BusStop;
using Serverside.Entities.Common.BusStop.Models;
using Serverside.Entities.Common.Carshop;
using Serverside.Entities.Common.Carshop.Models;
using Serverside.Entities.Common.Corners;
using Serverside.Entities.Common.Corners.Models;
using Serverside.Entities.Common.DriveThru;
using Serverside.Entities.Common.DriveThru.Models;
using Serverside.Entities.Common.Market;
using Serverside.Entities.Common.Market.Models;
using Serverside.Entities.Core;
using Serverside.Entities.Core.Building;
using Serverside.Entities.Core.Vehicle;
using Serverside.Entities.Peds.CrimeBot;
using Serverside.Entities.Peds.Employer;
using Color = System.Drawing.Color;

namespace Serverside.Entities
{
    public static class EntityHelper
    {
        static EntityHelper()
        {
            LoadCommonEntities();
            LoadCoreEntities();
            LoadPeds();
            GroupEntity.LoadGroups();
            BuildingEntity.LoadBuildings();
        }

        #region Core        
        /// <summary>
        /// Klucz to id konta
        /// </summary>
        private static readonly Dictionary<long, AccountEntity> Accounts = new Dictionary<long, AccountEntity>();
        private static readonly List<BuildingEntity> Buildings = new List<BuildingEntity>();
        private static readonly List<GroupEntity> Groups = new List<GroupEntity>();
        private static readonly List<VehicleEntity> Vehicles = new List<VehicleEntity>();
        #endregion

        #region Common
        private static readonly List<AtmEntity> Atms = new List<AtmEntity>();
        private static readonly List<BusStopEntity> BusStops = new List<BusStopEntity>();
        private static readonly List<CarshopEntity> Carshops = new List<CarshopEntity>();
        private static readonly List<DriveThruEntity> DriveThrus = new List<DriveThruEntity>();
        private static readonly List<MarketEntity> Markets = new List<MarketEntity>();
        private static readonly List<TelephoneBoothEntity> TelephoneBooths = new List<TelephoneBoothEntity>();
        private static readonly List<CornerEntity> Corners = new List<CornerEntity>();
        #endregion

        #region Peds
        private static readonly List<CrimePedEntity> CrimePeds = new List<CrimePedEntity>();
        private static readonly List<EmployerPedEntity> Employers = new List<EmployerPedEntity>();
        #endregion

        #region Atm

        public static void Add(AtmEntity atmEntity) => Atms.Add(atmEntity);

        public static AtmEntity GetAtm(int id) => Atms.Single(atm => atm.Data.Id == id);
        public static AtmEntity GetAtm(Vector3 position, float precision = float.MinValue)
        {
            return precision.Equals(float.MinValue)
                ? Atms.Single(atm => atm.ColShape.Position == position)
                : Atms.Single(atm => atm.ColShape.Position.DistanceTo(position) < precision);
        }
        public static AtmEntity GetAtm(string filePath) => Atms.Single(atm => atm.Data.FilePath == filePath);

        public static void Remove(AtmEntity atmEntity) => Atms.Remove(atmEntity);

        public static IEnumerable<AtmEntity> GetAtms() => Atms;

        #endregion

        #region BusStop

        public static void Add(BusStopEntity busStopEntity) => BusStops.Add(busStopEntity);

        public static BusStopEntity GetBusStop(int id) => BusStops.Single(busStop => busStop.Data.Id == id);

        public static BusStopEntity GetBusStop(Vector3 position, float precision = float.MinValue)
        {
            return precision.Equals(float.MinValue)
                ? BusStops.Single(busStop => busStop.ColShape.Position == position)
                : BusStops.Single(busStop => busStop.ColShape.Position.DistanceTo(position) < precision);
        }

        public static BusStopEntity GetBusStop(string name) => BusStops.Single(busStop => busStop.Data.Name == name);

        public static void Remove(BusStopEntity busStopEntity) => BusStops.Remove(busStopEntity);

        public static IEnumerable<BusStopEntity> GetBusStops() => BusStops;

        #endregion

        #region Carshop

        public static void Add(CarshopEntity carshopEntity) => Carshops.Add(carshopEntity);

        public static CarshopEntity GetCarshop(int id) => Carshops.Single(carshop => carshop.Data.Id == id);

        public static CarshopEntity GetCarshop(Vector3 position, float precision = float.MinValue)
        {
            return precision.Equals(float.MinValue)
                ? Carshops.Single(carshop => carshop.CarshopMarker.Position == position)
                : Carshops.Single(carshop => carshop.ColShape.Position.DistanceTo(position) < precision);
        }

        public static CarshopEntity GetCarshop(string filePath) => Carshops.Single(carshop => carshop.Data.FilePath == filePath);

        public static void Remove(CarshopEntity carshopEntity) => Carshops.Remove(carshopEntity);

        public static IEnumerable<CarshopEntity> GetCarshops() => Carshops;

        #endregion

        #region DriveThru

        public static void Add(DriveThruEntity driveThruEntity) => DriveThrus.Add(driveThruEntity);

        public static DriveThruEntity GetDriveThru(int id) => DriveThrus.Single(driveThru => driveThru.Data.Id == id);

        public static DriveThruEntity GetDriveThru(Vector3 position, float precision = 0f)
        {
            return precision.Equals(float.MinValue)
                ? DriveThrus.Single(driveThru => driveThru.ColShape.Position == position)
                : DriveThrus.First(driveThru => driveThru.ColShape.Position.DistanceTo(position) < precision);
        }

        public static DriveThruEntity GetDriveThru(string filePath) =>
            DriveThrus.Single(driveThru => driveThru.Data.FilePath == filePath);

        public static void Remove(DriveThruEntity driveThruEntity) => DriveThrus.Remove(driveThruEntity);

        public static IEnumerable<DriveThruEntity> GetDriveThrus() => DriveThrus;

        #endregion

        #region Market

        public static void Add(MarketEntity martketEntity) => Markets.Add(martketEntity);

        public static MarketEntity GetMarket(int id) => Markets.Single(market => market.Data.Id == id);

        public static MarketEntity GetMarket(Vector3 position, float precision = float.MinValue)
        {
            return precision.Equals(float.MinValue)
                ? Markets.Single(market => market.ColShape.Position == position)
                : Markets.First(market => market.ColShape.Position.DistanceTo(position) < precision);
        }

        public static MarketEntity GetMarket(string filePath) => Markets.Single(market => market.Data.FilePath == filePath);

        public static void Remove(MarketEntity marketEntity) => Markets.Remove(marketEntity);

        public static IEnumerable<MarketEntity> GetMarkets() => Markets;

        #endregion

        #region TelephoneBooth

        public static void Add(TelephoneBoothEntity telephoneBoothEntity) => TelephoneBooths.Add(telephoneBoothEntity);

        public static TelephoneBoothEntity GetTelephoneBooth(int id) => TelephoneBooths.Single(telephoneBooth => telephoneBooth.Data.Id == id);

        public static TelephoneBoothEntity GetTelephoneBoothByNumber(int number) => TelephoneBooths.Single(telephoneBooth => telephoneBooth.Data.Number == number);

        public static TelephoneBoothEntity GetTelephoneBooth(Vector3 position, float precision = float.MinValue)
        {
            return precision.Equals(float.MinValue)
                ? TelephoneBooths.Single(telephoneBooth => telephoneBooth.ColShape.Position == position)
                : TelephoneBooths.First(telephoneBooth => telephoneBooth.ColShape.Position.DistanceTo(position) < precision);
        }

        public static TelephoneBoothEntity GetTelephoneBooth(string filePath) => TelephoneBooths.Single(telephoneBooth => telephoneBooth.Data.FilePath == filePath);

        public static void Remove(TelephoneBoothEntity telephoneBooth) => TelephoneBooths.Remove(telephoneBooth);

        public static IEnumerable<TelephoneBoothEntity> GetTelephoneBooths() => TelephoneBooths;


        #endregion

        #region Corners

        public static void Add(CornerEntity cornerEntity) => Corners.Add(cornerEntity);

        public static CornerEntity GetCorner(int id) => Corners.Single(corner => corner.Data.Id == id);

        public static CornerEntity GetCorner(Vector3 position, float precision = float.MinValue)
        {
            return precision.Equals(float.MinValue)
                ? Corners.Single(corner => corner.ColShape.Position == position)
                : Corners.First(corner => corner.ColShape.Position.DistanceTo(position) < precision);
        }

        public static CornerEntity GetCorner(string filePath) => Corners.Single(corner => corner.Data.FilePath == filePath);

        public static void Remove(CornerEntity corner) => Corners.Remove(corner);

        public static IEnumerable<CornerEntity> GetCorners() => Corners;


        #endregion

        #region Account

        public static void Add(AccountEntity accountEntity)
        {
            if (Accounts.ContainsKey(accountEntity.AccountId))
            {
                Colorful.Console.WriteLine($"[Error][{nameof(EntityHelper)}] Nastąpiła interferencja zalogowanych użytkowników.", Color.DarkRed);
                return;
            }

            Accounts.Add(accountEntity.AccountId, accountEntity);
        }

        public static void Remove(AccountEntity accountEntity) => Accounts.Remove(accountEntity.AccountId);

        public static AccountEntity Get(long accountId)
        {
            return Accounts.TryGetValue(accountId, out AccountEntity value) ? value : null;
        }

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
                Colorful.Console.Write($"[Error][{nameof(EntityHelper)}] Próbowano uzyskać ID dla gracza który nie jest zalogowany.", Color.DarkRed);
                return -1;
            }
            return Accounts.Values.ToList().IndexOf(account);
        }

        #endregion

        #region Character

        public static CharacterEntity GetCharacterByOnlineCellphoneNumber(int number)
        {
            return GetAccounts().Values
                .Single(account => account.CharacterEntity?.CurrentCellphone.Number == number).CharacterEntity;
        }

        #endregion

        #region Vehicle
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

        #region Group
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

        #region Building

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

        #region Loader

        private static void LoadCoreEntities()
        {

        }

        private static void LoadCommonEntities()
        {
            foreach (AtmModel atmModel in XmlHelper.GetXmlObjects<AtmModel>(Path.Combine(Constant.ServerInfo.XmlDirectory,
                nameof(AtmModel))))
            {
                AtmEntity atmEntity = new AtmEntity(atmModel);
                atmEntity.Spawn();
                Add(atmEntity);
            }

            foreach (TelephoneBoothModel telephoneBoothModel in XmlHelper.GetXmlObjects<TelephoneBoothModel>(Path.Combine(Constant.ServerInfo.XmlDirectory,
                nameof(TelephoneBoothModel))))
            {
                TelephoneBoothEntity telephoneBoothEntity = new TelephoneBoothEntity(telephoneBoothModel);
                telephoneBoothEntity.Spawn();
                Add(telephoneBoothEntity);
            }

            foreach (BusStopModel atmModel in XmlHelper.GetXmlObjects<BusStopModel>(Path.Combine(Constant.ServerInfo.XmlDirectory,
                nameof(BusStopModel))))
            {
                BusStopEntity busStopEntity = new BusStopEntity(atmModel);
                busStopEntity.Spawn();
                Add(busStopEntity);
            }

            foreach (CarshopModel carshopModel in XmlHelper.GetXmlObjects<CarshopModel>(Path.Combine(Constant.ServerInfo.XmlDirectory,
                nameof(CarshopModel))))
            {
                CarshopEntity carshopEntity = new CarshopEntity(carshopModel);
                carshopEntity.Spawn();
                Add(carshopEntity);
            }

            foreach (CornerModel cornerModel in XmlHelper.GetXmlObjects<CornerModel>(Path.Combine(Constant.ServerInfo.XmlDirectory,
                nameof(CornerModel))))
            {
                CornerEntity cornerEntity = new CornerEntity(cornerModel);
                cornerEntity.Spawn();
                Add(cornerEntity);
            }

            foreach (DriveThruModel driveThruModel in XmlHelper.GetXmlObjects<DriveThruModel>(Path.Combine(Constant.ServerInfo.XmlDirectory,
                nameof(DriveThruModel))))
            {
                DriveThruEntity driveThruEntity = new DriveThruEntity(driveThruModel);
                driveThruEntity.Spawn();
                Add(driveThruEntity);
            }

            foreach (MarketModel marketModel in XmlHelper.GetXmlObjects<MarketModel>(Path.Combine(Constant.ServerInfo.XmlDirectory,
                nameof(MarketModel))))
            {
                MarketEntity marketEntity = new MarketEntity(marketModel);
                marketEntity.Spawn();
                Add(marketEntity);
            }

        }

        private static void LoadPeds()
        {

        }

        #endregion
    }
}
