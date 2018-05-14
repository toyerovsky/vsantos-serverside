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
using VRP.Core.Interfaces;

namespace VRP.Core.Repositories
{
    public class GroupWarehouseItemsRepository : IRepository<GroupWarehouseItemModel>
    {
        private readonly RoleplayContext _context;

        public GroupWarehouseItemsRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public GroupWarehouseItemsRepository() : this(RolePlayContextFactory.NewContext())
        {
        }

        public void Insert(GroupWarehouseItemModel model)
        {
            _context.GroupWarehouseItems.Add(model);
        }

        public bool Contains(GroupWarehouseItemModel model)
        {
            return _context.GroupWarehouseItems.Any(groupWarehouseItem => groupWarehouseItem.Id == model.Id);
        }

        public void Update(GroupWarehouseItemModel model) => _context.Entry(model).State = EntityState.Modified;

        public void Delete(int id)
        {
            GroupWarehouseItemModel warehouseItem = _context.GroupWarehouseItems.Find(id);
            _context.GroupWarehouseItems.Remove(warehouseItem);
        }

        public GroupWarehouseItemModel Get(int id) => GetAll(groupWarehouseItem => groupWarehouseItem.Id == id).SingleOrDefault();

        public GroupWarehouseItemModel GetNoRelated(int id)
        {
            GroupWarehouseItemModel warehouseItem = _context.GroupWarehouseItems.Find(id);
            return warehouseItem;
        }

        public GroupWarehouseItemModel Get(Expression<Func<GroupWarehouseItemModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public IEnumerable<GroupWarehouseItemModel> GetAll(Expression<Func<GroupWarehouseItemModel, bool>> expression = null)
        {
            IQueryable<GroupWarehouseItemModel> groupWarehouseItems = expression != null ?
                _context.GroupWarehouseItems.Where(expression) :
                _context.GroupWarehouseItems;

            return groupWarehouseItems;
        }

        public GroupWarehouseItemModel GetNoRelated(Expression<Func<GroupWarehouseItemModel, bool>> expression) => Get(expression);

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}