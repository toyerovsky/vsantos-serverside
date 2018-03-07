/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using VRP.Core.Tools;
using VRP.Serverside.Core.Exceptions;

namespace VRP.Serverside.Core.Extensions
{
    public static class ApiExtensions
    {
        public static List<Client> GetNearestPlayers(this Vector3 position) =>
            NAPI.Pools.GetAllPlayers().OrderBy(
                n => n.Position.DistanceTo(position)).ToList();

        public static Client GetNearestPlayer(this Vector3 position) =>
            position.GetNearestPlayers()[0];

        
        private static Dictionary<string, string> _rocstarColors = new Dictionary<string, string>()
        {
            {"~r~", "DE3232"},
            {"~g~", "71CA71"},
            {"~h~", "5CB4EB"},
            {"~y~", "EEC650"},
            {"~p~", "AD65E5"},
            {"~q~", "EB4F80"},
            {"~o~", "FE8455"},
            {"~m~", "636378"},
            {"~u~", "252525"}
        };

        public static string ToRocstar(this Color color)
        {
            foreach (KeyValuePair<string, string> c in _rocstarColors)
            {
                //ConsoleOutput(c.Key.Substring(1, 2).ToString(), ConsoleColor.Red);
                //ConsoleOutput((Convert.ToInt32(c.Key.Substring(1, 2)) < 20).ToString(), ConsoleColor.Green);
                //ConsoleOutput((Math.Abs(color.red - Convert.ToInt32(c.Key.Substring(1, 2))) < 20).ToString(), ConsoleColor.Blue);
                if (Math.Abs(color.Red - int.Parse(c.Value.Substring(0, 2), System.Globalization.NumberStyles.HexNumber)) < 20 ||
                    Math.Abs(color.Green - int.Parse(c.Value.Substring(2, 2), System.Globalization.NumberStyles.HexNumber)) < 20 ||
                    Math.Abs(color.Red - int.Parse(c.Value.Substring(4, 2), System.Globalization.NumberStyles.HexNumber)) < 20)
                    return c.Key;
            }
            return "~w~";
        }

        public static Color GetRandomColor(this Color color) =>
            new Color(Utils.RandomRange(256), Utils.RandomRange(256), Utils.RandomRange(256), Utils.RandomRange(256));

        public static Color ToColor(this string hex)
        {
            if (hex == null)
                throw new NullReferenceException();
            if (hex.StartsWith("#"))
                hex = hex.Substring(1);
            if (hex.All(c => !char.IsDigit(c)))
                throw new ColorConvertException("Podany kolor zawiera niedozwolone znaki.");

            if (hex.Length != 6 || hex.Length != 8)
                return new Color(255, 0, 0);

            if (hex.Length == 6)
            {
                return new Color(
                    int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                    int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                    int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
            }
            return new Color(
                int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
                int.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber));
        }

        public static string GetColoredString(this string text, string color) => $"~{color}~{text}";
    }
}
