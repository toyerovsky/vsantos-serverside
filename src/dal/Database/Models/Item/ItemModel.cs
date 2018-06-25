/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations;
using VRP.DAL.Database.Models.Building;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.Database.Models.Vehicle;
using VRP.DAL.Enums;

namespace VRP.DAL.Database.Models.Item
{
    public class ItemModel
    {
        public int Id { get; set; }
        public int? CreatorId { get; set; }

        public string Name { get; set; }

        public short Weight { get; set; }
        public string ItemHash { get; set; }

        public int? FirstParameter { get; set; }
        public int? SecondParameter { get; set; }
        public int? ThirdParameter { get; set; }
        public int? FourthParameter { get; set; }

        [EnumDataType(typeof(ItemEntityType))]
        public virtual ItemEntityType ItemEntityType { get; set; }

        // navigation properties
        public virtual CharacterModel Character { get; set; }
        public virtual BuildingModel Building { get; set; }

        public virtual VehicleModel OwnerVehicle { get; set; }
        public virtual VehicleModel TuningInVehicle { get; set; }
    }
}
