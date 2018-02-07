/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkInternals;
using Serverside.Core.Database.Models;
using Serverside.Entities.Core;

namespace Serverside.Items
{
    internal class WeaponClip : Item
    {
        /// <summary>
        /// Pierwszy parametr to hash broni do której pasuje, a drugi to ilość amunicji
        /// </summary>
        /// <param name="events"></param>
        /// <param name="itemModel"></param>
        public WeaponClip(EventClass events, ItemModel itemModel) : base(events, itemModel) { }

        public override void UseItem(AccountEntity player)
        {
        }

        public override string UseInfo => $"Ten przedmiot dodaje {DbModel.SecondParameter} naboi do broni.";
    }
}