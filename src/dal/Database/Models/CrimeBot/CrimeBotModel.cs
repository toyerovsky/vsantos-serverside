/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using VRP.DAL.Database.Models.Bot;
using VRP.DAL.Database.Models.Group;

namespace VRP.DAL.Database.Models.CrimeBot
{
    public class CrimeBotModel
    {
        public CrimeBotModel()
        {
            CrimeBotItems = new HashSet<CrimeBotItemModel>();
        }

        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string VehicleModel { get; set; }

        // navigation properties
        // items for sale
        public virtual ICollection<CrimeBotItemModel> CrimeBotItems { get; set; }
        public virtual GroupModel GroupModel { get; set; }
        public virtual BotModel BotModel { get; set; }
    }
}