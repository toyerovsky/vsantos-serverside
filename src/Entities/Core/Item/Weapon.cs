/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Linq;
using GTANetworkAPI;
using GTANetworkInternals;
using Serverside.Core.Database.Models;
using Serverside.Core.Extensions;

namespace Serverside.Entities.Core.Item
{
    internal class Weapon : Item
    {
        public WeaponHash WeaponHash => (WeaponHash)DbModel.FirstParameter.Value;
        public int Ammo => DbModel.SecondParameter.Value;

        /// <summary>
        /// Pierwszy parametr to Hash broni, a drugi to ilość amunicji
        /// </summary>
        public Weapon(ItemModel itemModel) : base(itemModel) { }

        public override void UseItem(AccountEntity player)
        {
            if (Ammo <= 0)
            {
                player.Client.Notify("Twoja broń nie ma amunicji.");
                return;
            }

            if (player.CharacterEntity.ItemsInUse.Any(item => ReferenceEquals(item, this)))
            {
                DbModel.SecondParameter = NAPI.Player.GetPlayerWeaponAmmo(player.Client, WeaponHash);
                Save();
                NAPI.Player.RemovePlayerWeapon(player.Client, WeaponHash);

                player.CharacterEntity.ItemsInUse.Remove(this);
                
                AccountEntity.AccountLoggedOut -= OnAccountLoggedOut;
            }
            else
            {
                NAPI.Player.GivePlayerWeapon(player.Client, WeaponHash, Ammo);
                player.CharacterEntity.ItemsInUse.Add(this);

                AccountEntity.AccountLoggedOut += OnAccountLoggedOut;
            }
        }

        private void OnPlayerWeaponSwitch(Client client, WeaponHash oldWeaponHash, WeaponHash newWeaponHash)
        {

        }

        private void OnAccountLoggedOut(Client sender, AccountEntity account)
        {
            if (DbModel.ItemType == ItemType.Weapon)
            {
                if (DbModel.FirstParameter.HasValue)
                    DbModel.SecondParameter = NAPI.Player.GetPlayerWeaponAmmo(sender, (WeaponHash)DbModel.FirstParameter);
                Save();
            }
            AccountEntity.AccountLoggedOut -= OnAccountLoggedOut;
        }
    }
}