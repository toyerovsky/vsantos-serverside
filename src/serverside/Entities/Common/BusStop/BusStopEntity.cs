/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Timers;
using GTANetworkAPI;
using VRP.Core.Enums;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Core.Scripts;
using VRP.Serverside.Entities.Base;
using VRP.Serverside.Entities.Common.BusStop.Models;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Entities.Interfaces;

namespace VRP.Serverside.Entities.Common.BusStop
{
    public class BusStopEntity : GameEntity, IInteractive
    {
        public BusStopModel Data { get; set; }

        public ColShape ColShape { get; private set; }
        public TextLabel NameLabel { get; set; }

        public BusStopEntity(BusStopModel data)
        {
            Data = data;
        }

        public override void Spawn()
        {
            NameLabel = NAPI.TextLabel.CreateTextLabel($"~y~PRZYSTANEK~w~\n {Data.Name}", Data.Center, 15f, 0.64f, 1, new Color(255, 255, 255));
            ColShape = NAPI.ColShape.CreateSphereColShape(Data.Center, 5f);
            ColShape.OnEntityEnterColShape += OnEntityEnterColShape;
            ColShape.OnEntityExitColShape += OnEntityExitColShape;
        }

        private void OnEntityEnterColShape(ColShape shape, Client entity)
        {
            entity.SetData("Bus", this);
        }

        private void OnEntityExitColShape(ColShape shape, Client entity)
        {
            entity.ResetData("Bus");
        }

        public static void StartTransport(CharacterEntity player, decimal cost, int seconds, Vector3 position, string name)
        {
            if (player.DbModel.PlayedTime.Hours > 5 && !player.HasMoney(cost))
            {
                player.Notify("Nie posiadasz wystarczającej ilości gotówki.", NotificationType.Error);
                return;
            }

            player.RemoveMoney(cost);

            // TODO: Zaciemnianie ekranu
            // NAPI.Native.SendNativeToPlayer(player, Hash.DO_SCREEN_FADE_OUT, 2000);

            ChatScript.SendMessageToNearbyPlayers(player,
                $"wsiadł do autobusu zmierzającego w stronę {name}", ChatMessageType.ServerMe);

            player.Dimension = ((uint)Dimension.Bus);

            // Teleport po danym czasie
            Timer busTimer = new Timer(seconds * 1000);
            busTimer.Start();
            busTimer.Elapsed += (sender, args) =>
            {
                player.Position = position;
                // TODO: Odciemnianie ekranu
                // NAPI.Native.SendNativeToPlayer(player, Hash.DO_SCREEN_FADE_IN, 2000); //Odciemnianie ekranu
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
            ColShape.OnEntityEnterColShape -= OnEntityEnterColShape;
            ColShape.OnEntityExitColShape -= OnEntityExitColShape;
            NAPI.ColShape.DeleteColShape(ColShape);
        }
    }
}
