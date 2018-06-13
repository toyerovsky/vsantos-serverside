/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using GTANetworkAPI;
using VRP.Core.Enums;
using VRP.Serverside.Interfaces;
using FullPosition = VRP.Serverside.Core.FullPosition;

namespace VRP.Serverside.Entities.Base
{
    public class PedEntity : IGameEntity
    {
        public string Name { get; set; }
        public PedHash PedHash { get; set; }

        protected TextLabel NameLabel { get; set; }
        /// <summary>
        /// Jest to tylko i wyłącznie pozycja spawnu, potem korzystamy z BotHandle.InDbPosition
        /// </summary>
        protected FullPosition SpawnPosition { get; set; }
        protected Ped BotHandle { get; set; }

        public PedEntity(string name, PedHash pedHash, FullPosition spawnPosition)
        {
            Name = name;
            PedHash = pedHash;
            SpawnPosition = spawnPosition;
        }

        public void GoToPoint(Vector3 position)
        {
            NAPI.Native.SendNativeToPlayersInRange(BotHandle.Position, 100, Hash.TASK_GO_STRAIGHT_TO_COORD, BotHandle.Handle, position.X, position.Y, position.Z, 2, -1);

            //Zakładamy, że bot idzie z predkoscia 1.20 j/s
            BotHandle.MovePosition(position, (int)(BotHandle.Position.DistanceTo(position) / 1.20) * 1000);
        }

        /// <summary>
        /// Metoda do incjalizacji bota w grze
        /// </summary>
        public virtual void Spawn()
        {
            BotHandle = NAPI.Ped.CreatePed(PedHash, SpawnPosition.Position, 1f);
            NAPI.Entity.SetEntityRotation(BotHandle, SpawnPosition.Rotation);
            NameLabel = NAPI.TextLabel.CreateTextLabel(Name, new Vector3(SpawnPosition.Position.X, SpawnPosition.Position.Y, SpawnPosition.Position.Z + 1), 10f, 0.7f, 1, new Color(255, 255, 255), false, BotHandle.Dimension);
            NameLabel.AttachTo(BotHandle, "SKEL_Head", new Vector3(0f, 0f, 1f), BotHandle.Rotation);
        }

        protected void SendMessageToNerbyPlayers(string message, ChatMessageType chatMessageType)
        {
            List<Client> players = NAPI.Player.GetPlayersInRadiusOfPosition((int)chatMessageType, BotHandle.Position);

            string color = null;

            if (chatMessageType == ChatMessageType.Normal)
            {
                message = Name + " mówi: " + message;
                color = "~#FFFFFF~";
            }
            else if (chatMessageType == ChatMessageType.Quiet)
            {
                message = Name + " szepcze: " + message;
                color = "~#FFFFFF~";
            }
            else if (chatMessageType == ChatMessageType.Loud)
            {
                message = Name + " krzyczy: " + message + "!";
                color = "~#FFFFFF~";
            }
            else if (chatMessageType == ChatMessageType.Me)
            {
                message = "** " + Name + " " + message;
                color = "~#C2A2DA~";
            }
            else if (chatMessageType == ChatMessageType.ServerMe)
            {
                message = "* " + Name + " " + message;
                color = "~#C2A2DA~";
            }
            else if (chatMessageType == ChatMessageType.Do)
            {
                message = "** " + message + " (( " + Name + " )) **";
                color = "~#847DB7~";
            }

            foreach (Client player in players)
            {
                NAPI.Chat.SendChatMessageToPlayer(player, color, message);
            }
        }

        public virtual void Dispose()
        {
            NAPI.Entity.DeleteEntity(BotHandle);
            NAPI.Entity.DeleteEntity(NameLabel);
        }
    }
}
