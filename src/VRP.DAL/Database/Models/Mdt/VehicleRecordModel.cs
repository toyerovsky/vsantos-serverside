/* Copyright (C) Przemys³aw Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VRP.DAL.Database.Models.Mdt
{
    public class VehicleRecordModel
    {
        public VehicleRecordModel()
        {
            CriminalCases = new HashSet<CriminalCaseVehicleRecordRelation>();
        }

        public int Id { get; set; }
        public string NumberPlate { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public byte[] Image { get; set; }
        public bool Towed { get; set; }
        public bool Wanted { get; set; }
        public string SpecialFeatures { get; set; }

        // foreign keys
        [ForeignKey("Owner")]
        public int? OwnerId { get; set; }

        // navigation properties
        public virtual CharacterRecordModel Owner { get; set; }
        public virtual ICollection<CriminalCaseVehicleRecordRelation> CriminalCases { get; set; }
    }
}