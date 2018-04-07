﻿using System.Linq;
using GTANetworkAPI;
using VRP.Core.Enums;

namespace VRP.Serverside.Core.Extensions
{
    public static class EnumExtensions
    {
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