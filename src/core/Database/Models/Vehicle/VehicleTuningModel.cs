/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

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
