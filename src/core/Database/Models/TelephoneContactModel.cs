/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations;

namespace VRP.Core.Database.Models
{
    public class TelephoneContactModel
    {
        public int Id { get; set; }
        public ItemModel Cellphone { get; set; }
        public string Name { get; set; }
        [Phone]
        public int Number { get; set; }
    }
}