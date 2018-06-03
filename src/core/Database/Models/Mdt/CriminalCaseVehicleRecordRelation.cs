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
