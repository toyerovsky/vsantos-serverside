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

namespace Serverside.Autonomic.Market
{
    public class Market : IDisposable
    {
        public EventClass Events { get; set; }
        public Models.Market MarketData { get; set; }

        private ColShape MarketColshape { get; set; }
        private Bot MarketNpc { get; set; }
        private Blip MarketBlip { get; set; }

        public Market(EventClass events, Models.Market data)
        {
            Events = events;
            MarketData = data;

            var botInfo =
                Constant.ConstantItems.ConstantNames.OrderBy(x => new Random().Next(Constant.ConstantItems.ConstantNames
                    .Count)).ElementAt(0);

            MarketNpc = new Bot(events, botInfo.Key, botInfo.Value, new FullPosition(MarketData.Center, new Vector3(1f, 1f, 1f)));

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