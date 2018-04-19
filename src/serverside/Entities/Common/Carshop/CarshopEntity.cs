/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using VRP.Core.Enums;
using VRP.Serverside.Entities.Base;
using VRP.Serverside.Entities.Common.Carshop.Models;
using VRP.Serverside.Interfaces;

namespace VRP.Serverside.Entities.Common.Carshop
{
    public class CarshopEntity : GameEntity, IInteractive
    {
        public CarshopModel Data { get; set; }
        public Marker CarshopMarker { get; set; }
        public ColShape ColShape { get; private set; }
        public Blip CarshopBlip { get; set; }

        public CarshopEntity(CarshopModel data)
        {
            Data = data;
        }

        public override void Spawn()
        {
            CarshopMarker = NAPI.Marker.CreateMarker(0, Data.Position, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f),
                1f, new Color(255, 106, 148, 40));

            ColShape = NAPI.ColShape.CreateCylinderColShape(Data.Position, 2f, 5f);
            ColShape.OnEntityEnterColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) != EntityType.Player) return;

                Client player = NAPI.Player.GetPlayerFromHandle(entity);
                string compactsJson =
                    JsonConvert.SerializeObject(CarshopScript.Vehicles.Where(v => v.Category == VehicleClass.Compact && v.CarshopTypes == Data.Type));

                string coupesJson =
                    JsonConvert.SerializeObject(CarshopScript.Vehicles.Where(v => v.Category == VehicleClass.Coupe && v.CarshopTypes == Data.Type));

                string suvsJson =
                    JsonConvert.SerializeObject(CarshopScript.Vehicles.Where(v => v.Category == VehicleClass.Suv && v.CarshopTypes == Data.Type));

                string sedansJson =
                    JsonConvert.SerializeObject(CarshopScript.Vehicles.Where(v => v.Category == VehicleClass.Sedans && v.CarshopTypes == Data.Type));

                string sportsJson =
                    JsonConvert.SerializeObject(CarshopScript.Vehicles.Where(v => v.Category == VehicleClass.Sports && v.CarshopTypes == Data.Type));

                string motorcyclesJson =
                    JsonConvert.SerializeObject(CarshopScript.Vehicles.Where(v => v.Category == VehicleClass.Motorcycles && v.CarshopTypes == Data.Type));

                string bicyclesJson =
                    JsonConvert.SerializeObject(CarshopScript.Vehicles.Where(v => v.Category == VehicleClass.Cycle && v.CarshopTypes == Data.Type));

                NAPI.ClientEvent.TriggerClientEvent(player, "OnPlayerEnteredCarshop", compactsJson, coupesJson, suvsJson, sedansJson, sportsJson, motorcyclesJson, bicyclesJson);
            };

            CarshopBlip = NAPI.Blip.CreateBlip(Data.Position);
            CarshopBlip.Sprite = 490;
            CarshopBlip.Transparency = 100;
        }

        public override void Dispose()
        {
            NAPI.ColShape.DeleteColShape(ColShape);
            NAPI.Entity.DeleteEntity(CarshopMarker);
            CarshopBlip.Transparency = 0;
        }
    }
}