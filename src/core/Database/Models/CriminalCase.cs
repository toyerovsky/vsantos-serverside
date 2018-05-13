

namespace VRP.Core.Database.Models
{
    public class CriminalCase
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Record> InvolvedPeople { get; set; }
        public virtual ICollection<VehicleRecord> InvolvedVehicles { get; set; }
    }
}