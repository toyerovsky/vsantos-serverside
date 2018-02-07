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
    public class GroupWarehouseOrdersRepository : IRepository<GroupWarehouseOrderModel>
    {
        private RoleplayContext Context { get; } = RolePlayContextFactory.NewContext();

        public void Insert(GroupWarehouseOrderModel model) => Context.GroupWarehouseOrders.Add(model);

        public bool Contains(GroupWarehouseOrderModel model)
        {
            return Context.GroupWarehouseOrders.Any(groupWarehouseOrder => groupWarehouseOrder.Id == model.Id);
        }

        public void Update(GroupWarehouseOrderModel model)
        {
            Context.Entry(model).State = EntityState.Modified;
        }

        public void Delete(long id)
        {
            var order = Context.GroupWarehouseOrders.Find(id);
            Context.GroupWarehouseOrders.Remove(order);
        }

        public GroupWarehouseOrderModel Get(long id) => GetAll().Single(b => b.Id == id);

        public IEnumerable<GroupWarehouseOrderModel> GetAll()
        {
            return Context.GroupWarehouseOrders.Include(
                order => order.Getter).ToList();
        }

        public void Save() => Context.SaveChanges();

        public void Dispose() => Context?.Dispose();
    }
}