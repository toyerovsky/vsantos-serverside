/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Serverside.Core.Scripts;

namespace VRP.Serverside.Entities.Core.Item
{
    internal class Food : ItemEntity
    {
        public int HealthToRestore => DbModel.FirstParameter.Value;
        /// <summary>
        /// Pierwszy parametr to ilość HP do przyznania
        /// </summary>
        /// <param name="events"></param>
        /// <param name="itemModel"></param>
        public Food(ItemModel itemModel) : base(itemModel)
        {
        }

        public override void UseItem(CharacterEntity sender)
        {
            ChatScript.SendMessageToNearbyPlayers(sender, $"zjada {DbModel.Name}", ChatMessageType.ServerMe);
            sender.Health += Convert.ToByte(HealthToRestore);
            Delete();
        }

        public override string UseInfo => $"Ten przedmiot odnawia: {HealthToRestore} punktów życia.";
    }
}