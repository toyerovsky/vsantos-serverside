using System;
using System.Collections.Generic;
using System.Text;

namespace VRP.Core.Database.Models.Vehicle
{
    public class VehicleTuningModel
    {
        public int Id { get; set; }
        public float EngineMultiplier { get; set; }
        public float BreaksMultiplier { get; set; }
        public float TorqueMultiplier { get; set; }

        // navigation properties
        public virtual VehicleModel Vehicle { get; set; }
    }
}
