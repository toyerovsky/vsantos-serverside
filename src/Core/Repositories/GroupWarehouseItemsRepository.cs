/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Serverside.Core.Database;
using Serverside.Core.Database.Models;
using Serverside.Core.Interfaces;

namespace Serverside.Core.Repositories
{
    public class GroupWarehouseItemsRepository : IRepository<GroupWarehouseItemModel>
    {
        private RoleplayContext Context { get; } = RolePlayContextFactory.NewContext();

        public void Insert(GroupWarehouseItemModel model) => Context.GroupWarehouseItems.Add(model);

        public bool Contains(GroupWarehouseItemModel model)
        {
            return Context.GroupWarehouseItems.Any(groupWarehouseItem => groupWarehouseItem.Id == model.Id);
        }

        public void Update(GroupWarehouseItemModel model) => Context.Entry(model).State = EntityState.Modified;

        public void Delete(long id)
        {
            var warehouseItem = Context.GroupWarehouseItems.Find(id);
            Context.GroupWarehouseItems.Remove(warehouseItem);
        }

        public GroupWarehouseItemModel Get(long id) => GetAll().Single(b => b.Id == id);

        public IEnumerable<GroupWarehouseItemModel> GetAll()
        {
            return Context.GroupWarehouseItems.ToList();
        }

        public void Save() => Context.SaveChanges();

        public void Dispose() => Context?.Dispose();
    }
}