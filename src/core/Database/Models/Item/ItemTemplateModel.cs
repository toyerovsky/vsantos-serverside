/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations;
using VRP.Core.Enums;

namespace VRP.Core.Database.Models.Item
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
        public virtual ItemEntityType ItemEntityType { get; set; }
    }
}
