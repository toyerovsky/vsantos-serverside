/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Globalization;
using VRP.Serverside.Constant.RemoteEvents;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Economy.Money
{
    public static class MoneyManager
    {
        private delegate void MoneyChangedEventHandler(CharacterEntity sender);

        private static event MoneyChangedEventHandler MoneyChanged;

        static MoneyManager()
        {
            MoneyChanged += RPMoney_MoneyChanged;
        }

        private static void RPMoney_MoneyChanged(CharacterEntity sender)
        {
            sender.AccountEntity.Client.TriggerEvent(RemoteEvents.CharacterMoneyChangeRequested, sender.DbModel.Money.ToString(CultureInfo.InvariantCulture));
        }

        public static bool HasMoney(CharacterEntity sender, decimal count, bool bank = false)
        {
            if (bank) return sender.DbModel.BankMoney >= count;
            return sender.DbModel.Money >= count;
        }

        public static void AddMoney(CharacterEntity sender, decimal count, bool bank = false)
        {
            if (bank)
            {
                sender.DbModel.BankMoney += count;
            }
            else
            {
                sender.DbModel.Money += count;
                MoneyChanged?.Invoke(sender);
            }
            sender.Save();
        }

        public static void RemoveMoney(CharacterEntity sender, decimal count, bool bank = false)
        {
            if (bank)
            {
                sender.DbModel.BankMoney -= count;
            }
            else
            {
                sender.DbModel.Money -= count;
                MoneyChanged?.Invoke(sender);
            }
            sender.Save();
        }
    }
}