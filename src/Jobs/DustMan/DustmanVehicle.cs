/* Copyright (C) Przemysław Postrach - All Rights Reserved
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

namespace Serverside.Jobs.DustMan
{
    public class DustmanVehicle : JobVehicleEntity
    {
        private DustmanWorker WorkerInVehicle { get; set; }

        public DustmanVehicle(EventClass events, VehicleModel model) : base(events, model)
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
                player.Notify("Aby skorzystać z tego pojazdu musisz podjąć pracę ~h~ Śmieciarz");
                return;
            }

            player.Notify("Pojazd do którego wsiadłeś zapewnił Ci pracodawca. Jesteś zobowiązany umową do pokrycia wszelkich strat.");

            WorkerInVehicle = new DustmanWorker(Events, player.GetAccountEntity(), this);
            WorkerInVehicle.Start();
        }
    }
}