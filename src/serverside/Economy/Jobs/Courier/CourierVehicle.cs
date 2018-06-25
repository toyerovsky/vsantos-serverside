/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using VRP.DAL.Database.Models.Vehicle;
using VRP.Serverside.Economy.Jobs.Base;

namespace VRP.Serverside.Economy.Jobs.Courier
{
    public class CourierVehicle : JobVehicleEntity
    {
        public CourierVehicle(VehicleModel model) : base(model)
        {
        }

        //public CourierVehicle(EventClass events, FullPosition spawnPosition, VehicleHash hash, string numberplate, int numberplatestyle, int creatorId, Color primaryColor, Color secondaryColor, float enginePowerMultiplier = 0, float engineTorqueMultiplier = 0, Character character = null, GroupModel groupModel = null) : base(events, spawnPosition, hash, numberplate, numberplatestyle, creatorId, primaryColor, secondaryColor, enginePowerMultiplier, engineTorqueMultiplier, character, groupModel)
        //{
        //}
    }
}