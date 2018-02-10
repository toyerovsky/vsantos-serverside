/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkAPI;

namespace Serverside.Core.Scripts
{
    public class VehicleIndicatorsScript : Script
    {
        public VehicleIndicatorsScript()
        {
        }

        private void API_OnClientEventTrigger(Client player, string eventName, params object[] arguments)
        {
            if (eventName == "toggle_indicator_left")
            {
                var veh = NAPI.Player.GetPlayerVehicle(player);
                int indicator = 1;
                Vector3 pos = NAPI.Entity.GetEntityPosition(player);

                if (NAPI.Data.HasEntitySharedData(veh, "indicator_left") && NAPI.Data.GetEntitySharedData(veh, "indicator_left") == true)
                {
                    NAPI.Data.ResetEntitySharedData(veh, "indicator_left");
                    NAPI.Native.SendNativeToPlayersInRange(pos, 500f, 0xB5D45264751B7DF0, veh, indicator, false);
                }
                else
                {
                    NAPI.Data.SetEntitySharedData(veh, "indicator_left", true);
                    NAPI.Native.SendNativeToPlayersInRange(pos, 500f, 0xB5D45264751B7DF0, veh, indicator, true);
                }
            }
            else if (eventName == "toggle_indicator_right")
            {
                var veh = NAPI.Player.GetPlayerVehicle(player);
                int indicator = 0;
                Vector3 pos = NAPI.Entity.GetEntityPosition(player);

                if (NAPI.Data.HasEntitySharedData(veh, "indicator_right") && NAPI.Data.GetEntitySharedData(veh, "indicator_right") == true)
                {
                    NAPI.Data.ResetEntitySharedData(veh, "indicator_right");
                    NAPI.Native.SendNativeToPlayersInRange(pos, 500f, 0xB5D45264751B7DF0, veh, indicator, false);
                }
                else
                {
                    NAPI.Data.SetEntitySharedData(veh, "indicator_right", true);
                    NAPI.Native.SendNativeToPlayersInRange(pos, 500f, 0xB5D45264751B7DF0, veh, indicator, true);
                }
            }
        }
    }
}