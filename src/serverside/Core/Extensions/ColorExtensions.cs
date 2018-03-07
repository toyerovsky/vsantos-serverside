/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkAPI;

namespace VRP.Serverside.Core.Extensions
{
    public static class ColorExtensions
    {
        public static string ToGameColor(this Color c) => $"~{c.ToHex()}~";

        public static string ToHex(this Color c)
        {
            if (c.Alpha == 0)
                return $"#{c.Red:X2}{c.Green:X2}{c.Blue:X2}";

            return $"#{c.Red:X2}{c.Green:X2}{c.Blue:X2}{c.Alpha:X2}";
        }

        public static string ToRgb(this Color c) => $"RGB({c.Red},{c.Green},{c.Blue})";

        public static string ToRgba(this Color c) => $"RGBA({c.Red},{c.Green},{c.Blue},{c.Alpha})";
    }
}