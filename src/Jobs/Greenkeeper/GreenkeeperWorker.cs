/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using GTANetworkInternals;
using Serverside.Core.Extensions;
using Serverside.Entities.Core;
using Serverside.Jobs.Base;

namespace Serverside.Jobs.Greenkeeper
{
    public class GreenkeeperWorker : JobWorkerController
    {
        private bool InProgress { get; set; }
        private int MetersCounter { get; set; }
        private List<Vector3> VisitedPoints { get; set; } = new List<Vector3>();

        public GreenkeeperWorker(EventClass events, AccountEntity player, JobVehicleEntity vehicle)
            : base(events, player, vehicle)
        {
            player.Client.TriggerEvent("JobTextVisibility", true);
            Player = player;
            InProgress = true;
            JobVehicle = vehicle;

            Events.OnUpdate += OnUpdateHandler;
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
            Player.Client.Notify("Rozpocząłeś pracę ogrodnika, jeździj po polu golfowym, aby kosić trawnik.");
            Player.Client.Notify("Jeżeli wyjedziesz z obszaru pola golfowego zostanie wysłane zgłoszenie na numer alarmowy.");
        }

        public override void Stop()
        {
            InProgress = false;
            Events.OnUpdate -= OnUpdateHandler;
        }
    }
}