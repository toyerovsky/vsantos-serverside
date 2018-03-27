/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VRP.Core.Enums;

namespace VRP.Core.Database.Models
{
    public class AccountModel
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public long ForumUserId { get; set; }
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
        public long ForumGroup { get; set; }
        public string OtherForumGroups { get; set; }
        [StringLength(50)]
        public string SocialClub { get; set; }
        [StringLength(16)]
        public string Ip { get; set; }
        public bool Online { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime LastLogin { get; set; }
        [EnumDataType(typeof(ServerRank))]
        public ServerRank ServerRank { get; set; }
        public string Serial { get; set; }

        public virtual ICollection<CharacterModel> Characters { get; set; }
        public virtual ICollection<PenaltyModel> Penalties { get; set; }
    }
}
