/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

namespace VRP.DAL.Database.Models.Mdt
{
    public class CriminalCaseCharacterRecordRelation
    {
        public int Id { get; set; }

        // navigation properties
        public virtual CriminalCaseModel CriminalCase { get; set; }
        public virtual CharacterRecordModel CharacterRecord { get; set; }
    }
}
