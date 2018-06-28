/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using GTANetworkAPI;
using VRP.Core.Serialization;
using VRP.Core.Tools;
using VRP.DAL.Enums;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Economy.Groups;
using VRP.Serverside.Economy.Groups.Structs;
using VRP.Serverside.Economy.Jobs.Courier.CourierWarehouse.Models;
using VRP.Serverside.Entities;
using VRP.Serverside.Constant.RemoteEvents;

namespace VRP.Serverside.Economy.Jobs.Courier.CourierWarehouse
{
    public class CourierWarehouseScript : Script
    {
     
        public List<CourierWarehouse> Warehouses { get; set; } = new List<CourierWarehouse>();

        [RemoteEvent(RemoteEvents.OnPlayerTakePackage)]
        public void OnPlayerTakePackageHandler(Client sender , params object[] arguments)
        {
            WarehouseOrderInfo package = GroupWarehouseScript.CurrentOrders.Single(x => x.Data.Id == (int)arguments[0]);
            if (package.CurrentCourier != null)
            {
                sender.SendError("Ktoś obecnie dostarcza tę paczkę.");
                return;
            }

            sender.SendInfo($"Podjąłeś się dostarczenia przesyłki do: {EntityHelper.GetGroup(package.Data.Getter.Id).GetColoredName()}");
            package.CurrentCourier = sender.GetAccountEntity();
            GroupWarehouseScript.CurrentOrders.Remove(package);

            Timer timer = new Timer(1800000);
            timer.Start();
            timer.Elapsed += (o, args) =>
            {
                if (package.CurrentCourier.CharacterEntity.DbModel.Online)
                {
                    package.CurrentCourier.Client.SendInfo("Nie dostarczyłeś paczki na czas.");
                }

                package.CurrentCourier = null;
                GroupWarehouseScript.CurrentOrders.Add(package);
            };
        }

     

        [Command("dodajmagazyn", "~y~ UŻYJ ~w~ /dodajmagazyn [nazwa]")]
        public void AddVehicleToJob(Client sender, string name)
        {
            if (!sender.HasRank(ServerRank.AdministratorRozgrywki2))
            {
                sender.SendWarning("Nie posiadasz uprawnień do dodawania auta do pracy.");
                return;
            }

            sender.SendInfo("Ustaw się w wybranej pozycji a następnie wpisz \"tu\"");

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
                    XmlHelper.AddXmlObject(warehouse, Path.Combine(Utils.XmlDirectory, "CourierWarehouses"));

                    Warehouses.Add(new CourierWarehouse(warehouse));
                    o.SendInfo("Dodawanie magazynu do pracy kuriera zakończyło się pomyślnie.");
                }
            }
        }
    }
}