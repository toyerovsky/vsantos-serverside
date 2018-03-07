/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Serverside.Core.Scripts;

namespace VRP.Serverside.Entities.Core.Item
{
    internal class Food : ItemEntity
    {
        /// <summary>
        /// Pierwszy parametr to ilość HP do przyznania
        /// </summary>
        /// <param name="events"></param>
        /// <param name="itemModel"></param>
        public Food(ItemModel itemModel) : base(itemModel)
        {
        }

        public override void UseItem(AccountEntity player)
        {
            ChatScript.SendMessageToNearbyPlayers(player.Client, $"zjada {DbModel.Name}", ChatMessageType.ServerMe);
            player.Client.Health += Convert.ToInt32(DbModel.FirstParameter);
            Delete();
        }

        public override string UseInfo => $"Ten przedmiot odnawia: {DbModel.FirstParameter} punktów życia.";
    }
}