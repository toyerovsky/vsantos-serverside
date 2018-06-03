using VRP.Core.Database.Models;

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
