/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Core.Scripts;

namespace VRP.Serverside.Entities.Core.Item
{
    internal class Watch : ItemEntity
    {
        /// <summary>
        /// Brak parametrów
        /// </summary>
        /// <param name="itemModel"></param>
        public Watch(ItemModel itemModel) : base(itemModel) { }

        public override void UseItem(AccountEntity player)
        {
            ChatScript.SendMessageToNearbyPlayers(player.Client, $"spogląda na zegarek {DbModel.Name}", ChatMessageType.Me);
            player.Client.Notify($"Godzina: {DateTime.Now.ToShortTimeString()}");
        }

        public override string UseInfo => "Ten przedmiot pokazuje bieżącą godzinę.";
    }
}