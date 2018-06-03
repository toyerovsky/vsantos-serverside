using System.ComponentModel.DataAnnotations;
using VRP.Core.Database.Models;
using VRP.Core.Enums;

namespace VRP.Core.Database
{
    public class BuildingForSaleModel
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }

        // navigation properties
        public virtual BuildingModel Building { get; set; }
    }
}
