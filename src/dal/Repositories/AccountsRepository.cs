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
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class AccountsRepository : Repository<RoleplayContext, AccountModel>, IJoinableRepository<AccountModel>
    {
        public AccountsRepository(RoleplayContext context) : base(context)
        {
        }

        public AccountModel JoinAndGet(int id) => JoinAndGetAll(account => account.Id == id).SingleOrDefault();

        public async Task<AccountModel> JoinAndGetAsync(int id)
        {
            return await JoinAndGetAll(account => account.Id == id).AsQueryable().SingleOrDefaultAsync();
        }

        public AccountModel JoinAndGet(Expression<Func<AccountModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public async Task<AccountModel> JoinAndGetAsync(Expression<Func<AccountModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }

        public IEnumerable<AccountModel> JoinAndGetAll(Expression<Func<AccountModel, bool>> expression = null)
        {
            IQueryable<AccountModel> accounts = expression != null ?
                Context.Accounts.Where(expression) :
                Context.Accounts;

            return accounts
                .Include(account => account.Characters)
                    .ThenInclude(character => character.Buildings)
                        .ThenInclude(building => building.ItemsInBuilding)
                .Include(account => account.Characters)
                    .ThenInclude(character => character.Buildings)
                .Include(account => account.Characters)
                    .ThenInclude(character => character.Items)
                .Include(account => account.Characters)
                    .ThenInclude(character => character.Descriptions)
                .Include(account => account.Characters)
                    .ThenInclude(character => character.Vehicles)
                .Include(account => account.Characters)
                    .ThenInclude(character => character.Vehicles)
                        .ThenInclude(vehicle => vehicle.ItemsInVehicle)
                .Include(account => account.Characters)
                    .ThenInclude(character => character.Workers)
                        .ThenInclude(group => group.Group)
                            .ThenInclude(group => group.BossCharacter)
                .Include(account => account.Characters)
                    .ThenInclude(character => character.Workers)
                        .ThenInclude(worker => worker.Group)
                            .ThenInclude(group => group.Workers)
                .Include(account => account.Penalties)
                .Include(account => account.AdminInTickets)
                .Include(account => account.UserInTickets);
        }

        public async Task<IEnumerable<AccountModel>> JoinAndGetAllAsync(Expression<Func<AccountModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().ToArrayAsync();
        }

        public override AccountModel Get(Expression<Func<AccountModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override IEnumerable<AccountModel> GetAll(Expression<Func<AccountModel, bool>> expression = null)
        {
            IQueryable<AccountModel> accounts = expression != null ?
                Context.Accounts.Where(expression) :
                Context.Accounts;

            return accounts;
        }
    }
}