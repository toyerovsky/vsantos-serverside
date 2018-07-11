/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel;

namespace VRP.DAL.Enums
{
    public enum ServerRank
    {
        [Description("Użytkownik")]
        Uzytkownik,
        [Description("Donator")]
        Donator,
        [Description("Support I")]
        Support,
        [Description("Support II")]
        Support2,
        [Description("Support III")]
        Support3,
        [Description("Support IV")]
        Support4,
        [Description("Support V")]
        Support5,
        [Description("Administrator rozgrywki I")]
        AdministratorRozgrywki,
        [Description("Administrator rozgrywki II")]
        AdministratorRozgrywki2,
        [Description("Administrator rozgrywki III")]
        AdministratorRozgrywki3,
        [Description("Administrator rozgrywki IV")]
        AdministratorRozgrywki4,
        [Description("Administrator rozgrywki V")]
        AdministratorRozgrywki5,
        [Description("Administrator rozgrywki I")]
        AdministratorTechniczny,
        [Description("Administrator rozgrywki II")]
        AdministratorTechniczny2,
        [Description("Administrator rozgrywki III")]
        AdministratorTechniczny3,
        [Description("Administrator rozgrywki IV")]
        AdministratorTechniczny4,
        [Description("Zarząd I")]
        Zarzad,
        [Description("Zarząd II")]
        Zarzad2,
        [Description("Zarząd III")]
        Zarzad3,
    }
}