/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using GTANetworkAPI;
using VRP.Core.Enums;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Core.Extensions
{
    public class SaidEventArgs : EventArgs
    {
        public SaidEventArgs(CharacterEntity character, string message, ChatMessageType chatMessageType)
        {
            Character = character;
            Message = message;
            ChatMessageType = chatMessageType;
        }

        public CharacterEntity Character { get; }
        public string Message { get; }
        public ChatMessageType ChatMessageType { get; }
    }

    public delegate void SaidEventHandler(object sender, SaidEventArgs e);

    public class DimensionChangeEventArgs : EventArgs
    {
        public DimensionChangeEventArgs(CharacterEntity character, uint currentDimension, uint oldDimension)
        {
            Character = character;
            CurrentDimension = currentDimension;
            OldDimension = oldDimension;
        }

        public CharacterEntity Character { get; }
        public uint CurrentDimension { get; }
        public uint OldDimension { get; }
    }
    public delegate void DimensionChangeEventHandler(object sender, DimensionChangeEventArgs e);

    public delegate void AccountLoginEventHandler(Client sender, AccountEntity account);
    public delegate void CharacterSelectEventHandler(Client sender, CharacterEntity character);
}
