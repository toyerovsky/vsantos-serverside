/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Entities.Core.Building;
using VRP.Serverside.Entities.Core.Vehicle;

namespace VRP.Serverside.Core.Scripts
{
    public class CoreScript : Script
    {
        [ServerEvent(Event.ChatMessage)]
        public void OnChatMessage(Client sender, string message)
        {
            AccountEntity account = sender.GetAccountEntity();
            if (message == "tu" && account.HereHandler != null)
            {
                account.HereHandler.Invoke(sender);
                account.HereHandler = null;
            }
        }

        [ServerEvent(Event.PlayerDisconnected)]
        private void OnOnPlayerDisconnected(Client client, byte type, string reason)
        {
            AccountEntity account = client.GetAccountEntity();
            account?.Dispose();
        }

        /// <summary>
        /// Server stuff initialization
        /// </summary>
        [ServerEvent(Event.ResourceStart)]
        public void Event_OnResourceStart()
        {
            NAPI.Server.SetDefaultSpawnLocation(new Vector3(-1666f, -1020f, 12f));
            EntityHelper.LoadEntities();
        }

        private float _currentRotation = 0f;
        [ServerEvent(Event.Update)]
        private void Event_OnUpdate()
        {
            if (!EntityHelper.GetBuildings().Any())
                return;
            //Kręcące się markery od budynków
            if (Math.Abs(_currentRotation - 360f) < 0.4)
                _currentRotation = 0;

            _currentRotation += 0.4f;

            foreach (BuildingEntity building in EntityHelper.GetBuildings())
            {
                building.BuildingMarker.Rotation =
                    new Vector3(building.BuildingMarker.Rotation.X, building.BuildingMarker.Rotation.Y, _currentRotation);
            }
        }

        private void Event_OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            //args[0] float X
            //args[1] float Y
            //args[2] float Z
            //Jak przesyłamy Vector3 to nie działa
            if (eventName == "ChangePosition")
            {
                sender.Position = new Vector3((float)arguments[0], (float)arguments[1], (float)arguments[2]);
            }
            //To zdarzenie musi mieć tylko jedną subskrypcę
            else if (eventName == "InvokeWaypointVector")
            {
                if (sender.HasData("WaypointVectorHandler"))
                {
                    Action<Vector3> waypointAction = (Action<Vector3>)sender.GetData("WaypointPositionHandler");
                    waypointAction.Invoke(new Vector3((float)arguments[0], (float)arguments[1], (float)arguments[2]));
                }
            }
        }

        [ServerEvent(Event.ResourceStop)]
        public void OnResourceStop()
        {
            Task dbStop = Task.Run(() =>
            {
                foreach (AccountEntity account in EntityHelper.GetAccounts()
                    .Where(x => x.CharacterEntity != null))
                    account.Dispose();

                foreach (VehicleEntity vehicle in EntityHelper.GetVehicles())
                {
                    vehicle.Save();
                    vehicle.Dispose();
                }
            });
            dbStop.Wait();
        }
    }
}
