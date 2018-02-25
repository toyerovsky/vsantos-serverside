/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using GTANetworkInternals;
using Serverside.Core.Extensions;
using Serverside.Entities.Core;

namespace Serverside.Jobs.Base
{
    public abstract class JobWorkerController
    {
        public AccountEntity Player { get; set; }
        public JobVehicleEntity JobVehicle { get; set; }

        protected JobWorkerController(AccountEntity player, JobVehicleEntity jobVehicle)
        {
            Player = player;
            JobVehicle = jobVehicle;
        }

        public virtual decimal CurrentSalary
        {
            get => Player.CharacterEntity.DbModel.MoneyJob ?? 0;
            set
            {
                if (value < 0)
                {
                    Player.CharacterEntity.DbModel.MoneyJob = 0;
                    ReleaseWorker();
                }
                else
                {
                    Player.CharacterEntity.DbModel.MoneyJob = value;
                }
                Player.CharacterEntity.Save();
            }
        }

        /// <summary>
        /// Metoda do zwolnienia pracownika w razie nieprawidłowego postępowania
        /// </summary>
        public virtual void ReleaseWorker()
        {
            Player.Client.Notify("Nie wypracowałeś wystarczającej ilości gotówki na pokrycie szkód.");
            Player.Client.Notify("Zostałeś zwolniony. Zanim ponownie znajdziesz zatrudnienie minie trochę czasu.");
            Stop();
            Player.CharacterEntity.DbModel.Job = 0;
            Player.CharacterEntity.DbModel.JobReleaseDate = DateTime.Now;
            Player.CharacterEntity.Save();
        }

        public virtual void Start()
        {

        }

        public virtual void Stop()
        {

        }
    }
}