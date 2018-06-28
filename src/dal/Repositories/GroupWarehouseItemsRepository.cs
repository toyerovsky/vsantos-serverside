﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
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
    public class GroupWarehouseItemsRepository : Repository<RoleplayContext, GroupWarehouseItemModel>, IJoinableRepository<GroupWarehouseItemModel>
    {
        public GroupWarehouseItemsRepository(RoleplayContext context) : base(context)
        {
        }

        public GroupWarehouseItemModel JoinAndGet(int id) =>
            JoinAndGetAll(groupWarehouseItem => groupWarehouseItem.Id == id).SingleOrDefault();

        public GroupWarehouseItemModel JoinAndGet(Expression<Func<GroupWarehouseItemModel, bool>> expression) =>
            JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<GroupWarehouseItemModel> JoinAndGetAll(Expression<Func<GroupWarehouseItemModel, bool>> expression)
        {
            IQueryable<GroupWarehouseItemModel> groupWarehouseItems = expression != null ?
                Context.GroupWarehouseItems.Where(expression) :
                Context.GroupWarehouseItems;

            return groupWarehouseItems
                .Include(groupWarehouseItem => groupWarehouseItem.GroupWarehouseModel)
                .Include(groupWarehouseItem => groupWarehouseItem.ItemTemplateModel);
        }

        public override GroupWarehouseItemModel Get(Func<GroupWarehouseItemModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<GroupWarehouseItemModel> GetAll(Func<GroupWarehouseItemModel, bool> func = null)
        {
            IEnumerable<GroupWarehouseItemModel> groupWarehouseItems = func != null ?
                Context.GroupWarehouseItems.Where(func) :
                Context.GroupWarehouseItems;

            return groupWarehouseItems;
        }
    }
}