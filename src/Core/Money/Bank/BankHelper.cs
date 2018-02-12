/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkAPI;
using Serverside.Core.Extensions;

namespace Serverside.Core.Money.Bank
{
    public static class BankHelper
    {
        public static void DepositMoney(Client player, decimal count)
        {
            if (player.HasMoney(count))
            {
                player.RemoveMoney(count);
                player.AddMoney(count, true);

                player.Notify(
                    $"Wpłacono ${count} na konto o numerze {player.GetAccountEntity().CharacterEntity.DbModel.BankAccountNumber}");
            }
            else
            {
                player.Notify("Nie posiadasz wystarczającej ilości gotówki.");
            }
        }

        public static void WithdrawMoney(Client player, decimal count)
        {
            if (player.HasMoney(count, true))
            {
                player.RemoveMoney(count, true);
                player.AddMoney(count);

                player.Notify(
                    $"Wypłacono ${count} z konta o numerze {player.GetAccountEntity().CharacterEntity.DbModel.BankAccountNumber}");
            }
            else
            {
                player.Notify("Nie posiadasz wystarczającej ilości środków na koncie bankowym.");
            }
        }
    }
}