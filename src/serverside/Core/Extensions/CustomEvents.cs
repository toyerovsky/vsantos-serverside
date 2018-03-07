/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using GTANetworkAPI;
using VRP.Core.Enums;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Core.Extensions
{
    public class SaidEventArgs : EventArgs
    {
        public SaidEventArgs(Client player, string message, ChatMessageType chatMessageType)
        {
            Player = player;
            Message = message;
            ChatMessageType = chatMessageType;
        }

        public Client Player { get; }
        public string Message { get; }
        public ChatMessageType ChatMessageType { get; }
    }

    public delegate void SaidEventHandler(object sender, SaidEventArgs e);

    public class DimensionChangeEventArgs : EventArgs
    {
        public DimensionChangeEventArgs(Client player, uint currentDimension, uint oldDimension)
        {
            Player = player;
            CurrentDimension = currentDimension;
            OldDimension = oldDimension;
        }

        public Client Player { get; }
        public uint CurrentDimension { get; }
        public uint OldDimension { get; }
    }
    public delegate void DimensionChangeEventHandler(object sender, DimensionChangeEventArgs e);

    public delegate void AccountLoginEventHandler(Client sender, AccountEntity account);
    public delegate void CharacterLoginEventHandler(Client sender, CharacterEntity account);
}
