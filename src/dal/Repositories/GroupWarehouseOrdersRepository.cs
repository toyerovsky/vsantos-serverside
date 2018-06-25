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
using VRP.DAL.Database;
using VRP.DAL.Database.Models.Warehouse;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class GroupWarehouseOrdersRepository : Repository<RoleplayContext, GroupWarehouseOrderModel>, IJoinableRepository<GroupWarehouseOrderModel>
    {
        public GroupWarehouseOrdersRepository(RoleplayContext context) : base(context)
        {
        }

        public GroupWarehouseOrderModel JoinAndGet(int id) => JoinAndGetAll(groupWarehouseOrder => groupWarehouseOrder.Id == id).SingleOrDefault();

        public GroupWarehouseOrderModel JoinAndGet(Expression<Func<GroupWarehouseOrderModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<GroupWarehouseOrderModel> JoinAndGetAll(Expression<Func<GroupWarehouseOrderModel, bool>> expression = null)
        {
            IQueryable<GroupWarehouseOrderModel> groupWarehouseOrders = expression != null ?
                Context.GroupWarehouseOrders.Where(expression) :
                Context.GroupWarehouseOrders;

            return groupWarehouseOrders
                .Include(order => order.Getter);
        }

        public override GroupWarehouseOrderModel Get(Func<GroupWarehouseOrderModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<GroupWarehouseOrderModel> GetAll(Func<GroupWarehouseOrderModel, bool> func = null)
        {
            IEnumerable<GroupWarehouseOrderModel> groupWarehouseOrders = func != null ?
                Context.GroupWarehouseOrders.Where(func) :
                Context.GroupWarehouseOrders;

            return groupWarehouseOrders;
        }
    }
}