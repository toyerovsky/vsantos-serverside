using System;
using System.Linq;
using VRP.Core.Enums;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Core
{
    public class ChatMessageFormatter
    {
        public string Format(CharacterEntity senderCharacter, string message, ChatMessageType chatMessageType, string color = "")
        {
            if (char.IsLower(message.First()))
                message = $"{char.ToUpper(message[0])}{message.Substring(1)}";
            
            switch (chatMessageType)
            {
                case ChatMessageType.Normal:
                    message = $"{senderCharacter.FormatName} mówi: {message}";
                    color = "#FFFFFF";
                    break;
                case ChatMessageType.Quiet:
                    message = $"{senderCharacter.FormatName} szepcze: {message}";
                    color = "#FFFFFF";
                    break;
                case ChatMessageType.Loud:
                    message = $"{senderCharacter.FormatName} krzyczy: {message}!";
                    color = "#FFFFFF";
                    break;
                case ChatMessageType.Me:
                    message = $"** {senderCharacter.FormatName} {message}";
                    color = "#C2A2DA";
                    break;
                case ChatMessageType.ServerMe:
                    message = $"* {senderCharacter.FormatName} {message}";
                    color = "#C2A2DA";
                    break;
                case ChatMessageType.Do:
                    message = $"** {message} (( {senderCharacter.FormatName} )) **";
                    color = "#847DB7";
                    break;
                case ChatMessageType.PhoneOthers:
                    message = $"{senderCharacter.FormatName} mówi(telefon): {message}";
                    color = "#FFFFFF";
                    break;
                case ChatMessageType.ServerInfo:
                    message = $"[INFO] ~w~ {message}";
                    color = "#6A9828";
                    break;
                case ChatMessageType.ServerDo:
                    message = $"** {message} **";
                    color = "#847DB7";
                    break;
                case ChatMessageType.Megaphone:
                    message = $"{senderCharacter.FormatName} \U0001F4E3 {message}";
                    color = "#FFDB00";
                    break;
                case ChatMessageType.Phone:
                    color = "#FFDB00";
                    break;
                case ChatMessageType.Ooc:
                    message = $"(( [{senderCharacter.AccountEntity.ServerId}] {senderCharacter.FormatName} {message} ))";
                    color = "#CCCCCC";
                    break;
                case ChatMessageType.GroupOoc:
                    message = $"[{senderCharacter.AccountEntity.ServerId}] {senderCharacter.FormatName}: {message}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(chatMessageType), chatMessageType, null);
            }

            if (message.Last() != '.' &&
                message.Last() != '!' &&
                message.Last() != '?')
            {
                message += '.';
            }

            return $"<p style=\"color:{color}\">{message}</p>";
        }
    }
}