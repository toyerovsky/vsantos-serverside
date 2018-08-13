using System.Collections.Generic;

namespace VRP.vAPI.Dto
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public int Dotation { get; set; }
        public int MaxPayday { get; set; }
        public decimal Money { get; set; }
        public string Color { get; set; }
        public string GroupType { get; set; }
        public IEnumerable<WorkerDto> Workers { get; set; }
        public IEnumerable<VehicleDto> Vehicles { get; set; }
        public IEnumerable<GroupDto> Groups { get; set; }
    }
}
