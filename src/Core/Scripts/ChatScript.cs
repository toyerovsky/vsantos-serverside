/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Serverside.Core.Enums;
using Serverside.Core.Extensions;
using Serverside.Entities;
using Serverside.Entities.Core;
using Serverside.Groups.Base;
using Serverside.Groups.Enums;

namespace Serverside.Core.Scripts
{
    public class ChatScript : Script
    {
        public ChatScript()
        {
            Event.OnChatMessage += API_onChatMessageHandler;
        }

        public static event SaidEventHandler OnPlayerSaid;

        private void API_onChatMessageHandler(Client sender, string message, CancelEventArgs e)
        {
            e.Cancel = true;
            if (sender.GetAccountEntity() == null || !sender.GetAccountEntity().CharacterEntity.CanTalk) return;

            if (sender.GetAccountEntity().CharacterEntity.CurrentCellphone != null)
                SendMessageToNearbyPlayers(sender, message, sender.GetAccountEntity().CharacterEntity.CurrentCellphone.CurrentCall != null ? ChatMessageType.PhoneOthers : ChatMessageType.Normal);

            SendMessageToNearbyPlayers(sender, message, ChatMessageType.Normal);

            SaidEventArgs eventArgs = new SaidEventArgs(sender, message, ChatMessageType.Normal);
            OnPlayerSaid?.Invoke(this, eventArgs);
        }

        #region PLAYER COMMANDS

        [Command("sprobuj", "~y~UŻYJ: ~w~ /sprobuj [treść]", GreedyArg = true)]
        public void Try(Client player, string message)
        {
            SendMessageToNearbyPlayers(player, new Random().Next(2) == 0 ? "zawiódł " : "odniósł sukces " + " próbując " + message, ChatMessageType.ServerMe);

            SaidEventHandler handler = OnPlayerSaid;
            SaidEventArgs eventArgs = new SaidEventArgs(player, message, ChatMessageType.ServerMe);
            handler?.Invoke(this, eventArgs);
        }

        [Command("c", "~y~UŻYJ: ~w~ /c [treść]", GreedyArg = true)]
        public void SendQuietMessage(Client player, string message)
        {
            SendMessageToNearbyPlayers(player, message, ChatMessageType.Quiet);

            SaidEventHandler handler = OnPlayerSaid;
            SaidEventArgs eventArgs = new SaidEventArgs(player, message, ChatMessageType.Quiet);
            handler?.Invoke(this, eventArgs);
        }

        [Command("k", "~y~UŻYJ: ~w~ /k [treść]", GreedyArg = true)]
        public void SendScreamMessage(Client player, string message)
        {
            SendMessageToNearbyPlayers(player, message, ChatMessageType.Loud);

            SaidEventHandler handler = OnPlayerSaid;
            SaidEventArgs eventArgs = new SaidEventArgs(player, message, ChatMessageType.Loud);
            handler?.Invoke(this, eventArgs);
        }

        [Command("me", "~y~UŻYJ: ~w~ /me [czynność]", GreedyArg = true)]
        public void SendMeMessage(Client player, string message)
        {
            SendMessageToNearbyPlayers(player, message, ChatMessageType.Me);
        }

        [Command("do", "~y~UŻYJ: ~w~ /do [czynność]", GreedyArg = true)]
        public void SendDoMessage(Client player, string message)
        {
            SendMessageToNearbyPlayers(player, message, ChatMessageType.Do);
        }


        [Command("w", "~y~UŻYJ: ~w~ /w [id] [treść]", GreedyArg = true)]
        public void SendPrivateMessageToPlayer(Client sender, int id, string message)
        {
            if (!sender.GetAccountEntity().CharacterEntity.CanSendPrivateMessage)
            {
                sender.Notify("Nie możesz teraz pisać wiadomości!");
                return;
            }

            if (sender.GetAccountEntity().ServerId.Equals(id))
            {
                sender.Notify("Nie możesz wysłać wiadomości samemu sobie.");
                return;
            }

            Client getter = EntityManager.GetAccountByServerId(id).Client;
            if (getter == null)
            {
                sender.Notify("Nie znaleziono gracza o podanym Id.");
                return;
            }

            sender.SendChatMessage($"~o~ [{getter.GetAccountEntity().ServerId}] {getter.Name}: {message}");
            getter.SendChatMessage($"~o~ [{sender.GetAccountEntity().ServerId}] {sender.Name}: {message}");
        }

        [Command("b", GreedyArg = true)]
        public void SendOocMessage(Client sender, string message)
        {
            SendMessageToNearbyPlayers(sender, message, ChatMessageType.Ooc);
        }

        [Command("go", "~y~UŻYJ: ~w~ /go [slot] [treść]", GreedyArg = true)]
        public void SendMessageOnGroupChat(Client sender, string message)
        {
            var slot = message.Split(' ')[0];
            short groupSlot = slot.All(char.IsDigit) ? Convert.ToInt16(slot) : (short)-1;
            if (groupSlot != -1 && Validator.IsGroupSlotValid(groupSlot))
            {
                sender.Notify("Podany slot grupy jest nieprawidłowy.");
                return;
            }

            if (sender.TryGetGroupByUnsafeSlot(groupSlot, out GroupEntity group) && group != null)
            {
                if (group.CanPlayerWriteOnChat(sender.GetAccountEntity()))
                {
                    var m = string.Join(" ", message);
                    var clients = EntityManager.GetAccounts().Where(a => group.DbModel.Workers.Any(
                        w => w.Character.Id.Equals(a.Value.CharacterEntity.DbModel.Id))).Select(
                        c => c.Value.Client).ToList();
                    SendMessageToSpecifiedPlayers(sender, clients, m, ChatMessageType.GroupOoc, $"~{group.DbModel.Color}~");
                }
                else
                {
                    sender.Notify("Nie posiadasz uprawnień do czatu w tej grupie.");
                }
            }
        }

        [Command("m", "~y~ UŻYJ ~w~ /m [tekst]", GreedyArg = true)]
        public void SayThroughTheMegaphone(Client sender, string message)
        {
            var group = sender.GetAccountEntity().CharacterEntity.OnDutyGroup;
            if (group == null) return;
            if (group.DbModel.GroupType != GroupType.Police || !((Police)group).CanPlayerUseMegaphone(sender.GetAccountEntity()))
            {
                sender.Notify("Twoja grupa, bądź postać nie posiada uprawnień do używania megafonu.");
                return;
            }
            SendMessageToNearbyPlayers(sender, message, ChatMessageType.Megaphone);
        }

        #endregion

        #region ADMIN COMMANDS

        [Command("ado", GreedyArg = true)]
        public void SendAdministratorDoMessage(Client player, string message)
        {
            message = PrepareMessage(player.Name, message, ChatMessageType.ServerDo, out string color);
            NAPI.Chat.SendChatMessageToAll(color, message);
        }

        #endregion

        public static void SendMessageToSpecifiedPlayers(Client sender, List<Client> players, string message, ChatMessageType chatMessageType, string color = "")
        {
            message = PrepareMessage(sender.Name, message, chatMessageType, out string messageColor);
            if (color != "") messageColor = color;
            if (chatMessageType == ChatMessageType.GroupOoc)
            {
                message = $"[{sender.GetAccountEntity().ServerId}] {sender.Name}: {message}";
            }

            foreach (var p in players)
            {
                p.SendChatMessage(messageColor, message);
            }
        }

        public static void SendMessageToNearbyPlayers(Client player, string message, ChatMessageType chatMessageType, string color = "")
        {
            message = PrepareMessage(player.Name, message, chatMessageType, out string messageColor);
            if (color != "") messageColor = color;
            switch (chatMessageType)
            {
                case ChatMessageType.Ooc:
                    message = $"(( [{player.GetAccountEntity().ServerId}] {player.Name} {message} ))";
                    messageColor = "~#CCCCCC~";
                    break;
            }

            //Dla każdego klienta w zasięgu wyświetl wiadomość, zasięg jest pobierany przez rzutowanie enuma do floata
            NAPI.Player.GetPlayersInRadiusOfPlayer((float)chatMessageType, player)
                .ForEach(c => c.SendChatMessage(messageColor, message));
        }

        public static void SendMessageToPlayer(Client player, string message, ChatMessageType chatMessageType)
        {
            message = PrepareMessage(player.Name, message, chatMessageType, out string color);

            if (chatMessageType == ChatMessageType.Phone)
            {
                color = "~#FFDB00~";
                message = player.GetAccountEntity().CharacterEntity.DbModel.Gender
                    ? $"Głos z telefonu (Mężczyzna): {message}"
                    : $"Głos z telefonu (Kobieta): {message}";
            }
            player.SendChatMessage(color, message);
        }

        private static string PrepareMessage(string name, string message, ChatMessageType chatMessageType, out string color)
        {
            color = string.Empty;

            if (char.IsLower(message.First()))
                message = $"{char.ToUpper(message[0])}{message.Substring(1)}";

            switch (chatMessageType)
            {
                case ChatMessageType.Normal:
                    message = $"{name} mówi: {message}";
                    color = "~#FFFFFF~";
                    break;
                case ChatMessageType.Quiet:
                    message = $"{name} szepcze: {message}";
                    color = "~#FFFFFF~";
                    break;
                case ChatMessageType.Loud:
                    message = $"{name} krzyczy: {message}!";
                    color = "~#FFFFFF~";
                    break;
                case ChatMessageType.Me:
                    message = $"** {name} {message}";
                    color = "~#C2A2DA~";
                    break;
                case ChatMessageType.ServerMe:
                    message = $"* {name} {message}";
                    color = "~#C2A2DA~";
                    break;
                case ChatMessageType.Do:
                    message = $"** {message} (( {name} )) **";
                    color = "~#847DB7~";
                    break;
                case ChatMessageType.PhoneOthers:
                    message = $"{name} mówi(telefon): {message}";
                    color = "~#FFFFFF~";
                    break;
                case ChatMessageType.ServerInfo:
                    message = $"[INFO] ~w~ {message}";
                    color = "~#6A9828~";
                    break;
                case ChatMessageType.ServerDo:
                    message = $"** {message} **";
                    color = "~#847DB7~";
                    break;
                case ChatMessageType.Megaphone:
                    message = $"{name} \U0001F4E3 {message}";
                    color = "~#FFDB00~";
                    break;
            }

            if (message.Last() != '.' && message.Last() != '!' && message.Last() != '?')
                message += '.';

            return message;
        }
    }
}
