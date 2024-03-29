/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VRP.DAL.Enums;

namespace VRP.DAL.Database.Models.Mdt
{
    public class CharacterRecordModel
    {
        public CharacterRecordModel()
        {
            Vehicles = new HashSet<VehicleRecordModel>();
            CriminalCases = new HashSet<CriminalCaseCharacterRecordRelation>();
        }

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

        // navigation properties
        public virtual ICollection<VehicleRecordModel> Vehicles { get; set; }
        public virtual ICollection<CriminalCaseCharacterRecordRelation> CriminalCases { get; set; }
    }
}