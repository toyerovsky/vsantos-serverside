/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Globalization;
using GTANetworkAPI;
using Newtonsoft.Json;
using Serverside.Bank.Models;
using Serverside.Core.Extensions;

namespace Serverside.Bank
{
    public class Atm : IDisposable
    {
        public Marker AtmMarker { get; }
        public ColShape AtmShape { get; }
        public AtmModel Data { get; set; }
        public Blip AtmBlip { get; set; }

        public Atm(AtmModel data)
        {
            Data = data;

            AtmMarker = NAPI.Marker.CreateMarker(1, Data.Position.Position,
                new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 1f, new Color(100, 100, 100, 100));

            AtmShape = NAPI.ColShape.CreateCylinderColShape(Data.Position.Position, 1f, 2f);

            AtmShape.OnEntityEnterColShape += (shape, entity) =>
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

            AtmShape.OnEntityExitColShape += (shape, entity) =>
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

        public void Dispose()
        {
            NAPI.Entity.DeleteEntity(AtmMarker);
            NAPI.ColShape.DeleteColShape(AtmShape);
            AtmBlip.Transparency = 0;
        }
    }
}