/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using GTANetworkAPI;
using Serverside.Entities.Common.DriveThru.Models;
using Serverside.Entities.Interfaces;

namespace Serverside.Entities.Common.DriveThru
{
    public class DriveThru : IGameEntity
    {
        public DriveThruModel Data { get; }

        private Marker DriveThruMarker { get; set; }
        private ColShape DriveThruColshape { get; set; }

        public DriveThru(DriveThruModel data)
        {
            Data = data;
        }

        public void Dispose()
        {
            NAPI.ColShape.DeleteColShape(DriveThruColshape);
            NAPI.Entity.DeleteEntity(DriveThruMarker);
        }

        public void Spawn()
        {
            DriveThruMarker = NAPI.Marker.CreateMarker(1, Data.Position, new Vector3(), new Vector3(1f, 1f, 1f),
                1f, new Color(255, 106, 148, 40));

            DriveThruColshape = NAPI.ColShape.CreateCylinderColShape(Data.Position, 2f, 5f);

            DriveThruColshape.OnEntityEnterColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) != EntityType.Player) return;

                NAPI.Player.GetPlayerFromHandle(entity).TriggerEvent("ShowDriveThruMenu");

            };

            DriveThruColshape.OnEntityExitColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) != EntityType.Player) return;

                NAPI.Player.GetPlayerFromHandle(entity).TriggerEvent("DisposeDriveThruMenu");

            };
        }
    }

}