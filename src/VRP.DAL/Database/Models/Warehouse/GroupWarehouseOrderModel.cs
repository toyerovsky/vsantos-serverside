/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Item;

namespace VRP.DAL.Database.Models.Warehouse
{
    public class GroupWarehouseOrderModel
    {
        public int Id { get; set; }
        public string ShipmentLog { get; set; }

        public int OrderedItemCount { get; set; }

        // foreign keys
        [ForeignKey("OrderedItemTemplate")]
        public int OrderedItemTemplateId { get; set; }
        [ForeignKey("Getter")]
        public int GetterId { get; set; }

        // navigation properties
        public virtual ItemTemplateModel OrderedItemTemplate { get; set; }
        public virtual GroupWarehouseModel Getter { get; set; }
    }
}