/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkAPI;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Economy.Offers
{
    public static class OfferActions
    {
        public static void GiveIdCard(CharacterEntity getter)
        {
            getter.DbModel.HasIdCard = true;
            getter.Save();
        }

        public static void GiveDrivingLicense(CharacterEntity getter)
        {
            getter.DbModel.HasDrivingLicense = true;
            getter.Save();
        }

        public static void RepairVehicle(Client getter) =>
            EntityHelper.GetVehicle(getter.Vehicle)?.Repair();
    }
}
