/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Database.Models.Item;


namespace VRP.Serverside.Entities.Core.Item
{
    internal class WeaponClip : ItemEntity
    {
        public WeaponHash MatchWeaponHash => (WeaponHash)DbModel.FirstParameter.Value;
        public int Ammo => DbModel.SecondParameter.Value;

        /// <summary>
        /// Pierwszy parametr to hash broni do której pasuje, a drugi to ilość amunicji
        /// </summary>
        /// <param name="events"></param>
        /// <param name="itemModel"></param>
        public WeaponClip(ItemModel itemModel) : base(itemModel) { }

        public override void UseItem(CharacterEntity player)
        {
        }

        public override string UseInfo => $"Ten przedmiot dodaje {Ammo} naboi do broni {MatchWeaponHash}.";
    }
}