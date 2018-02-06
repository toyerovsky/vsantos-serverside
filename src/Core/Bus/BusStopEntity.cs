﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using Serverside.Core.Bus.Models;
using System.Timers;
using GTANetworkAPI;
using GTANetworkInternals;
using Serverside.Core.Enums;
using Serverside.Core.Extensions;
using Serverside.Core.Scripts;
using Serverside.Entities.Base;

namespace Serverside.Core.Bus
{
    public class BusStopEntity : GameEntity
    {
        public BusStopModel Data { get; set; }
        public ColShape BusStopShape { get; set; }
        public TextLabel NameLabel { get; set; }

        public BusStopEntity(EventClass events, BusStopModel data)
            : base(events)
        {
            Data = data;
            //Tworzymy napis na przystanku
            NameLabel = NAPI.TextLabel.CreateTextLabel($"~y~PRZYSTANEK~w~\n {data.Name}", Data.Center, 15f, 0.64f, 1, new Color(255, 255, 255));
            BusStopShape = NAPI.ColShape.CreateSphereColShape(data.Center, 5f);
            BusStopShape.OnEntityEnterColShape += OnEntityEnterColShape;
            BusStopShape.OnEntityExitColShape += OnEntityExitColShape;
        }

        private void OnEntityEnterColShape(ColShape shape, Client entity)
        {
            entity.SetData("Bus", this);
        }

        private void OnEntityExitColShape(ColShape shape, Client entity)
        {
            entity.ResetData("Bus");
        }

        public static void StartTransport(Client player, decimal cost, int seconds, Vector3 position, string name)
        {
            //TODO: Przez pierwsze 5h autobus za darmo dla nowych graczy
            if (!player.HasMoney(cost))
            {
                player.Notify("Nie posiadasz wystarczającej ilości gotówki.");
                return;
            }

            player.RemoveMoney(cost);

            //Zaciemnianie ekranu
            NAPI.Native.SendNativeToPlayer(player, Hash.DO_SCREEN_FADE_OUT, 2000);

            ChatScript.SendMessageToNearbyPlayers(player,
                $"wsiadł do autobusu zmierzającego w stronę {name}", ChatMessageType.ServerMe);

            player.Dimension = (uint)Dimension.Bus;

            //Teleport po danym czasie
            Timer busTimer = new Timer(seconds * 1000);
            busTimer.Start();
            busTimer.Elapsed += (sender, args) =>
            {
                player.Position = position;
                NAPI.Native.SendNativeToPlayer(player, Hash.DO_SCREEN_FADE_IN, 2000); //Odciemnianie ekranu
                player.Dimension = (uint)Dimension.Global;
                ChatScript.SendMessageToNearbyPlayers(player, "wysiadł z autobusu", ChatMessageType.ServerMe);

                busTimer.Stop();
                busTimer.Dispose();
            };

            //TODO: Nie działa - Wyświatla się pod zaciemnionym ekranem
            //Timer shardTimer = new Timer(1000);
            //shardTimer.Start();

            ////Odliczanie czasu do przyjazdu
            //shardTimer.Elapsed += (sender, args) =>
            //{
            //    if (DateTime.Now == arrivalTime)
            //    {
            //        shardTimer.Stop();
            //        shardTimer.Dispose();
            //    }

            //    string secondsToShow = (arrivalTime - DateTime.Now).Seconds.ToString();
            //    if (secondsToShow.Length == 1) secondsToShow = "0" + secondsToShow;
            //    player.TriggerEvent("BWTimerTick",
            //        $"{(arrivalTime - DateTime.Now).Minutes}:{secondsToShow}");
            //};
        }

        public override void Dispose()
        {
            BusStopShape.OnEntityEnterColShape -= OnEntityEnterColShape;
            BusStopShape.OnEntityExitColShape -= OnEntityExitColShape;
            NAPI.ColShape.DeleteColShape(BusStopShape);
        }
    }
}
