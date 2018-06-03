/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using VRP.Core.Database.Models.Character;

namespace VRP.Core.Database.Models.Misc
{
    public class DescriptionModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        // navigation properties
        public virtual CharacterModel Character { get; set; }
    }
}