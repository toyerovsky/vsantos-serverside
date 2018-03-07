/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using Serverside.Core.Database.Models;
using Serverside.Core.Enums;
using Serverside.Core.Extensions;
using Serverside.Core.Scripts;

namespace Serverside.Entities.Core.Item
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