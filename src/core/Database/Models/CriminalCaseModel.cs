using System.Collections.Generic;

namespace VRP.Core.Database.Models
{
    public class CriminalCaseModel
    {
        public int Id { get; set; }
        public string Description { get; set; }


        public virtual ICollection<RecordModel> InvolvedPeople { get; set; }
        public virtual ICollection<VehicleRecordModel> InvolvedVehicles { get; set; }
    }
}