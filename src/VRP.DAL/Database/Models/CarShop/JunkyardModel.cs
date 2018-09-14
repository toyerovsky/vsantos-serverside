/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Enums;

namespace VRP.DAL.Database.Models.CarShop
{
    public class JunkyardModel
    {
        public int Id { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        [EnumDataType(typeof(CarshopType))]
        public CarshopType Type { get; set; }

        // foreign keys
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }

        // navigation properties
        public virtual AccountModel Creator { get; set; }
    }
}