/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using GTANetworkAPI;
using VRP.Core.Enums;

namespace VRP.Serverside.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    if (Attribute.GetCustomAttribute(field,
                        typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                    {
                        return attribute.Description;
                    }
                }
            }
            return null;
        }

        public static string GetColoredRankName(this ServerRank serverRank)
        {
            Color color;

            if (serverRank >= ServerRank.Support && serverRank <= ServerRank.Support6)
                color = new Color(51, 143, 255);
            else if (serverRank >= ServerRank.GameMaster && serverRank <= ServerRank.GameMaster5)
                color = new Color(0, 109, 15);
            else if (serverRank >= ServerRank.Administrator && serverRank <= ServerRank.Adminadministrator3)
                color = new Color(117, 13, 18);
            else if (serverRank >= ServerRank.Zarzad && serverRank <= ServerRank.Zarzad2)
                color = new Color(255, 0, 0);
            else
                color = new Color(255, 255, 255);

            return $"<p style='color:{color.ToHex()}'>{serverRank.ToString().Where(char.IsLetter)}</p>";
        }
    }
}
