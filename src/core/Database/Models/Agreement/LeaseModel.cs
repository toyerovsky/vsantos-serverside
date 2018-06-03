/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.ComponentModel.DataAnnotations.Schema;
using VRP.Core.Database.Models.Building;
using VRP.Core.Database.Models.Vehicle;

namespace VRP.Core.Database.Models.Agreement
{
    public class LeaseModel
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public TimeSpan ChargeFrequency { get; set; }

        // foreign keys
        public int AgreementId { get; set; }

        // navigation properties
        public virtual AgreementModel AgreementModel { get; set; }
        public virtual VehicleModel Vehicle { get; set; }
        public virtual BuildingModel Building { get; set; }
    }
}
