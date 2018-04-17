/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using VRP.Core.Enums;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Entities.Core.Vehicle;
using VRP.Serverside.Constant.RemoteEvents;

namespace VRP.Serverside.Core.WheelMenu
{
    public class WheelMenuScript : Script
    {
        [RemoteEvent(RemoteEvents.RequestWheelMenu)]
        public void RequestWheelMenuHandler(Client sender, params object[] arguments)
        {
            int entityId = Convert.ToInt32(arguments[0]);
            if (Enum.TryParse(typeof(EntityType), arguments[1].ToString(), true, out var entityType))
            {
                if ((EntityType)entityType == EntityType.Player)
                {
                    var character = EntityHelper.GetAccountByCharacterId(entityId);
                }
                else if ((EntityType)entityType == EntityType.Vehicle)
                {
                    var vehicle = EntityHelper.GetVehicle(entityId);
                    WheelMenu wheel = new WheelMenu(PrepareDataSource(sender, vehicle), sender);
                    sender.SetData("WheelMenu", wheel);
                }
            }
        }

        [RemoteEvent(RemoteEvents.UseWheelMenuItem)]
        public void UseWheelMenuItemHandler(Client sender, params object[] arguments)
        {
            //args[0] to nazwa opcji
            WheelMenu wheel = (WheelMenu)sender.GetData("WheelMenu");
            wheel.WheelMenuItems.First(x => x.Name == (string)arguments[0]).Use();
            wheel.Dispose();
        }

        private List<WheelMenuItem> PrepareDataSource(Client sender, object target, params object[] args)
        {
            List<WheelMenuItem> menuItems = new List<WheelMenuItem>();
            if (target is Client)
            {

            }
            else if (target is VehicleEntity vehicle)
            {
                CharacterEntity senderCharacter = sender.GetAccountEntity().CharacterEntity;
                if (VehicleScript.GetVehicleDoorCount((VehicleHash)vehicle.GameVehicle.Model) >= 4)
                {
                    menuItems.Add(new WheelMenuItem("hood", senderCharacter, target,
                        (s, e) => VehicleScript.ChangeDoorState(s, ((VehicleEntity)e).GameVehicle.Handle, (int)Doors.Hood)));
                    menuItems.Add(new WheelMenuItem("trunk", senderCharacter, target,
                        (s, e) => VehicleScript.ChangeDoorState(s, ((VehicleEntity)e).GameVehicle.Handle, (int)Doors.Trunk)));
                }
                if (vehicle.DbModel.Character == sender.GetAccountEntity().CharacterEntity.DbModel)
                {
                    menuItems.Add(new WheelMenuItem("lock", senderCharacter, null,
                        (s, e) => VehicleScript.ChangePlayerVehicleLockState(s)));
                }
                menuItems.Add(new WheelMenuItem("numberplate", senderCharacter, target,
                    (s, e) => VehicleScript.ShowVehiclesInformation(s, ((VehicleEntity)e).DbModel, true)));
            }
            return menuItems;
        }
    }
}