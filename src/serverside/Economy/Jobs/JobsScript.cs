/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using System.IO;
using System.Linq;
using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Core.Repositories;
using VRP.Core.Serialization;
using VRP.Core.Tools;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Economy.Jobs.Base;
using VRP.Serverside.Economy.Jobs.Courier;
using VRP.Serverside.Economy.Jobs.Dustman;
using VRP.Serverside.Economy.Jobs.Dustman.Models;
using VRP.Serverside.Economy.Jobs.Greenkeeper;
using VRP.Serverside.Entities.Core.Vehicle;

namespace VRP.Serverside.Economy.Jobs
{
    public class JobsScript : Script
    {
        public static List<Job> Jobs { get; set; }
        public static List<GarbageModel> Garbages { get; set; } = new List<GarbageModel>();

        //private bool _resetFlag = true;

        public JobsScript()
        {
            DustmanJob dustmanJob = new DustmanJob("Śmieciarz", 500, Path.Combine(Utils.JsonDirectory, "DustmanVehicles\\"));

            GreenkeeperJob greenkeeperJob = new GreenkeeperJob("Greenkeeper", 400, Path.Combine(Utils.JsonDirectory, "GreenkeeperVehicles\\"));

            CourierJob courierJob = new CourierJob("Kurier", 500, Path.Combine(Utils.JsonDirectory, "CourierVehicles\\"));

            Jobs = new List<Job>
            {
                dustmanJob, greenkeeperJob, courierJob
            };
        }

        private void API_OnResourceStart()
        {
            Garbages = XmlHelper.GetXmlObjects<GarbageModel>($"{Utils.XmlDirectory}JobGarbages\\");

            foreach (GarbageModel garbage in Garbages)
            {
                if (garbage.GtaPropId != 0)
                    NAPI.Object.CreateObject(garbage.GtaPropId, garbage.Position, garbage.Rotation);
            }
        }

        //private void OnUpdate()
        //{
        //    if (_resetFlag && Math.Abs(ServerInfo.Instance.Model.JobsResetTime.CompareTo(DateTime.Now)) <= 1000)
        //    {
        //        _resetFlag = false;
        //        ResetJobLimit();
        //    }
        //}

        #region STATIC

        private static void ResetJobLimit()
        {
            using (CharactersRepository repository = new CharactersRepository())
            {
                foreach (CharacterModel character in repository.GetAll())
                {
                    if (character.JobLimit == 0)
                        continue;

                    character.JobLimit = 0;
                }
                repository.Save();
            }
        }
        public static GarbageModel GetRandomGarbage() => Garbages[Utils.RandomRange(Garbages.Count)];

        #endregion

        #region ADMIN COMMANDS

        [Command("usunsmietnik")]
        public void DeleteGarbage(Client sender)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster)
            {
                sender.SendWarning("Nie posiadasz uprawnień do usuwania śmietnika.");
                return;
            }

            // FixMe
            //var garbage = Garbages.Where().OrderBy(x => x.Position).ToList()[0];

            //if (XmlHelper.TryDeleteXmlObject(garbage.FilePath))
            //{
            //    if (garbage.GtaPropId != 0)
            //        NAPI.Object.DeleteObject(sender, garbage.Position, garbage.GtaPropId);
            //    Garbages.Remove(garbage);
            //    sender.Notify($"Usuwanie śmietnika na pozycji {garbage.Position} zakończyło się pomyślnie.");
            //}
            //else
            //{
            //    sender.Notify("Usuwanie śmietnika zakończyło się niepomyślnie.");
            //}
        }

        [Command("dodajsmietnik", "~y~ UŻYJ ~w~ /dodajsmietnik (id obiektu)")]
        public void AddGarbage(Client sender, int prop = 0)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster)
            {
                sender.SendWarning("Nie posiadasz uprawnień do tworzenia śmietnika.");
                return;
            }

            sender.SendInfo("Ustaw się w wybranej pozycji, a następnie wpisz \"tu\", użyj ctrl + alt + shift + d aby poznać swoją obecną pozycję.");

            void Handler(Client o, string command)
            {
                if (o == sender && command == "tu")
                {
                    GarbageModel garbage = new GarbageModel
                    {
                        GtaPropId = prop,
                        CreatorForumName = sender.GetAccountEntity().DbModel.Name,
                        Position = sender.Position,
                        Rotation = sender.Rotation
                    };

                    XmlHelper.AddXmlObject(garbage, Path.Combine(Utils.XmlDirectory, "JobGarbages"));
                    Garbages.Add(garbage);
                    sender.SendInfo("Dodawanie śmietnika zakończyło się pomyślnie.");
                }
            }
        }

        [Command("dodajautopraca", "~y~ UŻYJ ~w~ /dodajautopraca [typ]")]
        public void AddVehicleToJob(Client sender, JobType type)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster)
            {
                sender.SendWarning("Nie posiadasz uprawnień do dodawania auta do pracy.");
                return;
            }

            sender.SendInfo("Wsiądź do pojazdu a następnie wpisz \"ok\".");

            void Handler(Client o, string command)
            {
                if (o == sender && command == "ok")
                {
                    if (!o.IsInVehicle || o.Vehicle.GetVehicleEntity() == null)
                    {
                        o.SendError("Musisz znajdować się w pojeździe.");
                        o.SendError("Dodawanie auta do pracy zakończyło się niepomyślnie.");
                        return;
                    }

                    VehicleEntity vehicle = o.Vehicle.GetVehicleEntity();
                    AddVehicleToJob(vehicle.DbModel, type);

                    o.SendInfo("Dodawanie auta do pracy zakończyło się pomyślnie.");
                }
            }
        }

        private void AddVehicleToJob(VehicleModel data, JobType type)
        {
            if (type == JobType.Dustman)
            {
                DustmanVehicle vehicle = new DustmanVehicle(data);
                if (Jobs.First(x => x is DustmanJob) is DustmanJob job)
                {
                    job.Vehicles.Add(vehicle);
                    JsonHelper.AddJsonObject(vehicle.DbModel, job.JsonDirectory);
                    vehicle.Spawn();
                }
            }
            else if (type == JobType.Greenkeeper)
            {
                GreenkeeperVehicle vehicle = new GreenkeeperVehicle(data);
                if (Jobs.First(x => x is GreenkeeperJob) is GreenkeeperJob job)
                {
                    job.Vehicles.Add(vehicle);
                    JsonHelper.AddJsonObject(vehicle.DbModel, job.JsonDirectory);
                    vehicle.Spawn();
                }
            }
            else if (type == JobType.Courier)
            {
                CourierVehicle vehicle = new CourierVehicle(data);
                if (Jobs.First(x => x is CourierJob) is CourierJob job)
                {
                    job.Vehicles.Add(vehicle);
                    JsonHelper.AddJsonObject(vehicle.DbModel, job.JsonDirectory);
                    vehicle.Spawn();
                }
            }
        }
        #endregion
    }
}