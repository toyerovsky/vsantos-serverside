/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations;
using VRP.DAL.Database.Models.Item;
using VRP.DAL.Enums;

namespace VRP.DAL.Database.Models.Warehouse
{
    // Tabela do trzymania przedmiotów bazowych w magazynie
    public class GroupWarehouseItemModel
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        // Ile jest obecnie?
        public int Count { get; set; }
        // Ile nadaje się co tydzień?
        public int ResetCount { get; set; }

        public virtual ItemTemplateModel ItemTemplateModel { get; set; }

        [EnumDataType(typeof(GroupType))]
        public virtual GroupType GroupType { get; set; }

        // navigation properties
        public virtual GroupWarehouseModel GroupWarehouseModel { get; set; }
    }
}