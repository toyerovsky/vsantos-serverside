/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Core.Serialization;
using VRP.Core.Tools;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities.Common.Market.Models;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Constant.RemoteEvents;

namespace VRP.Serverside.Entities.Common.Market
{
    public class MarketScript : Script
    {
        private static List<MarketEntity> Markets { get; set; } = new List<MarketEntity>();

        [RemoteEvent(RemoteEvents.AddMarketItem)]
        public void AddMarketItemHandler(Client sender, params object[] arguments)
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
            MarketItem item = new MarketItem
            {
                Name = arguments[0].ToString(),
                ItemEntityType = (ItemEntityType)Enum.Parse(typeof(ItemEntityType), (string)arguments[1]),
                Cost = (decimal)arguments[2],
                FirstParameter = (int)arguments[4],
                SecondParameter = (int)arguments[5],
                ThirdParameter = (int)arguments[6]
            };

            List<string> names = (List<string>)arguments[3];
            foreach (string name in names)
            {
                MarketEntity market = Markets.First(x => x.Data.Name == name);
                if (market != null)
                {
                    market.Data.Items.Add(item);
                    XmlHelper.AddXmlObject(market.Data, Path.Combine(Utils.XmlDirectory, "Markets", market.Data.Name));
                }
            }
        }

        [RemoteEvent(RemoteEvents.OnPlayerBoughtMarketItem)]
        public void OnPlayerBoughtMarketItemHandler(Client sender, params object[] arguments)
        {
            //args[0] index
            CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
            if (character.CurrentInteractive is MarketEntity market)
            {
                MarketItem item = market.Data.Items[(int)arguments[0]];

                if (!character.HasMoney(item.Cost))
                {
                    character.SendInfo("Nie posiadasz wystarczającej ilości gotówki.");
                    return;
                }
                character.RemoveMoney(item.Cost);

                AccountEntity controller = sender.GetAccountEntity();
                controller.CharacterEntity.DbModel.Items.Add(new ItemModel
                {
                    Creator = null,
                    ItemEntityType = item.ItemEntityType,
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

            foreach (MarketModel data in XmlHelper.GetXmlObjects<MarketModel>(Path.Combine(Utils.XmlDirectory, "Markets")))
            {
                MarketEntity market = new MarketEntity(data);
                market.Spawn();
                Markets.Add(market);
            }
        }

        [Command("dodajprzedmiotsklep")]
        public void AddItemToShop(Client sender)
        {
            List<string> values = Enum.GetNames(typeof(ItemEntityType)).ToList();
            var markets = XmlHelper.GetXmlObjects<MarketModel>(Path.Combine(Utils.XmlDirectory, "Markets"))
                .Select(x => new { x.Id, x.Name }).ToList();
            NAPI.ClientEvent.TriggerClientEvent(sender, "ShowAdminMarketItemMenu", values, markets);
        }

        [Command("kup")]
        public void BuyItemFromShop(Client sender)
        {
            CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
            if (character.CurrentInteractive is MarketEntity market)
            {
                if (market.Data.Items == null || market.Data.Items.Count == 0)
                    return;
                sender.TriggerEvent("ShowMarketMenu", JsonConvert.SerializeObject(market.Data.Items));
            }
            else
            {
                sender.SendInfo("Nie znajdujesz się w sklepie");
            }
        }

        [Command("dodajsklep", "~y~UŻYJ ~w~ /dodajsklep [nazwa]")]
        public void AddMarket(Client sender, string name)
        {
            sender.SendInfo("Ustaw się w pozycji NPC, a następnie wpisz /tu użyj ctrl + alt + shift + d aby poznać swoją obecną pozycję.");
            
            Vector3 center = null;

            void Handler(Client o, string message)
            {
                if (center == null && o == sender && message == "/tu")
                {
                    center = o.Position;
                    sender.SendInfo("Przejdź do pozycji końca promienia zasięgu i wpisz \"tu.\"");
                }
                else
                {
                    float radius = float.MinValue;
                    if (center != null && radius.Equals(float.MinValue) && o == sender && message == "tu")
                    {
                        radius = center.DistanceTo2D(o.Position);
                        MarketModel market = new MarketModel
                        {
                            Id = XmlHelper.GetXmlObjects<MarketModel>(Path.Combine(Utils.XmlDirectory, "Markets")).Count,
                            Name = name,
                            Center = center,
                            Radius = radius
                        };
                        XmlHelper.AddXmlObject(market, Path.Combine(Utils.XmlDirectory, "Markets"), market.Name);
                        Markets.Add(new MarketEntity(market));
                    }
                }
            }
        }
    }
}