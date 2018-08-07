﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.Enums;

namespace VRP.DAL.Database.Models.Group
{
    public class WorkerModel
    {
        public int Id { get; set; }

        public decimal Salary { get; set; }
        public int DutyMinutes { get; set; }

        [EnumDataType(typeof(GroupRights))]
        public GroupRights Rights { get; set; }

        // navigation properties
        public virtual GroupModel Group { get; set; }
        public virtual CharacterModel Character { get; set; }
        public virtual GroupRankModel GroupRank { get; set; }
    }
}