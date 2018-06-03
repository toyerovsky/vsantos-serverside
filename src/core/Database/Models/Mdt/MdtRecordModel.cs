/* Copyright (C) Przemys³aw Postrach - All Rights Reserved
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
    public class MdtRecordModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BornDate { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
        public byte[] Image { get; set; }

        [EnumDataType(typeof(Race))]
        public Race Race { get; set; }

        public char EyeColor { get; set; }
        public string FingerPrints { get; set; }
        public Guid DNACode { get; set; }

        public bool Wanted { get; set; }
        public virtual ICollection<VehicleRecordModel> Vehicles { get; set; }
        public virtual ICollection<CriminalCaseModel> CriminalCases { get; set; }
    }
}