using System.Collections.Generic;

namespace VRP.vAPI.Dto
{
    public class GroupRankDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rights { get; set; }
        public IEnumerable<WorkerDto> Workers { get; set; }
    }
}
