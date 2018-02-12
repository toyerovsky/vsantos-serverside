/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Serverside.Core.Extensions;
using Serverside.Core.Scripts;
using Serverside.Entities;
using Serverside.Entities.Core;
using Serverside.Entities.Core.Vehicle;

namespace Serverside.WheelMenu
{
    public class WheelMenuScript : Script
    {
        private void Event_OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            if (eventName == "RequestWheelMenu")
            {
                //args[0] to NetHandle które przyszło z RayCast
                //nie używam as ponieważ do struktury nie wolno
                if (!(arguments[0] is NetHandle)) return;
                if (NAPI.Player.GetPlayerFromHandle((NetHandle)arguments[0]) != null)
                {

                }
                else if (EntityManager.GetVehicle((NetHandle)arguments[0]) != null)
                {
                    WheelMenu wheel = new WheelMenu(PrepareDataSource(sender, EntityManager.GetVehicle((NetHandle)arguments[0])), sender);
                    sender.SetData("WheelMenu", wheel);
                }
            }
            else if (eventName == "UseWheelMenuItem")
            {
                //args[0] to nazwa opcji
                WheelMenu wheel = (WheelMenu)sender.GetData("WheelMenu");
                wheel.WheelMenuItems.First(x => x.Name == (string)arguments[0]).Use();
                wheel.Dispose();
            }
        }

        private List<WheelMenuItem> PrepareDataSource(Client sender, object target, params object[] args)
        {
            List<WheelMenuItem> menuItems = new List<WheelMenuItem>();
            if (target is Client)
            {

            }
            else if (target is VehicleEntity vehicle)
            {
                if (VehicleScript.GetVehicleDoorCount((VehicleHash)vehicle.GameVehicle.Model) >= 4)
                {
                    menuItems.Add(new WheelMenuItem("Maska", sender, target, (s, e) => VehicleScript.ChangeDoorState(s, ((VehicleEntity)e).GameVehicle.Handle, (int)Doors.Hood)));
                    menuItems.Add(new WheelMenuItem("Bagaznik", sender, target, (s, e) => VehicleScript.ChangeDoorState(s, ((VehicleEntity)e).GameVehicle.Handle, (int)Doors.Trunk)));
                }
                if (vehicle.DbModel.Character == sender.GetAccountEntity().CharacterEntity.DbModel)
                {
                    menuItems.Add(new WheelMenuItem("Zamek", sender, null, (s, e) => VehicleScript.ChangePlayerVehicleLockState(s)));
                }
                menuItems.Add(new WheelMenuItem("Rejestracja", sender, target, (s, e) => VehicleScript.ShowVehiclesInformation(s, ((VehicleEntity)e).DbModel, true)));
            }
            return menuItems;
        }
    }
}