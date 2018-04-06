/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using VRP.Core.Enums;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Economy.Jobs.Base;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Economy.Jobs.Greenkeeper
{
    public class GreenkeeperWorker : JobWorkerController
    {
        private bool InProgress { get; set; }
        private int MetersCounter { get; set; }
        private List<Vector3> VisitedPoints { get; set; } = new List<Vector3>();

        public GreenkeeperWorker(AccountEntity player, JobVehicleEntity vehicle)
            : base(player, vehicle)
        {
            player.Client.TriggerEvent("JobTextVisibility", true);
            Player = player;
            InProgress = true;
            JobVehicle = vehicle;
        }

        private void OnUpdateHandler()
        {
            if (Player.Client.IsInVehicle && Player.Client.Vehicle == JobVehicle.GameVehicle)
            {
                if (VisitedPoints.Any(p => p.DistanceTo(Player.Client.Position) < 20) || VisitedPoints.Any(p => p == Player.Client.Position)) return;
                MetersCounter++;
                VisitedPoints.Add(Player.Client.Position);
                Player.Client.TriggerEvent("JobText_Changed", $"{MetersCounter}/300m\u00B2");
            }
        }

        public override void Start()
        {
            Player.Client.Notify("Rozpocząłeś pracę ogrodnika, jeździj po polu golfowym, aby kosić trawnik.", NotificationType.Info);
            //TODO: dzwonienie na numer alarmowy
            Player.Client.Notify("Jeżeli wyjedziesz z obszaru pola golfowego zostanie wysłane zgłoszenie na numer alarmowy.", NotificationType.Info);
        }

        public override void Stop()
        {
            InProgress = false;
            
        }
    }
}