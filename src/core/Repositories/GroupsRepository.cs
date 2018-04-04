/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
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
    public class GroupsRepository : IRepository<GroupModel>
    {
        private readonly RoleplayContext _context;

        public GroupsRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public GroupsRepository() : this(RolePlayContextFactory.NewContext())
        {
        }

        public void Insert(GroupModel model) => _context.Groups.Add(model);

        public bool Contains(GroupModel model)
        {
            return _context.Groups.Any(groupModel => groupModel.Id == model.Id);
        }

        public void Update(GroupModel model) => _context.Entry(model).State = EntityState.Modified;

        public void Delete(int id)
        {
            GroupModel group = _context.Groups.Find(id);
            _context.Groups.Remove(group);
        }

        public GroupModel Get(int id) => GetAll(g => g.Id == id).Single();

        public IEnumerable<GroupModel> GetAll(Expression<Func<GroupModel, bool>> predicate = null)
        {
            return _context.Groups
                .Include(group => group.BossCharacter)
                .Include(group => group.Workers)
                    .ThenInclude(worker => worker.Character)
                .Include(group => group.Workers)
                    .ThenInclude(worker => worker.Character)
                        .ThenInclude(character => character.Account);
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();

    }
}