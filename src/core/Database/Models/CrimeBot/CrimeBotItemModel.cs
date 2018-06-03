/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using VRP.Core.Database.Models.Item;

namespace VRP.Core.Database.Models.CrimeBot
{
    public class CrimeBotItemModel
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public int Count { get; set; }
        public int ResetCount { get; set; }
        public virtual ItemTemplateModel ItemTemplateModel { get; set; }
    }
}
