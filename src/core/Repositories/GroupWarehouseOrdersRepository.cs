/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VRP.Core.Database;
using VRP.Core.Database.Models;
using VRP.Core.Database.Models.Warehouse;
using VRP.Core.Interfaces;

namespace VRP.Core.Repositories
{
    public class GroupWarehouseOrdersRepository : IRepository<GroupWarehouseOrderModel>
    {
        private readonly RoleplayContext _context;

        public GroupWarehouseOrdersRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public GroupWarehouseOrdersRepository() : this(RoleplayContextFactory.NewContext())
        {
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

        public void Delete(int id)
        {
            GroupWarehouseOrderModel order = _context.GroupWarehouseOrders.Find(id);
            _context.GroupWarehouseOrders.Remove(order);
        }

        public GroupWarehouseOrderModel Get(int id) => GetAll(groupWarehouseOrder => groupWarehouseOrder.Id == id).SingleOrDefault();

        public GroupWarehouseOrderModel GetNoRelated(int id)
        {
            GroupWarehouseOrderModel order = _context.GroupWarehouseOrders.Find(id);
            return order;
        }

        public GroupWarehouseOrderModel Get(Expression<Func<GroupWarehouseOrderModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public GroupWarehouseOrderModel GetNoRelated(Expression<Func<GroupWarehouseOrderModel, bool>> expression) => GetAllNoRelated(expression).FirstOrDefault();

        public IEnumerable<GroupWarehouseOrderModel> GetAll(Expression<Func<GroupWarehouseOrderModel, bool>> expression = null)
        {
            IQueryable<GroupWarehouseOrderModel> groupWarehouseOrders = expression != null ?
                _context.GroupWarehouseOrders.Where(expression) :
                _context.GroupWarehouseOrders;

            return groupWarehouseOrders
                .Include(order => order.Getter);
        }

        public IEnumerable<GroupWarehouseOrderModel> GetAllNoRelated(Expression<Func<GroupWarehouseOrderModel, bool>> expression = null)
        {
            IQueryable<GroupWarehouseOrderModel> groupWarehouseOrders = expression != null ?
                _context.GroupWarehouseOrders.Where(expression) :
                _context.GroupWarehouseOrders;

            return groupWarehouseOrders;
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}