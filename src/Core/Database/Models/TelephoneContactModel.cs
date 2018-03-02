/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.ComponentModel.DataAnnotations;

namespace Serverside.Core.Database.Models
{
    public class TelephoneContactModel
    {
        public long Id { get; set; }
        public ItemModel Cellphone { get; set; }
        public string Name { get; set; }
        [Phone]
        public int Number { get; set; }
    }
}