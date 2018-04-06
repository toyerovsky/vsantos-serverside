/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkAPI;
using VRP.Core.Enums;
using VRP.Core.Tools;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Economy.Money
{
    sealed class MoneyScript : Script
    {
        [Command("plac", "~y~UŻYJ: ~w~ /plac [id] [kwota]", Alias = "pay")]
        public void TransferWalletMoney(Client sender, int id, decimal safeMoneyCount)
        {
            if (!ValidationHelper.IsMoneyValid(safeMoneyCount))
            {
                sender.Notify("Podano kwotę gotówki w nieprawidłowym formacie.", NotificationType.Error);
            }

            CharacterEntity sendingCharacter = sender.GetAccountEntity().CharacterEntity;
            if (!sendingCharacter.CanPay) return;

            if (!sendingCharacter.HasMoney(safeMoneyCount))
            {
                sender.Notify("Nie posiadasz wystarczającej ilości gotówki.", NotificationType.Warning);
                return;
            }

            if (sender.GetAccountEntity().ServerId == id)
            {
                sender.Notify("Nie możesz podać gotówki samemu sobie.", NotificationType.Error);
                return;
            }
            
            CharacterEntity gettingPlayer = NAPI.Player.GetPlayersInRadiusOfPlayer(6f, sender).Find(
                x => x.GetAccountEntity().ServerId == id).GetAccountEntity().CharacterEntity;

            if (gettingPlayer == null)
            {
                sender.Notify("Nie znaleziono gracza o podanym Id", NotificationType.Warning);
                return;
            }

            //temu zabieramy
            sendingCharacter.RemoveMoney(safeMoneyCount);

            //temu dodajemy gotówke
            gettingPlayer.AddMoney(safeMoneyCount);

            sender.SendChatMessage($"~g~Osoba {gettingPlayer.FormatName} otrzymała od ciebie ${safeMoneyCount}.");
            gettingPlayer.AccountEntity.Client.SendChatMessage($"~g~Osoba {sender.Name} przekazała ci ${safeMoneyCount}.");
        }
    }
}