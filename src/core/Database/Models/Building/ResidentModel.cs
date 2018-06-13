﻿using System.ComponentModel.DataAnnotations.Schema;
using VRP.Core.Database.Models.Agreement;
using VRP.Core.Database.Models.Character;

namespace VRP.Core.Database.Models.Building
{
    public class ResidentModel
    {
        public int Id { get; set; }

        // foreign keys
        [ForeignKey("Character")]
        public int CharacterId { get; set; }
        [ForeignKey("Building")]
        public int BuildingId { get; set; }

        // navigation properties
        public virtual CharacterModel Character { get; set; }
        public virtual BuildingModel Building { get; set; }
    }
}