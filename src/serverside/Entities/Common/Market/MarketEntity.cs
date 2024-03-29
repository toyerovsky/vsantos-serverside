﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using VRP.Serverside.Entities.Base;
using VRP.Serverside.Entities.Common.Market.Models;
using VRP.Serverside.Interfaces;
using FullPosition = VRP.Serverside.Core.FullPosition;

namespace VRP.Serverside.Entities.Common.Market
{
    public class MarketEntity : GameEntity, IInteractive
    {
        public MarketModel Data { get; }

        public ColShape ColShape { get; private set; }

        public PedEntity MarketNpc { get; set; }
        public Blip MarketBlip { get; set; }

        public MarketEntity(MarketModel data) : base()
        {
            Data = data;
        }

        public override void Spawn()
        {
            base.Spawn();

            Random random = new Random();
            KeyValuePair<string, PedHash> botInfo =
                Constant.Items.ConstantNames.OrderBy(x => random.Next(Constant.Items.ConstantNames
                    .Count)).ElementAt(0);

            MarketNpc = new PedEntity(botInfo.Key, botInfo.Value, new FullPosition(Data.Center, new Vector3(1f, 1f, 1f)));

            ColShape = NAPI.ColShape.CreateCylinderColShape(Data.Center, Data.Radius, 5f);

            ColShape.OnEntityEnterColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) == EntityType.Player)
                {
                    NAPI.Player.GetPlayerFromHandle(entity).SetData("CurrentMarket", this);
                }
            };

            ColShape.OnEntityExitColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) == EntityType.Player)
                {
                    NAPI.Player.GetPlayerFromHandle(entity).ResetData("CurrentMarket");
                }
            };

            MarketBlip = NAPI.Blip.CreateBlip(Data.Center);
            MarketBlip.Sprite = 93;
        }

        public override void Dispose()
        {
            MarketNpc?.Dispose();
            NAPI.ColShape.DeleteColShape(ColShape);
            MarketBlip.Transparency = 0;
        }
    }
}