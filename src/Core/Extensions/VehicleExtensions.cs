/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkAPI;
using Serverside.Entities.Game;

namespace Serverside.Core.Extensions
{
    public static class VehicleExtensions
    {
        public static VehicleEntity GetVehicleEntity(this Vehicle handle)
        {
            return handle.HasData("VehicleEntity") ? handle.GetData("VehicleEntity") : null;
        }
    }
}