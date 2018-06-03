﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using VRP.Core.Database.Models.Building;
using VRP.Core.Database.Models.Vehicle;

namespace VRP.Core.Database.Models.Misc
{
    public class AutoSaleModel
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }

        // navigation properties
        public virtual VehicleModel VehicleModel { get; set; }
        public virtual BuildingModel BuildingModel { get; set; }
    }
}
