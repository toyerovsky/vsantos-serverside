/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GTANetworkAPI;

namespace Serverside.Core.Database.Models
{
    public class VehicleModel
    {
        public long Id { get; set; }

        public virtual CharacterModel Character { get; set; }
        public virtual GroupModel Group { get; set; }

        public virtual AccountModel Creator { get; set; }

        public string NumberPlate { get; set; }
        public int NumberPlateStyle { get; set; }

        public string Name { get; set; }

        [EnumDataType(typeof(VehicleHash))]
        public virtual VehicleHash VehicleHash { get; set; }

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
        
        public virtual ICollection<ItemModel> ItemsInVehicle { get; set; }
    }
}
