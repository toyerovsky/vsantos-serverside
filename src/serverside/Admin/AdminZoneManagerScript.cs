/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using GTANetworkAPI;
using VRP.DAL.Enums;
using VRP.Serverside.Constant.RemoteEvents;
using VRP.Serverside.Core.Extensions;

namespace VRP.Serverside.Admin
{
    public class AdminZoneManagerScript : Script
    {
        [Command("strefa")]
        public void OpenZoneManager(Client sender)
        {
            if (!sender.HasRank(ServerRank.AdministratorGry2))
            {
                sender.SendWarning("Nie posiadasz uprawnień do latania.");
                return;
            }

            sender.TriggerEvent(RemoteEvents.PlayerZoneManagerRequested);
        }
    }
}