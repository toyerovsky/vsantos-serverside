/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkAPI;
using GTANetworkInternals;
using Serverside.Core.Extensions;
using Serverside.Core.Telephone;
using Serverside.Entities.Base;
using Serverside.Entities.Common.Booth.Models;
using Serverside.Entities.Interfaces;

namespace Serverside.Entities.Common.Booth
{
    public class TelephoneBooth : GameEntity, IInteractive
    {
        public TelephoneCall CurrentCall { get; set; }
        public Client CurrentClient { get; set; }

        public TelephoneBoothModel Data { get; set; }
        public Marker Marker { get; set; }
        public ColShape ColShape { get; set; }

        public bool IncomingCall = false;

        public TelephoneBooth(EventClass events, TelephoneBoothModel data) : base(events)
        {
            Data = data;
        }

        public override void Spawn()
        {
            base.Spawn();
            NAPI.TextLabel.CreateTextLabel($"~y~BUDKA\n~w~Numer: {Data.Number}",
                new Vector3(Data.Position.Position.X, Data.Position.Position.Y, Data.Position.Position.Z + 1),
                7f, 1f, 1, new Color(255, 255, 255));

            ColShape = NAPI.ColShape.CreateCylinderColShape(Data.Position.Position, 1f, 2f);

            Marker = NAPI.Marker.CreateMarker(1, Data.Position.Position,
                new Vector3(0, 0, 0), new Vector3(1f, 1f, 1f),
                1f, new Color(100, 255, 0, 100));

            ColShape.OnEntityEnterColShape += (shape, entity) =>
            {
                if (CurrentCall != null && CurrentCall.Accepted)
                {
                    //Budka jest używana
                    entity.Notify("Ta budka obecnie jest używana.");
                }
                else if (CurrentCall != null)
                {
                    //Budka nie jest używana i dzwoni
                    CurrentClient = NAPI.Player.GetPlayerFromHandle(entity);
                    CurrentCall?.Open();
                }
                else if (CurrentClient == null)
                {
                    //Budka nie jest używana i nie dzwoni
                    CurrentClient = NAPI.Player.GetPlayerFromHandle(entity);
                    CurrentClient.GetAccountEntity().CharacterEntity.CurrenInteractive = this;
                    NAPI.ClientEvent.TriggerClientEvent(CurrentClient, "OnPlayerEnteredTelephonebooth");
                }
            };

            ColShape.OnEntityExitColShape += (shape, entity) =>
            {
                if (CurrentCall != null && ReferenceEquals(entity, CurrentClient))
                {
                    //Opuszczanie budki kiedy dzwoni
                    CurrentClient.GetAccountEntity().CharacterEntity.CurrenInteractive = null;
                    CurrentClient = null;
                    CurrentCall?.Dispose();
                }
            };
        }

        public override void Dispose()
        {
            var character = CurrentClient.GetAccountEntity().CharacterEntity;
            if (ReferenceEquals(character.CurrenInteractive, this))
                character.CurrenInteractive = null;
            CurrentCall?.Dispose();
            NAPI.ColShape.DeleteColShape(ColShape);
            NAPI.Entity.DeleteEntity(Marker);
        }
    }
}