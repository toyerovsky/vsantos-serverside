/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
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
        private readonly RoleplayContext _context;

        public GroupWarehouseOrdersRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public GroupWarehouseOrdersRepository()
        {
            _context = RolePlayContextFactory.NewContext();
        }

        public void Insert(GroupWarehouseOrderModel model) => _context.GroupWarehouseOrders.Add(model);

        public bool Contains(GroupWarehouseOrderModel model)
        {
            return _context.GroupWarehouseOrders.Any(groupWarehouseOrder => groupWarehouseOrder.Id == model.Id);
        }

        public void Update(GroupWarehouseOrderModel model)
        {
            _context.Entry(model).State = EntityState.Modified;
        }

        public void Delete(long id)
        {
            var order = _context.GroupWarehouseOrders.Find(id);
            _context.GroupWarehouseOrders.Remove(order);
        }

        public GroupWarehouseOrderModel Get(long id) => GetAll().Single(b => b.Id == id);

        public IEnumerable<GroupWarehouseOrderModel> GetAll()
        {
            return _context.GroupWarehouseOrders.Include(
                order => order.Getter).ToList();
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}