/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;

namespace VRP.Serverside.Constant
{
    public static class Messages
    {
        public const string ResourceStartMessage = "Skrypt został uruchomiony pomyślnie.";

        public static readonly List<string> YesMessagesList = new List<string>
        {
            "Tak",
            "Tak.",
            "tak",
            "tak.",
            "Ta",
            "Ta.",
            "ta",
            "ta.",
            "No",
            "No.",
            "no",
            "no.",
            "Jasne",
            "Jasne.",
            "jasne",
            "jasne.",
            "No kurwa",
            "No kurwa.",
            "no kurwa",
            "no kurwa.",
            "Mam",
            "Mam.",
            "mam",
            "mam."
        };

        public static readonly List<string> NoMessagesList = new List<string>
        {
            "Nie",
            "Nie.",
            "nie",
            "nie.",
            "Nope",
            "Nope.",
            "nope",
            "nope.",
            "Niestety",
            "Niestety.",
            "niestety",
            "niestety.",
            "Chuja",
            "Chuja.",
            "chuja",
            "chuja."
        };
    }
}
