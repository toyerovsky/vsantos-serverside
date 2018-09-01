/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Enums;

namespace VRP.DAL.Database.Models.Item
{
    public class ItemTemplateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public short Weight { get; set; }
        public string ItemHash { get; set; }

        public int? FirstParameter { get; set; }
        public int? SecondParameter { get; set; }
        public int? ThirdParameter { get; set; }
        public int? FourthParameter { get; set; }

        [EnumDataType(typeof(ItemEntityType))]
        public ItemEntityType ItemEntityType { get; set; }
        public DateTime CreationTime { get; set; }

        // foreign keys 
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }
        // navigation properties
        public virtual AccountModel Creator { get; set; }
    }
}
