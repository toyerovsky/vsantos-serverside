﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Serverside.Autonomic.Market.Models;
using Serverside.Constant;
using Serverside.Core;
using Serverside.Core.Database.Models;
using Serverside.Core.Extensions;
using Serverside.Core.Serialization.Xml;
using Serverside.Items;

namespace Serverside.Autonomic.Market
{
    public sealed class MarketScript : Script
    {
        public static List<Market> Markets { get; set; } = new List<Market>();

        public MarketScript()
        {
            Event.OnResourceStart += OnResourceStart;
        }

        private void OnClientEventTriggerHandler(Client sender, string eventName, params object[] arguments)
        {
            if (eventName == "AddMarketItem")
            {
                /*
                 * args[0] nameResult
                 * args[1] typeResult
                 * args[2] decimal costResult
                 * args[3] List<string> names
                 * args[4] FirstParameter
                 * args[5] SecondParameter
                 * args[6] ThirdParameter
                 */
                var item = new MarketItem
                {
                    Name = arguments[0].ToString(),
                    ItemType = (ItemType)Enum.Parse(typeof(ItemType), (string)arguments[1]),
                    Cost = (decimal)arguments[2],
                    FirstParameter = (int)arguments[4],
                    SecondParameter = (int)arguments[5],
                    ThirdParameter = (int)arguments[6]
                };

                var names = (List<string>)arguments[3];
                foreach (var name in names)
                {
                    var market = Markets.First(x => x.MarketData.Name == name);
                    if (market != null)
                    {
                        market.MarketData.Items.Add(item);
                        XmlHelper.AddXmlObject(market.MarketData, $@"{ServerInfo.XmlDirectory}\Markets\", market.MarketData.Name);
                    }
                }
            }
            else if (eventName == "OnPlayerBoughtMarketItem")
            {
                //args[0] index
                if (!sender.HasData("CurrentMarket")) return;
                var market = (Market)sender.GetData("CurrentMarket");
                var item = market.MarketData.Items[(int)arguments[0]];
                if (!sender.HasMoney(item.Cost))
                {
                    sender.Notify("Nie posiadasz wystarczającej ilości gotówki.");
                    return;
                }
                sender.RemoveMoney(item.Cost);

                var controller = sender.GetAccountEntity();
                controller.CharacterEntity.DbModel.Items.Add(new ItemModel
                {
                    Creator = null,
                    ItemType = item.ItemType,
                    Name = item.Name,
                    Character = controller.CharacterEntity.DbModel,
                    FirstParameter = item.FirstParameter,
                    SecondParameter = item.SecondParameter,
                    ThirdParameter = item.ThirdParameter
                });
                controller.Save();
            }
        }

        private void OnResourceStart()
        {
            //TODO: Wczytywanie wszystkich IPL sklepów

            Tools.ConsoleOutput($"[{nameof(MarketScript)}] {ConstantMessages.ResourceStartMessage}", ConsoleColor.DarkMagenta);
            XmlHelper.GetXmlObjects<Models.Market>($@"{ServerInfo.XmlDirectory}\Markets\")
                .ForEach(market => Markets.Add(new Market(Event, market)));
        }

        [Command("dodajprzedmiotsklep")]
        public void AddItemToShop(Client sender)
        {
            var values = Enum.GetNames(typeof(ItemType)).ToList();
            var markets = XmlHelper.GetXmlObjects<Models.Market>($@"{ServerInfo.XmlDirectory}\Markets\").Select(x => new { x.Id, x.Name }).ToList();
            NAPI.ClientEvent.TriggerClientEvent(sender, "ShowAdminMarketItemMenu", values, markets);
        }

        [Command("kup")]
        public void BuyItemFromShop(Client sender)
        {
            if (!sender.HasData("CurrentMarket"))
            {
                sender.Notify("Nie znajdujesz się w sklepie");
                return;
            }
            var market = (Market)sender.GetData("CurrentMarket");
            if (market.MarketData.Items == null || market.MarketData.Items.Count == 0) return;
            sender.TriggerEvent("ShowMarketMenu", JsonConvert.SerializeObject(market.MarketData.Items));
        }

        [Command("dodajsklep", "~y~UŻYJ ~w~ /dodajsklep [nazwa]")]
        public void AddMarket(Client sender, string name)
        {
            sender.Notify("Ustaw się w pozycji NPC, a następnie wpisz /tu.");
            sender.Notify("...użyj /diag aby poznać swoją obecną pozycję.");

            Vector3 center = null;

            Event.OnChatMessage += Handler;

            void Handler(Client o, string message, CancelEventArgs cancel)
            {
                if (center == null && o == sender && message == "/tu")
                {
                    cancel.Cancel = true;
                    center = o.Position;
                    sender.Notify("Przejdź do pozycji końca promienia zasięgu i wpisz \"tu.\"");
                }
                else
                {
                    var radius = float.MinValue;
                    if (center != null && radius.Equals(float.MinValue) && o == sender && message == "tu")
                    {
                        radius = center.DistanceTo2D(o.Position);
                        var market = new Models.Market
                        {
                            Id = XmlHelper.GetXmlObjects<Models.Market>(ServerInfo.XmlDirectory + @"Markets\").Count,
                            Name = name,
                            Center = center,
                            Radius = radius
                        };
                        XmlHelper.AddXmlObject(market, ServerInfo.XmlDirectory + @"Markets\", market.Name);
                        Markets.Add(new Market(Event, market));
                        Event.OnChatMessage -= Handler;
                    }
                }
            }
        }
    }
}