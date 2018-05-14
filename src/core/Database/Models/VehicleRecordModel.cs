using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VRP.Core.Enums;

namespace VRP.Core.Database.Models
{
    public class VehicleRecordModel
    {
        public int Id { get; set; }
        public string NumberPlate { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public RecordModel Owner { get; set; }
        public byte[] Image { get; set; }

        public bool Towed { get; set; }
        public bool Wanted { get; set; }
        public string[] SpecialFeatures { get; set; }
        public virtual ICollection<CriminalCaseModel> CriminalCases { get; set; }
    }
}