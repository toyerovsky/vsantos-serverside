﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Database.Models.Bot;
using VRP.DAL.Enums;

namespace VRP.DAL.Database.Models.Corner
{
    public class CornerBotModel
    {
        public int Id { get; set; }
        public DrugType DrugType { get; set; }
        public decimal MoneyCount { get; set; }

        public string Greeting { get; set; }
        public string GoodFarewell { get; set; }
        public string BadFarewell { get; set; }


        // foreign keys
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }
        [ForeignKey("BotModel")]
        public int BotModelId { get; set; }

        // navigation properties
        public virtual AccountModel Creator { get; set; }
        public virtual BotModel BotModel { get; set; }
    }
}