/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VRP.Core.Database.Models
{
    public class BuildingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual CharacterModel Character { get; set; }
        public virtual GroupModel Group { get; set; }
        public ICollection<ItemModel> Items { get; set; }
        [Required]
        public virtual AccountModel Creator { get; set; }
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
        public decimal? Cost { get; set; }
    }
}
