﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using GTANetworkAPI;
using GTANetworkInternals;
using Serverside.Core.Database.Models;
using Serverside.Core.Serialization.Json;
using Serverside.Jobs.Base;

namespace Serverside.Jobs.Greenkeeper
{
    public class GreenkeeperJob : Job
    {
        public List<GreenkeeperVehicle> Vehicles { get; set; } = new List<GreenkeeperVehicle>();

        public GreenkeeperJob(EventClass events, string jobName, decimal moneyLimit, string jsonDirectory) :
            base(events, jobName, moneyLimit, jsonDirectory)
        {
            foreach (var vehicleModel in JsonHelper.GetJsonObjects<VehicleModel>(jsonDirectory))
            {
                Vehicles.Add(new GreenkeeperVehicle(Events, vehicleModel));
            }
        }
    }
}
