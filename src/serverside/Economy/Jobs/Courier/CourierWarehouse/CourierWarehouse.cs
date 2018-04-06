/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using VRP.Core.Enums;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Economy.Groups;
using VRP.Serverside.Economy.Jobs.Courier.CourierWarehouse.Models;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Economy.Jobs.Courier.CourierWarehouse
{
    public class CourierWarehouse : IDisposable
    {
        public CourierWarehouseModel Data { get; set; }

        public Marker WarehouseMarker { get; set; }
        public ColShape WarehouseColshape { get; set; }
        public Blip WarehouseBlip { get; set; }

        public CourierWarehouse(CourierWarehouseModel data)
        {
            Data = data;
            WarehouseMarker = NAPI.Marker.CreateMarker(0, Data.Position, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f),
                1f, new Color(255, 255, 0, 100));
            WarehouseColshape = NAPI.ColShape.CreateCylinderColShape(Data.Position, 5f, 5f);
            WarehouseBlip = NAPI.Blip.CreateBlip(Data.Position);
            WarehouseBlip.Sprite = 478;

            WarehouseColshape.OnEntityEnterColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) == EntityType.Player)
                {
                    AccountEntity player = NAPI.Player.GetPlayerFromHandle(entity).GetAccountEntity();
                    if (player.CharacterEntity.DbModel.Job != JobType.Courier)
                    {
                        player.Client.SendInfo("Aby podjąć pracę kuriera udaj się do pracodawcy.");
                        return;
                    }

                    if (GroupWarehouseScript.CurrentOrders.Count == 0)
                    {
                        player.Client.SendInfo("Obecnie nie ma żadnych paczek w magazynie.");
                        return;
                    }

                    player.Client.TriggerEvent("ShowCourierMenu", JsonConvert.SerializeObject(
                        GroupWarehouseScript.CurrentOrders.Select(x => new
                    {
                        Id = x.Data.Id,
                        Getter = x.Data.Getter.Name,
                    })));
                }
            };

            WarehouseColshape.OnEntityExitColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) == EntityType.Player)
                {
                    AccountEntity player = NAPI.Player.GetPlayerFromHandle(entity).GetAccountEntity();
                    if (player.CharacterEntity.DbModel.Job != JobType.Courier) return;
                    player.Client.TriggerEvent("DisposeCourierMenu");
                }
            };
        }

        public void Dispose()
        {
            NAPI.ColShape.DeleteColShape(WarehouseColshape);
            NAPI.Entity.DeleteEntity(WarehouseMarker);
            WarehouseBlip.Transparency = 0;
        }
    }
}