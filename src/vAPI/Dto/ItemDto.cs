using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRP.vAPI.Dto
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ItemType { get; set; }
        public int CharacterId { get; set; }
        public CharacterDto Character { get; set; }
        public int BuildingId { get; set; }
        public BuildingDto Building { get; set; }
        public int VehicleId { get; set; }
        public VehicleDto Vehicle { get; set; }
        public int TuningInVehicleId { get; set; }
        public VehicleDto TuningInVehicle { get; set; }
    }
}
