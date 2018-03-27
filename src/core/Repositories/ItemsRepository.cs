/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VRP.Core.Database;
using VRP.Core.Database.Models;
using VRP.Core.Interfaces;

namespace VRP.Core.Repositories
{
    public class ItemsRepository : IRepository<ItemModel>
    {
        private readonly RoleplayContext _context;

        public ItemsRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public ItemsRepository() : this(RolePlayContextFactory.NewContext())
        {

        }

        public void Insert(ItemModel model) => _context.Items.Add(model);

        public bool Contains(ItemModel model)
        {
            return _context.Items.Any(item => item.Id == model.Id);
        }

        public void Update(ItemModel model) => _context.Entry(model).State = EntityState.Modified;

        public void Delete(int id)
        {
            ItemModel item = _context.Items.Find(id);
            _context.Items.Remove(item);
        }

        public ItemModel Get(int id) => GetAll().Single(i => i.Id == id);

        public IEnumerable<ItemModel> GetAll()
        {
            return _context.Items
                .Include(item => item.Creator)
                .Include(item => item.Building)
                    .ThenInclude(building => building.Creator)
                .Include(item => item.Character)
                    .ThenInclude(character => character.Account)
                .Include(item => item.Group)
                    .ThenInclude(group => group.BossCharacter)
                        .ThenInclude(bossCharacter => bossCharacter.Account)
                .ToList();
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}