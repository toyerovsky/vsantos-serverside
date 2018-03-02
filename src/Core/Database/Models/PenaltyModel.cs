/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.ComponentModel.DataAnnotations;
using Serverside.Core.Enums;

namespace Serverside.Core.Database.Models
{
    public class PenaltyModel
    {
        public long Id { get; set; }

        public virtual AccountModel Creator { get; set; }

        public virtual AccountModel Account { get; set; }

        public DateTime Date { get; set; }
        public DateTime ExpiryDate { get; set; }
        [StringLength(256)]
        public string Reason { get; set; }
        
        [EnumDataType(typeof(PenaltyType))]
        public virtual PenaltyType PenaltyType { get; set; }
    }

}