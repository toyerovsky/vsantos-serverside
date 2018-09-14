/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public async Task<GroupWarehouseOrderModel> JoinAndGetAsync(int id)
        {
            return await JoinAndGetAll(account => account.Id == id).AsQueryable().SingleOrDefaultAsync();
        }

        public GroupWarehouseOrderModel JoinAndGet(Expression<Func<GroupWarehouseOrderModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public async Task<GroupWarehouseOrderModel> JoinAndGetAsync(Expression<Func<GroupWarehouseOrderModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }

        public IEnumerable<GroupWarehouseOrderModel> JoinAndGetAll(Expression<Func<GroupWarehouseOrderModel, bool>> expression = null)
        {
            IQueryable<GroupWarehouseOrderModel> groupWarehouseOrders = expression != null ?
                Context.GroupWarehouseOrders.Where(expression) :
                Context.GroupWarehouseOrders;

            return groupWarehouseOrders
                .Include(order => order.Getter);
        }

        public async Task<IEnumerable<GroupWarehouseOrderModel>> JoinAndGetAllAsync(Expression<Func<GroupWarehouseOrderModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().ToArrayAsync();
        }

        public override GroupWarehouseOrderModel Get(Expression<Func<GroupWarehouseOrderModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override IEnumerable<GroupWarehouseOrderModel> GetAll(Expression<Func<GroupWarehouseOrderModel, bool>> expression = null)
        {
            IQueryable<GroupWarehouseOrderModel> groupWarehouseOrders = expression != null ?
                Context.GroupWarehouseOrders.Where(expression) :
                Context.GroupWarehouseOrders;

            return groupWarehouseOrders;
        }
    }
}