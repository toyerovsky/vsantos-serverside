/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using System.Linq;
using System.Timers;
using GTANetworkAPI;
using Serverside.Admin.Enums;
using Serverside.Constant;
using Serverside.Core.Extensions;
using Serverside.Core.Serialization;
using Serverside.Economy.Groups;
using Serverside.Economy.Groups.Stucts;
using Serverside.Economy.Jobs.Courier.CourierWarehouse.Models;
using Serverside.Entities;

namespace Serverside.Economy.Jobs.Courier.CourierWarehouse
{
    public class CourierWarehouseScript : Script
    {
        public List<CourierWarehouse> Warehouses { get; set; } = new List<CourierWarehouse>();

        private void Event_OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            if (eventName == "OnPlayerTakePackage")
            {
                WarehouseOrderInfo package = GroupWarehouseScript.CurrentOrders.Single(x => x.Data.Id == (int)arguments[0]);
                if (package.CurrentCourier != null)
                {
                    sender.Notify("Ktoś obecnie dostarcza tę paczkę.");
                    return;
                }

                sender.Notify($"Podjąłeś się dostarczenia przesyłki do: {EntityHelper.GetGroup(package.Data.Getter.Id).GetColoredName()}");
                package.CurrentCourier = sender.GetAccountEntity();
                GroupWarehouseScript.CurrentOrders.Remove(package);

                Timer timer = new Timer(1800000);
                timer.Start();
                timer.Elapsed += (o, args) =>
                {
                    if (package.CurrentCourier.CharacterEntity.DbModel.Online)
                    {
                        package.CurrentCourier.Client.Notify("Nie dostarczyłeś paczki na czas.");
                    }

                    package.CurrentCourier = null;
                    GroupWarehouseScript.CurrentOrders.Add(package);
                };

            }
        }

        [Command("dodajmagazyn", "~y~ UŻYJ ~w~ /dodajmagazyn [nazwa]")]
        public void AddVehicleToJob(Client sender, string name)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster)
            {
                sender.Notify("Nie posiadasz uprawnień do dodawania auta do pracy.");
                return;
            }

            sender.Notify("Ustaw się w wybranej pozycji a następnie wpisz \"tu\"");

            void Handler(Client o, string message)
            {
                if (o == sender && message == "tu")
                {
                    CourierWarehouseModel warehouse = new CourierWarehouseModel()
                    {
                        Name = name,
                        Position = o.Position,
                        CreatorForumName = o.GetAccountEntity().DbModel.Name
                    };
                    XmlHelper.AddXmlObject(warehouse, $@"{ServerInfo.XmlDirectory}CourierWarehouses\");

                    Warehouses.Add(new CourierWarehouse(warehouse));
                    o.Notify("Dodawanie magazynu do pracy kuriera zakończyło się ~h~ ~g~pomyślnie.");
                }
            }
        }
    }
}