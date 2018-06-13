using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VRP.Core.Database.Models.Vehicle;
using VRP.Core.Enums;

namespace VRP.Core.Database.PartTimeJob
{
    public class PartTimeJobModel
    {
        public int Id { get; set; }
        [EnumDataType(typeof(JobType))]
        public JobType JobType { get; set; }
        public decimal DailyMoneyLimit { get; set; }

        // navigation properties
        public ICollection<VehicleModel> Vehicles { get; set; }
        public ICollection<PartTimeJobEmployerModel> Employers { get; set; }
    }
}