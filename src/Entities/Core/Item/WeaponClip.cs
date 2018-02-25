/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkAPI;
using GTANetworkInternals;
using Serverside.Core.Database.Models;

namespace Serverside.Entities.Core.Item
{
    internal class WeaponClip : Item
    {
        public WeaponHash MatchWeaponHash => (WeaponHash)DbModel.FirstParameter.Value;
        public int Ammo => DbModel.SecondParameter.Value;

        /// <summary>
        /// Pierwszy parametr to hash broni do której pasuje, a drugi to ilość amunicji
        /// </summary>
        /// <param name="events"></param>
        /// <param name="itemModel"></param>
        public WeaponClip(ItemModel itemModel) : base(itemModel) { }

        public override void UseItem(AccountEntity player)
        {
        }

        public override string UseInfo => $"Ten przedmiot dodaje {Ammo} naboi do broni {MatchWeaponHash}.";
    }
}