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
    public class GroupsRepository : IRepository<GroupModel>
    {
        private RoleplayContext Context { get; } = RolePlayContextFactory.NewContext();

        public void Insert(GroupModel model) => Context.Groups.Add(model);

        public bool Contains(GroupModel model)
        {
            return Context.Groups.Contains(model);
        }

        public void Update(GroupModel model) => Context.Entry(model).State = EntityState.Modified;

        public void Delete(long id)
        {
            var account = Context.Groups.Find(id);
            Context.Groups.Remove(account);
        }

        public GroupModel Get(long id) => GetAll().Single(g => g.Id == id);

        public IEnumerable<GroupModel> GetAll()
        {
            return Context.Groups
                .Include(group => group.BossCharacter)
                .Include(group => group.Workers)
                    .ThenInclude(worker => worker.Character)
                .Include(group => group.Workers)
                    .ThenInclude(worker => worker.Character)
                        .ThenInclude(character => character.AccountModel)
                .ToList();
        }

        public void Save() => Context.SaveChanges();

        public void Dispose() => Context?.Dispose();

    }
}