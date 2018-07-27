using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRP.vAPI.Dto
{
    public class WorkerDto
    {
        public int Id { get; set; }

        public decimal Salary { get; set; }
        public int DutyMinutes { get; set; }

      
        public string Rights { get; set; }

  
        public GroupDto Group { get; set; }
        public  CharacterDto Character { get; set; }
    }
}
