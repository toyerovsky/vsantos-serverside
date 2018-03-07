/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using VRP.Core.Enums;

namespace VRP.Serverside.Entities.Peds.CrimeBot.Models
{
    public class CrimeBotItem
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public int Count { get; set; }
        public int DefaultCount { get; set; }
        public ItemEntityType Type { get; set; }
        public string DatabaseField { get; set; }
        
        public CrimeBotItem(string name, decimal cost, int count, int defaultCount, ItemEntityType type, string databaseField)
        {
            Name = name;
            Cost = cost;
            Count = count;
            DefaultCount = defaultCount;
            Type = type;
            DatabaseField = databaseField;
        }
    }
}