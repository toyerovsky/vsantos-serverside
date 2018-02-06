/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.ComponentModel.DataAnnotations;

namespace Serverside.Core.Database.Models
{
    public class TelephoneMessageModel
    {
        [Key]
        public long Id { get; set; }
        public virtual ItemModel Cellphone { get; set; }
        [MaxLength(256)]
        public string Content { get; set; }
        [Phone]
        public int SenderNumber { get; set; }
    }
}