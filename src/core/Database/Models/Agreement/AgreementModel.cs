using System;
using VRP.Core.Database.Models.Misc;

namespace VRP.Core.Database.Models.Lease
{
    public class AgreementModel
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool AutoRenewal { get; set; }

        // navigation properties
        public virtual LeaseModel LeaseModel { get; set; }

        public virtual GroupModel LeaserGroup { get; set; }
        public virtual CharacterModel LeaserCharacter { get; set; }
    }
}
