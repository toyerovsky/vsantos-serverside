﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using GTANetworkAPI;
using VRP.Serverside.Entities.Core.Vehicle;

namespace VRP.Serverside.Core.Extensions
{
    public static class VehicleExtensions
    {
        public static VehicleEntity GetVehicleEntity(this Vehicle handle)
        {
            return handle.HasData("VehicleEntity") ? handle.GetData("VehicleEntity") : null;
        }
    }
}