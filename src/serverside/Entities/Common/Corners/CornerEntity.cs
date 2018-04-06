/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Linq;
using System.Timers;
using GTANetworkAPI;
using VRP.Core.Enums;
using VRP.Core.Tools;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities.Base;
using VRP.Serverside.Entities.Common.Corners.Models;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Entities.Interfaces;

namespace VRP.Serverside.Entities.Common.Corners
{
    public class CornerEntity : GameEntity, IInteractive
    {
        public CornerModel Data { get; set; }

        public Marker Marker { get; set; }
        public ColShape ColShape { get; set; }

        private bool CornerBusy { get; set; }
        private CharacterEntity SellingCharacter { get; set; }
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
                entity.Notify("Handel na rogu możliwy jest od 16 do 23.", NotificationType.Warning);
                return;
            }

            if (!CornerBusy)
            {
                SellingCharacter = entity.GetAccountEntity().CharacterEntity;

                CornerBusy = true;
                SellingCharacter.CurrentInteractive = this;

                SellingCharacter.Notify("Rozpocząłeś proces sprzedaży, pozostań w znaczniku.", NotificationType.Info);

                StartProcess();
                SellingCharacter.AccountEntity.Client.PlayAnimation("amb@world_human_drug_dealer_hard@male@idle_a", "idle_a", (int)AnimationFlags.Loop | (int)AnimationFlags.AllowPlayerControl | (int)AnimationFlags.Cancellable);
            }
        }

        private void StartProcess()
        {
            //Przychodzące boty
            Timer timer = new Timer(Utils.RandomRange(90, 180) * 1000);
            timer.Start();

            timer.Elapsed += (sender, args) =>
            {
                timer.Stop();

                int random = Utils.RandomRange(Data.CornerBots.Count);

                CurrentPedEntity = new CornerPedEntity(
                    Data.CornerBots[random].Name,
                    Data.CornerBots[random].PedHash,
                    Data.BotPositions[0],
                    Data.BotPositions.Where(x => x != Data.BotPositions[0]).ToList(),
                    Data.CornerBots[random].DrugType,
                    Data.CornerBots[random].MoneyCount,
                    Data.CornerBots[random].Greeting,
                    Data.CornerBots[random].GoodFarewell,
                    Data.CornerBots[random].BadFarewell,
                    SellingCharacter,
                    Data.CornerBots[random].BotId);

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
                    entity == SellingCharacter.AccountEntity.Client)
                {
                    timer.Dispose();

                    CornerBusy = false;
                    SellingCharacter.AccountEntity.Client.StopAnimation();

                    if (CurrentPedEntity != null)
                    {
                        CurrentPedEntity.Dispose();
                        CurrentPedEntity = null;
                    }

                    SellingCharacter = null;
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