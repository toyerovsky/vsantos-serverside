/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

namespace VRP.BLL.Enums
{
    public enum ChatMessageType
    {
        Quiet = 5,
        Normal = 15,
        PhoneOthers = 16,
        Me = 24,
        Do = 25,
        ServerMe = 26,
        Loud = 30,
        Ooc = 31,
        Megaphone = 50,
        ServerDo,
        ServerInfo,
        Phone,
        GroupOoc,
        GroupRadio,
        Warning
    }
}