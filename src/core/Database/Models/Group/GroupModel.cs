﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VRP.Core.Database.Models.Lease;
using VRP.Core.Enums;

namespace VRP.Core.Database.Models
{
    public class GroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public int Dotation { get; set; }
        public int MaxPayday { get; set; }
        public decimal Money { get; set; }
        public string Color { get; set; }

        [EnumDataType(typeof(GroupType))]
        public virtual GroupType GroupType { get; set; }
        
        // navigation properties
        /// <summary>
        /// Pierwotny szef biznesu, który nie moze go opuscic
        /// </summary>
        public virtual CharacterModel BossCharacter { get; set; }
        public virtual ICollection<WorkerModel> Workers { get; set; }
        public virtual ICollection<AgreementModel> Agreements { get; set; }
    }
}
