/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations;
using VRP.Core.Database.Models.Item;

namespace VRP.Core.Database.Models.Telephone
{
    public class TelephoneMessageModel
    {
        public int Id { get; set; }
        [MaxLength(256)]
        public string Content { get; set; }
        [Phone]
        public int SenderNumber { get; set; }

        // navigation properties
        public virtual ItemModel Cellphone { get; set; }
    }
}