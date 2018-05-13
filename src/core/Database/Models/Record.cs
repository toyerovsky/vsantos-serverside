
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VRP.Core.Enums;

namespace VRP.Core.Database.Models
{
    public class Record
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BornDate { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
        [EnumDataType(typeof(Race))]
        public Race Race { get; set; }
        //jednoLiteroweString starcz¹?

        public string EyeColor { get; set; }
        public string FingerPrints { get; set; }
        public string DNACode { get; set; }

        public bool Wanted { get; set; }
        public virtual ICollection<VehicleModel> Vehicles { get; set; }
        public virtual ICollection<CriminalCase> CriminalCases{ get; set; }


    }
}