using System;

namespace VRP.BLL.Dto
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ItemType { get; set; }
        public int CharacterId { get; set; }
        public int? FirstParameter { get; set; }
        public int? SecondParameter { get; set; }
        public int? ThirdParameter { get; set; }
        public int? FourthParameter { get; set; }
        public CharacterDto Character { get; set; }
        public int BuildingId { get; set; }
        public BuildingDto Building { get; set; }
        public int VehicleId { get; set; }
        public VehicleDto Vehicle { get; set; }
        public int TuningInVehicleId { get; set; }
        public VehicleDto TuningInVehicle { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
