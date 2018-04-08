﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using GTANetworkAPI;
using VRP.Core.Interfaces;
using VRP.Serverside.Entities.Core.Item;

namespace VRP.Serverside.Entities.Common.Corners.Models
{
    [Serializable]
    public class CornerBotModel : IXmlObject
    {
        public int BotId { get; set; }
        public string Name { get; set; }
        public PedHash PedHash { get; set; }
        public DrugType DrugType { get; set; }
        public decimal MoneyCount { get; set; }
        public string Greeting { get; set; }
        public string GoodFarewell { get; set; }
        public string BadFarewell { get; set; }
        public string CreatorForumName { get; set; }
        public string FilePath { get; set; }
    }
}