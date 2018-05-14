/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations;
using VRP.Core.Enums;

namespace VRP.Core.Database.Models
{
    public class ZoneModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreatorId { get; set; }
        [EnumDataType(typeof(ZoneType))]
        public virtual ZoneType ZoneType { get; set; }
        public string ZonePropertiesJson { get; set; }
    }
}