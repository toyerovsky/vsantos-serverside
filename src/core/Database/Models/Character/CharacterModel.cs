/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VRP.Core.Database.Models.Account;
using VRP.Core.Database.Models.Agreement;
using VRP.Core.Database.Models.Building;
using VRP.Core.Database.Models.Group;
using VRP.Core.Database.Models.Item;
using VRP.Core.Database.Models.Misc;
using VRP.Core.Database.Models.Vehicle;
using VRP.Core.Enums;

namespace VRP.Core.Database.Models.Character
{
    public class CharacterModel
    {
        public int Id { get; set; }
        public bool Online { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastLoginTime { get; set; }
        public TimeSpan TodayPlayedTime { get; set; }
        public TimeSpan PlayedTime { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Model { get; set; }
        public decimal Money { get; set; }
        public int? BankAccountNumber { get; set; }
        public decimal BankMoney { get; set; }
        public bool Gender { get; set; }
        public DateTime? BornDate { get; set; }
        public bool HasIdCard { get; set; }
        public bool HasDrivingLicense { get; set; }
        public string ForumDescription { get; set; }
        public string Story { get; set; }
        public bool IsAlive { get; set; }
        public byte Health { get; set; }
        public float LastPositionX { get; set; }
        public float LastPositionY { get; set; }
        public float LastPositionZ { get; set; }
        public float LastRotationX { get; set; }
        public float LastRotationY { get; set; }
        public float LastRotationZ { get; set; }
        public uint CurrentDimension { get; set; }
        public int MinutesToRespawn { get; set; }
        public bool Freemode { get; set; }
        [EnumDataType(typeof(JobType))]
        public JobType Job { get; set; }
        public decimal? MoneyJob { get; set; }
        public decimal? JobLimit { get; set; }
        public DateTime? JobReleaseDate { get; set; }

        // navigation properties
        [ForeignKey("CharacterLook")]
        public int CharacterLookModelId { get; set; }
        public virtual CharacterLookModel CharacterLook { get; set; }
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public virtual AccountModel Account { get; set; }

        public virtual ICollection<VehicleModel> Vehicles { get; set; }
        public virtual ICollection<ItemModel> Items { get; set; }
        public virtual ICollection<BuildingModel> Buildings { get; set; }
        public virtual ICollection<DescriptionModel> Descriptions { get; set; }
        public virtual ICollection<WorkerModel> Workers { get; set; }
        public virtual ICollection<AgreementModel> Agreements { get; set; }
    }
}
