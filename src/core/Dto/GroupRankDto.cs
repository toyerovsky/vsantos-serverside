using System.Collections.Generic;

namespace VRP.BLL.Dto
{
    public class GroupRankDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rights { get; set; }
        public decimal Salary { get; set; }
        public int GroupId { get; set; }
        public GroupDto Group { get; set; }
        public ICollection<WorkerDto> Workers { get; set; }
    }
}
