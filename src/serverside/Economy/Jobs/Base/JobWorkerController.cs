/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Economy.Jobs.Base
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
            get => Player.CharacterEntity.DbModel.PartTimeJobWorkerModel.Salary;
            set
            {
                if (value < 0)
                {
                    Player.CharacterEntity.DbModel.PartTimeJobWorkerModel.Salary = 0;
                    ReleaseWorker();
                }
                else
                {
                    Player.CharacterEntity.DbModel.PartTimeJobWorkerModel.Salary = value;
                }
                Player.CharacterEntity.Save();
            }
        }

        /// <summary>
        /// Metoda do zwolnienia pracownika w razie nieprawidłowego postępowania
        /// </summary>
        public virtual void ReleaseWorker()
        {
            Player.Client.SendInfo("Nie wypracowałeś wystarczającej ilości gotówki na pokrycie szkód.");
            Player.Client.SendInfo("Zostałeś zwolniony. Zanim ponownie znajdziesz zatrudnienie minie trochę czasu.");
            Stop();
            Player.CharacterEntity.DbModel.PartTimeJobWorkerModel = null;
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