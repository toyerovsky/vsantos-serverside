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
using VRP.Core.Serialization;
using VRP.Core.Tools;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities.Common.Corners.Models;
using VRP.Serverside.Entities.Core.Item;
using VRP.Serverside.Constant.RemoteEvents;

namespace VRP.Serverside.Entities.Common.Corners
{
    public class CornerBotsScript : Script
    {
        [RemoteEvent(RemoteEvents.AddCornerBot)]
        public void AddCornerBotHandler(Client sender, params object[] arguments)
        {
            CornerBotModel bot = new CornerBotModel
            {
                CreatorForumName = sender.GetAccountEntity().DbModel.Name,
                BotId = XmlHelper.GetXmlObjects<CornerBotModel>(Path.Combine(Utils.XmlDirectory, "CornerBots")).Count + 1,
                Name = Convert.ToString(arguments[0]),
                PedHash = (PedHash)arguments[1],
                MoneyCount = Convert.ToDecimal(arguments[2]),
                DrugType = (DrugType)Enum.Parse(typeof(DrugType), (string)arguments[3]),
                Greeting = (string)arguments[4],
                GoodFarewell = (string)arguments[5],
                BadFarewell = (string)arguments[6]
            };
            XmlHelper.AddXmlObject(bot, Path.Combine(Utils.XmlDirectory, "CornerBots"));
            sender.SendInfo("Dodanie NPC zakończyło się pomyślnie.");
        }
     

        [Command("dodajbotc")]
        public void AddCornerBot(Client sender)
        {
            List<int> values = Enum.GetNames(typeof(PedHash)).Select(ped => (int) Enum.Parse(typeof(PedHash), ped)).ToList();
            NAPI.ClientEvent.TriggerClientEvent(sender, "ShowCornerBotMenu", values);
        } 
    }
}