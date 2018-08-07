using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VRP.DAL.Database.Models.Vehicle;
using VRP.DAL.Enums;

namespace VRP.DAL.Database.Models.PartTimeJob
{
    public class PartTimeJobModel
    {
        public PartTimeJobModel()
        {
            Vehicles = new HashSet<VehicleModel>();
            Employers = new HashSet<PartTimeJobEmployerModel>();
        }

        public int Id { get; set; }
        [EnumDataType(typeof(JobType))]
        public JobType JobType { get; set; }
        public decimal DailyMoneyLimit { get; set; }

        // navigation properties
        public ICollection<VehicleModel> Vehicles { get; set; }
        public ICollection<PartTimeJobEmployerModel> Employers { get; set; }
    }
}