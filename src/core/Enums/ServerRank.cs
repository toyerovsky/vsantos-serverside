/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

namespace VRP.Core.Enums
{
    //To jest po to, żeby można było awansować ludzi, żeby się cieszyli
    //np supporter lvl 1 nie ma dostępu do czegoś do czego ma dostęp supporter lvl2
    //Jeśli zmienimy danemu użytkownikowi grupe przez ACP i zaloguje się na forum to wrzuci go na pierwszy poziom danej rangi
    public enum ServerRank
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
}