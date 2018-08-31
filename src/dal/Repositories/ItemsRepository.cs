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

        public ItemModel JoinAndGet(int id) => JoinAndGetAll(item => item.Id == id).SingleOrDefault();

        public async Task<ItemModel> JoinAndGetAsync(int id)
        {
            return await JoinAndGetAll(account => account.Id == id).AsQueryable().SingleOrDefaultAsync();
        }

        public ItemModel JoinAndGet(Expression<Func<ItemModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public async Task<ItemModel> JoinAndGetAsync(Expression<Func<ItemModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }

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
                .Include(item => item.Vehicle);
        }

        public async Task<IEnumerable<ItemModel>> JoinAndGetAllAsync(Expression<Func<ItemModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().ToArrayAsync();
        }

        public override ItemModel Get(Expression<Func<ItemModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override IEnumerable<ItemModel> GetAll(Expression<Func<ItemModel, bool>> expression = null)
        {
            IQueryable<ItemModel> items = expression != null ?
                Context.Items.Where(expression) :
                Context.Items;

            return items;
        }
    }
}