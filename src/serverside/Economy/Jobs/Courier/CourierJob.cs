﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using VRP.Core.Database.Models;
using VRP.Core.Serialization;
using VRP.Serverside.Economy.Jobs.Base;

namespace VRP.Serverside.Economy.Jobs.Courier
{
    public class CourierJob : Job
    {
        public List<CourierVehicle> Vehicles { get; set; } = new List<CourierVehicle>();

        public CourierJob(string jobName, decimal moneyLimit, string jsonDirectory) : base(jobName, moneyLimit, jsonDirectory)
        {
            foreach (VehicleModel vehicleData in JsonHelper.GetJsonObjects<VehicleModel>(jsonDirectory))
            {
                CourierVehicle courierVehicle = new CourierVehicle(vehicleData);
                courierVehicle.Spawn();
                Vehicles.Add(courierVehicle);
            }
        }
    }
}