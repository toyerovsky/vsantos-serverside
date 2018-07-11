/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel;

namespace VRP.DAL.Enums
{
    public enum PenaltyType
    {
        [Description("Ostrzeżenie")]
        Warn,
        [Description("Admin jail")]
        AdminJail,
        [Description("Ban")]
        Ban,
        [Description("Kick")]
        Kick,
        [Description("Blokada postaci")]
        CharacterBlockage
    }
}