/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Linq;
using GTANetworkAPI;
using GTANetworkInternals;
using Serverside.Core;
using Serverside.Entities.Interfaces;

namespace Serverside.Autonomic.Market
{
    public class Market : IGameEntity, IDisposable
    {
        public Models.Market MarketData { get; set; }

        private ColShape MarketColshape { get; set; }
        private Bot MarketNpc { get; set; }
        private Blip MarketBlip { get; set; }

        public Market(Models.Market data)
        {
            MarketData = data;
        }

        public void Spawn()
        {
            var botInfo =
                Constant.Items.ConstantNames.OrderBy(x => new Random().Next(Constant.Items.ConstantNames
                    .Count)).ElementAt(0);

            Tools

            MarketNpc = new Bot(botInfo.Key, botInfo.Value, new FullPosition(MarketData.Center, new Vector3(1f, 1f, 1f)));

            MarketColshape = NAPI.ColShape.CreateCylinderColShape(data.Center, data.Radius, 5f);

            MarketColshape.OnEntityEnterColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) == EntityType.Player)
                {
                    NAPI.Player.GetPlayerFromHandle(entity).SetData("CurrentMarket", this);
                }
            };

            MarketColshape.OnEntityExitColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) == EntityType.Player)
                {
                    NAPI.Player.GetPlayerFromHandle(entity).ResetData("CurrentMarket");
                }
            };

            MarketBlip = NAPI.Blip.CreateBlip(MarketData.Center);
            MarketBlip.Sprite = 93;
        }

        public void Dispose()
        {
            MarketNpc?.Dispose();
            NAPI.ColShape.DeleteColShape(MarketColshape);
            MarketBlip.Transparency = 0;
        }
    }
}