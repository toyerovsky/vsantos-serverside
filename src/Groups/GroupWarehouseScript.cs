/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Serverside.Core.Extensions;
using GTANetworkAPI;
using Newtonsoft.Json;
using Serverside.Admin.Enums;
using Serverside.Core.Database.Models;
using Serverside.Core.Repositories;
using Serverside.Groups.Enums;
using Serverside.Groups.Stucts;
using Serverside.Items;

namespace Serverside.Groups
{
    public class GroupWarehouseScript : Script
    {
        public static List<WarehouseOrderInfo> CurrentOrders { get; set; } = new List<WarehouseOrderInfo>();

        public GroupWarehouseScript()
        {
            Event.OnResourceStart += API_onResourceStart;
        }

        private void API_OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            if (eventName == "OnPlayerAddWarehouseItem")
            {
                /* Argumenty
                 * args[0] string nameResult,
                 * args[1] string itemTypeResult,
                 * args[2] int costResult, 
                 * args[3] string groupTypeResult, 
                 * args[4] int minimalCostResult, 
                 * args[5] int weeklyCountResult, 
                 * args[6] int firstParameterResult = null, 
                 * args[7] int secondParameterResult = null, 
                 * args[8] int thirdParameterResult = null
                 */

                if (Enum.TryParse(arguments[3].ToString(), out GroupType groupType) &&
                    Enum.TryParse(arguments[1].ToString(), out ItemType itemType))
                {
                    var groupWarehouseItem = new GroupWarehouseItemModel
                    {
                        ItemModel = new ItemModel
                        {
                            Creator = sender.GetAccountEntity().DbModel,
                            Name = arguments[0].ToString(),
                            ItemType = itemType,
                        },
                        Cost = Convert.ToDecimal(arguments[2]),
                        MinimalCost = Convert.ToDecimal(arguments[4]),
                        ResetCount = Convert.ToInt32(arguments[5]),
                        GroupType = groupType
                    };

                    if ((int?)arguments[6] != null)
                        groupWarehouseItem.ItemModel.FirstParameter = (int)arguments[6];
                    if ((int?)arguments[6] != null)
                        groupWarehouseItem.ItemModel.SecondParameter = (int)arguments[7];
                    if ((int?)arguments[6] != null)
                        groupWarehouseItem.ItemModel.ThirdParameter = (int)arguments[8];

                    using (GroupWarehouseItemsRepository repository = new GroupWarehouseItemsRepository())
                    {
                        repository.Insert(groupWarehouseItem);
                        repository.Save();
                    }
                    sender.Notify("Dodawanie przedmiotu zakończyło się ~h~ ~g~ pomyślnie.");
                }
                else
                {
                    sender.Notify("Dodawanie przedmiotu zakończone ~h~ ~r~ niepowodzeniem.");
                }

            }
            else if (eventName == "OnPlayerPlaceOrder")
            {
                /* Argumenty
                 * args[0] - List<WarehouseItemInfo> JSON
                 */

                var player = sender.GetAccountEntity();
                var group = player.CharacterEntity.OnDutyGroup;
                if (group != null)
                {
                    var items =
                        JsonConvert.DeserializeObject<List<WarehouseItemInfo>>(arguments[0].ToString());

                    var sum = items.Sum(x => x.ItemModelInfo.Cost * x.Count);
                    if (group.HasMoney(sum))
                    {
                        var shipment = new GroupWarehouseOrderModel
                        {
                            Getter = group.DbModel,
                            OrderItemsJson = JsonConvert.SerializeObject(items),
                            ShipmentLog = $"[{DateTime.Now}] Złożenie zamówienia w magazynie. \n"
                        };

                        using (GroupWarehouseOrdersRepository repository = new GroupWarehouseOrdersRepository())
                        {
                            repository.Insert(shipment);
                            repository.Save();
                        }
                        group.RemoveMoney(sum);

                        CurrentOrders.Add(new WarehouseOrderInfo
                        {
                            Data = shipment
                        });

                        sender.Notify("Zamawianie przesyłki zakończyło się ~h~ ~g~ pomyślnie.");
                    }
                    else
                    {
                        sender.Notify($"Grupa {group.GetColoredName()} nie posiada wystarczającej ilości środków.");
                    }
                }
            }
        }

        private void API_onResourceStart()
        {
            //foreach (var order in ContextFactory.Instance.GroupWarehouseOrders)
            //{
            //    CurrentOrders.Add(order);
            //}
        }

        [Command("dodajprzedmiotmag")]
        public void AddWarehouseItem(Client sender)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster4)
            {
                sender.Notify("Nie posiadasz uprawnień do tworzenia grupy.");
                return;
            }

            sender.TriggerEvent("ShowAdminWarehouseItemMenu");
        }
    }
}