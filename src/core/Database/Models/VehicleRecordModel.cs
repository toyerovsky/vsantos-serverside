/* Copyright (C) Przemys�aw Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VRP.Core.Enums;

namespace VRP.Core.Database.Models
{
    public class VehicleRecordModel
    {
        public int Id { get; set; }
        public string NumberPlate { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public RecordModel Owner { get; set; }
        public byte[] Image { get; set; }

        public bool Towed { get; set; }
        public bool Wanted { get; set; }
        public string[] SpecialFeatures { get; set; }
        public virtual ICollection<CriminalCaseModel> CriminalCases { get; set; }
    }
}