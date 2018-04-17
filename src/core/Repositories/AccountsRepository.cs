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
    public class AccountsRepository : IRepository<AccountModel>
    {
        private readonly RoleplayContext _context;

        public AccountsRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public AccountsRepository() : this(RolePlayContextFactory.NewContext())
        {
        }

        public void Insert(AccountModel model) => _context.Accounts.Add(model);

        public bool Contains(AccountModel model)
        {
            return _context.Accounts.Any(account => account.ForumUserId == model.ForumUserId);
        }

        public void Update(AccountModel model) => _context.Entry(model).State = EntityState.Modified;

        public void Delete(int id)
        {
            AccountModel account = _context.Accounts.Find(id);
            _context.Accounts.Remove(account);
        }

        public AccountModel Get(int id) => GetAll(account => account.Id == id).Single();

        public AccountModel GetByUserId(long userId) => GetAll(account => account.ForumUserId == userId).Single();

        public IEnumerable<AccountModel> GetAll(Expression<Func<AccountModel, bool>> expression = null)
        {
            IQueryable<AccountModel> accounts = expression != null ?
                _context.Accounts.Where(expression) :
                _context.Accounts;

            return accounts
                .Include(account => account.Characters)
                    .ThenInclude(character => character.Buildings)
                        .ThenInclude(building => building.Items)
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

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}