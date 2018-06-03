/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Tools;
using VRP.Core.Validators;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Economy.Groups.Base;
using VRP.Serverside.Entities;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Entities.Core.Group;
using ChatMessageType = VRP.Core.Enums.ChatMessageType;

namespace VRP.Serverside.Core.Scripts
{
    public class ChatScript : Script
    {
        public static event SaidEventHandler OnPlayerSaid;

        [ServerEvent(Event.ChatMessage)]
        private void OnChatMessageHandler(CharacterEntity sender, string message)
        {
            if (sender.CanTalk) return;

            if (sender.CurrentCellphone != null)
                SendMessageToNearbyPlayers(sender, message, sender.CurrentCellphone.CurrentCall != null
                    ? ChatMessageType.PhoneOthers
                    : ChatMessageType.Normal);

            SendMessageToNearbyPlayers(sender, message, ChatMessageType.Normal);

            SaidEventArgs eventArgs = new SaidEventArgs(sender, message, ChatMessageType.Normal);
            OnPlayerSaid?.Invoke(this, eventArgs);
        }

        #region PLAYER COMMANDS

        [Command("try", "~y~UŻYJ: ~w~ /try [treść]", GreedyArg = true, Alias = "sprobuj")]
        public void Try(Client sender, string message)
        {
            CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
            SendMessageToNearbyPlayers(character, Utils.RandomRange(100) <= 49
                ? "zawiódł próbując " + message
                : "odniósł sukces próbując " + message,
                ChatMessageType.ServerMe);

            SaidEventHandler handler = OnPlayerSaid;
            SaidEventArgs eventArgs = new SaidEventArgs(character, message, ChatMessageType.ServerMe);
            handler?.Invoke(this, eventArgs);
        }

        [Command("c", "~y~UŻYJ: ~w~ /c [treść]", GreedyArg = true)]
        public void SendQuietMessage(Client sender, string message)
        {
            CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
            SendMessageToNearbyPlayers(character, message, ChatMessageType.Quiet);

            SaidEventHandler handler = OnPlayerSaid;
            SaidEventArgs eventArgs = new SaidEventArgs(character, message, ChatMessageType.Quiet);
            handler?.Invoke(this, eventArgs);
        }

        [Command("k", "~y~UŻYJ: ~w~ /k [treść]", GreedyArg = true)]
        public void SendScreamMessage(Client sender, string message)
        {
            CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
            SendMessageToNearbyPlayers(character, message, ChatMessageType.Loud);

            SaidEventHandler handler = OnPlayerSaid;
            SaidEventArgs eventArgs = new SaidEventArgs(character, message, ChatMessageType.Loud);
            handler?.Invoke(this, eventArgs);
        }

        [Command("me", "~y~UŻYJ: ~w~ /me [czynność]", GreedyArg = true)]
        public void SendMeMessage(Client sender, string message)
        {
            CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
            SendMessageToNearbyPlayers(character, message, ChatMessageType.Me);
        }

        [Command("do", "~y~UŻYJ: ~w~ /do [czynność]", GreedyArg = true)]
        public void SendDoMessage(Client sender, string message)
        {
            CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
            SendMessageToNearbyPlayers(character, message, ChatMessageType.Do);
        }

        [Command("w", "~y~UŻYJ: ~w~ /w [id] [treść]", GreedyArg = true)]
        public void SendPrivateMessageToPlayer(Client sender, int id, string message)
        {
            if (!sender.GetAccountEntity().CharacterEntity.CanSendPrivateMessage)
            {
                sender.SendError("Nie możesz teraz pisać wiadomości!");
                return;
            }

            if (sender.GetAccountEntity().ServerId.Equals(id))
            {
                sender.SendError("Nie możesz wysłać wiadomości samemu sobie.");
                return;
            }

            Client getter = EntityHelper.GetAccountByServerId(id)?.Client;

            if (getter == null)
            {
                sender.SendError("Nie znaleziono gracza o podanym Id.");
                return;
            }

            sender.SendChatMessage($"[{getter.GetAccountEntity().ServerId}] {getter.Name}: {message}");
            getter.SendChatMessage($"[{sender.GetAccountEntity().ServerId}] {sender.Name}: {message}");
        }

        [Command("b", GreedyArg = true)]
        public void SendOocMessage(Client sender, string message)
        {
            CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
            SendMessageToNearbyPlayers(character, message, ChatMessageType.Ooc);
        }

        [Command("go", "~y~UŻYJ: ~w~ /go [slot] [treść]", GreedyArg = true)]
        public void SendMessageOnGroupChat(Client sender, string message)
        {
            string slot = message.Split(' ')[0];
            byte groupSlot = slot.All(char.IsDigit)
                ? Convert.ToByte(slot)
                : (byte)0;

            GroupSlotValidator validator = new GroupSlotValidator();
            if (groupSlot != 0 && validator.IsValid(groupSlot))
            {
                sender.SendError("Podany slot grupy jest nieprawidłowy.");
                return;
            }

            if (sender.TryGetGroupByUnsafeSlot(groupSlot, out GroupEntity group, out WorkerModel worker))
            {
                if (group.CanPlayerWriteOnChat(worker))
                {
                    message = string.Join(" ", message);
                    IEnumerable<CharacterEntity> characters = 
                        EntityHelper.GetCharacters(c => group.ContainsWorker(c));
                    CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
                    SendMessageToSpecifiedPlayers(character,
                        characters, message, ChatMessageType.GroupOoc, group.DbModel.Color);
                }
                else
                {
                    sender.SendWarning("Nie posiadasz uprawnień do czatu w tej grupie.");
                }
            }
        }

        [Command("m", "~y~ UŻYJ ~w~ /m [tekst]", GreedyArg = true)]
        public void SayThroughTheMegaphone(Client sender, string message)
        {
            CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
            if (character.OnDutyGroup is Police police && police.CanPlayerUseMegaphone(sender.GetAccountEntity()))
            {
                SendMessageToNearbyPlayers(character, message, ChatMessageType.Megaphone);
            }
            else
            {
                sender.SendWarning("Twoja grupa, bądź postać nie posiada uprawnień do używania megafonu.");
            }
        }

        #endregion

        #region ADMIN COMMANDS

        [Command("ado", GreedyArg = true)]
        public void SendAdministratorDoMessage(Client player, string message)
        {
            ChatMessageFormatter chatMessageFormatter = new ChatMessageFormatter();
            message = chatMessageFormatter.Format(player.GetAccountEntity().CharacterEntity, message, ChatMessageType.ServerDo);
            NAPI.Chat.SendChatMessageToAll(message);
        }

        #endregion

        public static void SendMessageToSpecifiedPlayers(CharacterEntity sender, IEnumerable<CharacterEntity> players,
            string message, ChatMessageType chatMessageType, string color = "")
        {
            ChatMessageFormatter chatMessageFormatter = new ChatMessageFormatter();
            message = chatMessageFormatter.Format(sender, message, chatMessageType);

            foreach (CharacterEntity p in players)
                p.AccountEntity.Client.SendChatMessage(message);

        }

        public static void SendMessageToNearbyPlayers(CharacterEntity sender, string message, ChatMessageType chatMessageType)
        {
            ChatMessageFormatter chatMessageFormatter = new ChatMessageFormatter();
            message = chatMessageFormatter.Format(sender, message, chatMessageType);

            // Dla każdego klienta w zasięgu wyświetl wiadomość, zasięg jest pobierany przez rzutowanie enuma do floata
            NAPI.Player.GetPlayersInRadiusOfPlayer((float)chatMessageType, sender.AccountEntity.Client)
                .ForEach(c => NAPI.Chat.SendChatMessageToPlayer(c, message, true));
        }

        public static void SendMessageToPlayer(CharacterEntity sender, string message, ChatMessageType chatMessageType)
        {
            ChatMessageFormatter chatMessageFormatter = new ChatMessageFormatter();
            message = chatMessageFormatter.Format(sender, message, chatMessageType);

            sender.AccountEntity.Client.SendChatMessage(message);
        }
    }
}
