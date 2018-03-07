/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using VRP.Core.Database.Models;
using VRP.Core.Serialization;
using VRP.Serverside.Economy.Jobs.Base;

namespace VRP.Serverside.Economy.Jobs.Dustman
{
    public class DustmanJob : Job
    {
        public List<DustmanVehicleEntity> Vehicles { get; set; } = new List<DustmanVehicleEntity>();

        public DustmanJob(string name, decimal moneyLimit, string jsonDirectory) : base(name, moneyLimit, jsonDirectory)
        {
            foreach (VehicleModel vehicleData in JsonHelper.GetJsonObjects<VehicleModel>(jsonDirectory))
            {
                Vehicles.Add(new DustmanVehicleEntity(vehicleData));
            }
        }
    }
}