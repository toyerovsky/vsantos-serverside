﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkAPI;
using Serverside.Core;
using Serverside.Core.Extensions;

namespace Serverside.Economy.Money
{
    sealed class MoneyScript : Script
    {
        [Command("plac", "~y~UŻYJ: ~w~ /plac [id] [kwota]", Alias = "pay")]
        public void TransferWalletMoney(Client sender, int id, decimal safeMoneyCount)
        {
            if (!ValidationHelper.IsMoneyValid(safeMoneyCount))
            {
                sender.Notify("Podano kwotę gotówki w nieprawidłowym formacie.");
            }

            if (!sender.GetAccountEntity().CharacterEntity.CanPay) return;

            if (!sender.HasMoney(safeMoneyCount))
            {
                sender.Notify("Nie posiadasz wystarczającej ilości gotówki.");
                return;
            }

            if (sender.GetAccountEntity().ServerId == id)
            {
                sender.Notify("Nie możesz podać gotówki samemu sobie.");
                return;
            }
            
            Client gettingPlayer = NAPI.Player.GetPlayersInRadiusOfPlayer(6f, sender).Find(
                x => x.GetAccountEntity().ServerId == id);
            if (gettingPlayer == null)
            {
                sender.Notify("Nie znaleziono gracza o podanym Id");
                return;
            }

            //temu zabieramy
            sender.RemoveMoney(safeMoneyCount);

            //temu dodajemy gotówke
            gettingPlayer.AddMoney(safeMoneyCount);

            sender.SendChatMessage($"~g~Osoba {gettingPlayer.Name} otrzymała od ciebie ${safeMoneyCount}.");
            gettingPlayer.SendChatMessage($"~g~Osoba {sender.Name} przekazała ci ${safeMoneyCount}.");
        }
    }
}