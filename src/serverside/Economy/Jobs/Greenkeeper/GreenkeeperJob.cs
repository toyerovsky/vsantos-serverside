/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using VRP.Core.Database.Models;
using VRP.Core.Database.Models.Vehicle;
using VRP.Core.Serialization;
using VRP.Serverside.Economy.Jobs.Base;

namespace VRP.Serverside.Economy.Jobs.Greenkeeper
{
    public class GreenkeeperJob : Job
    {
        public List<GreenkeeperVehicle> Vehicles { get; set; } = new List<GreenkeeperVehicle>();

        public GreenkeeperJob(string jobName, decimal moneyLimit, string jsonDirectory) :
            base(jobName, moneyLimit, jsonDirectory)
        {
            foreach (VehicleModel vehicleData in JsonHelper.GetJsonObjects<VehicleModel>(jsonDirectory))
            {
                GreenkeeperVehicle vehicle = new GreenkeeperVehicle(vehicleData);
                vehicle.Spawn();
                Vehicles.Add(vehicle);
            }
        }
    }
}
