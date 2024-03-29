﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using GTANetworkAPI;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Core.Telephone;
using VRP.Serverside.Entities.Base;
using VRP.Serverside.Entities.Common.Booth.Models;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Interfaces;

namespace VRP.Serverside.Entities.Common.Booth
{
    public class TelephoneBoothEntity : GameEntity, IInteractive
    {
        public TelephoneCall CurrentCall { get; set; }
        public Client CurrentClient { get; set; }

        public TelephoneBoothModel Data { get; set; }
        public Marker Marker { get; set; }
        public ColShape ColShape { get; set; }

        public bool IncomingCall = false;

        public TelephoneBoothEntity(TelephoneBoothModel data)
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
                    // Budka jest używana
                    entity.SendWarning("Ta budka obecnie jest używana.");
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
                    CurrentClient.GetAccountEntity().CharacterEntity.CurrentInteractive = this;
                    NAPI.ClientEvent.TriggerClientEvent(CurrentClient, "OnPlayerEnteredTelephonebooth");
                }
            };

            ColShape.OnEntityExitColShape += (shape, entity) =>
            {
                if (CurrentCall != null && ReferenceEquals(entity, CurrentClient))
                {
                    //Opuszczanie budki kiedy dzwoni
                    CurrentClient.GetAccountEntity().CharacterEntity.CurrentInteractive = null;
                    CurrentClient = null;
                    CurrentCall?.Dispose();
                }
            };
        }

        public override void Dispose()
        {
            CharacterEntity character = CurrentClient.GetAccountEntity().CharacterEntity;
            if (ReferenceEquals(character.CurrentInteractive, this))
                character.CurrentInteractive = null;
            CurrentCall?.Dispose();
            NAPI.ColShape.DeleteColShape(ColShape);
            NAPI.Entity.DeleteEntity(Marker);
        }
    }
}