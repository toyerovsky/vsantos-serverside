/* Copyright (C) Przemysław Postrach - All Rights Reserved
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

namespace Serverside.Jobs.DustMan
{
    public class DustmanJob : Job
    {
        public List<DustmanVehicleEntity> Vehicles { get; set; } = new List<DustmanVehicleEntity>();

        public DustmanJob(EventClass events, string name, decimal moneyLimit, string jsonDirectory) : base(events, name, moneyLimit, jsonDirectory)
        {
            foreach (var vehicleData in JsonHelper.GetJsonObjects<VehicleModel>(jsonDirectory))
            {
                Vehicles.Add(new DustmanVehicleEntity(Events, vehicleData));
            }
        }
    }
}