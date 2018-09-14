/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */


using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Database.Models.Misc;

namespace VRP.DAL.Database.Models.TelephoneBooth
{
    public class TelephoneBoothModel
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public decimal Cost { get; set; }

        // foreign keys
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }
        [ForeignKey("ZoneModel")]
        public int ZoneModelId { get; set; }

        // navigation properties
        public virtual AccountModel Creator { get; set; }
        public virtual ZoneModel ZoneModel { get; set; }
    }
}