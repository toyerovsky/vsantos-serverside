/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using GTANetworkAPI;
using GTANetworkInternals;
using Serverside.Entities.Base;
using Serverside.Entities.Common.DriveThru.Models;
using Serverside.Entities.Interfaces;

namespace Serverside.Entities.Common.DriveThru
{
    public class DriveThruEntity : GameEntity, IInteractive
    {
        public DriveThruModel Data { get; }

        public Marker DriveThruMarker { get; private set; }
        public ColShape ColShape { get; private set; }

        public DriveThruEntity(DriveThruModel data) : base()
        {
            Data = data;
        }

        public override void Dispose()
        {
            NAPI.ColShape.DeleteColShape(ColShape);
            NAPI.Entity.DeleteEntity(DriveThruMarker);
        }

        public override void Spawn()
        {
            base.Spawn();

            DriveThruMarker = NAPI.Marker.CreateMarker(1, Data.Position, new Vector3(), new Vector3(1f, 1f, 1f),
                1f, new Color(255, 106, 148, 40));

            ColShape = NAPI.ColShape.CreateCylinderColShape(Data.Position, 2f, 5f);

            ColShape.OnEntityEnterColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) != EntityType.Player) return;

                NAPI.Player.GetPlayerFromHandle(entity).TriggerEvent("ShowDriveThruMenu");

            };

            ColShape.OnEntityExitColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) != EntityType.Player) return;

                NAPI.Player.GetPlayerFromHandle(entity).TriggerEvent("DisposeDriveThruMenu");

            };
        }
    }

}