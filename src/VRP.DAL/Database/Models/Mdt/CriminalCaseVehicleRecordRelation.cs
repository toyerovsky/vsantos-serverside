﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations.Schema;

namespace VRP.DAL.Database.Models.Mdt
{
    public class CriminalCaseVehicleRecordRelation
    {
        public int Id { get; set; }

        // foreign keys
        [ForeignKey("CriminalCase")]
        public int CriminalCaseId { get; set; }
        [ForeignKey("VehicleRecord")]
        public int VehicleRecordId { get; set; }

        // navigation properties
        public virtual CriminalCaseModel CriminalCase { get; set; }
        public virtual VehicleRecordModel VehicleRecord { get; set; }
    }
}
