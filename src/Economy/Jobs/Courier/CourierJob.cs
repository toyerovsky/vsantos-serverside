/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using Serverside.Core.Database.Models;
using Serverside.Core.Serialization;
using Serverside.Economy.Jobs.Base;

namespace Serverside.Economy.Jobs.Courier
{
    public class CourierJob : Job
    {
        public List<CourierVehicle> Vehicles { get; set; } = new List<CourierVehicle>();

        public CourierJob(string jobName, decimal moneyLimit, string jsonDirectory) : base(jobName, moneyLimit, jsonDirectory)
        {
            foreach (var vehicleData in JsonHelper.GetJsonObjects<VehicleModel>(jsonDirectory))
            {
                var courierVehicle = new CourierVehicle(vehicleData);
                courierVehicle.Spawn();
                Vehicles.Add(courierVehicle);
            }
        }
    }
}