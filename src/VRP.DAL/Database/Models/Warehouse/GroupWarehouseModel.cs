﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Group;

namespace VRP.DAL.Database.Models.Warehouse
{
    public class GroupWarehouseModel
    {
        public GroupWarehouseModel()
        {
            ItemsInWarehouse = new HashSet<GroupWarehouseItemModel>();
        }

        public int Id { get; set; }

        // foreign keys
        [ForeignKey("Group")]
        public int GroupId { get; set; }

        // navigation properties
        public virtual GroupModel Group { get; set; }
        public virtual ICollection<GroupWarehouseItemModel> ItemsInWarehouse { get; set; }
    }
}
