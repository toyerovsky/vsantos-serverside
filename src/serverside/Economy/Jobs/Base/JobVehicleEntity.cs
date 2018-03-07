/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Interfaces;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities.Core.Vehicle;

namespace VRP.Serverside.Economy.Jobs.Base
{
    public abstract class JobVehicleEntity : VehicleEntity, IXmlObject
    {
        protected JobVehicleEntity(VehicleModel model) : base(model)
        {
        }

        public virtual void Respawn()
        {
            Repair();
            DbModel.Fuel = GetFuelTankSize((VehicleClass)GameVehicle.Class);
            GameVehicle.Position = new Vector3(DbModel.SpawnPositionX, DbModel.SpawnPositionY,
                DbModel.SpawnPositionZ);
            GameVehicle.Rotation = new Vector3(DbModel.SpawnRotationX, DbModel.SpawnRotationY,
                DbModel.SpawnRotationZ);
        }

        public string FilePath { get; set; }
        public string CreatorForumName { get; set; }
    }
}