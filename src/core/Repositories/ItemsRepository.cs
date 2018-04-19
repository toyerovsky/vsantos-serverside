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

        public void Insert(ItemModel model)
        {
            if ((model.Building?.Id ?? 0) != 0)
                _context.Attach(model.Building);

            if ((model.Group?.Id ?? 0) != 0)
                _context.Attach(model.Group);

            if ((model.Character?.Id ?? 0) != 0)
                _context.Attach(model.Character);

            if ((model.Vehicle?.Id ?? 0) != 0)
                _context.Attach(model.Vehicle);

            _context.Items.Add(model);
        }

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

        public ItemModel Get(int id) => GetAll(i => i.Id == id).Single();

        public IEnumerable<ItemModel> GetAll(Expression<Func<ItemModel, bool>> expression = null)
        {
            IQueryable<ItemModel> items = expression != null ?
                _context.Items.Where(expression) :
                _context.Items;

            return items
                .Include(item => item.Building)
                .Include(item => item.Character)
                    .ThenInclude(character => character.Account)
                .Include(item => item.Group)
                    .ThenInclude(group => group.BossCharacter)
                        .ThenInclude(bossCharacter => bossCharacter.Account);
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}