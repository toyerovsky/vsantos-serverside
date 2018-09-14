using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public decimal Salary { get; set; }

        // foreign keys
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        [ForeignKey("DefaultForGroup")]
        public int DefaultForGroupId { get; set; }

        // navigation properties
        public virtual GroupModel Group { get; set; }
        public virtual GroupModel DefaultForGroup { get; set; }
        public virtual ICollection<WorkerModel> Workers { get; set; }
    }
}