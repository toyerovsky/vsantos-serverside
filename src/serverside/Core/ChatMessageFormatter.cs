/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Linq;
using VRP.Core.Enums;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Core
{
    public class ChatMessageFormatter
    {
        public string Format(string formatName, string message, ChatMessageType chatMessageType, int serverId = -1, string color = "")
        {
            if (char.IsLower(message.First()))
                message = $"{char.ToUpper(message[0])}{message.Substring(1)}";

            switch (chatMessageType)
            {
                case ChatMessageType.Normal:
                    message = $"{formatName} mówi: {message}";
                    color = "#FFFFFF";
                    break;
                case ChatMessageType.Quiet:
                    message = $"{formatName} szepcze: {message}";
                    color = "#FFFFFF";
                    break;
                case ChatMessageType.Loud:
                    message = $"{formatName} krzyczy: {message}!";
                    color = "#FFFFFF";
                    break;
                case ChatMessageType.Me:
                    message = $"** {formatName} {message}";
                    color = "#C2A2DA";
                    break;
                case ChatMessageType.ServerMe:
                    message = $"* {formatName} {message}";
                    color = "#C2A2DA";
                    break;
                case ChatMessageType.Do:
                    message = $"** {message} (( {formatName} )) **";
                    color = "#847DB7";
                    break;
                case ChatMessageType.PhoneOthers:
                    message = $"{formatName} mówi(telefon): {message}";
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
                    message = $"{formatName} \U0001F4E3 {message}";
                    color = "#FFDB00";
                    break;
                case ChatMessageType.Phone:
                    color = "#FFDB00";
                    break;
                case ChatMessageType.Ooc:
                    message = $"(( [{serverId}] {formatName} {message} ))";
                    color = "#CCCCCC";
                    break;
                case ChatMessageType.GroupOoc:
                    message = $"[{serverId}] {formatName}: {message}";
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

            return $"!{{{color}}} {message}";
        }
    }
}