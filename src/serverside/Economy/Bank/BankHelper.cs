/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using GTANetworkAPI;
using VRP.Core.Enums;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Economy.Bank
{
    public static class BankHelper
    {
        public static void DepositMoney(Client player, decimal count)
        {
            CharacterEntity character = player.GetAccountEntity().CharacterEntity;
            if (character.HasMoney(count))
            {
                character.RemoveMoney(count);
                character.AddMoney(count, true);

                player.SendInfo($"Wpłacono ${count} na konto o numerze {player.GetAccountEntity().CharacterEntity.DbModel.BankAccountNumber}");
            }
            else
            {
                player.SendWarning("Nie posiadasz wystarczającej ilości gotówki.");
            }
        }

        public static void WithdrawMoney(Client player, decimal count)
        {
            CharacterEntity character = player.GetAccountEntity().CharacterEntity;
            if (character.HasMoney(count, true))
            {
                character.RemoveMoney(count, true);
                character.AddMoney(count);

                player.SendInfo($"Wypłacono ${count} z konta o numerze {player.GetAccountEntity().CharacterEntity.DbModel.BankAccountNumber}");
            }
            else
            {
                player.SendWarning("Nie posiadasz wystarczającej ilości środków na koncie bankowym.");
            }
        }
    }
}