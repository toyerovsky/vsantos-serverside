/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Serverside.Constant.RemoteEvents;
using VRP.Serverside.Entities;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Entities.Core.Group;

namespace VRP.Serverside.Core.Extensions
{
    public static class ClientExtensions
    {
        public static AccountEntity GetAccountEntity(this Client client)
        {
            if (!client.HasData("RP_ACCOUNT"))
                return null;
            return client.GetData("RP_ACCOUNT") as AccountEntity;
        }

        // until rage mp won't fix utf 8 characters we don't use this method
        // public static void Notify(this Client client, string message, bool flashing = false)
        // {
        //    NAPI.Notification.SendNotificationToPlayer(client, message, flashing);
        // }

        public static bool HasRank(this Client client, ServerRank rank)
        {
            return client.GetAccountEntity().DbModel.ServerRank >= rank;
        }

        public static void SendWarning(this Client client, string message, string title = "")
        {
            client.TriggerEvent(RemoteEvents.PlayerNotifyRequested, message, NotificationType.Warning, title);
        }

        public static void SendError(this Client client, string message, string title = "")
        {
            client.TriggerEvent(RemoteEvents.PlayerNotifyRequested, message, NotificationType.Error, title);
        }

        public static void SendInfo(this Client client, string message, string title = "")
        {
            client.TriggerEvent(RemoteEvents.PlayerNotifyRequested, message, NotificationType.Info, title);
        }

        public static bool TryGetGroupByUnsafeSlot(this Client client, short slot, out GroupEntity group, out WorkerModel worker)
        {
            group = null;
            worker = null;
            if (slot > 0 || slot <= 3)
            {
                slot--; // slot-- żeby numerowanie dla graczy było od 1 do 3
                List<GroupEntity> groups = EntityHelper.GetPlayerGroups(client.GetAccountEntity()).ToList();
                group = slot < groups.Count ? groups[slot] : null;
                worker = client.GetAccountEntity().CharacterEntity.DbModel.Workers.SingleOrDefault(g => g.Id == groups[slot].Id);
            }
            return group != null && worker != null;
        }
    }
}
