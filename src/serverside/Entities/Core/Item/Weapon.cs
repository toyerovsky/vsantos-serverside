﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Linq;
using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Serverside.Core.Extensions;

namespace VRP.Serverside.Entities.Core.Item
{
    internal class Weapon : ItemEntity
    {
        public WeaponHash WeaponHash => (WeaponHash)DbModel.FirstParameter.Value;
        public int Ammo => DbModel.SecondParameter.Value;

        /// <summary>
        /// Pierwszy parametr to Hash broni, a drugi to ilość amunicji
        /// </summary>
        public Weapon(ItemModel itemModel) : base(itemModel) { }

        public override void UseItem(CharacterEntity sender)
        {
            if (Ammo <= 0)
            {
                sender.SendWarning("Twoja broń nie ma amunicji.");
                return;
            }

            if (sender.ItemsInUse.Any(item => ReferenceEquals(item, this)))
            {
                DbModel.SecondParameter = NAPI.Player.GetPlayerWeaponAmmo(sender.AccountEntity.Client, WeaponHash);
                Save();
                NAPI.Player.RemovePlayerWeapon(sender.AccountEntity.Client, WeaponHash);

                sender.ItemsInUse.Remove(this);
                
                AccountEntity.AccountLoggedOut -= OnAccountLoggedOut;
            }
            else
            {
                NAPI.Player.GivePlayerWeapon(sender.AccountEntity.Client, WeaponHash, Ammo);
                sender.ItemsInUse.Add(this);

                AccountEntity.AccountLoggedOut += OnAccountLoggedOut;
            }
        }

        private void OnPlayerWeaponSwitch(Client client, WeaponHash oldWeaponHash, WeaponHash newWeaponHash)
        {

        }

        private void OnAccountLoggedOut(Client sender, AccountEntity account)
        {
            if (DbModel.ItemEntityType == ItemEntityType.Weapon)
            {
                if (DbModel.FirstParameter.HasValue)
                    DbModel.SecondParameter = NAPI.Player.GetPlayerWeaponAmmo(sender, (WeaponHash)DbModel.FirstParameter);
                Save();
            }
            AccountEntity.AccountLoggedOut -= OnAccountLoggedOut;
        }
    }
}