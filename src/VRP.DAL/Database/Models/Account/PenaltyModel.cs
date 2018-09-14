/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.Enums;

namespace VRP.DAL.Database.Models.Account
{
    public class PenaltyModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public DateTime ExpiryDate { get; set; }
        [StringLength(256)]
        public string Reason { get; set; }
        [EnumDataType(typeof(PenaltyType))]
        public PenaltyType PenaltyType { get; set; }
        public bool Deactivated { get; set; }
        public DateTime DeactivationDate { get; set; }

        // foreign keys
        [ForeignKey("Deactivator")]
        public int DeactivatorId { get; set; }
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }
        [ForeignKey("Character")]
        public int? CharacterId { get; set; }

        // navigation properties
        public virtual AccountModel Deactivator { get; set; }
        public virtual AccountModel Account { get; set; }
        public virtual AccountModel Creator { get; set; }
        /// <summary>
        /// used in the case of character blockage
        /// </summary>
        public virtual CharacterModel Character { get; set; }
    }
}