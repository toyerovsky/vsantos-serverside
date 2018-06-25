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
using VRP.DAL.Database.Models.Item;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class ItemsRepository : Repository<RoleplayContext, ItemModel>, IJoinableRepository<ItemModel>
    {
        public ItemsRepository(RoleplayContext context) : base(context)
        {
        }

        public override void Insert(ItemModel model)
        {
            if ((model.Building?.Id ?? 0) != 0)
                Context.Attach(model.Building);

            if ((model.Character?.Id ?? 0) != 0)
                Context.Attach(model.Character);

            if ((model.OwnerVehicle?.Id ?? 0) != 0)
                Context.Attach(model.OwnerVehicle);

            if ((model.TuningInVehicle?.Id ?? 0) != 0)
                Context.Attach(model.TuningInVehicle);

            Context.Items.Add(model);
        }

        public ItemModel JoinAndGet(int id) => JoinAndGetAll(item => item.Id == id).SingleOrDefault();

        public ItemModel JoinAndGet(Expression<Func<ItemModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<ItemModel> JoinAndGetAll(Expression<Func<ItemModel, bool>> expression = null)
        {
            IQueryable<ItemModel> items = expression != null ?
                Context.Items.Where(expression) :
                Context.Items;

            return items
                .Include(item => item.Building)
                .Include(item => item.Character)
                    .ThenInclude(character => character.Account)
                .Include(item => item.TuningInVehicle)
                .Include(item => item.OwnerVehicle);
        }

        public override ItemModel Get(Func<ItemModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<ItemModel> GetAll(Func<ItemModel, bool> func = null)
        {
            IEnumerable<ItemModel> items = func != null ?
                Context.Items.Where(func) :
                Context.Items;

            return items;
        }
    }
}