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
    public class AccountsRepository : IRepository<AccountModel>
    {
        private readonly RoleplayContext _context;

        public AccountsRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public AccountsRepository()
        {
            _context = RolePlayContextFactory.NewContext();
        }

        public void Insert(AccountModel model) => _context.Accounts.Add(model);

        public bool Contains(AccountModel model)
        {
            return _context.Accounts.Any(account => account.UserId == model.UserId);
        }

        public void Update(AccountModel model) => _context.Entry(model).State = EntityState.Modified;

        public void Delete(long id)
        {
            AccountModel account = _context.Accounts.Find(id);
            _context.Accounts.Remove(account);
        }

        public AccountModel Get(long id) => GetAll().Single(account => account.Id == id);

        public AccountModel GetByUserId(long userId) => GetAll().Single(account => account.UserId == userId);

        public IEnumerable<AccountModel> GetAll()
        {
            return _context.Accounts
                .Include(account => account.Characters)
                    .ThenInclude(character => character.Buildings)
                        .ThenInclude(building => building.Items)
                .Include(account => account.Characters)
                    .ThenInclude(character => character.Buildings)
                        .ThenInclude(building => building.Creator)
                .Include(account => account.Characters)
                    .ThenInclude(character => character.Items)
                        .ThenInclude(item => item.Creator)
                .Include(account => account.Characters)
                    .ThenInclude(character => character.Descriptions)
                .Include(account => account.Characters)
                    .ThenInclude(character => character.Vehicles)
                        .ThenInclude(vehicle => vehicle.Creator)
                .Include(account => account.Characters)
                    .ThenInclude(character => character.Vehicles)
                        .ThenInclude(vehicle => vehicle.ItemsInVehicle)
                            .ThenInclude(item => item.Creator)
                .Include(account => account.Characters)
                    .ThenInclude(character => character.Workers)
                        .ThenInclude(group => group.Group)
                            .ThenInclude(group => group.BossCharacter)
                .Include(account => account.Characters)
                    .ThenInclude(character => character.Workers)
                        .ThenInclude(group => group.Group)
                .ThenInclude(group => group.Workers)
                    .Include(account => account.Penalties).ToList();
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}