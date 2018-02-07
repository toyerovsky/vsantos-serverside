/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Serverside.Admin.Enums;
using Serverside.Constant;
using Serverside.Core;
using Serverside.Core.Database.Models;
using Serverside.Core.Extensions;
using Serverside.Core.Repositories;
using Serverside.Jobs.Greenkeeper;
using Serverside.Core.Serialization.Json;
using Serverside.Core.Serialization.Xml;
using Serverside.Jobs.Base;
using Serverside.Jobs.Courier;
using Serverside.Jobs.DustMan;
using Serverside.Jobs.DustMan.Models;
using Serverside.Jobs.Enums;

namespace Serverside.Jobs
{
    public class JobsScript : Script
    {
        public static List<Job> Jobs { get; set; }
        public static List<GarbageModel> Garbages { get; set; } = new List<GarbageModel>();

        //private bool _resetFlag = true;

        public JobsScript()
        {
            string jsonDir = ServerInfo.JsonDirectory;

            var dustmanJob = new DustmanJob(Event, "Śmieciarz", 500, $"{jsonDir}DustmanVehicles\\");

            var greenkeeperJob = new GreenkeeperJob(Event, "Greenkeeper", 400, $"{jsonDir}GreenkeeperVehicles\\");

            var courierJob = new CourierJob(Event, "Kurier", 500, $"{jsonDir}CourierVehicles\\");

            Jobs = new List<Job>
            {
                dustmanJob, greenkeeperJob, courierJob
            };

            Event.OnResourceStart += API_OnResourceStart;
            //NAPI.onUpdate += OnUpdate;   
        }

        private void API_OnResourceStart()
        {
            Tools.ConsoleOutput($"[{nameof(JobsScript)}] {Messages.ResourceStartMessage}", ConsoleColor.DarkMagenta);
            Garbages = XmlHelper.GetXmlObjects<GarbageModel>($"{ServerInfo.XmlDirectory}JobGarbages\\");

            foreach (var garbage in Garbages)
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
                foreach (var character in repository.GetAll())
                {
                    if (character.JobLimit == 0)
                        continue;

                    character.JobLimit = 0;
                }
                repository.Save();
            }
        }

        public static GarbageModel GetRandomGarbage() => Garbages[new Random().Next(Garbages.Count)];

        #endregion

        #region ADMIN COMMANDS

        [Command("usunsmietnik")]
        public void DeleteGarbage(Client sender)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster)
            {
                sender.Notify("Nie posiadasz uprawnień do usuwania śmietnika.");
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
                sender.Notify("Nie posiadasz uprawnień do tworzenia śmietnika.");
                return;
            }

            sender.Notify("Ustaw się w wybranej pozycji, a następnie wpisz \"tu\".");
            sender.Notify("...użyj /diag aby poznać swoją obecną pozycję.");

            Event.OnChatMessage += Handler;

            void Handler(Client o, string command, CancelEventArgs cancel)
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

                    XmlHelper.AddXmlObject(garbage, $@"{ServerInfo.XmlDirectory}JobGarbages\");
                    Garbages.Add(garbage);
                    sender.Notify("Dodawanie śmietnika zakończyło się pomyślnie.");
                    Event.OnChatMessage -= Handler;
                }
            }
        }

        [Command("dodajautopraca", "~y~ UŻYJ ~w~ /dodajautopraca [typ]")]
        public void AddVehicleToJob(Client sender, JobType type)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster)
            {
                sender.Notify("Nie posiadasz uprawnień do dodawania auta do pracy.");
                return;
            }

            sender.Notify("Wsiądź do pojazdu a następnie wpisz \"ok\".");

            Event.OnChatMessage += Handler;

            void Handler(Client o, string command, CancelEventArgs cancel)
            {
                if (o == sender && command == "ok")
                {
                    if (!o.IsInVehicle || o.Vehicle.GetVehicleEntity() == null)
                    {
                        o.Notify("Musisz znajdować się w pojeździe.");
                        o.Notify("Dodawanie auta do pracy zakończyło się ~h~ ~r~niepomyślnie.");
                        return;
                    }

                    var vehicle = o.Vehicle.GetVehicleEntity();
                    AddVehicleToJob(vehicle.DbModel, type);

                    o.Notify("Dodawanie auta do pracy zakończyło się ~h~ ~g~pomyślnie.");
                    Event.OnChatMessage -= Handler;
                }
            }
        }

        private void AddVehicleToJob(VehicleModel data, JobType type)
        {
            if (type == JobType.Dustman)
            {
                var vehicle = new DustmanVehicleEntity(Event, data);
                var job = (DustmanJob)Jobs.First(
                    x => x.GetType() == typeof(DustmanJob));
                job.Vehicles.Add(vehicle);
                JsonHelper.AddJsonObject(vehicle.DbModel, job.JsonDirectory);
            }
            else if (type == JobType.Greenkeeper)
            {
                var vehicle = new GreenkeeperVehicle(Event, data);
                var job = (GreenkeeperJob)Jobs.First(
                    x => x.GetType() == typeof(GreenkeeperJob));
                job.Vehicles.Add(vehicle);
                JsonHelper.AddJsonObject(vehicle.DbModel, job.JsonDirectory);
            }
            else if (type == JobType.Courier)
            {
                var vehicle = new CourierVehicle(Event, data);
                var job = (CourierJob)Jobs.First
                    (x => x.GetType() == typeof(CourierJob));
                JsonHelper.AddJsonObject(vehicle.DbModel, job.JsonDirectory);
            }
        }
        #endregion
    }
}