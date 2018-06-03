﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;

namespace VRP.Core.Database.Models
{
    public class CrimeBotModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreatorId { get; set; }
        public virtual GroupModel GroupModel { get; set; }
        public string VehicleModel { get; set; }
        // ped model
        public string PedSkin { get; set; }

        // navigation properties
        public virtual ICollection<CrimeBotItemModel> CrimeBotItems { get; set; } // items for sale
    }
}