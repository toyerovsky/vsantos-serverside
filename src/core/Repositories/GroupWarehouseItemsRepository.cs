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
    public class GroupWarehouseItemsRepository : Repository<RoleplayContext, GroupWarehouseItemModel>
    {
        private readonly RoleplayContext _context;

        public GroupWarehouseItemsRepository(RoleplayContext context) : base(context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public GroupWarehouseItemsRepository() : this(Singletons.RoleplayContextFactory.Create())
        {
        }

        public override GroupWarehouseItemModel Get(int id) => GetAll(groupWarehouseItem => groupWarehouseItem.Id == id).SingleOrDefault();

        public override GroupWarehouseItemModel Get(Expression<Func<GroupWarehouseItemModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override IEnumerable<GroupWarehouseItemModel> GetAll(Expression<Func<GroupWarehouseItemModel, bool>> expression = null)
        {
            IQueryable<GroupWarehouseItemModel> groupWarehouseItems = expression != null ?
                _context.GroupWarehouseItems.Where(expression) :
                _context.GroupWarehouseItems;

            return groupWarehouseItems;
        }

        public override GroupWarehouseItemModel GetNoRelated(Expression<Func<GroupWarehouseItemModel, bool>> expression) => Get(expression);

        public override IEnumerable<GroupWarehouseItemModel> GetAllNoRelated(Expression<Func<GroupWarehouseItemModel, bool>> expression = null)
        {
            throw new NotImplementedException();
        }
    }
}