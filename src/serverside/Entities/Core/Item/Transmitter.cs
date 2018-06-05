/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using VRP.Core.Database.Models.Item;

namespace VRP.Serverside.Entities.Core.Item
{
    internal class Transmitter : ItemEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemModel"></param>
        public Transmitter(ItemModel itemModel) : base(itemModel) { }

        public override void UseItem(CharacterEntity player)
        {
        }

        public override string UseInfo => $"Przedmiot służy do komunikacji na podanej częstotliwości w zasięgu: {DbModel.SecondParameter}";
    }
}