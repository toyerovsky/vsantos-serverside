/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.ComponentModel.DataAnnotations;

namespace Serverside.Core.Database.Models
{
    public class DescriptionModel
    {
        [Key]
        public long Id { get; set; }
        public CharacterModel Character { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}