/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Agreement;
using VRP.DAL.Database.Models.Building;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.Database.Models.Vehicle;
using VRP.DAL.Enums;

namespace VRP.DAL.Database.Models.Group
{
    public class GroupModel
    {
        public GroupModel()
        {
            Workers = new HashSet<WorkerModel>();
            Agreements = new HashSet<AgreementModel>();
            Vehicles = new HashSet<VehicleModel>();
            Buildings = new HashSet<BuildingModel>();
            GroupRanks = new HashSet<GroupRankModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public int Grant { get; set; }
        public int MaxPayday { get; set; }
        public decimal Money { get; set; }
        public string Color { get; set; }
        [EnumDataType(typeof(GroupType))]
        public GroupType GroupType { get; set; }
        public DateTime CreationTime { get; set; }
        public string ImageUrl { get; set; }
        public DateTime ImageUploadDate { get; set; }

        // foreign keys
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }
        [ForeignKey("BossCharacter")]
        public int BossCharacterId { get; set; }
        [ForeignKey("DefaultRank")]
        public int DefaultRankId { get; set; }

        // navigation properties
        /// <summary>
        /// Prime business boss which cannot be removed from it
        /// </summary>
        public virtual CharacterModel BossCharacter { get; set; }
        public virtual GroupRankModel DefaultRank { get; set; }
        public virtual ICollection<WorkerModel> Workers { get; set; }
        public virtual ICollection<AgreementModel> Agreements { get; set; }
        public virtual ICollection<VehicleModel> Vehicles { get; set; }
        public virtual ICollection<BuildingModel> Buildings { get; set; }
        public virtual ICollection<GroupRankModel> GroupRanks { get; set; }
    }
}
