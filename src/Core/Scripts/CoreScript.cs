/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using Serverside.Constant;
using Serverside.Core.Database;
using Serverside.Core.Extensions;
using Serverside.Core.Login;
using Serverside.Entities;
using Serverside.Entities.Core;

namespace Serverside.Core.Scripts
{
    public class CoreScript : Script
    {
        public CoreScript()
        {
            Event.OnResourceStart += Event_OnResourceStart;
            Event.OnResourceStop += Event_OnResourceStop;
            Event.OnUpdate += Event_OnUpdate;
        }

        private void Event_OnResourceStart()
        {
            EntityManager.LoadEntities(Event);
            Tools.ConsoleOutput($"[{nameof(CoreScript)}] {ConstantMessages.ResourceStartMessage}", ConsoleColor.DarkMagenta);
        }

        private float _currentRotation = 0;
        private void Event_OnUpdate()
        {
            if (EntityManager.GetBuildings().Count == 0)
                return;
            //Kręcące się markery od budynków
            if (Math.Abs(_currentRotation - 360f) < 0.4)
                _currentRotation = 0;

            _currentRotation += 0.4f;

            foreach (var building in EntityManager.GetBuildings())
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
                    var waypointAction = (Action<Vector3>)sender.GetData("WaypointPositionHandler");
                    waypointAction.Invoke(new Vector3((float)arguments[0], (float)arguments[1], (float)arguments[2]));
                }
            }
        }

        private void Event_OnResourceStop()
        {
            Task dbStop = Task.Run(() =>
            {
                foreach (var account in EntityManager.GetAccounts().Where(x => x.Value?.CharacterEntity != null))
                {
                    //Zmiana postaci pola Online w postaci po wyłączeniu serwera dla graczy którzy byli online
                    account.Value.CharacterEntity.DbModel.Online = false;

                    //Zmiana w przedmiocie pola CurrentlyInUse na false
                    foreach (var item in account.Value.CharacterEntity.DbModel.Items.Where(
                        i => i.CurrentlyInUse).ToList())
                    {
                        item.CurrentlyInUse = false;
                    }

                    account.Value.DbModel.Online = false;
                }

                foreach (var vehicle in EntityManager.GetVehicles())
                {
                    vehicle.Dispose();
                }

                using (var ctx = RolePlayContextFactory.NewContext())
                    ctx.SaveChanges();
            });
            dbStop.Wait();
        }

        private void API_onPlayerFinishedDownload(Client player)
        {
            Tools.ConsoleOutput($"PlayerFinishedDownload: {player.SocialClubName}", ConsoleColor.Blue);
            LoginScript.LoginMenu(player);
        }

        private void API_OnPlayerDisconnectedHandler(Client player, string reason)
        {
            Tools.ConsoleOutput($"PlayerDisconnected: {player.SocialClubName}", ConsoleColor.Blue);
            AccountEntity account = player.GetAccountEntity();
            if (account == null) return;
            LoginScript.LogOut(account);
        }

        private void Client_OnPlayerDimensionChanged(object player, DimensionChangeEventArgs e)
        {
            AccountEntity account = e.Player.GetAccountEntity();
            account.CharacterEntity.DbModel.CurrentDimension = e.CurrentDimension;
            account.CharacterEntity.Save();
        }
    }
}
