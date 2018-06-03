/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Database.Models.Vehicle;
using VRP.Core.Enums;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Economy.Jobs.Base;

namespace VRP.Serverside.Economy.Jobs.Dustman
{
    public class DustmanVehicle : JobVehicleEntity
    {
        private DustmanWorker WorkerInVehicle { get; set; }

        public DustmanVehicle(VehicleModel model) : base( model)
        {
        }

        //public DustmanVehicle(FullPosition spawnPosition, VehicleHash hash, string numberplate, int numberplatestyle, int creatorId, Color primaryColor, Color secondaryColor, float enginePowerMultiplier = 0, float engineTorqueMultiplier = 0, CharacterModel character = null, GroupModel groupModel = null)
        //    : base(spawnPosition, hash, numberplate, numberplatestyle, creatorId, primaryColor, secondaryColor, enginePowerMultiplier, engineTorqueMultiplier, character, groupModel)
        //{
        //    Events.OnPlayerEnterVehicle += OnPlayerEnterVehicle;
        //    Events.OnVehicleDamage += OnVehicleHealthChange;
        //}

        private void OnVehicleHealthChange(NetHandle entity, float oldValue)
        {
            if (entity == GameVehicle)
            {
                decimal charge = Convert.ToDecimal(oldValue - GameVehicle.Health) / 10;
                WorkerInVehicle.CurrentSalary -= charge;
            }
        }

        private void OnPlayerEnterVehicle(Client player, NetHandle vehicle)
        {
            if (player.GetAccountEntity().CharacterEntity.DbModel.Job != JobType.Dustman)
            {
                player.SendInfo("Aby skorzystać z tego pojazdu musisz podjąć pracę operator śmieciarki.");
                return;
            }

            player.SendInfo("Pojazd do którego wsiadłeś zapewnił Ci pracodawca. Jesteś zobowiązany umową do pokrycia wszelkich strat.");

            WorkerInVehicle = new DustmanWorker(player.GetAccountEntity(), this);
            WorkerInVehicle.Start();
        }
    }
}