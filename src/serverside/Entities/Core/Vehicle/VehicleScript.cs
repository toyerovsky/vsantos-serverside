/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using VRP.Core.Database;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Core.Scripts;
using ChatMessageType = VRP.Core.Enums.ChatMessageType;
using VRP.Serverside.Constant.RemoteEvents;
using VRP.Serverside.Core;

namespace VRP.Serverside.Entities.Core.Vehicle
{
    public class VehicleScript : Script
    {
        [RemoteEvent(RemoteEvents.OnPlayerSelectedVehicle)]
        public void OnPlayerSelectedVehicleHandler(Client sender, params object[] args)
        {
            sender.SetData("SelectedVehicleID", Convert.ToInt64(args[0]));
        }

        [RemoteEvent(RemoteEvents.OnPlayerSpawnVehicle)]
        public void OnPlayerSpawnVehicleHandler(Client sender, params object[] args)
        {
            VehicleEntity vehicleEntity = EntityHelper.GetVehicle(
                (long)sender.GetData("SelectedVehicleID"));

            if (vehicleEntity != null)
            {
                // unspawn
                vehicleEntity.Dispose();
                sender.SendInfo($"Pojazd {vehicleEntity.DbModel.Name} został odspawnowany");
            }
            else
            {
                // spawn
                VehicleModel vehicleModel = sender.GetAccountEntity().CharacterEntity.DbModel.Vehicles
                    .Single(v => v.Id == (int)sender.GetData("SelectedVehicleID"));

                vehicleEntity = new VehicleEntity(vehicleModel);

                sender.TriggerEvent("DrawVehicleComponents", vehicleEntity.GameVehicle.Position,
                    GetVehicleBlip((VehicleClass)vehicleEntity.GameVehicle.Class), 24);

                sender.SendInfo($"Pojazd {vehicleModel.Name} został zespawnowany.");
            }
            sender.ResetData("SelectedVehicleID");
        }


        [RemoteEvent(RemoteEvents.OnPlayerParkVehicle)]
        public void OnPlayerParkVehicleHandler(Client sender, params object[] args)
        {
            if (EntityHelper.GetVehicle(sender.Vehicle) == null)
                return;

            VehicleEntity vehicleEntity = EntityHelper.GetVehicle(sender.Vehicle);
            vehicleEntity.ChangeSpawnPosition();
            sender.SendInfo($"Pojazd {vehicleEntity.DbModel.Name} został zaparkowany.");
        }

        [RemoteEvent(RemoteEvents.OnPlayerInformationsVehicle)]
        public void OnPlayerInformationsVehicleHandler(Client sender, params object[] args)
        {
            AccountEntity player = sender.GetAccountEntity();

            if (player.CharacterEntity.DbModel.Vehicles.Any(v => v.Id == sender.GetData("SelectedVehicleID")))
                ShowVehiclesInformation(player.CharacterEntity, player.CharacterEntity.DbModel.Vehicles.Single(
                    v => v.Id == sender.GetData("SelectedVehicleID")));
        }

        [RemoteEvent(RemoteEvents.OnPlayerInformationsInVehicle)]
        public void OnPlayerInformationsInVehicleHandler(Client sender, params object[] args)
        {
            VehicleEntity vehicle = EntityHelper.GetVehicle(sender.Vehicle);
            if (vehicle == null) return;

            float enginePower = (float)((vehicle.DbModel.EnginePowerMultiplier - 1.0) * 20.0 + 80);

            sender.SendInfo($"Nazwa pojazdu: {vehicle.DbModel.Name}" +
                            $"\nRejestracja pojazdu: {vehicle.DbModel.NumberPlate}" +
                            $"\nMoc silnika: {enginePower}KM");
        }

        [RemoteEvent(RemoteEvents.OnPlayerChangeLockVehicle)]
        public void OnPlayerChangeLockVehicleHandler(Client sender, params object[] args)
        {
            ChangePlayerVehicleLockState(sender.GetAccountEntity().CharacterEntity);
        }

        [RemoteEvent(RemoteEvents.OnPlayerEngineStateChangeVehicle)]
        public void OnPlayerEngineStateChangeVehicleHandler(Client sender, params object[] args)
        {
            sender.SendInfo(sender.Vehicle.EngineStatus ?
                "Pojazd został zgaszony." : "Pojazd został uruchomiony.");
            sender.Vehicle.EngineStatus = !sender.Vehicle.EngineStatus;
        }

  

        [Command("vspawn", "~y~UŻYJ ~w~ /vspawn [model]")]
        public void SpawnCarCommand(Client sender, VehicleHash model)
        {
            AccountModel acc = sender.GetAccountEntity().DbModel;
            CharacterModel ch = sender.GetAccountEntity().CharacterEntity.DbModel;

            FullPosition position = new FullPosition(sender.Position, sender.Rotation);

            VehicleEntity.Create(position, model, "Test", 1, acc, new Color(255, 255, 255),
               new Color(255, 255, 255), 0F, 0F, ch);

            sender.SendInfo($"Utworzono pojazd: {model}!");
        }

        private void API_onPlayerEnterVehicle(Client player, NetHandle vehicle)
        {
            AccountEntity account = player.GetAccountEntity();

            VehicleEntity vehicleEntity = EntityHelper.GetVehicle(vehicle);

            if (vehicleEntity != null)
            {
                if (vehicleEntity.DbModel.Character == account.CharacterEntity.DbModel)
                {
                    player.SendInfo("Wsiadłeś do swojego pojazdu.");
                    player.TriggerEvent("DisposeVehicleComponents");
                }
            }
            //NAPI.TriggerClientEvent(player, "show_vehicle_hud"); // sprawdzanie po stronie klienta
        }

        #region Komendy
        [Command("v", "~y~UŻYJ: ~w~ /v (z)")]
        public void ShowVehiclesList(Client sender, string trigger = null)
        {
            AccountEntity player = sender.GetAccountEntity();
            if (trigger == null)
            {
                if (NAPI.Player.IsPlayerInAnyVehicle(sender))
                {
                    VehicleEntity vehicleEntity = EntityHelper.GetVehicle(player.Client.Vehicle);
                    if (vehicleEntity == null) return;

                    string tuningJson = JsonConvert.SerializeObject(vehicleEntity.DbModel.ItemsInVehicle
                        .Where(i => i.ItemEntityType == ItemEntityType.Tuning && i.FourthParameter.HasValue)
                        .Select(i => new
                        {
                            i.Name
                        }));

                    string itemsInVehicleJson = JsonConvert.SerializeObject(vehicleEntity.DbModel.ItemsInVehicle
                        .Select(i => new
                        {
                            i.Name
                        }));

                    string playerGroups = JsonConvert.SerializeObject(EntityHelper.GetPlayerGroups(sender.GetAccountEntity())
                        .Select(g => new
                        {
                            g.DbModel.Name
                        }));

                    sender.TriggerEvent("OnPlayerManageVehicle", tuningJson, itemsInVehicleJson, playerGroups);
                }
                else
                {
                    string vehiclesJson = JsonConvert.SerializeObject(
                        player.CharacterEntity.DbModel.Vehicles.Select(v => new
                        {
                            Id = v.Id,
                            Name = v.VehicleHash.ToString(),
                            Plate = v.NumberPlate
                        }));

                    sender.TriggerEvent("OnPlayerShowVehicles", vehiclesJson);
                }
            }
            //v zamknij
            else if (trigger == "z")
            {
                ChangePlayerVehicleLockState(player.CharacterEntity);
            }
        }
        #endregion

        public static void ChangeDoorState(CharacterEntity sender, NetHandle vehicle, int doorId)
        {
            if (NAPI.Vehicle.IsVehicleDoorBroken(vehicle, doorId))
            {
                ChatScript.SendMessageToPlayer(sender,
                    "Drzwi wyglądają na zepsute, nie chcą nawet drgnąć.", ChatMessageType.ServerDo);
                return;
            }
            NAPI.Vehicle.SetVehicleDoorState(vehicle, doorId, !NAPI.Vehicle.GetVehicleDoorState(vehicle, doorId));
        }

        //Nie dajemy tutaj VehicleEntity, żeby gracz mógł sprawdziæ też informacje odspawnowanego auta
        public static void ShowVehiclesInformation(CharacterEntity sender, VehicleModel model, bool shortInfo = false)
        {
            if (!shortInfo && model.Character.Id == sender.DbModel.Id)
            {
                float enginePower = (float)((model.EnginePowerMultiplier - 1.0) * 20.0 + 80);

                sender.SendInfo($"Nazwa pojazdu: {model.VehicleHash} " +
                              $"\nRejestracja pojazdu: {model.NumberPlate} " +
                              $"\nMoc silnika: {enginePower}KM");
            }
            else
                sender.SendInfo($"\nRejestracja pojazdu: {model.NumberPlate}");
        }

        /// <summary>
        /// Metoda zamyka/otwiera pojazd należący do gracza który jest blisko niego
        /// </summary>
        /// <param name="senderCharacter"></param>
        public static void ChangePlayerVehicleLockState(CharacterEntity senderCharacter)
        {
            foreach (var vehicleEntity in EntityHelper.GetVehicles())
            {
                senderCharacter.SendInfo(vehicleEntity.DbModel.Name);
            }

            VehicleEntity vehicle = EntityHelper.GetVehicles()
                .Where(v => v.DbModel.Character.Id == senderCharacter.DbModel.Id)
                .FirstOrDefault(x => x.GameVehicle.Position.DistanceTo(senderCharacter.Position) < 100f);

            if (vehicle == null)
            {
                senderCharacter.SendInfo("W pobliżu nie ma żadnego pojazdu należącego do Ciebie.");
            }
            else
            {
                senderCharacter.SendInfo(vehicle.GameVehicle.Locked
                    ? "Twój pojazd został otwarty."
                    : "Twój pojazd został zamknięty.");
                vehicle.GameVehicle.Locked = !vehicle.GameVehicle.Locked;

            }
        
        }

        public static int GetVehicleDoorCount(VehicleHash vehicle)
        {
            if (NAPI.Vehicle.GetVehicleClass(vehicle).Equals(0) || NAPI.Vehicle.GetVehicleClass(vehicle).Equals(20) ||
                NAPI.Vehicle.GetVehicleClass(vehicle).Equals(3) || NAPI.Vehicle.GetVehicleClass(vehicle).Equals(7) ||
                NAPI.Vehicle.GetVehicleClass(vehicle).Equals(20) || NAPI.Vehicle.GetVehicleClass(vehicle).Equals(15) ||
                NAPI.Vehicle.GetVehicleClass(vehicle).Equals(19) || NAPI.Vehicle.GetVehicleClass(vehicle).Equals(10) ||
                NAPI.Vehicle.GetVehicleClass(vehicle).Equals(4) || NAPI.Vehicle.GetVehicleClass(vehicle).Equals(5) ||
                NAPI.Vehicle.GetVehicleClass(vehicle).Equals(6))
                return 4;
            if (NAPI.Vehicle.GetVehicleClass(vehicle).Equals(13) ||
                NAPI.Vehicle.GetVehicleClass(vehicle).Equals(8) ||
                NAPI.Vehicle.GetVehicleClass(vehicle).Equals(14))
                return 0;
            return 6;
        }

        public static int GetVehicleBlip(VehicleClass vehicleClass)
        {
            switch (vehicleClass)
            {
                case VehicleClass.Motorcycles:
                    return 226;
                case VehicleClass.Utility:
                case VehicleClass.Service:
                case VehicleClass.Industrial:
                    return 447;
                case VehicleClass.Heli:
                    return 481;
                case VehicleClass.Planes:
                    return 307;
            }
            return 225;
        }

    }
}
