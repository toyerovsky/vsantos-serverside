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
    public class GroupWarehouseItemsRepository : IRepository<GroupWarehouseItemModel>
    {
        private readonly RoleplayContext _context;

        public GroupWarehouseItemsRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public GroupWarehouseItemsRepository()
        {
            _context = RolePlayContextFactory.NewContext();
        }

        public void Insert(GroupWarehouseItemModel model) => _context.GroupWarehouseItems.Add(model);

        public bool Contains(GroupWarehouseItemModel model)
        {
            return _context.GroupWarehouseItems.Any(groupWarehouseItem => groupWarehouseItem.Id == model.Id);
        }

        public void Update(GroupWarehouseItemModel model) => _context.Entry(model).State = EntityState.Modified;

        public void Delete(long id)
        {
            GroupWarehouseItemModel warehouseItem = _context.GroupWarehouseItems.Find(id);
            _context.GroupWarehouseItems.Remove(warehouseItem);
        }

        public GroupWarehouseItemModel Get(long id) => GetAll().Single(b => b.Id == id);

        public IEnumerable<GroupWarehouseItemModel> GetAll()
        {
            return _context.GroupWarehouseItems.ToList();
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}