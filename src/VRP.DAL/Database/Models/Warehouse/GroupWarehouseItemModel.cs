/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Item;
using VRP.DAL.Enums;

namespace VRP.DAL.Database.Models.Warehouse
{
    public class GroupWarehouseItemModel
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public int Count { get; set; }
        public int ResetCount { get; set; }

        [EnumDataType(typeof(GroupType))]
        public virtual GroupType GroupType { get; set; }

        // foreign keys
        [ForeignKey("GroupWarehouseModel")]
        public int GroupWarehouseModelId { get; set; }
        [ForeignKey("ItemTemplateModel")]
        public int ItemTemplateModelId { get; set; }

        // navigation properties
        public virtual GroupWarehouseModel GroupWarehouseModel { get; set; }
        public virtual ItemTemplateModel ItemTemplateModel { get; set; }
    }
}