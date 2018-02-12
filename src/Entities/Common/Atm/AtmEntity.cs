/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Globalization;
using GTANetworkAPI;
using GTANetworkInternals;
using Newtonsoft.Json;
using Serverside.Core.Extensions;
using Serverside.Entities.Base;
using Serverside.Entities.Common.Atm.Models;
using Serverside.Entities.Interfaces;

namespace Serverside.Entities.Common.Atm
{
    public class AtmEntity : GameEntity, IInteractive
    {
        public Marker Marker { get; private set; }
        public ColShape ColShape { get; private set; }

        public AtmModel Data { get; set; }
        public Blip AtmBlip { get; set; }

        public AtmEntity(EventClass events, AtmModel data) : base(events)
        {
            Data = data;
        }

        public override void Spawn()
        {
            base.Spawn();

            Marker = NAPI.Marker.CreateMarker(1, Data.Position.Position,
                new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 1f, new Color(100, 100, 100, 100));

            ColShape = NAPI.ColShape.CreateCylinderColShape(Data.Position.Position, 1f, 2f);

            ColShape.OnEntityEnterColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) == EntityType.Player)
                {
                    var player = NAPI.Player.GetPlayerFromHandle(entity).GetAccountEntity();

                    if (player.CharacterEntity.DbModel.BankAccountNumber == null)
                    {
                        player.Client.Notify("Nie posiadasz karty bankomatowej, udaj się do banku, aby założyć konto.");
                        return;
                    }

                    NAPI.ClientEvent.TriggerClientEvent(player.Client, "OnPlayerEnteredAtm", JsonConvert.SerializeObject(new
                    {
                        player.CharacterEntity.FormatName,
                        BankMoney = player.CharacterEntity.DbModel.BankMoney.ToString(CultureInfo.CurrentCulture),
                        BankAccountNumber = player.CharacterEntity.DbModel.BankAccountNumber.ToString()
                    }));
                }
            };

            ColShape.OnEntityExitColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) == EntityType.Player)
                {
                    var player = NAPI.Player.GetPlayerFromHandle(entity);
                    NAPI.ClientEvent.TriggerClientEvent(player, "OnPlayerExitAtm");
                }
            };

            AtmBlip = NAPI.Blip.CreateBlip(Data.Position.Position);
            AtmBlip.Sprite = 434;
            AtmBlip.Color = 69;
            AtmBlip.Transparency = 60;
        }

        public override void Dispose()
        {
            NAPI.Entity.DeleteEntity(Marker);
            NAPI.ColShape.DeleteColShape(ColShape);
            AtmBlip.Transparency = 0;
        }
    }
}