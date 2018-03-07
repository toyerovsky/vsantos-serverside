/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

namespace Serverside.Admin.Enums
{
    public enum ForumGroup
    {
        Uzytkownik = 3,
        Zarzad = 4, 
        Moderator = 6,
        GameMaster = 7,
        Developer = 10,
        BetaTester = 11,
        Support = 12,
        Administrator = 13,
    }

    //To jest po to, żeby można było awansować ludzi, żeby się cieszyli
    //np supporter lvl 1 nie ma dostępu do czegoś do czego ma dostęp supporter lvl2
    //Jeśli zmienimy danemu użytkownikowi grupe przez ACP i zaloguje się na forum to wrzuci go na pierwszy poziom danej rangi
    public enum ServerRank : long
    {
        Uzytkownik,
        Premium,
        Support,
        Support2,
        Support3,
        Support4,
        Support5,
        Support6,
        GameMaster,
        GameMaster2,
        GameMaster3,
        GameMaster4,
        GameMaster5,
        Administrator,
        Administrator2,
        Adminadministrator3,
        Adminadministrator4,
        Zarzad,
        Zarzad2,
        Zarzad3,
    }

    public enum ReportType
    {
        Pomoc,
        Cheater,
        Bug,
        NaruszenieZasad
    }
}