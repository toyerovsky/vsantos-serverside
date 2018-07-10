/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.ComponentModel.DataAnnotations;
using VRP.DAL.Enums;

namespace VRP.DAL.Database.Models.Account
{
    public class PenaltyModel
    {
        public int Id { get; set; }
        public int? CreatorId { get; set; }

        public DateTime Date { get; set; }
        public DateTime ExpiryDate { get; set; }
        [StringLength(256)]
        public string Reason { get; set; }

        [EnumDataType(typeof(PenaltyType))]
        public PenaltyType PenaltyType { get; set; }

        // navigation properties
        public virtual AccountModel Account { get; set; }
    }
}