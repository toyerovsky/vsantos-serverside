/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using GTANetworkAPI;
using VRP.DAL.Database.Models.Vehicle;
using VRP.DAL.Enums;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Economy.Jobs.Base;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Economy.Jobs.Greenkeeper
{
    public class GreenkeeperVehicle : JobVehicleEntity
    {
        private GreenkeeperWorker WorkerInVehicle { get; set; }

        public GreenkeeperVehicle(VehicleModel model) : base(model)
        {
        }
        
        private void Events_OnVehicleDamage(Vehicle entity, float lossFirst, float lossSecond)
        {
            if (entity == GameVehicle)
                WorkerInVehicle.CurrentSalary -= Convert.ToDecimal(lossFirst + lossSecond) / 10;
        }

        private void Events_OnPlayerEnterVehicle(Client player, Vehicle vehicle, sbyte seatId)
        {
            CharacterEntity character = player.GetAccountEntity().CharacterEntity;
            if (character.DbModel.PartTimeJobWorkerModel.PartTimeJobEmployerModel.PartTimeJobModel.JobType!= JobType.Greenkeeper)
            {
                player.SendInfo("Aby skorzystać z tego pojazdu musisz podjąć pracę ogrodnik.");
                return;
            }

            player.SendInfo("Pojazd do którego wsiadłeś zapewnił Ci pracodawca. Jesteś zobowiązany umową do pokrycia wszelkich strat.");

            WorkerInVehicle = new GreenkeeperWorker(player.GetAccountEntity(), this);
            WorkerInVehicle.Start();
        }

        private void Events_OnPlayerExitVehicle(Client sender, Vehicle vehicle)
        {
            sender.TriggerEvent("JobTextVisibility", false);
        }
    }
}