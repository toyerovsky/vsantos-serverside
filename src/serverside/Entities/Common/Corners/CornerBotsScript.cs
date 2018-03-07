﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Serverside.Core.Extensions;
using Serverside.Core.Serialization;
using Serverside.Entities.Common.Corners.Models;
using Serverside.Entities.Core.Item;

namespace Serverside.Entities.Common.Corners
{
    public class CornerBotsScript : Script
    {
        private void ClientEventTriggerHandler(Client sender, string eventName, params object[] arguments)
        {
            if (eventName == "AddCornerBot")
            {
                CornerBotModel bot = new CornerBotModel
                {
                    CreatorForumName = sender.GetAccountEntity().DbModel.Name,
                    BotId = XmlHelper.GetXmlObjects<CornerBotModel>(Constant.ServerInfo.XmlDirectory + @"CornerBots\").Count + 1,
                    Name = Convert.ToString(arguments[0]),
                    PedHash = (PedHash) arguments[1],
                    MoneyCount = Convert.ToDecimal(arguments[2]),
                    DrugType = (DrugType) Enum.Parse(typeof(DrugType), (string) arguments[3]),
                    Greeting = (string) arguments[4],
                    GoodFarewell = (string) arguments[5],
                    BadFarewell = (string) arguments[6]
                };
                XmlHelper.AddXmlObject(bot, Constant.ServerInfo.XmlDirectory + @"CornerBots\");
                sender.Notify("Dodanie NPC zakończyło się ~h~~g~pomyślnie.");
            }
        }

        [Command("dodajbotc")]
        public void AddCornerBot(Client sender)
        {
            List<int> values = Enum.GetNames(typeof(PedHash)).Select(ped => (int) Enum.Parse(typeof(PedHash), ped)).ToList();
            NAPI.ClientEvent.TriggerClientEvent(sender, "ShowCornerBotMenu", values);
        } 
    }
}