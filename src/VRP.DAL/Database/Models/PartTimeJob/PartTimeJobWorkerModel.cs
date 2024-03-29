﻿using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Character;

namespace VRP.DAL.Database.Models.PartTimeJob
{
    public class PartTimeJobWorkerModel
    {
        public int Id { get; set; }
        public decimal Salary { get; set; }

        // foreign keys
        [ForeignKey("PartTimeJobEmployer")]
        public int PartTimeJobEmployerId { get; set; }
        [ForeignKey("Character")]
        public int CharacterId { get; set; }

        // navigation properties
        public CharacterModel Character { get; set; }
        public PartTimeJobEmployerModel PartTimeJobEmployerModel { get; set; }
    }
}