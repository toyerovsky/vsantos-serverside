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
    public class WorkersRepository : IRepository<WorkerModel>
    {
        private readonly RoleplayContext _context;

        public WorkersRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public WorkersRepository() : this(RolePlayContextFactory.NewContext())
        {
        }

        public void Insert(WorkerModel model) => _context.Workers.Add(model);

        public bool Contains(WorkerModel model)
        {
            return _context.Workers.Any(worker => worker.Id == model.Id);
        }

        public void Update(WorkerModel model) => _context.Entry(model).State = EntityState.Modified;

        public void Delete(int id)
        {
            WorkerModel worker = _context.Workers.Find(id);
            _context.Workers.Remove(worker);
        }

        public WorkerModel Get(int id) => GetAll(g => g.Id == id).Single();

        public IEnumerable<WorkerModel> GetAll(Expression<Func<WorkerModel, bool>> predicate = null)
        {
            return _context.Workers
                .Include(worker => worker.Character)
                    .ThenInclude(character => character.Account)
                .Include(worker => worker.Group)
                    .ThenInclude(group => group.BossCharacter)
                .Include(worker => worker.Group)
                    .ThenInclude(group => group.Workers)
                        .ThenInclude(group => group.Character);
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}