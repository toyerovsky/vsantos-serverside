using System.ComponentModel.DataAnnotations;
using VRP.Core.Database.Models;
using VRP.Core.Enums;

namespace VRP.Core.Database
{
    public class AutoSaleModel
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }

        // navigation properties
        public virtual VehicleModel VehicleModel { get; set; }
        public virtual BuildingModel BuildingModel { get; set; }
    }
}
