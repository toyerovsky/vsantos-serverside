/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel;

namespace VRP.DAL.Enums
{
    public enum ItemEntityType
    {
        [Description("Jedzenie")]
        Food,
        [Description("Broń")]
        Weapon,
        [Description("Magazynek")]
        WeaponClip,
        [Description("Maska")]
        Mask,
        [Description("Narkotyk")]
        Drug,
        [Description("Kość do gry")]
        Dice,
        [Description("Zegarek")]
        Watch,
        [Description("Ubranie")]
        Cloth,
        [Description("Krótkofalówka")]
        Transmitter,
        [Description("Alkohol")]
        Alcohol,
        [Description("Telefon komórkowy")]
        Cellphone,
        [Description("Tuning pojazdu")]
        Tuning
    }
}