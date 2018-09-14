/* Copyright (C) Przemys³aw Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;

namespace VRP.DAL.Database.Models.Mdt
{
    public class CriminalCaseModel
    {
        public CriminalCaseModel()
        {
            InvolvedPeople = new HashSet<CriminalCaseCharacterRecordRelation>();
            InvolvedVehicles = new HashSet<CriminalCaseVehicleRecordRelation>();
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<CriminalCaseCharacterRecordRelation> InvolvedPeople { get; set; }
        public virtual ICollection<CriminalCaseVehicleRecordRelation> InvolvedVehicles { get; set; }
    }
}