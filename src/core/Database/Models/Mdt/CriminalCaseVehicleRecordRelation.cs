/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace VRP.Core.Database.Models.Mdt
{
    public class CriminalCaseVehicleRecordRelation
    {
        public int Id { get; set; }

        // navigation properties
        public virtual CriminalCaseModel CriminalCase { get; set; }
        public virtual VehicleRecordModel VehicleRecord { get; set; }
    }
}
