/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using VRP.Core;
using VRP.DAL.Database;
using VRP.DAL.Database.Models.Item;
using VRP.DAL.Database.Models.Warehouse;
using VRP.DAL.Enums;
using VRP.DAL.Repositories;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Economy.Groups.Structs;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Entities.Core.Group;
using VRP.Serverside.Constant.RemoteEvents;

namespace VRP.Serverside.Economy.Groups
{
    public class GroupWarehouseScript : Script
    {
        public static List<WarehouseOrderInfo> CurrentOrders { get; set; } = new List<WarehouseOrderInfo>();

        [RemoteEvent(RemoteEvents.OnPlayerAddWarehouseItem)]
        public void OnPlayerAddWarehouseItemHandler(Client sender, params object[] arguments)
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
                Enum.TryParse(arguments[1].ToString(), out ItemEntityType itemType))
            {
                GroupWarehouseItemModel groupWarehouseItem = new GroupWarehouseItemModel
                {
                    ItemTemplateModel = new ItemTemplateModel
                    {
                        Name = arguments[0].ToString(),
                        ItemEntityType = itemType,
                    },
                    Cost = Convert.ToDecimal(arguments[2]),
                    ResetCount = Convert.ToInt32(arguments[5]),
                    GroupType = groupType
                };

                if (arguments[6] != null)
                    groupWarehouseItem.ItemTemplateModel.FirstParameter = (int)arguments[6];
                if (arguments[7] != null)
                    groupWarehouseItem.ItemTemplateModel.SecondParameter = (int)arguments[7];
                if (arguments[8] != null)
                    groupWarehouseItem.ItemTemplateModel.ThirdParameter = (int)arguments[8];

                RoleplayContext ctx = Singletons.RoleplayContextFactory.Create();
                using (GroupWarehouseItemsRepository repository = new GroupWarehouseItemsRepository(ctx))
                {
                    repository.Insert(groupWarehouseItem);
                    repository.Save();
                }
                sender.SendInfo("Dodawanie przedmiotu zakończyło się pomyślnie.");
            }
            else
            {
                sender.SendError("Dodawanie przedmiotu zakończone niepowodzeniem.");
            }
        }

        [RemoteEvent(RemoteEvents.OnPlayerPlaceOrder)]
        public void OnPlayerPlaceOrderHandler(Client sender, params object[] arguments)
        {
            /* Argumenty
             * args[0] - List<WarehouseItemInfo> JSON
             */

            AccountEntity player = sender.GetAccountEntity();
            GroupEntity group = player.CharacterEntity.OnDutyGroup;
            if (group != null)
            {
                List<WarehouseItemInfo> items =
                    JsonConvert.DeserializeObject<List<WarehouseItemInfo>>(arguments[0].ToString());

                decimal sum = items.Sum(x => x.ItemModelInfo.Cost * x.Count);
                if (group.HasMoney(sum))
                {
                    GroupWarehouseOrderModel shipment = new GroupWarehouseOrderModel
                    {
                        //Getter = group.DbModel,
                        //OrderItemsJson = JsonConvert.SerializeObject(items),
                        //ShipmentLog = $"[{DateTime.Now}] Złożenie zamówienia w magazynie. \n"
                    };

                    RoleplayContext ctx = Singletons.RoleplayContextFactory.Create();
                    using (GroupWarehouseOrdersRepository repository = new GroupWarehouseOrdersRepository(ctx))
                    {
                        repository.Insert(shipment);
                        repository.Save();
                    }
                    group.RemoveMoney(sum);

                    CurrentOrders.Add(new WarehouseOrderInfo
                    {
                        Data = shipment
                    });


                    sender.SendInfo("Zamawianie przesyłki zakończyło się pomyślnie.");

                }
                else
                {
                    sender.SendError($"Grupa {group.GetColoredName()} nie posiada wystarczającej ilości środków.");
                }
            }
        }

        [Command("dodajprzedmiotmag")]
        public void AddWarehouseItem(Client sender)
        {
            if (!sender.HasRank(ServerRank.AdministratorRozgrywki3))
            {
                sender.SendWarning("Nie posiadasz uprawnień do tworzenia grupy.");
                return;
            }

            sender.TriggerEvent("ShowAdminWarehouseItemMenu");
        }
    }
}