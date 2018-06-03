using System;
using System.Collections.Generic;
using System.Text;

namespace VRP.Core.Database.Models
{
    public class GroupWarehouseModel
    {
        public int Id { get; set; }
        public GroupModel Group { get; set; }

        public virtual ICollection<GroupWarehouseItemModel> ItemsInWarehouse { get; set; }
    }
}
