/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Linq;
using GTANetworkAPI;
using Serverside.Constant;
using Serverside.Core.Enums;
using Serverside.Core.Extensions;
using Serverside.Entities;

namespace Serverside.Core.Scripts
{
    public class MiscCommandsScript : Script
    {
        public MiscCommandsScript()
        {
            Event.OnResourceStart += API_onResourceStart;
        }

        private void API_onResourceStart()
        {
            Tools.ConsoleOutput($"[{nameof(MiscCommandsScript)}] {ConstantMessages.ResourceStartMessage}", ConsoleColor.DarkMagenta);
        }

        [Command("id", "~y~UŻYJ ~w~ /id [nazwa]", GreedyArg = true)]
        public void ShowPlayersWithSimilarName(Client sender, string name)
        {
            if (!EntityManager.GetAccounts().Any(x => x.Value.CharacterEntity.FormatName.ToLower().StartsWith(name)))
            {
                sender.Notify("Nie znaleziono gracza o podanej nazwie.");
                return;
            }

            var accounts = EntityManager.GetAccounts()
                .Where(x => x.Value.CharacterEntity.FormatName.ToLower().StartsWith(name));

            ChatScript.SendMessageToPlayer(sender, "Znalezieni gracze: ", ChatMessageType.ServerInfo);
            foreach (var account in accounts)
            {
                ChatScript.SendMessageToPlayer(sender, $"({account.Value.ServerId}) {account.Value.CharacterEntity.FormatName}", ChatMessageType.ServerInfo);
            }
        }

        [Command("pokaz", "~y~ UŻYJ ~w~ /pokaz [typ] [id]")]
        public void Show(Client sender, ShowType type, int id)
        {
            if (NAPI.Player.GetPlayersInRadiusOfPlayer(6f, sender).All(x => x.GetAccountEntity().ServerId != id))
            {
                sender.Notify("W twoim otoczeniu nie znaleziono gracza o podanym Id.");
                return;
            }

            var player = sender.GetAccountEntity();
            var getter = NAPI.Player.GetPlayersInRadiusOfPlayer(6f, sender)
                .Single(x => x.GetAccountEntity().ServerId == id);

            if (type == ShowType.IdCard)
            {
                ChatScript.SendMessageToNearbyPlayers(player.Client, $"pokazuje dowód osobisty {getter.Name}", ChatMessageType.ServerMe);
                getter.Notify($"Osoba {player.CharacterEntity.FormatName} pokazała Ci swój dowód osobisty.");
            }
            else if (type == ShowType.DrivingLicense)
            {
                ChatScript.SendMessageToNearbyPlayers(player.Client, $"pokazuje prawo jazdy {getter.Name}", ChatMessageType.ServerMe);
                getter.Notify($"Osoba {player.CharacterEntity.FormatName} pokazała Ci swoje prawo jazdy.");
            }
        }
    }
}