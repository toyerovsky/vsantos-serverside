/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using GTANetworkAPI;
using VRP.DAL.Enums;
using VRP.Serverside.Core.Exceptions;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities;
using VRP.Serverside.Entities.Core.Vehicle;

namespace VRP.Serverside.Admin
{
    public class AdminVehiclesScript : Script
    {
        [Command("kolor", "~y~ UŻYJ ~w~ /kolor [hexPodstawowy] [hexDodatkowy]")]
        public void ChangeVehicleColor(Client sender, string primaryHex, string secondaryHex, long vehicleId = -1)
        {
            if (!sender.HasRank(ServerRank.AdministratorGry))
            {
                sender.SendWarning("Nie posiadasz uprawnień do ustawiania koloru pojazdu.");
                return;
            }

            if (!sender.IsInVehicle && vehicleId == -1)
            {
                sender.SendWarning("Wsiądź do pojazu lub podaj Id aby ustawić jego kolor.");
                return;
            }

            VehicleEntity vehicle = sender.IsInVehicle
                ? sender.Vehicle.GetVehicleEntity()
                : EntityHelper.GetVehicle(vehicleId);

            Color primaryColor;
            Color secondaryColor;
            try
            {
                primaryColor = primaryHex.ToColor();
                secondaryColor = secondaryHex.ToColor();
            }
            catch (ColorConvertException e)
            {
                sender.SendError("Wprowadzony kolor jest nieprawidłowy.");
                Colorful.Console.WriteLine($"[Error] {nameof(AdminGroupsScript)} Nieprawidłowy kolor", System.Drawing.Color.DarkRed);
                Colorful.Console.WriteLine(e.Message, System.Drawing.Color.DarkRed);
                return;
            }

            vehicle?.ChangeColor(primaryColor, secondaryColor);
        }

        [Command("napraw", "~y~ UŻYJ ~w~ /napraw")]
        public void RepairVehicle(Client sender, long vehicleId = -1)
        {
            if (!sender.HasRank(ServerRank.AdministratorGry))
            {
                sender.SendWarning("Nie posiadasz uprawnień do naprawiania pojazdów.");
                return;
            }

            if (!sender.IsInVehicle && vehicleId == -1)
            {
                sender.SendWarning("Wsiądź do pojazu lub podaj Id aby go naprawić.");
                return;
            }

            VehicleEntity vehicle = sender.IsInVehicle
                ? sender.Vehicle.GetVehicleEntity()
                : EntityHelper.GetVehicle(vehicleId);

            vehicle?.Repair();
        }
    }
}