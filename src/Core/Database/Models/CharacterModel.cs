/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GTANetworkAPI;
using Serverside.Jobs.Enums;

namespace Serverside.Core.Database.Models
{
    public class CharacterModel
    {
        [Key]
        public long Id { get; set; }
        
        public AccountModel AccountModel { get; set; }

        public bool Online { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime? TodayPlayedTime { get; set; }
        public DateTime? PlayedTime { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        [EnumDataType(typeof(PedHash))]
        public virtual PedHash Model { get; set; }

        public virtual ICollection<VehicleModel> Vehicles { get; set; }
        public virtual ICollection<ItemModel> Items { get; set; }
        public virtual ICollection<BuildingModel> Buildings { get; set; }
        public virtual ICollection<DescriptionModel> Descriptions { get; set; }
        public virtual ICollection<WorkerModel> Workers { get; set; }

        public decimal Money { get; set; }
        public int? BankAccountNumber { get; set; }
        public decimal BankMoney { get; set; }
        public bool Gender { get; set; }
        public short Weight { get; set; }
        public DateTime BornDate { get; set; }
        public short Height { get; set; }
        public short Force { get; set; }
        public short RunningEfficiency { get; set; }
        public short DivingEfficiency { get; set; }
        public bool HasIdCard { get; set; }
        public bool HasDrivingLicense { get; set; }
        public string ForumDescription { get; set; }
        public string Story { get; set; }
        public bool IsAlive { get; set; }
        public int HitPoints { get; set; }
        public float LastPositionX { get; set; }
        public float LastPositionY { get; set; }
        public float LastPositionZ { get; set; }
        public float LastPositionRotX { get; set; }
        public float LastPositionRotY { get; set; }
        public float LastPositionRotZ { get; set; }
        public uint CurrentDimension { get; set; }
        public int MinutesToRespawn { get; set; }
        public bool? IsCreated { get; set; }
        public bool Freemode { get; set; }
        [EnumDataType(typeof(JobType))]
        public virtual JobType Job { get; set; }
        public decimal? MoneyJob { get; set; }
        public decimal? JobLimit { get; set; }
        public DateTime JobReleaseDate { get; set; }
        //Kreator postaci
        public byte? AccessoryId { get; set; }
        public byte? AccessoryTexture { get; set; }
        public byte? EarsId { get; set; }
        public byte? EarsTexture { get; set; }
        public byte? EyebrowsId { get; set; }
        public float? EyeBrowsOpacity { get; set; }
        public byte? FatherId { get; set; }
        public byte? ShoesId { get; set; }
        public byte? ShoesTexture { get; set; }
        public byte? FirstEyebrowsColor { get; set; }
        public byte? FirstLipstickColor { get; set; }
        public byte? FirstMakeupColor { get; set; }
        public byte? GlassesId { get; set; }
        public byte? GlassesTexture { get; set; }
        public byte? HairId { get; set; }
        public byte? HairTexture { get; set; }
        public byte? HairColor { get; set; }
        public byte? HatId { get; set; }
        public byte? HatTexture { get; set; }
        public byte? LegsId { get; set; }
        public byte? LegsTexture { get; set; }
        public float? LipstickOpacity { get; set; }
        public byte? MakeupId { get; set; }
        public float? MakeupOpacity { get; set; }
        public byte? MotherId { get; set; }
        public byte? SecondEyebrowsColor { get; set; }
        public byte? SecondLipstickColor { get; set; }
        public byte? SecondMakeupColor { get; set; }
        public float? ShapeMix { get; set; }
        public byte? TopId { get; set; }
        public byte? TopTexture { get; set; }
        public byte? TorsoId { get; set; }
        public byte? UndershirtId { get; set; }
    }
}
