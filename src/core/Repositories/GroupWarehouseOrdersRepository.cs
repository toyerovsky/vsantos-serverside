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
using VRP.Core.Database.Models.Warehouse;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class GroupWarehouseOrdersRepository : Repository<RoleplayContext, GroupWarehouseOrderModel>
    {
        private readonly RoleplayContext _context;

        public GroupWarehouseOrdersRepository(RoleplayContext context) : base(context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public GroupWarehouseOrdersRepository() : this(Singletons.RoleplayContextFactory.Create())
        {
        }

        public override GroupWarehouseOrderModel Get(int id) => GetAll(groupWarehouseOrder => groupWarehouseOrder.Id == id).SingleOrDefault();

        public override GroupWarehouseOrderModel Get(Expression<Func<GroupWarehouseOrderModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override GroupWarehouseOrderModel GetNoRelated(Expression<Func<GroupWarehouseOrderModel, bool>> expression) => GetAllNoRelated(expression).FirstOrDefault();

        public override IEnumerable<GroupWarehouseOrderModel> GetAll(Expression<Func<GroupWarehouseOrderModel, bool>> expression = null)
        {
            IQueryable<GroupWarehouseOrderModel> groupWarehouseOrders = expression != null ?
                _context.GroupWarehouseOrders.Where(expression) :
                _context.GroupWarehouseOrders;

            return groupWarehouseOrders
                .Include(order => order.Getter);
        }

        public override IEnumerable<GroupWarehouseOrderModel> GetAllNoRelated(Expression<Func<GroupWarehouseOrderModel, bool>> expression = null)
        {
            IQueryable<GroupWarehouseOrderModel> groupWarehouseOrders = expression != null ?
                _context.GroupWarehouseOrders.Where(expression) :
                _context.GroupWarehouseOrders;

            return groupWarehouseOrders;
        }
    }
}