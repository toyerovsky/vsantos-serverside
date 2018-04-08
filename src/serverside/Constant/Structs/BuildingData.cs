/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using GTANetworkAPI;

namespace VRP.Serverside.Constant.Structs
{
    public struct BuildingData
    {
        public string Name { get; set; }
        public Vector3 InternalPosition { get; set; }

        public BuildingData(string name, Vector3 internalPostion)
        {
            Name = name;
            InternalPosition = internalPostion;
        }
    }
}