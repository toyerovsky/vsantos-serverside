/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Serverside.Core.Database;
using Serverside.Core.Database.Models;
using Serverside.Core.Interfaces;

namespace Serverside.Core.Repositories
{
    public class AccountsRepository : IRepository<AccountModel>
    {
        private RoleplayContext Context { get; } = RolePlayContextFactory.NewContext();

        public void Insert(AccountModel model) => Context.Accounts.Add(model);

        public bool Contains(AccountModel model)
        {
            return Context.Accounts.Any(account => account.UserId == model.UserId);
        }

        public void Update(AccountModel model) => Context.Entry(model).State = EntityState.Modified;

        public void Delete(long id)
        {
            var account = Context.Accounts.Find(id);
            Context.Accounts.Remove(account);
        }

        public AccountModel Get(long id) => GetAll().Single(account => account.Id == id);

        public AccountModel GetByUserId(long userId) => GetAll().Single(account => account.UserId == userId);

        public IEnumerable<AccountModel> GetAll()
        {
            return Context.Accounts
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

        public void Save() => Context.SaveChanges();

        public void Dispose() => Context?.Dispose();
    }
}