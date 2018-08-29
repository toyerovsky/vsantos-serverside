using System;

namespace VRP.BLL.Dto
{
    public class BuildingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool SpawnPossible { get; set; }
        public float ExternalPickupPositionX { get; set; }
        public float ExternalPickupPositionY { get; set; }
        public float ExternalPickupPositionZ { get; set; }
        public float InternalPickupPositionX { get; set; }
        public float InternalPickupPositionY { get; set; }
        public float InternalPickupPositionZ { get; set; }
        public CharacterDto Character { get; set; }
        public int CharacterId { get; set; }
        public GroupDto Group { get; set; }
        public int GroupId { get; set; }
        public int CreatorId { get; set; }
        public AccountDto Creator { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
