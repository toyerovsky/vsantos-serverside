/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations;

namespace VRP.Core.Database.Models
{
    public class GroupWarehouseOrderModel
    {
        public int Id { get; set; }
        public string ShipmentLog { get; set; }

        public int OrderedItemCount { get; set; }

        // navigation properties
        public virtual ItemTemplateModel OrderedItemTemplate { get; set; }
        // it goes to specified magazine for specified group
        public virtual GroupWarehouseModel Getter { get; set; }
    }
}