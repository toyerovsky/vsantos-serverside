/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkInternals;
using Serverside.Core.Database.Models;

namespace Serverside.Entities.Core.Item
{
    internal class Cloth : Item
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="events"></param>
        /// <param name="itemModel"></param>
        public Cloth(EventClass events, ItemModel itemModel) : base(events, itemModel) { }

        public override void UseItem(AccountEntity player)
        {

        }

        public override string UseInfo => "Ten przedmiot zmienia ubranie Twojej postaci.";
    }
}