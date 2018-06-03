/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using VRP.Core.Database.Models;
using VRP.Core.Database.Models.Item;

namespace VRP.Serverside.Entities.Core.Item
{
    internal class Drug : ItemEntity
    {
        /// <summary>
        /// Pierwszy parametr to drugtype
        /// </summary>
        /// <param name="itemModel"></param>
        public Drug(ItemModel itemModel) : base(itemModel) { }
    }
}