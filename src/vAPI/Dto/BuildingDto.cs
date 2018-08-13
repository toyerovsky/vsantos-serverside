namespace VRP.vAPI.Dto
{
    public class BuildingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CharacterDto Character { get; set; }
        public int CharacterId { get; set; }
        public GroupDto Group { get; set; }
        public int GroupId { get; set; }
    }
}
