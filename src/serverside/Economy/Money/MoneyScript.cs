/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using GTANetworkAPI;
using VRP.Core.Validators;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Economy.Money
{
    sealed class MoneyScript : Script
    {
        [Command("plac", "~y~UŻYJ: ~w~ /plac [id] [kwota]", Alias = "pay")]
        public void TransferWalletMoney(Client sender, int id, decimal safeMoneyCount)
        {
            MoneyValidator validator = new MoneyValidator();
            if (!validator.IsValid(safeMoneyCount))
            {
                sender.SendError("Podano kwotę gotówki w nieprawidłowym formacie.");
            }

            CharacterEntity sendingCharacter = sender.GetAccountEntity().CharacterEntity;
            if (!sendingCharacter.CanPay) return;

            if (!sendingCharacter.HasMoney(safeMoneyCount))
            {
                sender.SendWarning("Nie posiadasz wystarczającej ilości gotówki.");
                return;
            }

            if (sender.GetAccountEntity().ServerId == id)
            {
                sender.SendError("Nie możesz podać gotówki samemu sobie.");
                return;
            }
            
            CharacterEntity gettingPlayer = NAPI.Player.GetPlayersInRadiusOfPlayer(6f, sender).Find(
                x => x.GetAccountEntity().ServerId == id).GetAccountEntity().CharacterEntity;

            if (gettingPlayer == null)
            {
                sender.SendWarning("Nie znaleziono gracza o podanym Id");
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