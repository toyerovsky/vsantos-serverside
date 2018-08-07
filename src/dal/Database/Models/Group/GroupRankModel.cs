using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VRP.DAL.Enums;

namespace VRP.DAL.Database.Models.Group
{
    public class GroupRankModel
    {
        public GroupRankModel()
        {
            Workers = new HashSet<WorkerModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        [EnumDataType(typeof(GroupRights))]
        public GroupRights Rights { get; set; }

        // navigation properties
        public virtual ICollection<WorkerModel> Workers { get; set; }
    }
}