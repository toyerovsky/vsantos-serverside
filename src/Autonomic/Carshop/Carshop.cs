/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Serverside.Autonomic.Carshop.Models;
using Serverside.Core.Extensions;

namespace Serverside.Autonomic.Carshop
{
    public class Carshop : IDisposable
    {
        public CarshopModel Data { get; set; }
        public Marker CarshopMarker { get; set; }
        public ColShape CarshopColshape { get; set; }
        public Blip CarshopBlip { get; set; }

        public Carshop(CarshopModel data)
        {
            Data = data;
            CarshopMarker = NAPI.Marker.CreateMarker(0, Data.Position, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f),
                1f, new Color(255, 106, 148, 40));

            CarshopColshape = NAPI.ColShape.CreateCylinderColShape(Data.Position, 2f, 5f);
            CarshopColshape.OnEntityEnterColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) != EntityType.Player) return;

                var player = NAPI.Player.GetPlayerFromHandle(entity);
                string compactsJson =
                    JsonConvert.SerializeObject(CarshopScript.Vehicles.Where(v => v.Category == VehicleClass.Compact && v.CarshopTypes.Contains(Data.Type)));

                string coupesJson =
                    JsonConvert.SerializeObject(CarshopScript.Vehicles.Where(v => v.Category == VehicleClass.Coupe && v.CarshopTypes.Contains(Data.Type)));

                string suvsJson =
                    JsonConvert.SerializeObject(CarshopScript.Vehicles.Where(v => v.Category == VehicleClass.SuVs && v.CarshopTypes.Contains(Data.Type)));

                string sedansJson =
                    JsonConvert.SerializeObject(CarshopScript.Vehicles.Where(v => v.Category == VehicleClass.Sedans && v.CarshopTypes.Contains(Data.Type)));

                string sportsJson =
                    JsonConvert.SerializeObject(CarshopScript.Vehicles.Where(v => v.Category == VehicleClass.Sports && v.CarshopTypes.Contains(Data.Type)));

                string motorcyclesJson =
                    JsonConvert.SerializeObject(CarshopScript.Vehicles.Where(v => v.Category == VehicleClass.Motorcycles && v.CarshopTypes.Contains(Data.Type)));

                string bicyclesJson =
                    JsonConvert.SerializeObject(CarshopScript.Vehicles.Where(v => v.Category == VehicleClass.Cycle && v.CarshopTypes.Contains(Data.Type)));

                NAPI.ClientEvent.TriggerClientEvent(player, "OnPlayerEnteredCarshop", compactsJson, coupesJson, suvsJson, sedansJson, sportsJson, motorcyclesJson, bicyclesJson);
            };

            CarshopBlip = NAPI.Blip.CreateBlip(Data.Position);
            CarshopBlip.Sprite = 490;
            CarshopBlip.Transparency = 100;
        }

        public void Dispose()
        {
            NAPI.ColShape.DeleteColShape(CarshopColshape);
            NAPI.Entity.DeleteEntity(CarshopMarker);
            CarshopBlip.Transparency = 0;
        }
    }
}