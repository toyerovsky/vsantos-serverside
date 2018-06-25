/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.Database.Models.Group;
using VRP.DAL.Database.Models.Item;
using VRP.DAL.Database.Models.Misc;

namespace VRP.DAL.Database.Models.Vehicle
{
    public class VehicleModel
    {
        public int Id { get; set; }

        public virtual CharacterModel Character { get; set; }
        public virtual GroupModel Group { get; set; }

        public int? CreatorId { get; set; }

        public string NumberPlate { get; set; }
        public int NumberPlateStyle { get; set; }

        public string Name { get; set; }

        public virtual string VehicleHash { get; set; }

        public float SpawnPositionX { get; set; }
        public float SpawnPositionY { get; set; }
        public float SpawnPositionZ { get; set; }

        public float SpawnRotationX { get; set; }
        public float SpawnRotationY { get; set; }
        public float SpawnRotationZ { get; set; }

        public bool IsSpawned { get; set; }
        public float EnginePowerMultiplier { get; set; }
        public float EngineTorqueMultiplier { get; set; }
        public float Health { get; set; }
        public float Milage { get; set; }
        public float Fuel { get; set; }
        public float FuelTank { get; set; }
        public float FuelConsumption { get; set; }
        public bool Door1Damage { get; set; }
        public bool Door2Damage { get; set; }
        public bool Door3Damage { get; set; }
        public bool Door4Damage { get; set; }
        public bool Window1Damage { get; set; }
        public bool Window2Damage { get; set; }
        public bool Window3Damage { get; set; }
        public bool Window4Damage { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public int WheelType { get; set; }
        public int WheelColor { get; set; }

        // foreign keys
        public int AutoSaleId { get; set; }
        // navigation properties
        public virtual AutoSaleModel AutoSaleModel { get; set; }
        [InverseProperty("OwnerVehicle")]
        public virtual ICollection<ItemModel> ItemsInVehicle { get; set; }
        [InverseProperty("TuningInVehicle")]
        public virtual ICollection<ItemModel> VehicleTuning { get; set; }
    }
}
