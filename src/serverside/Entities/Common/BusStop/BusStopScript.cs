/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.IO;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using VRP.Core.Enums;
using VRP.Core.Serialization;
using VRP.Core.Tools;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities.Common.BusStop.Models;

namespace VRP.Serverside.Entities.Common.BusStop
{
    public class BusStopScript : Script
    {
        private void Event_OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            //args[0] Czas
            //args[1] Koszt
            //args[2] Indeks przystanku na jaki chce się udać
            if (eventName == "RequestBus")
            {
                BusStopEntity busStop = EntityHelper.GetBusStops().ElementAt(Convert.ToInt32(arguments[2]));

                BusStopEntity.StartTransport(sender, Convert.ToDecimal(arguments[1]), Convert.ToInt32(arguments[0]),
                    busStop.Data.Center, busStop.Data.Name);
            }
        }

        [Command("bus")]
        public void ShowBusMenu(Client sender)
        {
            //Jeśli gracz nie jest na przystanku to anulujemy proces
            if (!sender.HasData("Bus") || EntityHelper.GetBusStops().Count() < 2)
            {
                sender.Notify("Liczba przystanków autobusowych musi być większa lub równa dwa.");
                return;
            }
            //Wybieramy wszystkie przystanki oprócz tego w którym obecnie się znajduje
            string json = JsonConvert.SerializeObject(EntityHelper.GetBusStops().Select(x => new
            {
                x.Data.Name,
                Time = (int)(sender.Position.DistanceTo(x.Data.Center) / 2.5f),
                Cost = (int)(sender.Position.DistanceTo(x.Data.Center) / 180f)
            }).Where(b => b.Name != ((BusStopEntity)sender.GetData("Bus")).Data.Name));

            sender.TriggerEvent("ShowBusMenu", json);
        }

        [Command("dodajbus", "~y~UŻYJ ~w~ /dodajbus [nazwa]",  GreedyArg = true)]
        public void AddBusStop(Client sender, string name)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster)
            {
                sender.Notify("Nie posiadasz uprawnień do usuwania przystanku autobusowego.");
                return;
            }

            sender.Notify("Ustaw się na środku przystanku aby stworzyć sferyczną strefę o promieniu 5f.");
            sender.Notify("Ustaw się w wybranej pozycji, a następnie wpisz /tu.");
            sender.Notify("...użyj /diag aby poznać swoją obecną pozycję.");

            Vector3 center = null;

            void Handler(Client o, string command)
            {
                if (center == null && o == sender && command == "/tu")
                {
                    center = o.Position;
                    BusStopModel data = new BusStopModel
                    {
                        Name = name,
                        Center = center,
                        CreatorForumName = o.GetAccountEntity().DbModel.Name,
                    };
                    XmlHelper.AddXmlObject(data, Path.Combine(ServerInfo.XmlDirectory, nameof(BusStopModel), data.Name));

                    sender.Notify("Dodawanie przystanku zakończyło się pomyślnie.");
                    BusStopEntity busStop = new BusStopEntity(data);
                    busStop.Spawn();
                    EntityHelper.Add(busStop);
                }
            }
        }

        [Command("usunbus")]
        public void DeleteBusStop(Client sender)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster)
            {
                sender.Notify("Nie posiadasz uprawnień do usuwania przystanku autobusowego.");
                return;
            }

            if (!EntityHelper.GetBusStops().Any())
            {
                sender.Notify("Nie znaleziono przystanku autobusowego który można usunąć.");
                return;
            }

            BusStopEntity busStop = EntityHelper.GetBusStop(sender.Position, 5f);

            if (busStop == null)
            {
                sender.Notify("Nie znaleziono przystanku autobusowego w Twoim otoczeniu.");
                return;
            }

            if (XmlHelper.TryDeleteXmlObject(busStop.Data.FilePath))
            {
                sender.Notify("Usuwanie przystanku autobusowego zakończyło się ~g~pomyślnie.");
                EntityHelper.Remove(busStop);
                busStop.Dispose();
            }
            else
            {
                sender.Notify("Usuwanie przystanku autobusowego zakończyło się ~r~niepowodzeniem.");
            }
        }
    }
}