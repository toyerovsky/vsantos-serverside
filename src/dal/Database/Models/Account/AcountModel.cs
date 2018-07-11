﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.Enums;

namespace VRP.DAL.Database.Models.Account
{
    public class AccountModel
    {
        [Key]
        public int Id { get; set; }
        [Key]
        public long ForumUserId { get; set; }
        [Key]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
        public string ForumUserName { get; set; }
        public long PrimaryForumGroup { get; set; }
        public string SecondaryForumGroups { get; set; }
        [StringLength(50)]
        public string SocialClub { get; set; }
        public DateTime LastLogin { get; set; }
        [EnumDataType(typeof(ServerRank))]
        public ServerRank ServerRank { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public string AvatarUrl { get; set; }

        // navigation properties
        public virtual ICollection<SerialModel> Serials { get; set; }
        public virtual ICollection<CharacterModel> Characters { get; set; }
        [InverseProperty("Account")]
        public virtual ICollection<PenaltyModel> Penalties { get; set; }
    }
}
