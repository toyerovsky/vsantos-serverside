﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations;
using VRP.Core.Enums;

namespace VRP.Core.Database.Models
{
    public class WorkerModel
    {
        public int Id { get; set; }

        public virtual GroupModel Group { get; set; }
        public virtual CharacterModel Character { get; set; }

        public int Salary { get; set; }
        public int DutyMinutes { get; set; }

        [EnumDataType(typeof(GroupRights))]
        public virtual GroupRights Rights { get; set; }
    }
}