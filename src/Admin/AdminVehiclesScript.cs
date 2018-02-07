/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using GTANetworkAPI;
using Serverside.Constant;
using Serverside.Core;
using Serverside.Core.Extensions;
using Serverside.Entities;
using Serverside.Exceptions;

namespace Serverside.Admin
{
    public class AdminVehiclesScript : Script
    {
        public AdminVehiclesScript()
        {
            Event.OnResourceStart += OnResourceStart;
        }

        private void OnResourceStart()
        {
            Tools.ConsoleOutput($"[{nameof(AdminVehiclesScript)}] {Messages.ResourceStartMessage}", ConsoleColor.DarkMagenta);
        }

        [Command("kolor", "~y~ UŻYJ ~w~ /kolor [hexPodstawowy] [hexDodatkowy]")]
        public void ChangeVehicleColor(Client sender, string primaryHex, string secondaryHex, long vehicleId = -1)
        {
            if (!sender.IsInVehicle && vehicleId == -1)
            {
                sender.Notify("Wsiądź do pojazu lub podaj Id aby ustawić jego kolor.");
                return;
            }

            var vehicle = sender.IsInVehicle
                ? sender.Vehicle.GetVehicleEntity()
                : EntityManager.GetVehicle(vehicleId);

            Color primaryColor;
            Color secondaryColor;
            try
            {
                primaryColor = primaryHex.ToColor();
                secondaryColor = secondaryHex.ToColor();
            }
            catch (ColorConvertException e)
            {
                sender.Notify("Wprowadzony kolor jest nieprawidłowy.");
                Tools.ConsoleOutput($"{nameof(AdminGroupsScript)}[Error] Nieprawidłowy kolor", ConsoleColor.Red);
                Tools.ConsoleOutput(e.Message, ConsoleColor.Red);
                return;
            }

            vehicle?.ChangeColor(primaryColor, secondaryColor);
        }

        [Command("napraw", "~y~ UŻYJ ~w~ /kolor [podstawowy] (dodatkowy)")]
        public void RepairVehicle(Client sender, long vehicleId = -1)
        {
            if (!sender.IsInVehicle && vehicleId == -1)
            {
                sender.Notify("Wsiądź do pojazu lub podaj Id aby ustawić jego kolor.");
                return;
            }

            var vehicle = sender.IsInVehicle
                ? sender.Vehicle.GetVehicleEntity()
                : EntityManager.GetVehicle(vehicleId);

            vehicle?.Repair();
        }
    }
}