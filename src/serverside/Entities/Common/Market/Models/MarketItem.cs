/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using VRP.Core.Interfaces;
using VRP.DAL.Enums;

namespace VRP.Serverside.Entities.Common.Market.Models
{
    [Serializable]
    public class MarketItem : IXmlObject
    {
        public string Name { get; set; }
        public ItemEntityType ItemEntityType { get; set; }
        public decimal Cost { get; set; } 
        public int? FirstParameter { get; set; }
        public int? SecondParameter { get; set; }
        public int? ThirdParameter { get; set; }
        public string CreatorForumName { get; set; }
        public string FilePath { get; set; }
    }
}