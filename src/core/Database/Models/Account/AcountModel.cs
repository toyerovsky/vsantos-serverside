/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VRP.Core.Database.Models.Character;
using VRP.Core.Enums;

namespace VRP.Core.Database.Models.Account
{
    public class AccountModel
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [Key]
        public long ForumUserId { get; set; }
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
        public long PrimaryForumGroup { get; set; }
        public string SecondaryForumGroups { get; set; }
        [StringLength(50)]
        public string SocialClub { get; set; }
        public DateTime LastLogin { get; set; }
        [EnumDataType(typeof(ServerRank))]
        public ServerRank ServerRank { get; set; }
        public string SerialsJson { get; set; }

        // navigation properties
        public virtual ICollection<CharacterModel> Characters { get; set; }
        public virtual ICollection<PenaltyModel> Penalties { get; set; }
    }
}
