﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Serverside.Core.Enums;
using Serverside.Core.Extensions;
using Serverside.Entities;
using Serverside.Entities.Core;

namespace Serverside.Core.Scripts
{
    public class MiscCommandsScript : Script
    {
        [Command("id", "~y~UŻYJ ~w~ /id [nazwa]", GreedyArg = true)]
        public void ShowPlayersWithSimilarName(Client sender, string name)
        {
            if (!EntityHelper.GetAccounts().Any(x => x.Value.CharacterEntity.FormatName.ToLower().StartsWith(name)))
            {
                sender.Notify("Nie znaleziono gracza o podanej nazwie.");
                return;
            }

            IEnumerable<KeyValuePair<long, AccountEntity>> accounts = EntityHelper.GetAccounts()
                .Where(x => x.Value.CharacterEntity.FormatName.ToLower().StartsWith(name));

            ChatScript.SendMessageToPlayer(sender, "Znalezieni gracze: ", ChatMessageType.ServerInfo);
            foreach (KeyValuePair<long, AccountEntity> account in accounts)
            {
                ChatScript.SendMessageToPlayer(sender, $"({account.Value.ServerId}) {account.Value.CharacterEntity.FormatName}", ChatMessageType.ServerInfo);
            }
        }

        [Command("pokaz", "~y~ UŻYJ ~w~ /pokaz [dowod/prawko] [id]")]
        public void Show(Client sender, string type, int id)
        {
            if (NAPI.Player.GetPlayersInRadiusOfPlayer(6f, sender).All(x => x.GetAccountEntity().ServerId != id))
            {
                sender.Notify("W twoim otoczeniu nie znaleziono gracza o podanym Id.");
                return;
            }

            AccountEntity player = sender.GetAccountEntity();
            Client getter = NAPI.Player.GetPlayersInRadiusOfPlayer(6f, sender)
                .Single(x => x.GetAccountEntity().ServerId == id);

            if (type.ToLower().Trim() == ShowType.IdCard.GetDescription())
            {
                ChatScript.SendMessageToNearbyPlayers(player.Client, $"pokazuje dowód osobisty {getter.Name}", ChatMessageType.ServerMe);
                getter.Notify($"Osoba {player.CharacterEntity.FormatName} pokazała Ci swój dowód osobisty.");
            }
            else if (type.ToLower().Trim() == ShowType.DrivingLicense.GetDescription())
            {
                ChatScript.SendMessageToNearbyPlayers(player.Client, $"pokazuje prawo jazdy {getter.Name}", ChatMessageType.ServerMe);
                getter.Notify($"Osoba {player.CharacterEntity.FormatName} pokazała Ci swoje prawo jazdy.");
            }
        }
    }
}