/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using GTANetworkInternals;
using Serverside.Core;
using Serverside.Core.Database.Models;
using Serverside.Core.Enums;
using Serverside.Core.Scripts;

namespace Serverside.Entities.Core.Item
{
    internal class Dice : Item
    {   
        /// <summary>
        /// Pierwszy parametr to liczba oczek na kostce
        /// </summary>
        /// <param name="itemModel"></param>
        public Dice(ItemModel itemModel) : base(itemModel) { }

        public override void UseItem(AccountEntity player)
        {
            if (DbModel.FirstParameter != null)
                ChatScript.SendMessageToNearbyPlayers(player.Client,
                    $"wyrzucił {Tools.RandomInt(1, DbModel.FirstParameter.Value)} oczek z {DbModel.FirstParameter} możliwych",
                    ChatMessageType.ServerMe);
        }

        public override string UseInfo => $"Ten przedmiot zwraca losową liczbę od 1 do {DbModel.FirstParameter}";
    }
}