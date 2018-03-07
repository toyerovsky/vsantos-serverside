﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Economy.Jobs.Base;

namespace VRP.Serverside.Economy.Jobs.Dustman
{
    public class DustmanVehicleEntity : JobVehicleEntity
    {
        private DustmanWorker WorkerInVehicle { get; set; }

        public DustmanVehicleEntity(VehicleModel model) : base(model)
        {

        }

        private void Events_OnPlayerEnterVehicle(Client player, Vehicle vehicle, sbyte seatId)
        {
            if (player.GetAccountEntity().CharacterEntity.DbModel.Job != JobType.Dustman)
            {
                player.Notify("Aby skorzystać z tego pojazdu musisz podjąć pracę ~h~ Śmieciarz");
                return;
            }

            player.Notify("Pojazd do którego wsiadłeś zapewnił Ci pracodawca. Jesteś zobowiązany umową do pokrycia wszelkich strat.");

            WorkerInVehicle = new DustmanWorker(player.GetAccountEntity(), this);
            WorkerInVehicle.Start();
        }

        private void Events_OnVehicleDamage(Vehicle entity, float lossFirst, float lossSecond)
        {
            if (entity == GameVehicle)
            {
                decimal charge = Convert.ToDecimal(lossFirst + lossSecond) / 10;
                WorkerInVehicle.CurrentSalary -= charge;
            }
        }
    }
}