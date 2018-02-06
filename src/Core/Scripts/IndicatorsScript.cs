/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkAPI;
using Serverside.Core.Enums;

namespace Serverside.Core.Scripts
{
    public class IndicatorsScript : Script
    {
        public IndicatorsScript()
        {
        }

        private void Event_OnClientEventTrigger(Client player, string eventName, params object[] arguments)
        {
            if (eventName == "toggle_indicator_left")
            {
                if (player.Vehicle.HasSharedData("indicator_left") && player.Vehicle.GetSharedData("indicator_left"))
                {
                    player.Vehicle.ResetSharedData("indicator_left");
                    NAPI.Native.SendNativeToPlayersInRange(player.Position, 500f, 0xB5D45264751B7DF0, player.Vehicle, (int)IndicatorType.Left, false);
                }
                else
                {
                    player.Vehicle.SetSharedData("indicator_left", true);
                    NAPI.Native.SendNativeToPlayersInRange(player.Position, 500f, 0xB5D45264751B7DF0, player.Vehicle, (int)IndicatorType.Left, true);
                }
            }
            else if (eventName == "toggle_indicator_right")
            {
                if (player.Vehicle.HasSharedData("indicator_right") && player.Vehicle.GetSharedData("indicator_right"))
                {
                    player.Vehicle.ResetSharedData("indicator_right");
                    NAPI.Native.SendNativeToPlayersInRange(player.Position, 500f, 0xB5D45264751B7DF0, player.Vehicle, (int)IndicatorType.Right, false);
                }
                else
                {
                    player.Vehicle.SetSharedData("indicator_right", true);
                    NAPI.Native.SendNativeToPlayersInRange(player.Position, 500f, 0xB5D45264751B7DF0, player.Vehicle, (int)IndicatorType.Right, true);
                }
            }
        }
    }
}