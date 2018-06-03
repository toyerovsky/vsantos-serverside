using System;
using VRP.Core.Database.Models.Lease;

namespace VRP.Core.Database.Models.Misc
{
    public class LeaseModel
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public TimeSpan ChargeFrequency { get; set; }

        // navigation properties
        public virtual AgreementModel AgreementModel { get; set; }

        public virtual VehicleModel Vehicle { get; set; }
        public virtual BuildingModel Building { get; set; }
    }
}
