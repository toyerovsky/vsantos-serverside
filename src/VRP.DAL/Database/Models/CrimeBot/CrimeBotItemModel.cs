/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Item;

namespace VRP.DAL.Database.Models.CrimeBot
{
    public class CrimeBotItemModel
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public int Count { get; set; }
        public int ResetCount { get; set; }

        // foreign keys
        [ForeignKey("ItemTemplateModel")]
        public int ItemTemplateModelId { get; set; }
        // navigation properties
        public virtual ItemTemplateModel ItemTemplateModel { get; set; }
    }
}
