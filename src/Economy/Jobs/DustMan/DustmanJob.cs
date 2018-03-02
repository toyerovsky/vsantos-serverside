/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using Serverside.Core.Database.Models;
using Serverside.Core.Serialization;
using Serverside.Economy.Jobs.Base;

namespace Serverside.Economy.Jobs.DustMan
{
    public class DustmanJob : Job
    {
        public List<DustmanVehicleEntity> Vehicles { get; set; } = new List<DustmanVehicleEntity>();

        public DustmanJob(string name, decimal moneyLimit, string jsonDirectory) : base(name, moneyLimit, jsonDirectory)
        {
            foreach (var vehicleData in JsonHelper.GetJsonObjects<VehicleModel>(jsonDirectory))
            {
                Vehicles.Add(new DustmanVehicleEntity(vehicleData));
            }
        }
    }
}