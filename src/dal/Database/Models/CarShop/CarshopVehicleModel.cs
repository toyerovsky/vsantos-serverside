/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations;
using VRP.DAL.Enums;

namespace VRP.DAL.Database.Models.CarShop
{
    public class CarshopVehicleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public string Category { get; set; }
        public decimal Cost { get; set; }
        [EnumDataType(typeof(CarshopType))]
        public CarshopType CarshopFlags { get; set; }
    }
}