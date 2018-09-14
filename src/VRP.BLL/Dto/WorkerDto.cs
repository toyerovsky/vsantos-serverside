namespace VRP.BLL.Dto
{
    public class WorkerDto
    {
        public int Id { get; set; }
        public decimal Salary { get; set; }
        public int DutyMinutes { get; set; }
        public int Rights { get; set; }
        public int GroupId { get; set; }
        public GroupDto Group { get; set; }
        public int CharacterId { get; set; }
        public CharacterDto Character { get; set; }
        public int? GroupRankId { get; set; }
        public GroupRankDto GroupRank { get; set; }
    }
}
