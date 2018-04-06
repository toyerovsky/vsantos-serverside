/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Core.Tools;
using VRP.Serverside.Core.Scripts;

namespace VRP.Serverside.Entities.Core.Item
{
    internal class Dice : ItemEntity
    {   
        /// <summary>
        /// Pierwszy parametr to liczba oczek na kostce
        /// </summary>
        /// <param name="itemModel"></param>
        public Dice(ItemModel itemModel) : base(itemModel) { }

        public override void UseItem(CharacterEntity sender)
        {
            if (DbModel.FirstParameter != null)
                ChatScript.SendMessageToNearbyPlayers(sender,
                    $"wyrzucił {Utils.RandomRange(1, DbModel.FirstParameter.Value)} oczek z {DbModel.FirstParameter} możliwych",
                    ChatMessageType.ServerMe);
        }

        public override string UseInfo => $"Ten przedmiot zwraca losową liczbę od 1 do {DbModel.FirstParameter}";
    }
}