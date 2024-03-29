﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Item;

namespace VRP.DAL.Database.Models.Telephone
{
    public class TelephoneMessageModel
    {
        public int Id { get; set; }
        [MaxLength(256)]
        public string Content { get; set; }
        [Phone]
        public int SenderNumber { get; set; }

        // foreign keys
        [ForeignKey("Cellphone")]
        public int CellphoneId { get; set; }
        
        // navigation properties
        public virtual ItemModel Cellphone { get; set; }
    }
}