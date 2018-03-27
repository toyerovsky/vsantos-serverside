/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Serverside.Economy.Money;
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

        public static void Notify(this Client client, string message, bool flashing = false)
        {
            client.SendNotification(message, flashing);
        }

        public static Color GetRankColor(this Client client)
        {
            AccountModel account = client.GetAccountEntity().DbModel;
            if ((ServerRank)account.ForumGroup >= ServerRank.Support && (ServerRank)account.ForumGroup <= ServerRank.Support6) return new Color(51, 143, 255);
            if ((ServerRank)account.ForumGroup >= ServerRank.GameMaster && (ServerRank)account.ForumGroup <= ServerRank.GameMaster5) return new Color(0, 109, 15);
            if ((ServerRank)account.ForumGroup >= ServerRank.Administrator && (ServerRank)account.ForumGroup <= ServerRank.Adminadministrator3) return new Color(117, 13, 18);
            if ((ServerRank)account.ForumGroup >= ServerRank.Zarzad && (ServerRank)account.ForumGroup <= ServerRank.Zarzad2) return new Color(255, 0, 0);
            return new Color(255, 255, 255);
        }

        //slot-- żeby numerowanie dla graczy było od 1 do 3
        public static bool TryGetGroupByUnsafeSlot(this Client client, short slot, out GroupEntity group)
        {
            group = null;
            if (slot > 0 || slot <= 3)
            {
                slot--;
                List<GroupEntity> groups = EntityHelper.GetPlayerGroups(client.GetAccountEntity()).ToList();
                group = slot < groups.Count ? groups[slot] : null;
            }
            return group != null;
        }

        #region Pieniądze
        public static bool HasMoney(this Client client, decimal count, bool bank = false)
        {
           return MoneyManager.HasMoney(client, count, bank);
        }

        public static void AddMoney(this Client client, decimal count, bool bank = false)
        {
            MoneyManager.AddMoney(client, count, bank);
        }

        public static void RemoveMoney(this Client client, decimal count, bool bank = false)
        {
            MoneyManager.RemoveMoney(client, count, bank);
        }
        #endregion
    }
}
