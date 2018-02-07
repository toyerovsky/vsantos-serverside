/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using GTANetworkInternals;
using Serverside.Core.Database.Models;
using Serverside.Core.Enums;
using Serverside.Core.Extensions;
using Serverside.Core.Scripts;
using Serverside.Entities.Core;

namespace Serverside.Items
{
    internal class Watch : Item
    {
        /// <summary>
        /// Brak parametrów
        /// </summary>
        /// <param name="events"></param>
        /// <param name="itemModel"></param>
        public Watch(EventClass events, ItemModel itemModel) : base(events, itemModel) { }

        public override void UseItem(AccountEntity player)
        {
            ChatScript.SendMessageToNearbyPlayers(player.Client, $"spogląda na zegarek {DbModel.Name}", ChatMessageType.Me);
            player.Client.Notify($"Godzina: {DateTime.Now.ToShortTimeString()}");
        }

        public override string UseInfo => "Ten przedmiot pokazuje bieżącą godzinę.";
    }
}