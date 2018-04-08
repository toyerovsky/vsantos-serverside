/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using GTANetworkAPI;
using VRP.Serverside.Constant.RemoteEvents;

namespace VRP.Serverside.Entities.Core.Vehicle
{
    public class VehicleIndicatorsScript : Script
    {
        [RemoteEvent(RemoteEvents.ToggleIndicatorLeft)]
        public void ToggleIndicatorLeftHandler(Client player, params object[] arguments)
        {
            GTANetworkAPI.Vehicle veh = NAPI.Player.GetPlayerVehicle(player);
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

        [RemoteEvent(RemoteEvents.ToggleIndicatorRight)]
        public void ToggleIndicatorRightHandler(Client player, params object[] arguments)
        {
            GTANetworkAPI.Vehicle veh = NAPI.Player.GetPlayerVehicle(player);
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