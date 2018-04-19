/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using GTANetworkAPI;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Economy.Jobs.Base;
using VRP.Serverside.Economy.Jobs.Dustman.Models;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Economy.Jobs.Dustman
{
    public class DustmanWorker : JobWorkerController
    {
        private List<GarbageModel> NonVisitedPoints { get; set; }
        private bool InProgress { get; set; }
        private int Count { get; set; }

        private DateTime NextUpdate { get; set; }

        private ColShape CurrentGarbageColshape { get; set; }

        public DustmanWorker(AccountEntity player, JobVehicleEntity vehicle)
            : base(player, vehicle)
        {
            Player = player;
            InProgress = true;
            NonVisitedPoints = JobsScript.Garbages;
            JobVehicle = vehicle;
        }

        private void OnUpdate()
        {
            if (!Player.Client.IsInVehicle && DateTime.Now >= NextUpdate && Player.Client.Position.DistanceTo2D(JobVehicle.GameVehicle.Position) > 25)
            {
                Player.Client.SendWarning("Oddaliłeś się od swojego pojazdu. Praca została przerwana.");
                Stop();
                JobVehicle.Respawn();
            }
        }

        public override void Start()
        {
            GarbageModel v = NonVisitedPoints[new Random().Next(NonVisitedPoints.Count)];
            NonVisitedPoints.Remove(v);

            NAPI.ClientEvent.TriggerClientEvent(Player.Client, "DrawJobComponents", v, 318);
            CurrentGarbageColshape = NAPI.ColShape.CreateCylinderColShape(v.Position, 2f, 3f);
            CurrentGarbageColshape.OnEntityEnterColShape += CurrentShapeOnEntityEnterColShape;
        }

        public override void Stop()
        {
            InProgress = false;
            NonVisitedPoints = null;
            NAPI.ColShape.DeleteColShape(CurrentGarbageColshape);
            CurrentGarbageColshape = null;
            NAPI.ClientEvent.TriggerClientEvent(Player.Client, "DisposeJobComponents");
        }

        private void CurrentShapeOnEntityEnterColShape(ColShape shape, Client entity)
        {
            if (entity == Player.Client && Player.Client.Position.DistanceTo2D(JobVehicle.GameVehicle.Position) <= 25 && !Player.Client.IsInVehicle && Count < 10)
            {
                //Dodać animację
                Count++;
                Player.Client.SendInfo($"Pomyślnie rozładowano kontener. Zapełnienie: {Count}/10.");
                NAPI.ColShape.DeleteColShape(shape);
                NAPI.Player.PlaySoundFrontEnd(NAPI.Player.GetPlayerFromHandle(entity), "CHECKPOINT_NORMAL", "HUD_MINI_GAME_SOUNDSET");
                DrawNextPoint(false);
            }
            else if (entity == Player.Client && Player.Client.Position.DistanceTo2D(JobVehicle.GameVehicle.Position) <= 25 && !Player.Client.IsInVehicle && Count == 10)
            {
                Player.Client.SendInfo("Śmieciarka została zapełniona udaj się na wysypisko, aby ją rozładować.");
                NAPI.Player.PlaySoundFrontEnd(NAPI.Player.GetPlayerFromHandle(entity), "CHECKPOINT_NORMAL", "HUD_MINI_GAME_SOUNDSET");
                DrawNextPoint(true);
                InProgress = false;
            }
            else if (!InProgress)
            {
                CharacterEntity characterEntity = Player.CharacterEntity;
                for (int i = 0; i < Count; i++)
                {
                    characterEntity.DbModel.MoneyJob += 25;
                }
                characterEntity.Save();
                Player.Client.SendInfo(
                    $"Zakończyłeś pracę operatora śmieciarki, zarobiłeś: ${characterEntity.DbModel.MoneyJob}.");
            }
            else
            {
                Player.Client.SendInfo("Twoja śmieciarka jest za daleko, aby można było pomyślnie załadować kontener.");
            }
        }

        private void DrawNextPoint(bool end)
        {
            NAPI.ClientEvent.TriggerClientEvent(Player.Client, "DisposeJobComponents");
            if (end)
            {
                NAPI.ClientEvent.TriggerClientEvent(Player.Client, "DrawJobComponents", JobsScript.GetRandomGarbage().Position, 318);
                CurrentGarbageColshape = NAPI.ColShape.CreateCylinderColShape(JobsScript.GetRandomGarbage().Position, 2f, 3f);
                CurrentGarbageColshape.OnEntityEnterColShape += CurrentShapeOnEntityEnterColShape;
                return;
            }

            GarbageModel v = NonVisitedPoints[new Random().Next(NonVisitedPoints.Count)];
            NonVisitedPoints.Remove(v);

            NAPI.ClientEvent.TriggerClientEvent(Player.Client, "DrawJobComponents", v, 318);
            CurrentGarbageColshape = NAPI.ColShape.CreateCylinderColShape(v.Position, 2f, 3f);

            CurrentGarbageColshape.OnEntityEnterColShape += CurrentShapeOnEntityEnterColShape;
        }

        public void Redraw(Vector3 lastPosition)
        {
            CurrentGarbageColshape = NAPI.ColShape.CreateCylinderColShape(lastPosition, 2f, 3f);
            CurrentGarbageColshape.OnEntityEnterColShape += CurrentShapeOnEntityEnterColShape;

            NAPI.ClientEvent.TriggerClientEvent(Player.Client, "DrawJobComponents", lastPosition, 318);
        }

        public Vector3 GetLastPoint() => CurrentGarbageColshape.Position;
    }
}