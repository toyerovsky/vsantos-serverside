using System;
using System.Collections.Generic;
using System.Text;

namespace VRP.Core.Database.Models.Mdt
{
    public class CriminalCaseCharacterRecordRelation
    {
        public int Id { get; set; }

        // navigation properties
        public virtual CriminalCaseModel CriminalCase { get; set; }
        public virtual CharacterRecordModel CharacterRecord { get; set; }
    }
}
