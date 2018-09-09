/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel;

namespace VRP.DAL.Enums
{
    public enum GroupType
    {
        [Description("Taxi")]
        Taxi,
        [Description("Bar")]
        Bar,
        [Description("Klub")]
        Club,
        [Description("Organizacja przestępcza")]
        Crime,
        [Description("Warsztat")]
        Workshop,
        [Description("Policja")]
        Police,
        [Description("Szpital")]
        Hospital,
        [Description("Wiadomości")]
        News
    }
}