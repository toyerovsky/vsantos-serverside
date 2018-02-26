/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Globalization;
using GTANetworkAPI;
using Serverside.Core.Extensions;

namespace Serverside.Core.Money
{    
    public static class MoneyManager
    {
        private delegate void MoneyChangedEventHandler(Client sender);

        private static event MoneyChangedEventHandler MoneyChanged;

        static MoneyManager()
        {
            MoneyChanged += RPMoney_MoneyChanged;
        }

        private static void RPMoney_MoneyChanged(Client sender)
        {
            NAPI.ClientEvent.TriggerClientEvent(sender, "MoneyChanged", sender.GetAccountEntity().CharacterEntity.DbModel.Money.ToString(CultureInfo.InvariantCulture));
        }

        public static bool HasMoney(Client sender, decimal count, bool bank = false)
        {
            var player = sender.GetAccountEntity();
            if (bank) return player.CharacterEntity.DbModel.BankMoney >= count;
            return player.CharacterEntity.DbModel.Money >= count;
        }

        public static void AddMoney(Client sender, decimal count, bool bank = false)
        {
            var player = sender.GetAccountEntity();
            if (bank)
            {
                player.CharacterEntity.DbModel.BankMoney += count;
            }
            else
            {
                player.CharacterEntity.DbModel.Money += count;
                MoneyChanged?.Invoke(sender);
            }
            player.CharacterEntity.Save();
        }

        public static void RemoveMoney(Client sender, decimal count, bool bank = false)
        {
            var player = sender.GetAccountEntity();
            if (bank)
            {
                player.CharacterEntity.DbModel.BankMoney -= count;
            }
            else
            {
                player.CharacterEntity.DbModel.Money -= count;
                MoneyChanged?.Invoke(sender);
            }
            player.CharacterEntity.Save();
        }
    }
}