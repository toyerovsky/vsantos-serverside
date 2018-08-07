using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VRP.DAL.Enums;

namespace VRP.vAPI.Dto
{
    public class WorkerDto
    {
        public int Id { get; set; }
        public decimal Salary { get; set; }
        public int DutyMinutes { get; set; }
        public int Rights { get; set; }
        public GroupDto Group { get; set; }
        public CharacterDto Character { get; set; }
        public GroupRankDto GroupRank { get; set; }
    }
}
