/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Enums;

namespace VRP.DAL.Database.Models.Misc
{
    public class ZoneModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [EnumDataType(typeof(ZoneType))]
        public ZoneType ZoneType { get; set; }
        public string ZonePropertiesJson { get; set; }
        public uint Dimension { get; set; }

        // foreign keys
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }

        // navigation properties
        public virtual AccountModel Creator { get; set; }
    }
}