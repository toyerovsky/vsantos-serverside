/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkAPI;
using GTANetworkInternals;
using Serverside.Core.Database.Models;
using Serverside.Core.Extensions;

namespace Serverside.Entities.Core.Item
{
    internal class Weapon : Item
    {
        /// <summary>
        /// Pierwszy parametr to Hash broni, a drugi to ilość amunicji
        /// </summary>
        public Weapon(EventClass events, ItemModel itemModel) : base(events, itemModel) { }

        public override void UseItem(AccountEntity player)
        {
            if (DbModel.SecondParameter <= 0)
            {
                player.Client.Notify("Twoja broń nie ma amunicji.");
                return;
            }

            if (DbModel.CurrentlyInUse && DbModel.FirstParameter.HasValue)
            {
                DbModel.CurrentlyInUse = false;
                DbModel.SecondParameter = NAPI.Player.GetPlayerWeaponAmmo(player.Client, (WeaponHash)DbModel.FirstParameter);
                Save();
                NAPI.Player.RemovePlayerWeapon(player.Client, (WeaponHash)DbModel.FirstParameter);
                Events.OnPlayerDisconnected -= API_OnPlayerDisconnected;
            }
            else if (!DbModel.CurrentlyInUse && DbModel.SecondParameter > 0)
            {
                //NAPI.givePlayerWeapon(Client player, WeaponHash weaponHash, int ammo, bool equipNow, bool ammoLoaded);
                if (DbModel.FirstParameter.HasValue)
                    NAPI.Player.GivePlayerWeapon(player.Client, (WeaponHash)DbModel.FirstParameter, DbModel.SecondParameter.Value);

                DbModel.CurrentlyInUse = true;
                Save();

                Events.OnPlayerDisconnected += API_OnPlayerDisconnected;
            }
        }

        private void API_OnPlayerDisconnected(Client sender, byte type, string reason)
        {
            if (DbModel.ItemType == ItemType.Weapon)
            {
                if (DbModel.FirstParameter.HasValue)
                    DbModel.SecondParameter = NAPI.Player.GetPlayerWeaponAmmo(sender, (WeaponHash)DbModel.FirstParameter);
                Save();
            }
            Events.OnPlayerDisconnected -= API_OnPlayerDisconnected;
        }
    }
}