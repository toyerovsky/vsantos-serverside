/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkAPI;
using Serverside.Core.Extensions;
using Serverside.Entities;

namespace Serverside.Offers
{
    public static class OfferActions
    {
        public static void GiveIdCard(Client getter)
        {
            var player = getter.GetAccountEntity();
            player.CharacterEntity.DbModel.HasIdCard = true;
            player.CharacterEntity.Save();
        }

        public static void GiveDrivingLicense(Client getter)
        {
            var player = getter.GetAccountEntity();
            player.CharacterEntity.DbModel.HasDrivingLicense = true;
            player.CharacterEntity.Save();
        }

        public static void RepairVehicle(Client getter) =>
            EntityManager.GetVehicle(getter.Vehicle)?.Repair();
    }
}
