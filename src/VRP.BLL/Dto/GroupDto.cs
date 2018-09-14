using System;
using System.Collections.Generic;

namespace VRP.BLL.Dto
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public int Grant { get; set; }
        public int MaxPayday { get; set; }
        public decimal Money { get; set; }
        public string Color { get; set; }
        public string GroupType { get; set; }
        public string ImageUploadUrl { get; set; }
        public DateTime ImageUploadDate { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
        public int BossCharacterId { get; set; }
        public CharacterDto BossCharacter { get; set; }
        public int DefaultRankId { get; set; }
        public GroupRankDto DefaultRank { get; set; }
        public ICollection<WorkerDto> Workers { get; set; }
        public ICollection<VehicleDto> Vehicles { get; set; }
        public ICollection<GroupDto> Groups { get; set; }
        public ICollection<GroupRankDto> GroupRanks { get; set; }
    }
}
