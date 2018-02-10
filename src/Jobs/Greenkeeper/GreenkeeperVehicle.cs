﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using GTANetworkAPI;
using GTANetworkInternals;
using Serverside.Core.Database.Models;
using Serverside.Core.Extensions;
using Serverside.Jobs.Base;
using Serverside.Jobs.Enums;

namespace Serverside.Jobs.Greenkeeper
{
    public class GreenkeeperVehicle : JobVehicleEntity
    {
        private GreenkeeperWorker WorkerInVehicle { get; set; }

        public GreenkeeperVehicle(EventClass events, VehicleModel model) : base(events, model)
        {
            Events.OnPlayerEnterVehicle += Events_OnPlayerEnterVehicle;
            Events.OnVehicleDamage += Events_OnVehicleDamage;
            Events.OnPlayerExitVehicle += Events_OnPlayerExitVehicle;
        }
        
        private void Events_OnVehicleDamage(Vehicle entity, float lossFirst, float lossSecond)
        {
            if (entity == GameVehicle)
                WorkerInVehicle.CurrentSalary -= Convert.ToDecimal(lossFirst + lossSecond) / 10;
        }

        private void Events_OnPlayerEnterVehicle(Client player, Vehicle vehicle, sbyte seatId)
        {
            if (player.GetAccountEntity().CharacterEntity.DbModel.Job != JobType.Greenkeeper)
            {
                player.Notify("Aby skorzystać z tego pojazdu musisz podjąć pracę ~h~ Greenkeeper");
                return;
            }

            player.Notify("Pojazd do którego wsiadłeś zapewnił Ci pracodawca. Jesteś zobowiązany umową do pokrycia wszelkich strat.");

            WorkerInVehicle = new GreenkeeperWorker(Events, player.GetAccountEntity(), this);
            WorkerInVehicle.Start();
        }

        private void Events_OnPlayerExitVehicle(Client sender, Vehicle vehicle)
        {
            sender.TriggerEvent("JobTextVisibility", false);
        }
    }
}