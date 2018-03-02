/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
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
        private readonly RoleplayContext _context;

        public GroupsRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public GroupsRepository()
        {
            _context = RolePlayContextFactory.NewContext();
        }

        public void Insert(GroupModel model) => _context.Groups.Add(model);

        public bool Contains(GroupModel model)
        {
            return _context.Groups.Any(groupModel => groupModel.Id == model.Id);
        }

        public void Update(GroupModel model) => _context.Entry(model).State = EntityState.Modified;

        public void Delete(long id)
        {
            var group = _context.Groups.Find(id);
            _context.Groups.Remove(group);
        }

        public GroupModel Get(long id) => GetAll().Single(g => g.Id == id);

        public IEnumerable<GroupModel> GetAll()
        {
            return _context.Groups
                .Include(group => group.BossCharacter)
                .Include(group => group.Workers)
                    .ThenInclude(worker => worker.Character)
                .Include(group => group.Workers)
                    .ThenInclude(worker => worker.Character)
                        .ThenInclude(character => character.Account)
                .ToList();
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();

    }
}