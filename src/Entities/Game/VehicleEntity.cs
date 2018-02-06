/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using GTANetworkAPI;
using GTANetworkInternals;
using Microsoft.EntityFrameworkCore;
using Serverside.Core;
using Serverside.Core.Database.Models;
using Serverside.Core.Description;
using Serverside.Core.Extensions;
using Serverside.Core.Repositories;
using Serverside.Entities.Base;
using Serverside.Entities.Core;
using Serverside.Entities.Interfaces;
using Serverside.Items;

namespace Serverside.Entities.Game
{
    public class VehicleEntity : GameEntity, IDbEntity<VehicleModel>, IGameEntity, IOfferable
    {
        public long VehicleId => DbModel.Id;
        public Vehicle GameVehicle { get; set; }
        public VehicleModel DbModel { get; set; }
        public Description Description;

        public VehicleEntity(EventClass events, VehicleModel model)
            : base(events)
        {
            DbModel = model;
            Initialize();

            NAPI.Vehicle.BreakVehicleDoor(GameVehicle, 1, DbModel.Door1Damage);
            NAPI.Vehicle.BreakVehicleDoor(GameVehicle, 2, DbModel.Door2Damage);
            NAPI.Vehicle.BreakVehicleDoor(GameVehicle, 3, DbModel.Door3Damage);
            NAPI.Vehicle.BreakVehicleDoor(GameVehicle, 4, DbModel.Door4Damage);
            NAPI.Vehicle.BreakVehicleWindow(GameVehicle, 1, DbModel.Window1Damage);
            NAPI.Vehicle.BreakVehicleWindow(GameVehicle, 2, DbModel.Window2Damage);
            NAPI.Vehicle.BreakVehicleWindow(GameVehicle, 3, DbModel.Window3Damage);
            NAPI.Vehicle.BreakVehicleWindow(GameVehicle, 4, DbModel.Window4Damage);

            //Dodajemy tuning do pojazdu
            float engineMultipier = 0f;
            float torqueMultipier = 0f;

            foreach (var tuning in DbModel.Tunings)
            {
                if (tuning.ItemType == ItemType.Tuning)
                {
                    if (tuning.FirstParameter != null && (TuningType)tuning.FirstParameter == TuningType.Speed)
                    {
                        if (tuning.SecondParameter != null)
                            engineMultipier += (float)tuning.SecondParameter;
                        if (tuning.ThirdParameter != null)
                            torqueMultipier += (float)tuning.ThirdParameter;
                    }
                }
            }

            //Te metody działają tak, że ujemny mnożnik zmniejsza | 0 to normalnie | a dodatni poprawia
            //Pola są potrzebne, ponieważ w salonie będą dostępne 3 wersje pojazdu
            //TODO: tańsza o 10% zmniejszone osiągi o -5f
            //TODO: normalna
            //TODO: droższa o 25% zwiększone osiągi o 5f
            NAPI.Vehicle.SetVehicleEnginePowerMultiplier(GameVehicle, engineMultipier);
            NAPI.Vehicle.SetVehicleEngineTorqueMultiplier(GameVehicle, torqueMultipier);

            NAPI.Data.SetEntitySharedData(GameVehicle.Handle, "_maxfuel", DbModel.Fuel);
            NAPI.Data.SetEntitySharedData(GameVehicle.Handle, "_fuel", DbModel.Fuel);
            NAPI.Data.SetEntitySharedData(GameVehicle.Handle, "_fuelConsumption", DbModel.FuelConsumption);
            NAPI.Data.SetEntitySharedData(GameVehicle.Handle, "_milage", DbModel.Milage);
            GameVehicle.SetData("VehicleEntity", this);
            EntityManager.Add(this);
            Save();
        }

        public static VehicleEntity Create(EventClass events, FullPosition spawnPosition, VehicleHash hash, string numberplate, int numberplatestyle, AccountModel creator, Color primaryColor, Color secondaryColor, float enginePowerMultiplier = 0f, float engineTorqueMultiplier = 0f, CharacterModel character = null, GroupModel groupModel = null)
        {
            VehicleModel vehicleModel = new VehicleModel
            {
                VehicleHash = hash,
                NumberPlate = numberplate,
                NumberPlateStyle = numberplatestyle,
                Character = character,
                Group = groupModel,
                Creator = creator,
                SpawnPositionX = spawnPosition.Position.X,
                SpawnPositionY = spawnPosition.Position.Y,
                SpawnPositionZ = spawnPosition.Position.Z,
                SpawnRotationX = spawnPosition.Rotation.X,
                SpawnRotationY = spawnPosition.Rotation.Y,
                SpawnRotationZ = spawnPosition.Rotation.Z,
                PrimaryColor = primaryColor.ToHex(),
                SecondaryColor = secondaryColor.ToHex(),
                EnginePowerMultiplier = enginePowerMultiplier,
                EngineTorqueMultiplier = engineTorqueMultiplier,
                Tunings = new List<ItemModel>(),
                Milage = 0.0f,
            };

            vehicleModel.FuelTank = GetFuelTankSize((VehicleClass)NAPI.Vehicle.GetVehicleClass(vehicleModel.VehicleHash));
            vehicleModel.Fuel = vehicleModel.FuelTank * 0.2f;
            vehicleModel.FuelConsumption = NAPI.Vehicle.GetVehicleMaxAcceleration(vehicleModel.VehicleHash) / 0.2f;

            bool nonDbVehicle = character == null && groupModel == null;

            if (!nonDbVehicle)
            {
                using (VehiclesRepository repository = new VehiclesRepository())
                {
                    repository.Insert(vehicleModel);
                    repository.Save();
                }
            }

            return new VehicleEntity(events, vehicleModel)
            {
                _nonDbVehicle = nonDbVehicle
            };
        }

        private void Initialize()
        {
            GameVehicle = NAPI.Vehicle.CreateVehicle(DbModel.VehicleHash,
                new Vector3(DbModel.SpawnPositionX, DbModel.SpawnPositionY, DbModel.SpawnPositionZ),
                DbModel.SpawnPositionX, DbModel.PrimaryColor.ToColor(),
                DbModel.SecondaryColor.ToColor(),
                DbModel.NumberPlate, 255, true);
            GameVehicle.Rotation = new Vector3(DbModel.SpawnRotationX,
                DbModel.SpawnRotationY,
                DbModel.SpawnRotationZ);
            GameVehicle.NumberPlateStyle = DbModel.NumberPlateStyle;
            GameVehicle.EnginePowerMultiplier = DbModel.EnginePowerMultiplier;
            GameVehicle.EngineTorqueMultiplier = DbModel.EngineTorqueMultiplier;
            GameVehicle.WheelType = DbModel.WheelType;
            GameVehicle.WheelColor = DbModel.WheelColor;
            GameVehicle.EngineStatus = false;
            DbModel.IsSpawned = true;
        }

        //Pojazdy z prac nie są trzymane w bazie danych
        private bool _nonDbVehicle;

        public void Save()
        {
            DbModel.Health = GameVehicle.Health;
            DbModel.Door1Damage = GameVehicle.IsDoorBroken(1);
            DbModel.Door2Damage = GameVehicle.IsDoorBroken(2);
            DbModel.Door3Damage = GameVehicle.IsDoorBroken(3);
            DbModel.Door4Damage = GameVehicle.IsDoorBroken(4);
            DbModel.Window1Damage = GameVehicle.IsWindowBroken(1);
            DbModel.Window2Damage = GameVehicle.IsWindowBroken(2);
            DbModel.Window3Damage = GameVehicle.IsWindowBroken(3);
            DbModel.Window4Damage = GameVehicle.IsWindowBroken(4);
            DbModel.PrimaryColor = GameVehicle.CustomPrimaryColor.ToHex();
            DbModel.SecondaryColor = GameVehicle.CustomSecondaryColor.ToHex();
            DbModel.EnginePowerMultiplier = GameVehicle.EnginePowerMultiplier;
            DbModel.NumberPlateStyle = GameVehicle.NumberPlateStyle;
            DbModel.NumberPlate = GameVehicle.NumberPlate;
            DbModel.Fuel = NAPI.Data.GetEntitySharedData(GameVehicle.Handle, "_fuel");
            DbModel.FuelConsumption = NAPI.Data.GetEntitySharedData(GameVehicle.Handle, "_fuelConsumption");
            DbModel.Milage = NAPI.Data.GetEntitySharedData(GameVehicle.Handle, "_milage");

            if (_nonDbVehicle) return;
            using (VehiclesRepository repository = new VehiclesRepository())
            {
                repository.Update(DbModel);
                repository.Save();
            }
        }

        public void ChangeFuelConsumption(float fuelConsumption)
        {
            NAPI.Data.SetEntitySharedData(GameVehicle.Handle, "_fuelConsumption", fuelConsumption);
            Save();
        }

        public void ChangeSpawnPosition()
        {
            DbModel.SpawnPositionX = GameVehicle.Position.X;
            DbModel.SpawnPositionY = GameVehicle.Position.Y;
            DbModel.SpawnPositionZ = GameVehicle.Position.Z;
            DbModel.SpawnRotationX = GameVehicle.Rotation.X;
            DbModel.SpawnRotationY = GameVehicle.Rotation.Y;
            DbModel.SpawnRotationZ = GameVehicle.Rotation.Z;
            if (_nonDbVehicle) return;
            Save();
        }

        public void ChangeColor(Color primary, Color secondary)
        {
            GameVehicle.CustomPrimaryColor = primary;
            GameVehicle.CustomSecondaryColor = secondary;
            if (_nonDbVehicle) return;
            DbModel.PrimaryColor = primary.ToHex();
            DbModel.SecondaryColor = secondary.ToHex();
            Save();
        }

        public void Repair()
        {
            GameVehicle.Repair();
            if (_nonDbVehicle) return;
            DbModel.Door1Damage = false;
            DbModel.Door2Damage = false;
            DbModel.Door3Damage = false;
            DbModel.Door4Damage = false;
            DbModel.Window1Damage = false;
            DbModel.Window2Damage = false;
            DbModel.Window3Damage = false;
            DbModel.Window4Damage = false;
            DbModel.Health = 1000f;
            Save();
        }

        protected static float GetFuelTankSize(VehicleClass vehicleClass)
        {
            switch (vehicleClass)
            {
                case VehicleClass.Compact:
                    return 70.0f;
                case VehicleClass.Sedans:
                    return 80.0f;
                case VehicleClass.SuVs:
                    return 85.0f;
                case VehicleClass.Coupe:
                    return 60.0f;
                case VehicleClass.Muscle:
                    return 70.0f;
                case VehicleClass.SportClassics:
                    return 70.0f;
                case VehicleClass.Sports:
                    return 60.0f;
                case VehicleClass.Super:
                    return 60.0f;
                case VehicleClass.Motorcycles:
                    return 40.0f;
                case VehicleClass.Offroad:
                    return 80.0f;
                case VehicleClass.Industrial:
                    return 200.0f;
                case VehicleClass.Utility:
                    return 100.0f;
                case VehicleClass.Vans:
                    return 80.0f;
                case VehicleClass.Cycle:
                    return 0.0f;
                case VehicleClass.Boat:
                    return 200.0f;
                case VehicleClass.Heli:
                    return 400.0f;
                case VehicleClass.Planes:
                    return 0.0f;
                case VehicleClass.Service:
                    return 250.0f;
                case VehicleClass.Emergency:
                    return 90.0f;
                case VehicleClass.Military:
                    return 250.0f;
                case VehicleClass.Commercial:
                    return 250.0f;
                case VehicleClass.Train:
                    return 0.0f;
                case VehicleClass.Trailer:
                    return 0.0f;
                default:
                    return 70.0f;
            }
        }

        private void ReleaseUnmanagedResources()
        {
            DbModel.IsSpawned = false;
            if (!_nonDbVehicle) Save();
            NAPI.Entity.DeleteEntity(GameVehicle);
            EntityManager.Remove(this);
        }

        private void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                Description?.Dispose();
            }
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override void Spawn()
        {
            throw new NotImplementedException();
        }

        ~VehicleEntity()
        {
            Dispose(false);
        }

        public void Offer(CharacterEntity seller, CharacterEntity getter, decimal money)
        {
            throw new NotImplementedException();
        }
    }
}