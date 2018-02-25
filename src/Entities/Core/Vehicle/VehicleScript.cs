/* Copyright (C) Przemys³aw Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemys³aw Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Serverside.Core.Database.Models;
using Serverside.Core.Enums;
using Serverside.Core.Extensions;
using Serverside.Core.Scripts;
using Serverside.Entities.Core.Item;

namespace Serverside.Entities.Core.Vehicle
{
    public class VehicleScript : Script
    {
        private void API_OnClientEventTrigger(Client sender, string eventName, params object[] args)
        {
            if (eventName == "OnPlayerSelectedVehicle")
            {
                sender.SetData("SelectedVehicleID", Convert.ToInt64(args[0]));
            }
            else if (eventName == "OnPlayerSpawnVehicle")
            {
                VehicleEntity vehicleEntity = EntityManager.GetVehicle(
                    (long)sender.GetData("SelectedVehicleID"));

                if (vehicleEntity != null)
                {
                    //UNSPAWN
                    vehicleEntity.Dispose();
                    sender.Notify("Pojazd zosta³ schowany w gara¿u!");
                }
                else
                {
                    //SPAWN
                    var vehicle = sender.GetAccountEntity().CharacterEntity.DbModel.Vehicles
                        .Single(v => v.Id == (long)sender.GetData("SelectedVehicleID"));

                    vehicleEntity = new VehicleEntity(vehicle);

                    sender.TriggerEvent("DrawVehicleComponents", vehicleEntity.GameVehicle.Position,
                        GetVehicleBlip((VehicleClass)vehicleEntity.GameVehicle.Class), 24);

                    sender.Notify($"Pojazd {vehicle.Name} zosta³ zespawnowany.");
                }
                sender.ResetData("SelectedVehicleID");
            }
            else if (eventName == "OnPlayerParkVehicle")
            {
                if (EntityManager.GetVehicle(sender.Vehicle) == null)
                    return;

                var controller = EntityManager.GetVehicle(sender.Vehicle);
                controller.ChangeSpawnPosition();
                sender.Notify($"Pojazd {controller.DbModel.Name} zosta³ zaparkowany.");
            }
            else if (eventName == "OnPlayerInformationsVehicle")
            {
                var player = sender.GetAccountEntity();

                if (player.CharacterEntity.DbModel.Vehicles.Any(v => v.Id == sender.GetData("SelectedVehicleID")))
                    ShowVehiclesInformation(sender, player.CharacterEntity.DbModel.Vehicles.Single(
                        v => v.Id == sender.GetData("SelectedVehicleID")));
            }
            else if (eventName == "OnPlayerInformationsInVehicle")
            {
                var vehicle = EntityManager.GetVehicle(sender.Vehicle);
                if (vehicle == null) return;

                float enginePower = (float)((vehicle.DbModel.EnginePowerMultiplier - 1.0) * 20.0 + 80);

                sender.Notify($"Nazwa pojazdu: {vehicle.DbModel.Name}" +
                              $"\nRejestracja pojazdu: {vehicle.DbModel.NumberPlate}" +
                              $"\nMoc silnika: {enginePower}KM");

            }
            else if (eventName == "OnPlayerChangeLockVehicle")
            {
                ChangePlayerVehicleLockState(sender);
            }
            else if (eventName == "OnPlayerEngineStateChangeVehicle")
            {
                sender.Notify(sender.Vehicle.EngineStatus ?
                    "Pojazd zosta³ zgaszony." : "Pojazd zosta³ uruchomiony.");
                sender.Vehicle.EngineStatus = !sender.Vehicle.EngineStatus;
            }
        }

        private void API_onPlayerEnterVehicle(Client player, NetHandle vehicle)
        {
            AccountEntity account = player.GetAccountEntity();

            VehicleEntity vehicleEntity = EntityManager.GetVehicle(vehicle);
            if (vehicleEntity != null)
            {
                if (vehicleEntity.DbModel.Character == account.CharacterEntity.DbModel)
                {
                    player.Notify("Wsiad³eœ do swojego pojazdu.");
                    player.TriggerEvent("DisposeVehicleComponents");
                }
            }
            //NAPI.TriggerClientEvent(player, "show_vehicle_hud"); // sprawdzanie po stronie klienta
        }

        #region Komendy
        [Command("v", "~y~U¯YJ: ~w~ /v (z)")]
        public void ShowVehiclesList(Client sender, string trigger = null)
        {
            AccountEntity player = sender.GetAccountEntity();
            if (trigger == null)
            {
                if (NAPI.Player.IsPlayerInAnyVehicle(sender))
                {
                    VehicleEntity vehicleEntity = EntityManager.GetVehicle(player.Client.Vehicle);
                    if (vehicleEntity == null) return;

                    string tuningJson = JsonConvert.SerializeObject(vehicleEntity.DbModel.ItemsInVehicle
                        .Where(i => i.ItemType == ItemType.Tuning && i.FourthParameter.HasValue)
                        .Select(i => new
                        {
                            i.Name
                        }));

                    string itemsInVehicleJson = JsonConvert.SerializeObject(vehicleEntity.DbModel.ItemsInVehicle
                        .Select(i => new
                        {
                            i.Name
                        }));

                    string playerGroups = JsonConvert.SerializeObject(EntityManager.GetPlayerGroups(sender.GetAccountEntity())
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
                ChangePlayerVehicleLockState(sender);
            }
        }
        #endregion

        public static void ChangeDoorState(Client sender, NetHandle vehicle, int doorId)
        {
            if (NAPI.Vehicle.IsVehicleDoorBroken(vehicle, doorId))
            {
                ChatScript.SendMessageToPlayer(sender,
                    "Drzwi wygl¹daj¹ na zepsute, nie chc¹ nawet drgn¹æ.", ChatMessageType.ServerDo);
                return;
            }
            NAPI.Vehicle.SetVehicleDoorState(vehicle, doorId, !NAPI.Vehicle.GetVehicleDoorState(vehicle, doorId));
        }

        //Nie dajemy tutaj VehicleEntity, ¿eby gracz móg³ sprawdziæ te¿ informacje odspawnowanego auta
        public static void ShowVehiclesInformation(Client sender, VehicleModel model, bool shortInfo = false)
        {
            if (!shortInfo && model.Character.Id == sender.GetAccountEntity().CharacterEntity.DbModel.Id)
            {
                float enginePower = (float)((model.EnginePowerMultiplier - 1.0) * 20.0 + 80);

                sender.Notify($"Nazwa pojazdu: {model.VehicleHash.ToString()} " +
                              $"\nRejestracja pojazdu: {model.NumberPlate} " +
                              $"\nMoc silnika: {enginePower}KM");
            }
            else
                sender.Notify($"\nRejestracja pojazdu: {model.NumberPlate}");
        }

        /// <summary>
        /// Metoda zamyka/otwiera pojazd nale¿¹cy do gracza który jest blisko niego
        /// </summary>
        /// <param name="player"></param>
        public static void ChangePlayerVehicleLockState(Client player)
        {
            AccountEntity accountEntity = player.GetAccountEntity();

            List<VehicleEntity> vehicles = EntityManager.GetVehicles()
                .Where(v => v.DbModel.Id == accountEntity.CharacterEntity.DbModel.Id)
                .Where(x => x.GameVehicle.Position.DistanceTo(player.Position) < 10).ToList();

            //Jeœli jakiœ pomys³owy gracz zapragnie postawiæ 2 pojazdy obok siebie, ¿eby sprawdziæ dzia³anie to zamknie mu obydwa
            foreach (VehicleEntity vehicle in vehicles)
            {
                if (!(vehicle.GameVehicle.Position.DistanceTo(player.Position) <= 7)) continue;
                player.Notify(vehicle.GameVehicle.Locked ?
                    "Twój pojazd zosta³ zamkniêty." : "Twój pojazd zosta³ otwarty.");
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
