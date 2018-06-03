/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VRP.Core.Database.Models
{
    public class BuildingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreatorId { get; set; }
        public decimal? EnterCharge { get; set; }
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

        // navigation properties
        public virtual CharacterModel Character { get; set; }
        public virtual GroupModel Group { get; set; }
        
        public virtual AutoSaleModel AutoSaleModel { get; set; }
        public virtual ICollection<ItemModel> ItemsInBuilding { get; set; }
    }
}
