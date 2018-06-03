/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using VRP.Core.Database.Models;
using VRP.Core.Database.Models.Item;
using VRP.Core.Enums;
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

        public override void UseItem(CharacterEntity sender)
        {
            ChatScript.SendMessageToNearbyPlayers(sender, $"spogląda na zegarek {DbModel.Name}", ChatMessageType.Me);
            sender.SendInfo($"Godzina: {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}");
        }

        public override string UseInfo => "Ten przedmiot pokazuje bieżącą godzinę.";
    }
}