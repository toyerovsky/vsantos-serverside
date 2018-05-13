using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VRP.Core.Enums;

namespace VRP.Core.Database.Models
{
    public class VehicleRecord
    {
        public int Id { get; set; }
        public string NumberPlate { get; set; }
        public string Model { get; set; }
        public string Color {get;set;}
        public Record Owner { get; set; }

        public bool Towed { get; set; }
        public bool Wanted { get; set; }
        public virtual ICollection<String> SpecialFeatures { get; set; }
        public virtual ICollection<CriminalCase> CriminalCases { get; set; }
    }
}