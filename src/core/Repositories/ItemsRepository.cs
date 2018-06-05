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
using VRP.Core.Database.Models.Item;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class ItemsRepository : Repository<RoleplayContext, ItemModel>
    {
        private readonly RoleplayContext _context;

        public ItemsRepository(RoleplayContext context) : base(context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public ItemsRepository() : this(Singletons.RoleplayContextFactory.Create())
        {
        }

        public override void Insert(ItemModel model)
        {
            if ((model.Building?.Id ?? 0) != 0)
                _context.Attach(model.Building);

            if ((model.Character?.Id ?? 0) != 0)
                _context.Attach(model.Character);

            if ((model.OwnerVehicle?.Id ?? 0) != 0)
                _context.Attach(model.OwnerVehicle);

            if ((model.TuningInVehicle?.Id ?? 0) != 0)
                _context.Attach(model.TuningInVehicle);

            _context.Items.Add(model);
        }

        public override ItemModel Get(int id) => GetAll(item => item.Id == id).SingleOrDefault();

        public override ItemModel Get(Expression<Func<ItemModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override ItemModel GetNoRelated(Expression<Func<ItemModel, bool>> expression) => GetAllNoRelated(expression).FirstOrDefault();

        public override IEnumerable<ItemModel> GetAll(Expression<Func<ItemModel, bool>> expression = null)
        {
            IQueryable<ItemModel> items = expression != null ?
                _context.Items.Where(expression) :
                _context.Items;

            return items
                .Include(item => item.Building)
                .Include(item => item.Character)
                    .ThenInclude(character => character.Account)
                .Include(item => item.TuningInVehicle)
                .Include(item => item.OwnerVehicle);
        }

        public override IEnumerable<ItemModel> GetAllNoRelated(Expression<Func<ItemModel, bool>> expression = null)
        {
            IQueryable<ItemModel> items = expression != null ?
                _context.Items.Where(expression) :
                _context.Items;

            return items;
        }
    }
}