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
using VRP.Core.Database.Models.Account;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class AccountsRepository : Repository<RoleplayContext, AccountModel>
    {
        private readonly RoleplayContext _context;

        public AccountsRepository(RoleplayContext context) : base(context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public AccountsRepository() : this(Singletons.RoleplayContextFactory.Create())
        {
        }
       
        public override AccountModel Get(int id) => GetAll(account => account.Id == id).SingleOrDefault();

        public override AccountModel Get(Expression<Func<AccountModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override AccountModel GetNoRelated(Expression<Func<AccountModel, bool>> expression) => GetAllNoRelated(expression).FirstOrDefault();

        public override IEnumerable<AccountModel> GetAll(Expression<Func<AccountModel, bool>> expression = null)
        {
            IQueryable<AccountModel> accounts = expression != null ?
                _context.Accounts.Where(expression) :
                _context.Accounts;

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
                        .ThenInclude(group => group.Group)
                            .ThenInclude(group => group.Workers)
                .Include(account => account.Penalties);
        }

        public override IEnumerable<AccountModel> GetAllNoRelated(Expression<Func<AccountModel, bool>> expression = null)
        {
            IQueryable<AccountModel> accounts = expression != null ?
                _context.Accounts.Where(expression) :
                _context.Accounts;

            return accounts;
        }
    }
}