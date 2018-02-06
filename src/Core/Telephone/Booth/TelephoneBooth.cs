/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkAPI;
using GTANetworkInternals;
using Serverside.Core.Extensions;
using Serverside.Core.Telephone.Booth.Models;
using Serverside.Entities.Base;

namespace Serverside.Core.Telephone.Booth
{
    public class TelephoneBooth : GameEntity
    {
        public TelephoneCall CurrentCall { get; set; }
        public Client CurrentClient { get; set; }

        public TelephoneBoothModel Data { get; set; }
        public Marker Marker { get; set; }
        public ColShape ColShape { get; set; }

        public TelephoneBooth(EventClass events, TelephoneBoothModel data)
            : base(events)
        {
            Data = data;

            NAPI.TextLabel.CreateTextLabel($"~y~BUDKA\n~w~Numer: {Data.Number}", new Vector3(Data.Position.Position.X, Data.Position.Position.Y, Data.Position.Position.Z + 1), 7f, 1f, 1, new Color(255, 255, 255));

            ColShape = NAPI.ColShape.CreateCylinderColShape(Data.Position.Position, 1f, 2f);

            Marker = NAPI.Marker.CreateMarker(1, Data.Position.Position, new Vector3(0, 0, 0), new Vector3(1f, 1f, 1f),
                1f, new Color(100, 255, 0, 100));

            ColShape.OnEntityEnterColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) == EntityType.Player && CurrentCall != null)
                {
                    NAPI.Player.GetPlayerFromHandle(entity).Notify("Ta budka obecnie jest używana.");
                }
                else if (NAPI.Entity.GetEntityType(entity) == EntityType.Player && NAPI.Data.HasEntityData(Marker, "BoothRinging"))
                {
                    CurrentClient = NAPI.Player.GetPlayerFromHandle(entity);
                    CurrentCall?.Open();
                }
                else if (NAPI.Entity.GetEntityType(entity) == EntityType.Player && CurrentClient == null)
                {
                    CurrentClient = NAPI.Player.GetPlayerFromHandle(entity);
                    NAPI.Data.SetEntityData(entity, "Booth", this);
                    NAPI.ClientEvent.TriggerClientEvent(NAPI.Player.GetPlayerFromHandle(entity), "OnPlayerEnteredTelephonebooth");
                }
            };

            ColShape.OnEntityExitColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) == EntityType.Player && CurrentCall != null)
                {
                    CurrentClient = null;
                    NAPI.Data.ResetEntityData(entity, "Booth");
                    CurrentCall?.Dispose();
                }
            };
        }

        public override void Dispose()
        {
            CurrentCall?.Dispose();
            NAPI.ColShape.DeleteColShape(ColShape);
            NAPI.Entity.DeleteEntity(Marker);
        }
    }
}