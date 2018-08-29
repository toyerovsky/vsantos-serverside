namespace VRP.BLL.Dto
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public string NumberPlate { get; set; }
        public int NumberPlateStyle { get; set; }
        public string Name { get; set; }
        public string VehicleHash { get; set; }
        public float Health { get; set; }
        public float Milage { get; set; }
        public float Fuel { get; set; }
        public float FuelTank { get; set; }
        public float FuelConsumption { get; set; }
        public CharacterDto Character { get; set; }
        public int CharacterId { get; set; }
        public GroupDto Group { get; set; }
        public int GroupId { get; set; }
    }
}