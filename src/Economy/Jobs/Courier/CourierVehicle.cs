/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using Serverside.Core.Database.Models;
using Serverside.Economy.Jobs.Base;

namespace Serverside.Economy.Jobs.Courier
{
    public class CourierVehicle : JobVehicleEntity
    {
        public CourierVehicle(VehicleModel model) : base(model)
        {
        }

        //public CourierVehicle(EventClass events, FullPosition spawnPosition, VehicleHash hash, string numberplate, int numberplatestyle, int creatorId, Color primaryColor, Color secondaryColor, float enginePowerMultiplier = 0, float engineTorqueMultiplier = 0, CharacterModel character = null, GroupModel groupModel = null) : base(events, spawnPosition, hash, numberplate, numberplatestyle, creatorId, primaryColor, secondaryColor, enginePowerMultiplier, engineTorqueMultiplier, character, groupModel)
        //{
        //}
    }
}