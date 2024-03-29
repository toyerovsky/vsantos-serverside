﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.Database.Models.Group;
using VRP.DAL.Database.Models.Item;
using VRP.DAL.Database.Models.Misc;

namespace VRP.DAL.Database.Models.Building
{
    public class BuildingModel
    {
        public BuildingModel()
        {
            ItemsInBuilding = new HashSet<ItemModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal EnterCharge { get; set; }
        [Required]
        public float ExternalPickupPositionX { get; set; }
        [Required]
        public float ExternalPickupPositionY { get; set; }
        [Required]
        public float ExternalPickupPositionZ { get; set; }
        [Required]
        public float InternalPickupPositionX { get; set; }
        [Required]
        public float InternalPickupPositionY { get; set; }
        [Required]
        public float InternalPickupPositionZ { get; set; }
        public short MaxObjectCount { get; set; }
        public short CurrentObjectCount { get; set; }
        [Required]
        public bool SpawnPossible { get; set; }
        [Required]
        public bool HasCctv { get; set; }
        [Required]
        public bool HasSafe { get; set; }
        [Required]
        public uint InternalDimension { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }

        // foreign keys
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }
        [ForeignKey("Character")]
        public int? CharacterId { get; set; }
        [ForeignKey("AutoSaleModel")]
        public int? AutoSaleId { get; set; }
        [ForeignKey("Group")]
        public int? GroupId { get; set; }

        // navigation properties
        public virtual AccountModel Creator { get; set; }
        public virtual CharacterModel Character { get; set; }
        public virtual GroupModel Group { get; set; }
        public virtual AutoSaleModel AutoSaleModel { get; set; }
        public virtual ICollection<ItemModel> ItemsInBuilding { get; set; }
        public virtual ICollection<ResidentModel> Residents { get; set; }
    }
}
