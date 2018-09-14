/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Database.Models.Bot;
using VRP.DAL.Database.Models.Group;

namespace VRP.DAL.Database.Models.CrimeBot
{
    public class CrimeBotModel
    {
        public CrimeBotModel()
        {
            ItemsForSale = new HashSet<CrimeBotItemModel>();
        }

        public int Id { get; set; }
        public string VehicleModel { get; set; }


        // foreign keys
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }
        [ForeignKey("GroupModel")]
        public int GroupModelId { get; set; }
        [ForeignKey("BotModel")]
        public int BotModelId { get; set; }

        // navigation properties
        public virtual AccountModel Creator { get; set; }
        public virtual GroupModel GroupModel { get; set; }
        public virtual BotModel BotModel { get; set; }

        public virtual ICollection<CrimeBotItemModel> ItemsForSale { get; set; }
    }
}