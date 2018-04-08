/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using GTANetworkAPI;

namespace VRP.Serverside.Constant.Structs
{
    public struct VehicleInfo
    {
        public VehicleHash Hash { get; set; }
        public int HorsePower { get; set; }
        public float EngineCapacity { get; set; }

        public VehicleInfo(VehicleHash hash, int horsePower, float engineCapacity)
        {
            Hash = hash;
            HorsePower = horsePower;
            EngineCapacity = engineCapacity;
        }
    }
}