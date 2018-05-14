/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using VRP.Core.Database.Forum;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Entities.Core.Building;
using VRP.Serverside.Entities.Core.Vehicle;
using VRP.Serverside.Constant.RemoteEvents;

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

        /// <summary>
        /// Server stuff initialization
        /// </summary>
        [ServerEvent(Event.ResourceStart)]
        public void Event_OnResourceStart()
        {
            NAPI.Server.SetDefaultSpawnLocation(new Vector3(-1666f, -1020f, 12f));
            NAPI.Server.SetAutoRespawnAfterDeath(false);
            EntityHelper.LoadEntities();
        }

        private float _currentRotation = 0f;
        [ServerEvent(Event.Update)]
        private void Event_OnUpdate()
        {
            if (EntityHelper.GetBuildings().Any())
            {
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
        }

        [RemoteEvent(RemoteEvents.ChangePosition)]
        public void ChangePositionHandler(Client sender, params object[] arguments)
        {
            //args[0] float X
            //args[1] float Y
            //args[2] float Z
            //Jak przesyłamy Vector3 to nie działa

            sender.Position = new Vector3((float)arguments[0], (float)arguments[1], (float)arguments[2]);
        }

        [RemoteEvent(RemoteEvents.InvokeWaypointVector)]
        public void InvokeWaypointVectorHandler(Client sender, params object[] arguments)
        {

            //To zdarzenie musi mieć tylko jedną subskrypcę
            if (sender.HasData("WaypointVectorHandler"))
            {
                Action<Vector3> waypointAction = (Action<Vector3>)sender.GetData("WaypointPositionHandler");
                waypointAction.Invoke(new Vector3((float)arguments[0], (float)arguments[1], (float)arguments[2]));
            }
        }


        [ServerEvent(Event.ResourceStop)]
        public void OnResourceStop()
        {
            foreach (AccountEntity account in EntityHelper.GetAccounts()
                .Where(x => x.CharacterEntity != null))
                account.Dispose();

            foreach (VehicleEntity vehicle in EntityHelper.GetVehicles())
            {
                vehicle.Save();
                vehicle.Dispose();
            }
        }
    }
}
