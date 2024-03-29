﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Building;
using VRP.DAL.Database.Models.Vehicle;

namespace VRP.DAL.Database.Models.Misc
{
    public class AutoSaleModel
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        
        // foreign keys
        [ForeignKey("Vehicle")]
        public int? VehicleId { get; set; }
        [ForeignKey("Building")]
        public int? BuildingId { get; set; }

        // navigation properties
        public virtual VehicleModel VehicleModel { get; set; }
        public virtual BuildingModel BuildingModel { get; set; }
    }
}
