﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using VRP.Core.Database;
using VRP.Core.Database.Models.Warehouse;
using VRP.Core.Interfaces;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class GroupWarehousesRepository : Repository<RoleplayContext, GroupWarehouseModel>, IJoinableRepository<GroupWarehouseModel>
    {
        public GroupWarehousesRepository(RoleplayContext context) : base(context)
        {
        }

        public override GroupWarehouseModel Get(Func<GroupWarehouseModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<GroupWarehouseModel> GetAll(Func<GroupWarehouseModel, bool> func = null)
        {
            IEnumerable<GroupWarehouseModel> groupWarehouses = func != null ?
                Context.GroupWarehouses.Where(func) :
                Context.GroupWarehouses;

            return groupWarehouses;
        }

        public GroupWarehouseModel JoinAndGet(int id) =>
            JoinAndGetAll(groupWarehouse => groupWarehouse.Id == id).SingleOrDefault();

        public GroupWarehouseModel JoinAndGet(Expression<Func<GroupWarehouseModel, bool>> expression) =>
            JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<GroupWarehouseModel> JoinAndGetAll(Expression<Func<GroupWarehouseModel, bool>> expression)
        {
            IQueryable<GroupWarehouseModel> groupWarehouses = expression != null ?
                Context.GroupWarehouses.Where(expression) :
                Context.GroupWarehouses;

            return groupWarehouses
                .Include(groupWarehouse => groupWarehouse.Group)
                .Include(groupWarehouse => groupWarehouse.ItemsInWarehouse);
        }
    }
}