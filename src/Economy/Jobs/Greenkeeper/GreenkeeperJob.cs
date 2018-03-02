/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using Serverside.Core.Database.Models;
using Serverside.Core.Serialization;
using Serverside.Economy.Jobs.Base;

namespace Serverside.Economy.Jobs.Greenkeeper
{
    public class GreenkeeperJob : Job
    {
        public List<GreenkeeperVehicle> Vehicles { get; set; } = new List<GreenkeeperVehicle>();

        public GreenkeeperJob(string jobName, decimal moneyLimit, string jsonDirectory) :
            base(jobName, moneyLimit, jsonDirectory)
        {
            foreach (var vehicleData in JsonHelper.GetJsonObjects<VehicleModel>(jsonDirectory))
            {
                var vehicle = new GreenkeeperVehicle(vehicleData);
                vehicle.Spawn();
                Vehicles.Add(vehicle);
            }
        }
    }
}
