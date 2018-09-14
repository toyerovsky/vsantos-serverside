/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.Database.Models.Vehicle;

namespace VRP.DAL.Database.Models.Misc
{
    public class DescriptionModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        // foreign keys
        [ForeignKey("Character")]
        public int? CharacterId { get; set; }
        [ForeignKey("Vehicle")]
        public int? VehicleId { get; set; }

        // navigation properties
        public virtual CharacterModel Character { get; set; }
        public virtual VehicleModel Vehicle { get; set; }
    }
}