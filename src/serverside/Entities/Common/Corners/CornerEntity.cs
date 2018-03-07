/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Linq;
using System.Timers;
using GTANetworkAPI;
using Serverside.Core;
using Serverside.Core.Extensions;
using Serverside.Entities.Base;
using Serverside.Entities.Common.Corners.Models;
using Serverside.Entities.Core;
using Serverside.Entities.Interfaces;

namespace Serverside.Entities.Common.Corners
{
    public class CornerEntity : GameEntity, IInteractive
    {
        public CornerModel Data { get; set; }

        public Marker Marker { get; set; }
        public ColShape ColShape { get; set; }

        private bool CornerBusy { get; set; }
        private AccountEntity Player { get; set; }
        private CornerPedEntity CurrentPedEntity { get; set; }

        public CornerEntity(CornerModel corner)
        {
            Data = corner;
        }

        public override void Spawn()
        {
            Marker = NAPI.Marker.CreateMarker(1, Data.Position.Position, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 1f, 100,
                100, 100);
            Marker.Invincible = true;
            ColShape = NAPI.ColShape.CreateCylinderColShape(Data.Position.Position, 1f, 2f);
            ColShape.OnEntityEnterColShape += OnEntityEnterColShape;
        }

        private void OnEntityEnterColShape(ColShape shape, Client entity)
        {
            if (DateTime.Now.Hour < 16 || DateTime.Now.Hour > 23)
            {
                entity.Notify("Handel na rogu możliwy jest od 16 do 23.", true);
                return;
            }

            if (!CornerBusy)
            {
                Player = entity.GetAccountEntity();

                CornerBusy = true;
                Player.CharacterEntity.CurrentInteractive = this;

                Player.Client.Notify("Rozpocząłeś proces sprzedaży, pozostań w znaczniku.", true);

                StartProcess();
                Player.Client.PlayAnimation("amb@world_human_drug_dealer_hard@male@idle_a", "idle_a", (int)AnimationFlags.Loop | (int)AnimationFlags.AllowPlayerControl | (int)AnimationFlags.Cancellable);
            }
        }

        private void StartProcess()
        {
            //Przychodzące boty
            Timer timer = new Timer(Tools.RandomRange(90, 180) * 1000);
            timer.Start();

            timer.Elapsed += (sender, args) =>
            {
                timer.Stop();

                int random = Tools.RandomRange(Data.CornerBots.Count);

                CurrentPedEntity = new CornerPedEntity(Data.CornerBots[random].Name, Data.CornerBots[random].PedHash, Data.BotPositions[0], Data.BotPositions.Where(x => x != Data.BotPositions[0]).ToList(), Data.CornerBots[random].DrugType, Data.CornerBots[random].MoneyCount, Data.CornerBots[random].Greeting, Data.CornerBots[random].GoodFarewell, Data.CornerBots[random].BadFarewell, Player, Data.CornerBots[random].BotId);

                CurrentPedEntity.Spawn();
                CurrentPedEntity.OnTransactionEnd += (o, eventArgs) =>
                {
                    timer.Start();
                    CurrentPedEntity.Dispose();
                };
            };

            ColShape.OnEntityExitColShape += (shape, entity) =>
            {
                if (CornerBusy && NAPI.Entity.GetEntityType(entity) == EntityType.Player &&
                    entity == Player.Client.Handle)
                {
                    timer.Dispose();

                    CornerBusy = false;
                    Player.Client.StopAnimation();

                    if (CurrentPedEntity != null)
                    {
                        CurrentPedEntity.Dispose();
                        CurrentPedEntity = null;
                    }

                    Player = null;
                }
            };
        }

        public override void Dispose()
        {
            ColShape.OnEntityEnterColShape -= OnEntityEnterColShape;
            NAPI.ColShape.DeleteColShape(ColShape);
            NAPI.Entity.DeleteEntity(Marker);
        }
    }
}